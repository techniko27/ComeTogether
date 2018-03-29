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

        public ObservableCollection<User> eventMemberList;
        private ActivityIndicator activityIndicator;
        private SearchBar memberSearchBar;
        private Frame listFrame;
        private StackLayout stackLayout;
        private ListView memberList;

        public EventDetailsMembersPage(Event ev)
        {
            InitializeComponent();

            initProperties();
            initLayout(ev);

            retrieveMemberListFromServer(ev);
        }

        private async void retrieveMemberListFromServer(Event ev)
        {
            String eventID = ev.ID;

            try
            {
                var usersInEvent = await App.firebase.Child("Veranstaltung_Benutzer").Child(eventID).OnceAsync<string>();

                foreach (FirebaseObject<string> e in usersInEvent)
                {
                    string userID = e.Key;
                    var userQuery = await App.firebase.Child("users").OrderByKey().StartAt(userID).LimitToFirst(1).OnceAsync<User>();
                    User user = userQuery.ElementAt(0).Object;
                    user.ID = userID;
                    eventMemberList.Add(user);
                    System.Diagnostics.Debug.WriteLine($"Name of {userID} is {user.userName}");
                }
                activityIndicatorSwitch();
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

        private void initLayout(Event ev)
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

            stackLayout = createStackLayout(ev);
            scrollView.Content = stackLayout;

            Content = scrollView;
        }

        private StackLayout createStackLayout(Event ev)
        {
            StackLayout stackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                Padding = new Thickness(10, 10, 10, 5)
            };

            memberSearchBar = new SearchBar
            {
                Placeholder = "Search members..."
            };

            memberSearchBar.TextChanged += searchBarTextChanged;

            Frame searchbarFrame = new Frame
            {
                Content = memberSearchBar,
                BackgroundColor = Color.LightGray,
                CornerRadius = 5,
                Padding = new Thickness(5, 0, 5, 10)
            };

            memberList = createMemberList(ev);

            listFrame = new Frame
            {
                Content = memberList,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5,
                Padding = new Thickness(5, 0, 5, 10),
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            stackLayout.Children.Add(searchbarFrame);
            stackLayout.Children.Add(activityIndicator);

            return stackLayout;
        }

        private ListView createMemberList(Event ev)
        {
            eventMemberList = new ObservableCollection<User>();
            ListView memberList = new ListView
            {
                ItemsSource = eventMemberList,
                ItemTemplate = new DataTemplate(() =>
                {
                    return new MemberListCell(ev, this);
                }),
                Margin = new Thickness(0, 0, 0, 10),
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                SeparatorColor = Color.LightSlateGray,
                
            };
            return memberList;
        }

        private void activityIndicatorSwitch()
        {
            if (activityIndicator.IsRunning)
            {
                activityIndicator.IsRunning = false;
                stackLayout.Children.Remove(activityIndicator);
                stackLayout.Children.Add(listFrame);
            }
            else
            {
                activityIndicator.IsRunning = true;
                stackLayout.Children.Remove(listFrame);
                stackLayout.Children.Add(activityIndicator);
            }
        }

        private void searchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            ObservableCollection<User> searchList = new ObservableCollection<User>();

            foreach (User user in eventMemberList)
            {
                if (user.userName.ToLower().Contains(memberSearchBar.Text.ToLower()))
                {
                    searchList.Add(user);
                }
            }

            memberList.ItemsSource = searchList;
        }
    }
}