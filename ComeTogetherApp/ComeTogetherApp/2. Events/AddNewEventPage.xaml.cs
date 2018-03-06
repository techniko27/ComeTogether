using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComeTogetherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNewEventPage : ContentPage
    {
        private StackLayout stack;
        private Entry nameEntry;
        private Entry descriptionEntry;
        private Entry locationEntry;
        private DatePicker datePicker;
        private Label messageLabel;
        private bool isDatePicked;
        private EventsPage eventsPage;

        public AddNewEventPage(EventsPage eventsPage)
        {
            InitializeComponent();

            this.eventsPage = eventsPage;

            Title = "Add new event";
            BackgroundColor = Color.FromHex(App.GetMenueColor());

            isDatePicked = false;

            ScrollView scroll = new ScrollView();

            stack = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(30, 0, 30, 0),
                Margin = new Thickness(10, 10, 10, 10)
            };
            Image eventImage = new Image {Aspect = Aspect.AspectFit, VerticalOptions = LayoutOptions.Start};
            eventImage.Source = "events_icon_130x130.png";
            int labelFontSize = 15;
            Label eventNameLabel = new Label
            {
                Text = "Name",
                FontSize = labelFontSize,
                TextColor = Color.Black
            };
            nameEntry = new Entry()
            {
                BackgroundColor = Color.White,

            };
            Label eventDescriptionLabel = new Label
            {
                Text = "Description",
                FontSize = labelFontSize,
                TextColor = Color.Black
            };
            descriptionEntry = new Entry()
            {
                BackgroundColor = Color.White,
            };
            Label eventLocationLabel = new Label
            {
                Text = "Location",
                FontSize = labelFontSize,
                TextColor = Color.Black
            };
            locationEntry = new Entry()
            {
                BackgroundColor = Color.White,
            };
            Label eventDateLabel = new Label
            {
                Text = "Date",
                FontSize = labelFontSize,
                TextColor = Color.Black
            };
            datePicker = new DatePicker
            {
                Format = "D",
                BackgroundColor = Color.White,
            };
            datePicker.Focused += DateSelected;
            messageLabel = new Label
            {
                FontSize = 20,
                TextColor = Color.Red
            };
            Button saveEventButton = new Button()
            {
                Text = "Create Event",
                BackgroundColor = Color.White,
                TextColor = Color.FromHex(App.GetMenueColor()),
                FontAttributes = FontAttributes.Bold
            };
            saveEventButton.Clicked += OnsaveEventButtonClicked;

            scroll.Content = stack;
            stack.Children.Add(eventImage);
            stack.Children.Add(eventNameLabel);
            stack.Children.Add(nameEntry);
            stack.Children.Add(eventDescriptionLabel);
            stack.Children.Add(descriptionEntry);
            stack.Children.Add(eventLocationLabel);
            stack.Children.Add(locationEntry);
            stack.Children.Add(eventDateLabel);
            stack.Children.Add(datePicker);
            stack.Children.Add(messageLabel);
            stack.Children.Add(saveEventButton);
            Content = scroll;
        }

        void DateSelected(object sender, EventArgs e)
        {
            isDatePicked = true;
        }

        async void OnsaveEventButtonClicked(object sender, EventArgs e)
        {
            if (nameEntry.Text == null)
            {
                messageLabel.Text = "Pleas enter a name for the Event!";
            }
            else if (descriptionEntry.Text == null)
            {
                messageLabel.Text = "Pleas enter a descripion for the Event!";
            }
            else if (locationEntry.Text == null)
            {
                messageLabel.Text = "Pleas enter a location for the Event!";
            }
            else if (!isDatePicked)
            {
                messageLabel.Text = "Pleas enter a date for the Event!";
            }
            else
            {
                string eventID = Guid.NewGuid().ToString("N").Substring(0, 20);

                DateTime dt = datePicker.Date;
                string eventDate = String.Format("{0:yyyy-MM-dd}", dt);

                Event ev = new Event(descriptionEntry.Text, eventDate, nameEntry.Text, locationEntry.Text, "1", eventID);       //TODO Picture

                try
                {
                    await App.firebase.Child("Veranstaltungen").Child(eventID).PutAsync(ev);

                    DisplayAlert("Success", "Event is successful created", "OK");

                    Navigation.PopToRootAsync();

                    eventsPage.eventList.Add(ev);
                    eventsPage.stack.Children.RemoveAt(eventsPage.stack.Children.IndexOf(eventsPage.stack.Children.Last()));         //Remove the last Grid from EventsPage
                    eventsPage.buildGrid(eventsPage.eventList);
                }
                catch (Exception)
                {
                    DisplayAlert("Server connection failure", "Communication problems occured while adding event", "OK");
                }
            }
        }
    }
}