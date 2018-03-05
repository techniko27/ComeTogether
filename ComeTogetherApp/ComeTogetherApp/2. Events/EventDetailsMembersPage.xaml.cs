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

        private List<User> eventMemberList;

        public EventDetailsMembersPage(Event ev)
        {
            InitializeComponent();

            retrieveMemberListFromServer(ev);

            initProperties();
            initLayout();
        }

        private async void retrieveMemberListFromServer(Event ev)
        {
            eventMemberList = new List<User>();
            String eventID = ev.ID;

            try
            {
                var eventUserMap = await App.firebase.Child("Benutzer_Veranstaltung").OrderByKey().StartAt("1").OnceAsync<EventUserMap>();

                foreach (FirebaseObject<EventUserMap> e in eventUserMap)
                {
                    if(e.Object.EventID.Equals(eventID))
                    {
                        string userID = e.Object.UserID;
                        var userQuery = await App.firebase.Child("users").OrderByKey().StartAt(userID).LimitToFirst(1).OnceAsync<User>();
                        User user = userQuery.ElementAt(0).Object;
                        eventMemberList.Add(user);
                        System.Diagnostics.Debug.WriteLine($"Name of {userID} is {user.userName}");
                    }
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
            BackgroundColor = Color.FromHex(App.GetMenueColor());
        }

        private void initLayout()
        {
            ScrollView scrollView = new ScrollView();
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
                Placeholder = "Search members...",
            };

            ObservableCollection<User> members = new ObservableCollection<User>();
            foreach(User user in eventMemberList)
            {
                members.Add(user);
            }

            ListView list = new ListView
            {
                ItemsSource = members,
                ItemTemplate = new DataTemplate(() =>
                {
                    Label nameLabel = new Label();
                    nameLabel.SetBinding(Label.TextProperty, "userName");

                    BoxView boxView = new BoxView();

                    // Return an assembled ViewCell.
                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Padding = new Thickness(0, 5),
                            Orientation = StackOrientation.Horizontal,
                            Children =
                                {
                                    boxView,
                                    new StackLayout
                                    {
                                        VerticalOptions = LayoutOptions.Center,
                                        Spacing = 0,
                                        Children =
                                        {
                                            nameLabel,
                                        }
                                        }
                                }
                        }
                    };
                })
            };

            stackLayout.Children.Add(memberSearchBar);
            stackLayout.Children.Add(list);

            return stackLayout;
        }
    }
}