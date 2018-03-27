using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace ComeTogetherApp
{
    class EventButton : StackLayout
    {
        private EventsPage eventsPage;

        public EventButton(Event ev, EventsPage eventsPage)
        {
            this.eventsPage = eventsPage;

            this.VerticalOptions = LayoutOptions.Fill;
            this.HorizontalOptions = LayoutOptions.Fill;
            this.Padding = new Thickness(2, 2, 2, 2);
            this.BackgroundColor = Color.FromHex("41BAC1");

            var eventImage = new Image {Aspect = Aspect.AspectFit, Scale = 0.9};
            if (ev.Bild.Length < 3)
            {
                eventImage.Source = "event_default.png";
            }
            else
            {
                eventImage.Source = ev.Bild;
            }
            this.Children.Add(eventImage);

            Label eventNameLabel = new Label
            {
                Text = ev.Name,
                VerticalOptions = LayoutOptions.Start,
                FontSize = 20
            };
            this.Children.Add(eventNameLabel);

            if (ev.ID != "0")
            {
                Label eventPersonalCostLabel = new Label
                {
                    /*Text = "Personal Cost: " + EventCostCalculator.getPersonalCost(ev, App.GetUserID()).Result + "€"*/
                    VerticalOptions = LayoutOptions.Start,
                    FontSize = 15
                };
                this.Children.Add(eventPersonalCostLabel);

                try
                {
                    Task<int> callTask = Task.Run(() => EventCostCalculator.getPersonalCost(ev, App.GetUserID()));
                    callTask.Wait();
                    eventPersonalCostLabel.Text = "Personal Cost: " + callTask.Result + "€";
                }
                catch (Exception)
                {
                    //eventsPage.DisplayAlert("Failure", "Cost for event " + ev.Name + " could not be calculated", "ok");
                    eventPersonalCostLabel.Text = "Personal Cost: ?";
                }
                
            }

            Label eventDateLabel = new Label
            {
                Text = ev.Datum,
                VerticalOptions = LayoutOptions.Start,
                FontSize = 15
            };
            this.Children.Add(eventDateLabel);

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
            {
                // handle the tap
                OnEventClicked(sender, e, ev);
            };

            this.GestureRecognizers.Add(tapGestureRecognizer);
        }
        public async void OnEventClicked(object sender, EventArgs e, Event ev)
        {
            if (ev.ID == "0")
            {
                string action = await eventsPage.DisplayActionSheet("", "Cancel", null, "Add new Event", "Enter joincode", "Scan joincode");
                Debug.WriteLine("Action: " + action);
                switch (action)
                {
                    case "Add new Event":
                        await Navigation.PushAsync(new AddNewEventPage(eventsPage));
                        break;
                    case "Enter joincode":
                        await Navigation.PushPopupAsync(new EnterJoinCodePopupPage(eventsPage));
                        break;
                    case "Scan joincode":
                        useScanPage();
                        break;
                    default:

                        break;
                }
            }
            else
            {
                Navigation.PushAsync(new SingleEventPage(ev)
                {
                    //Title = "Edit Event"
                });
            }
        }

        public async void useScanPage()
        {
            var options = new MobileBarcodeScanningOptions
            {
                AutoRotate = false,
                UseFrontCameraIfAvailable = false,
                TryHarder = true,
                PossibleFormats = new List<ZXing.BarcodeFormat>
                            {
                               ZXing.BarcodeFormat.EAN_8, ZXing.BarcodeFormat.EAN_13, ZXing.BarcodeFormat.QR_CODE
                            }
            };
            var scanPage = new ZXingScannerPage(options)
            {
                DefaultOverlayTopText = "Scan the join code",
                DefaultOverlayBottomText = "lala",
                DefaultOverlayShowFlashButton = true
            };
            // Navigate to our scanner page
            await Navigation.PushAsync(scanPage);
            scanPage.OnScanResult += (result) =>
            {
                // Stop scanning
                scanPage.IsScanning = false;

                // Pop the page and show the result
                Device.BeginInvokeOnMainThread(async () =>
                {
                    //Add user to event in Database
                    addUsertoEvent(result.Text);

                    await Navigation.PopAsync();
                });
            };
        }

        public async void addUsertoEvent(string eventID)
        {
            foreach (var eventPageEvent in eventsPage.eventList)
            {
                //Check if event allready exist for this user
                if (eventPageEvent.ID == eventID)
                {
                    await eventsPage.DisplayAlert("Note", "You are already member of this event named " + eventPageEvent.Name, "OK");
                    return;
                }
            }

            IReadOnlyCollection<Firebase.Database.FirebaseObject<Event>> ev = null;
            try
            {
                ev = await App.firebase.Child("Veranstaltungen").OrderByKey().StartAt(eventID).LimitToFirst(1).OnceAsync<Event>();

                if (ev.ElementAt(0).Object.ID != eventID)                //Check if right joincode exist in database.
                    throw null;
            }
            catch (Exception)
            {
                await eventsPage.DisplayAlert("Incorrect Joincode", "Event could not be found, with ID " + eventID, "OK");
                return;
            }
            try
            {
                await App.firebase.Child("Veranstaltung_Benutzer").Child(eventID).Child(App.GetUserID()).PutAsync<string>(App.GetUsername());
                await App.firebase.Child("Benutzer_Veranstaltung").Child(App.GetUserID()).Child(eventID).PutAsync<string>(ev.ElementAt(0).Object.Name);

                eventsPage.eventList.Add(ev.ElementAt(0).Object);
                eventsPage.stack.Children.RemoveAt(eventsPage.stack.Children.IndexOf(eventsPage.stack.Children.Last()));         //Remove the last Grid from EventsPage
                eventsPage.buildGrid(eventsPage.eventList);

                await eventsPage.DisplayAlert("Success", "Event "+ ev.ElementAt(0).Object.Name +" added", "OK");
            }
            catch (Exception)
            {
                await eventsPage.DisplayAlert("Server connection failure", "Communication problems occured while adding event", "OK");
            }
        }
    }
}
