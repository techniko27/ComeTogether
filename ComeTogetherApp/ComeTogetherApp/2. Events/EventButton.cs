using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
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

            var eventImage = new Image { Aspect = Aspect.AspectFit };
            if (ev.Bild.Length < 3)
            {
                eventImage.Source = "in_app_Logo_256x256.png";
            }
            else
            {
                eventImage.Source = ev.Bild;
            }
            Label eventNameLabel = new Label
            {
                Text = ev.Name,
                VerticalOptions = LayoutOptions.Start,
                FontSize = 20
            };
            Label eventMembercountLabel = new Label
            {
                Text = "EventMemberCount",
                VerticalOptions = LayoutOptions.Start,
                FontSize = 15
            };
            Label eventDateLabel = new Label
            {
                Text = ev.Datum,
                VerticalOptions = LayoutOptions.Start,
                FontSize = 15
            };
            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
            {
                // handle the tap
                OnEventClicked(sender, e, ev);
            };

            this.VerticalOptions = LayoutOptions.Fill;
            this.HorizontalOptions = LayoutOptions.Fill;
            this.Padding = new Thickness(2, 2, 2, 2);
            this.BackgroundColor = Color.FromHex("41BAC1");

            this.GestureRecognizers.Add(tapGestureRecognizer);
            this.Children.Add(eventImage);
            this.Children.Add(eventNameLabel);
            this.Children.Add(eventMembercountLabel);
            this.Children.Add(eventDateLabel);
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
                        Navigation.PushAsync(new AddNewEventPage(eventsPage));
                        break;
                    case "Enter joincode":
                        
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

                //TODO add user to event

                // Pop the page and show the result
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();
                    await eventsPage.DisplayAlert("Scanned Barcode", result.Text, "OK");
                });
            };
        }
    }
}
