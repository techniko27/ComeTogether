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
        private Event ev;
        public EventDetailsGeneralPage(Event ev)
        {
            InitializeComponent();

            this.ev = ev;

            initProperties();
            initLayout(ev);
        }

        private void initProperties()
        {
            Title = "General";
            BackgroundColor = Color.White;
        }

        private void initLayout(Event ev)
        {
            ScrollView scrollView = new ScrollView();
            StackLayout stackLayout = createStackLayout(ev);
            scrollView.Content = stackLayout;

            Content = scrollView;
        }

        private StackLayout createStackLayout(Event ev)
        {
            StackLayout stackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(10, 10, 10, 5)
            };

            StackLayout overviewInfoLayout = createOverviewInfoLayout(ev);
            StackLayout buttonOptionsLayout = createButtonOptionsLayout();

            Frame overviewInfoFrame = new Frame
            {
                Content = overviewInfoLayout,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5
            };

            Frame buttonOptionsFrame = new Frame
            {
                Content = buttonOptionsLayout,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5
            };

            stackLayout.Children.Add(overviewInfoFrame);
            stackLayout.Children.Add(buttonOptionsFrame);

            return stackLayout;
        }

        private StackLayout createOverviewInfoLayout(Event ev)
        {
            StackLayout overviewInfoLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.Start,
            };

            Image eventImage = new Image { Aspect = Aspect.AspectFit, VerticalOptions = LayoutOptions.Start };
            eventImage.Source = "icon.png";

            StackLayout infoLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(10, 0, 10, 5)
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
                Padding = new Thickness(10, 0, 10, 5),
                Margin = 5
            };

            Button startEventButton = new Button
            {
                Text = "Start Event",
                TextColor = Color.White,
                BackgroundColor = Color.LightGreen,
                FontAttributes = FontAttributes.Bold
            };

            Button inviteFriendsButton = new Button
            {
                Text = "Invite Friends",
                BackgroundColor = Color.White,
                TextColor = Color.FromHex(App.GetMenueColor()),
                FontAttributes = FontAttributes.Bold
            };
            inviteFriendsButton.Clicked += OninviteFriendsButtonClicked;

            Button editEventButton = new Button
            {
                Text = "Edit Event",
                BackgroundColor = Color.White,
                TextColor = Color.FromHex(App.GetMenueColor()),
                FontAttributes = FontAttributes.Bold
            };

            Button leaveEventButton = new Button
            {
                Text = "Leave Event",
                TextColor = Color.White,
                BackgroundColor = Color.OrangeRed,
                FontAttributes = FontAttributes.Bold
            };

            buttonOptionsLayout.Children.Add(startEventButton);
            buttonOptionsLayout.Children.Add(inviteFriendsButton);
            buttonOptionsLayout.Children.Add(editEventButton);
            buttonOptionsLayout.Children.Add(leaveEventButton);

            return buttonOptionsLayout;
        }

        async void OninviteFriendsButtonClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new InviteToEventPage(ev));
        }
    }
}