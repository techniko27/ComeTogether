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
        private ScrollView scrollView;

        public EventDetailsGeneralPage()
        {
            InitializeComponent();

            Title = "Allgemein";

            scrollView = new ScrollView();
            StackLayout stackLayout = createStackLayout();

            scrollView.Content = stackLayout;

            Content = scrollView;
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
            stackLayout.Children.Add(buttonOptionsLayout);

            return stackLayout;
        }


        private StackLayout createOverviewInfoLayout()
        {
            StackLayout overviewInfoLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5)
            };

            return overviewInfoLayout;
        }


        private StackLayout createButtonOptionsLayout()
        {
            StackLayout buttonOptionsLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5)
            };

            Button inviteFriendsButton = new Button
            {
                Text = "Freunde einladen",
                BackgroundColor = Color.White,
                TextColor = Color.FromHex(App.GetMenueColor()),

            };
            Button leaveEventButton = new Button
            {
                Text = "Veranstaltung verlassen",
                BackgroundColor = Color.White,
                TextColor = Color.FromHex(App.GetMenueColor()),

            };

            buttonOptionsLayout.Children.Add(inviteFriendsButton);
            buttonOptionsLayout.Children.Add(leaveEventButton);

            return buttonOptionsLayout;
        }
    }
}