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
            StackLayout stackLayout = createStackLayout(ev);
            scrollView.Content = stackLayout;

            Content = scrollView;
        }

        private void initProperties()
        {
            Title = "General";
            BackgroundColor = Color.LightGray;
        }

        private StackLayout createStackLayout(Event ev)
        {
            StackLayout stackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5)
            };

            StackLayout overviewInfoLayout = createOverviewInfoLayout(ev);
            StackLayout buttonOptionsLayout = createButtonOptionsLayout();

            stackLayout.Children.Add(overviewInfoLayout);
            stackLayout.Children.Add(createSeparator(3));
            stackLayout.Children.Add(buttonOptionsLayout);

            return stackLayout;
        }

        private StackLayout createOverviewInfoLayout(Event ev)
        {
            StackLayout overviewInfoLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Start,
                Padding = new Thickness(10, Device.OnPlatform(20, 10, 0), 10, 5)
            };

            Image eventImage = new Image { Aspect = Aspect.AspectFit, VerticalOptions = LayoutOptions.Start };
            eventImage.Source = "icon.png";

            StackLayout infoLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5)
            };

            Label eventNameLabel = new Label
            {
                Text = ev.Name,
                FontSize = 20,
                TextColor = Color.Black
            };

            Label eventDescriptionLabel = new Label
            {
                Text = ev.Beschreibung + Environment.NewLine
            };

            Label eventPlaceLabel = new Label
            {
                Text = "Place: " + ev.Ort,
            };

            Label eventDateLabel = new Label
            {
                Text = "Date: " + ev.Datum,
            };

            infoLayout.Children.Add(eventNameLabel);
            infoLayout.Children.Add(createSeparator(2));
            infoLayout.Children.Add(eventDescriptionLabel);
            infoLayout.Children.Add(createSeparator(1));
            infoLayout.Children.Add(eventPlaceLabel);
            infoLayout.Children.Add(eventDateLabel);

            overviewInfoLayout.Children.Add(eventImage);
            overviewInfoLayout.Children.Add(infoLayout);

            return overviewInfoLayout;
        }

        private static BoxView createSeparator(int height)
        {
            return new BoxView()
            {
                Color = Color.Black,
                WidthRequest = 1000,
                HeightRequest = height,
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