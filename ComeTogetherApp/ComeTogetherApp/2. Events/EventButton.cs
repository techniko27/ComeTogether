using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComeTogetherApp
{
    class EventButton : StackLayout
    {
        public EventButton(Event ev, EventsPage page)
        {
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
        public void OnEventClicked(object sender, EventArgs e, Event ev)
        {
            Navigation.PushAsync(new SingleEventPage(ev.ID)
            {
                Title = "Edit Event"
            });
        }
    }
}
