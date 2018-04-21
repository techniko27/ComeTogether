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
        private Event ev;
        private EventsPage eventsPage;
        public Label eventPersonalCostLabel;

        private Task<int> personalCostCalculatingTask;

        public EventButton(Event ev, EventsPage eventsPage)
        {
            this.ev = ev;
            this.eventsPage = eventsPage;

            this.VerticalOptions = LayoutOptions.Fill;
            this.HorizontalOptions = LayoutOptions.Fill;
            this.Padding = new Thickness(2, 2, 2, 2);
            this.BackgroundColor = Color.FromHex("41BAC1");

            var eventImage = new Image { Aspect = Aspect.AspectFit, Scale = 0.9 };
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
            if (ev.Name.Length > 16)
            {
                eventNameLabel.Text = ev.Name.Substring(0, 16) + "...";         //Show dots at the end of the eventName when it is to long
            }

            if (ev.ID != "0")
            {
                eventPersonalCostLabel = new Label
                {
                    Text = "Personal Cost: ...",/* + EventCostCalculator.getPersonalCost(ev, App.GetUserID()).Result + "€"*/
                    VerticalOptions = LayoutOptions.Start,
                    FontSize = 15
                };
                this.Children.Add(eventPersonalCostLabel);

                try
                {
                    personalCostCalculatingTask = Task.Run(() => EventCostCalculator.getPersonalCost(ev, App.GetUserID(), eventPersonalCostLabel, new Button()));
                    //callTask.Wait();
                    //eventPersonalCostLabel.Text = "Personal Cost: " + callTask.Result + "€";
                }
                catch (Exception)
                {
                    //eventsPage.DisplayAlert("Failure", "Cost for event " + ev.Name + " could not be calculated", "ok");
                    eventPersonalCostLabel.Text = "Personal Cost: ?";
                }

            }

            StackLayout horizontalDateAndStateLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, 0, 0, 0),
                HeightRequest = 30
            };
            this.Children.Add(horizontalDateAndStateLayout);

            string eventDateText = "";

            if(!ev.Name.Equals("Add New Event"))
            {
                string date = ev.Datum;
                int year;
                int month;
                int day;
                Int32.TryParse(date.Substring(0, 4), out year);
                Int32.TryParse(date.Substring(5, 2), out month);
                Int32.TryParse(date.Substring(8, 2), out day);

                eventDateText = day.ToString();
                eventDateText += (day % 10 == 1 && day != 11) ? "st" : (day % 10 == 2 && day != 12) ? "nd" : (day % 10 == 3 && day != 13) ? "rd" : "th";

                eventDateText += $" {System.Globalization.DateTimeFormatInfo.CurrentInfo.GetMonthName(month)} {year}";
            }

            Label eventDateLabel = new Label
            {
                Text = eventDateText,
                VerticalOptions = LayoutOptions.Start,
                FontSize = 15
            };
            horizontalDateAndStateLayout.Children.Add(eventDateLabel);

            Image statusImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "Stop.png",
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Margin = new Thickness(0, 0, 10, 0)
            };
            if (ev.Status.Equals("stop"))
            {
                horizontalDateAndStateLayout.Children.Add(statusImage);
            }

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
            if (ev.Status.Equals("stop"))
            {
                await Navigation.PushAsync(new CostOverviewPage(ev, personalCostCalculatingTask));
            }
            else
            {
                await Navigation.PushAsync(new SingleEventPage(ev));
            }
        }

    }
}
