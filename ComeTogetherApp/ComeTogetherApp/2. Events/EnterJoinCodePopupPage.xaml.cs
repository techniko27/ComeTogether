using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;
using Rg.Plugins.Popup.Animations.Base;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComeTogetherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterJoinCodePopupPage : PopupPage
    {
        private Entry joinCodeEntry;
        private EventsPage eventsPage;
        public EnterJoinCodePopupPage(EventsPage eventsPage)
        {
            InitializeComponent();

            this.eventsPage = eventsPage;

            StackLayout stack = new StackLayout
            {
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                //Padding = new Thickness(100, 100, 100, 100),
                //Margin = new Thickness(50, 50, 50, 50),
                Padding = 10,
                HeightRequest = 100,
                WidthRequest = 300
            };
            joinCodeEntry = new Entry
            {
                Text = "Enter Join-Code here",
                FontSize = 15,
                TextColor = Color.White,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
            };
            joinCodeEntry.Focused += OnJoinCodeEntryClicked;
            Button enterJoinCodeButton = new Button()
            {
                Text = "Join Event",
                BackgroundColor = Color.White,
                TextColor = Color.FromHex(App.GetMenueColor()),
                FontAttributes = FontAttributes.Bold,
            };
            enterJoinCodeButton.Clicked += OnEnterJoinCodeButtonClicked;

            stack.Children.Add(joinCodeEntry);
            stack.Children.Add(enterJoinCodeButton);

            Content = stack;
            //BackgroundColor = Color.Aqua;

            //Versuch um BackgrondClick zu bekommen
            CloseWhenBackgroundIsClicked = true;
            HasSystemPadding = true;
            BackgroundClicked += OnBackgroundClicked;

            Padding = 30;
        }

        async void OnEnterJoinCodeButtonClicked(object sender, EventArgs e)
        {
            //Check if event allready exist for this user
            foreach (var eventPageEvent in eventsPage.eventList)
            {
                if (eventPageEvent.ID == joinCodeEntry.Text)
                {
                    await DisplayAlert("Note", "You are already member of this Event.", "OK");
                    await Navigation.PopPopupAsync();
                    return;
                }
            }

            IReadOnlyCollection<Firebase.Database.FirebaseObject<Event>> ev = null;
            try
            {
                ev = await App.firebase.Child("Veranstaltungen").OrderByKey().StartAt(joinCodeEntry.Text).LimitToFirst(1).OnceAsync<Event>();

                if (ev.ElementAt(0).Object.ID != joinCodeEntry.Text)                //Check if right joincode exist in database.
                    throw null;
            }
            catch (Exception)
            {
                await DisplayAlert("Incorrect Joincode", "Event could not be found", "OK");
                await Navigation.PopPopupAsync();
                return;
            }
            try
            {
                await App.firebase.Child("Veranstaltung_Benutzer").Child(joinCodeEntry.Text).Child(App.GetUserID()).PutAsync<string>(App.GetUsername());
                await App.firebase.Child("Benutzer_Veranstaltung").Child(App.GetUserID()).Child(joinCodeEntry.Text).PutAsync<string>(ev.ElementAt(0).Object.Name);

                eventsPage.eventList.Add(ev.ElementAt(0).Object);
                eventsPage.stack.Children.RemoveAt(eventsPage.stack.Children.IndexOf(eventsPage.stack.Children.Last()));         //Remove the last Grid from EventsPage
                eventsPage.buildGrid(eventsPage.eventList);

                await DisplayAlert("Success", "Event added", "OK");

                await Navigation.PopPopupAsync();
            }
            catch (Exception)
            {
                await DisplayAlert("Server connection failure", "Communication problems occured while adding event", "OK");
            }
        }
        async void OnJoinCodeEntryClicked(object sender, EventArgs e)
        {
            joinCodeEntry.Text = "";
        }

        async void OnBackgroundClicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
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