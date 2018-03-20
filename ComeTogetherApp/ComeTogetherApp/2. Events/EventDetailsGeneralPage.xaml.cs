using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
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
                TextColor = Color.Black,
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
            inviteFriendsButton.Clicked += OnInviteFriendsButtonClicked;

            Button editEventButton = new Button
            {
                Text = "Edit Event",
                BackgroundColor = Color.White,
                TextColor = Color.FromHex(App.GetMenueColor()),
                FontAttributes = FontAttributes.Bold
            };
            editEventButton.Clicked += OnEditEventButtonClicked;

            Button leaveEventButton = new Button
            {
                Text = "Leave Event",
                TextColor = Color.Black,
                BackgroundColor = Color.OrangeRed,
                FontAttributes = FontAttributes.Bold
            };
            leaveEventButton.Clicked += OnLeaveEventButtonClicked;

            if(ev.adminID.Equals(App.GetUserID()))
                buttonOptionsLayout.Children.Add(startEventButton);
            buttonOptionsLayout.Children.Add(inviteFriendsButton);
            if (ev.adminID.Equals(App.GetUserID()))
                buttonOptionsLayout.Children.Add(editEventButton);
            buttonOptionsLayout.Children.Add(leaveEventButton);

            return buttonOptionsLayout;
        }

        private async void OnEditEventButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditEventInfoPage(ev));
        }

        private async void OnLeaveEventButtonClicked(object sender, EventArgs e)
        {
            string action;
            action = await DisplayActionSheet("Member Options", "Cancel", null, "Leave Event");
            switch (action)
            {
                case "Leave Event":
                    bool answer;
                    if (!App.GetUserID().Equals(ev.adminID))
                        answer = await DisplayAlert("Are you sure?", $"Do you really want to leave this event?", "Yes", "No");
                    else
                        answer = await DisplayAlert("Are you sure?", $"Do you really want to leave this event? As the administrator of this event, leaving it will fully delete the event.", "Yes", "No");
                    if (answer)
                        removeUserFromEvent();
                    break;
                default:
                    break;
            }
        }

        private async void OnInviteFriendsButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InviteToEventPage(ev));
        }

        private async void removeUserFromEvent()
        {
            string eventID = ev.ID;

            // if the admin is leaving the event, dissolve the whole event
            if (App.GetUserID().Equals(ev.adminID))
            {
                var usersInEvent = await App.firebase.Child("Veranstaltung_Benutzer").Child(eventID).OnceAsync<string>();

                foreach (FirebaseObject<string> e in usersInEvent)
                {
                    string userID = e.Key;
                    await App.firebase.Child("Benutzer_Veranstaltung").Child(userID).Child(eventID).DeleteAsync();
                }

                await App.firebase.Child("Veranstaltung_Benutzer").Child(eventID).DeleteAsync();
                await App.firebase.Child("Veranstaltungen").Child(eventID).DeleteAsync();

                await Navigation.PushAsync(new EventsPage
                {
                    Title = "Events"
                });
                clearNavigationStack();

                return;
            }

            await App.firebase.Child("Benutzer_Veranstaltung").Child(App.GetUserID()).Child(eventID).DeleteAsync();
            await App.firebase.Child("Veranstaltung_Benutzer").Child(eventID).Child(App.GetUserID()).DeleteAsync();

            await Navigation.PushAsync(new EventsPage
            {
                Title = "Events"
            });
            clearNavigationStack();
        }

        private void clearNavigationStack()
        {
            var navigationPages = Navigation.NavigationStack.ToList();
            foreach (var page in navigationPages)
            {
                Navigation.RemovePage(page);
            }
        }
    }
}