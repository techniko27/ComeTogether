using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComeTogetherApp
{
    class EventButton : StackLayout
    {
        private EventsPage page;
        public EventButton(Event ev, EventsPage page)
        {
            this.page = page;

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
                string action = await page.DisplayActionSheet("", "Cancel", null, "Add new Event", "Enter joincode", "Scan joincode");
                Debug.WriteLine("Action: " + action);
                switch (action)
                {
                    case "Add new Event":
                        Navigation.PushAsync(new AddNewEventPage());
                        break;
                    case "Enter joincode":
                        
                        break;
                    case "Scan joincode":
                        
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
    }
}
