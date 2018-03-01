using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComeTogetherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventDetailsGeneralPage : ContentPage
    {

        public EventDetailsGeneralPage(Event ev)
        {
            InitializeComponent();

            initProperties();
            initLayout(ev);
        }

        private void initLayout(Event ev)
        {
            ScrollView scrollView = new ScrollView();
            StackLayout stackLayout = createStackLayout();
            scrollView.Content = stackLayout;

            Content = scrollView;
        }

        private void initProperties()
        {
            Title = "General";
            BackgroundColor = Color.FromHex(App.GetMenueColor());
        }

        private StackLayout createStackLayout()
        {
            StackLayout stackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5)
            };

            StackLayout overviewInfoLayout = createOverviewInfoLayout();
            StackLayout buttonOptionsLayout = createButtonOptionsLayout();

            stackLayout.Children.Add(overviewInfoLayout);
            stackLayout.Children.Add(createSeparator());
            stackLayout.Children.Add(buttonOptionsLayout);

            return stackLayout;
        }

        private StackLayout createOverviewInfoLayout()
        {
            StackLayout overviewInfoLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5)
            };

            Label eventDetailsLabel = new Label
            {
                Text = "Event Details",
                FontSize = 20
            };

            overviewInfoLayout.Children.Add(eventDetailsLabel);

            return overviewInfoLayout;
        }

        private static BoxView createSeparator()
        {
            return new BoxView()
            {
                Color = Color.Black,
                WidthRequest = 1000,
                HeightRequest = 2,
                HorizontalOptions = LayoutOptions.Center
            };
        }

        private StackLayout createButtonOptionsLayout()
        {
            StackLayout buttonOptionsLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5),
                Margin = 5
            };

            Button inviteFriendsButton = new Button
            {
                Text = "Invite Friends",
                BackgroundColor = Color.White,
                TextColor = Color.FromHex(App.GetMenueColor()),
                FontAttributes = FontAttributes.Bold
            };
            Button leaveEventButton = new Button
            {
                Text = "Leave Event",
                BackgroundColor = Color.White,
                TextColor = Color.FromHex(App.GetMenueColor()),
                FontAttributes = FontAttributes.Bold
            };

            buttonOptionsLayout.Children.Add(inviteFriendsButton);
            buttonOptionsLayout.Children.Add(leaveEventButton);

            return buttonOptionsLayout;
        }
    }
}