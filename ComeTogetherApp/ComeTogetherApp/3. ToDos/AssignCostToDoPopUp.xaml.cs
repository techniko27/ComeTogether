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
        private Cost cost;

        private StackLayout mainLayout;
        private Frame memberListFrame;


        public AssignCostToDoPopUp()
        {
            InitializeComponent();

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

            Label membersLabel = createFrameHeaderLabel("Create New Cost");

            Frame membersListFrame = createDetailsFrame();

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

        private Frame createDetailsFrame()
        {
            StackLayout detailsLayout = new StackLayout();

            Entry descriptionEntry = new Entry
            {
                Placeholder = "Description...",
                TextColor = Color.White,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                FontSize = 18
            };

            Entry costEntry = new Entry
            {
                Placeholder = "Amount...",
                TextColor = Color.White,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                FontSize = 18
            };

            Button saveCostButton = new Button
            {
                Text = "Add Cost",
                TextColor = Color.Black,
                BackgroundColor = Color.LightGreen,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            saveCostButton.Clicked += saveCost;

            detailsLayout.Children.Add(descriptionEntry);
            detailsLayout.Children.Add(costEntry);
            detailsLayout.Children.Add(saveCostButton);

            Frame listViewFrame = new Frame
            {
                Content = detailsLayout,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5,
                Padding = 5,
                WidthRequest = 250,
                HeightRequest = 150
            };
            return listViewFrame;
        }

        private void saveCost(object sender, EventArgs e)
        {
            Navigation.PopPopupAsync();
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