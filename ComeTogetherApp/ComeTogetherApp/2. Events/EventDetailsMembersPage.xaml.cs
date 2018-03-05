using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComeTogetherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventDetailsMembersPage : ContentPage
    {

        private ObservableCollection<User> eventMemberList;
        private ActivityIndicator activityIndicator;
        private Frame listFrame;
        private ListView memberList;

        public EventDetailsMembersPage(Event ev)
        {
            InitializeComponent();

            initProperties();
            initLayout();

            retrieveMemberListFromServer(ev);
        }

        private async void retrieveMemberListFromServer(Event ev)
        {
            eventMemberList = new ObservableCollection<User>();

            String eventID = ev.ID;

            try
            {
                var usersInEvent = await App.firebase.Child("Benutzer_Veranstaltung").Child(eventID).OnceAsync<string>();

                foreach (FirebaseObject<string> e in usersInEvent)
                {
                    string userID = e.Object;
                    var userQuery = await App.firebase.Child("users").OrderByKey().StartAt(userID).LimitToFirst(1).OnceAsync<User>();
                    User user = userQuery.ElementAt(0).Object;
                    eventMemberList.Add(user);
                    System.Diagnostics.Debug.WriteLine($"Name of {userID} is {user.userName}");
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("Server connection failure", "Communication problems occured while querying", "OK");
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        private void initProperties()
        {
            Title = "Members";
        }

        private void initLayout()
        {
            ScrollView scrollView = new ScrollView();

            activityIndicator = new ActivityIndicator()
            {
                Color = Color.Gray,
                IsRunning = true,
                WidthRequest = 80,
                HeightRequest = 80,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            StackLayout stackLayout = createStackLayout();
            scrollView.Content = stackLayout;

            Content = scrollView;
        }

        private StackLayout createStackLayout()
        {
            StackLayout stackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(10, 10, 10, 5)
            };

            SearchBar memberSearchBar = new SearchBar
            {
                Placeholder = "Search members..."
            };

            Frame searchbarFrame = new Frame
            {
                Content = memberSearchBar,
                BackgroundColor = Color.LightGray,
                CornerRadius = 5,
                Padding = new Thickness(5, 0, 5, 10)
            };

            memberList = createMemberList();

            listFrame = new Frame
            {
                Content = memberList,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5,
                Padding = new Thickness(5, 10, 5, 0),
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            stackLayout.Children.Add(searchbarFrame);
            stackLayout.Children.Add(listFrame);

            return stackLayout;
        }

        private ListView createMemberList()
        {
            ListView memberList = new ListView
            {
                ItemsSource = eventMemberList,
                ItemTemplate = new DataTemplate(() => {

                    TextCell cell = new TextCell();
                    cell.SetBinding(TextCell.TextProperty, "userName");
                    return cell;
                }),
                Margin = new Thickness(0, 0, 0, 10)
            };
            return memberList;
        }

        private void activityIndicatorSwitch()
        {
            if (activityIndicator.IsRunning)
            {
                activityIndicator.IsRunning = false;
                listFrame.Content = memberList;
            }
            else
            {
                activityIndicator.IsRunning = true;
                listFrame.Content = activityIndicator;
            }
        }
    }
}