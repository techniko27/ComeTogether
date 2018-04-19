using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Firebase.Database;
using Firebase.Database.Query;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComeTogetherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssignMemberToDoPopUp : PopupPage
    {

        private ObservableCollection<User> eventMemberList;
        private Event ev;
        private ToDo toDo;

        private StackLayout mainLayout;
        private ActivityIndicator activityIndicator;
        private Frame memberListFrame;

        private ToDoDetailsPage detailsPage;

        public AssignMemberToDoPopUp(Event ev, ToDo toDo, ToDoDetailsPage detailsPage)
        {
            InitializeComponent();

            this.ev = ev;
            this.toDo = toDo;
            this.detailsPage = detailsPage;
            eventMemberList = new ObservableCollection<User>();

            mainLayout = new StackLayout
            {
                Padding = 30,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 300
            };

            activityIndicator = new ActivityIndicator()
            {
                Color = Color.Gray,
                IsRunning = true,
                WidthRequest = 80,
                HeightRequest = 80,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            memberListFrame = createEventMemberListFrame();

            mainLayout.Children.Add(activityIndicator);
            Content = mainLayout;

            retrieveMemberListFromServer(ev);
        }

        private async void retrieveMemberListFromServer(Event ev)
        {
            String eventID = ev.ID;
            String toDoID = toDo.ID;

            try
            {
                var usersInEvent = await App.firebase.Child("Veranstaltung_Benutzer").Child(eventID).OnceAsync<string>();

                foreach (FirebaseObject<string> e in usersInEvent)
                {
                    string userID = e.Key;
                    var userQuery = await App.firebase.Child("users").OrderByKey().StartAt(userID).LimitToFirst(1).OnceAsync<User>();
                    User user = userQuery.ElementAt(0).Object;
                    user.ID = userID;
                    if (userID.Equals(toDo.OrganisatorID))
                        continue;
                    eventMemberList.Add(user);
                }
                activityIndicatorSwitch();
            }
            catch (Exception e)
            {
                await DisplayAlert("Server connection failure", "Communication problems occured while querying", "OK");
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        private Frame createEventMemberListFrame()
        {
            StackLayout membersLayout = new StackLayout();

            Label membersLabel = createFrameHeaderLabel("Assign Event Member:");

            ListView membersListView = createMembersListView();

            Frame membersListFrame = createListViewFrame(membersListView);

            membersLayout.Children.Add(membersLabel);
            membersLayout.Children.Add(membersListFrame);

            Frame completedToDosFrame = createToDosFrame(membersLayout);

            return completedToDosFrame;
        }

        private Label createFrameHeaderLabel(String text)
        {
            return new Label
            {
                Text = text,
                TextColor = Color.White,
                FontSize = 20,
                Margin = new Thickness(5, 0, 0, 0),
            };
        }

        private ListView createMembersListView()
        {
            ListView memberListView = new ListView
            {
                ItemsSource = eventMemberList,
                ItemTemplate = new DataTemplate(() =>
                {
                    return new AssignMembersPopUpListCell(ev, this);
                }),
                Margin = new Thickness(0, 0, 0, 10),
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                SeparatorColor = Color.LightSlateGray
            };
            return memberListView;
        }

        private Frame createListViewFrame(ListView listView)
        {
            ScrollView scrollableList = new ScrollView
            {
                Content = listView
            };

            Frame listViewFrame = new Frame
            {
                Content = scrollableList,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5,
                Padding = 5
            };
            return listViewFrame;
        }

        private Frame createToDosFrame(StackLayout toDoLayout)
        {
            return new Frame
            {
                Content = toDoLayout,
                Padding = 0,
                BackgroundColor = Color.FromHex("376467"),
                CornerRadius = 5
            };
        }

        public void assignMemberToToDo(User user)
        {
            detailsPage.assignMemberToToDo(user);
        }

        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }
        private void activityIndicatorSwitch()
        {
            if (activityIndicator.IsRunning)
            {
                activityIndicator.IsRunning = false;
                mainLayout.Children.Remove(activityIndicator);
                mainLayout.Children.Add(memberListFrame);
            }
            else
            {
                activityIndicator.IsRunning = true;
                mainLayout.Children.Remove(memberListFrame);
                mainLayout.Children.Add(activityIndicator);
            }
        }
    }
}