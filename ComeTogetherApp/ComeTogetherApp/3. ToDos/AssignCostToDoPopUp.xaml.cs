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
    public partial class AssignCostToDoPopUp : PopupPage
    {
        private ObservableCollection<User> eventMemberList;
        private ObservableCollection<User> assignedCostsMemberList;

        private StackLayout mainLayout;
        private ActivityIndicator activityIndicator;
        private Frame memberListFrame;

        public AssignCostToDoPopUp(ObservableCollection<User> eventMemberList, ObservableCollection<User> assignedCostsMemberList)
        {
            InitializeComponent();
            this.eventMemberList = eventMemberList;
            this.assignedCostsMemberList = assignedCostsMemberList;

            mainLayout = new StackLayout
            {
                Padding = 30,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                HeightRequest = 300
            };

            memberListFrame = createEventMemberListFrame();

            mainLayout.Children.Add(memberListFrame);
            Content = mainLayout;
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
            ObservableCollection<User> possibleMembersList = new ObservableCollection<User>();
            foreach(User user in eventMemberList)
            {
                if (!assignedCostsMemberList.Contains(user))
                    possibleMembersList.Add(user);
            }

            ListView memberListView = new ListView
            {
                ItemsSource = possibleMembersList,
                ItemTemplate = new DataTemplate(() =>
                {
                    return new AssignCostMembersPopUpListCell(this, assignedCostsMemberList);
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
    }
}