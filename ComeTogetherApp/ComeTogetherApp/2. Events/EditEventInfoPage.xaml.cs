using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;
using Rg.Plugins.Popup.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComeTogetherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditEventInfoPage : ContentPage
    {
        private Event ev;
        private Entry titleEntry;
        private Editor descriptionEntry;
        private Image eventImage;
        private Entry locationEntry;
        private DatePicker datePicker;
        private Button saveChangesButton;

        private Label errorLabel;

        public EditEventInfoPage(Event ev)
        {
            InitializeComponent();

            this.ev = ev;

            initProperties();
            initLayout(ev);
        }

        private void initProperties()
        {
            Title = "Edit Event";
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

            Frame eventTitleFrame = createEventTitleFrame();
            Frame eventDescriptionFrame = creatEventDescriptionFrame();
            Frame eventIconFrame = createEventIconFrame();
            Frame eventLocationFrame = createEventLocationFrame();
            Frame eventDateFrame = createEventDateFrame();
            Frame buttonsFrame = createButtonsFrame();

            stackLayout.Children.Add(eventTitleFrame);
            stackLayout.Children.Add(eventDescriptionFrame);
            stackLayout.Children.Add(eventIconFrame);
            stackLayout.Children.Add(eventLocationFrame);
            stackLayout.Children.Add(eventDateFrame);
            stackLayout.Children.Add(buttonsFrame);

            titleEntry.TextChanged += textChangesMade;
            descriptionEntry.TextChanged += textChangesMade;
            locationEntry.TextChanged += textChangesMade;
            datePicker.DateSelected += dateChangesMade;

            return stackLayout;
        }

        private Frame createEventTitleFrame()
        {
            StackLayout eventTitleLayout = new StackLayout();

            Label eventTitleLabel = new Label
            {
                Text = "Name:",
                TextColor = Color.White,
                FontSize = 20,
                Margin = new Thickness(5, 0, 0, 0),
            };

            titleEntry = new Entry
            {
                Text = ev.Name,
                BackgroundColor = Color.White
            };


            Frame titleEntryFrame = new Frame
            {
                Content = titleEntry,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5
            };

            eventTitleLayout.Children.Add(eventTitleLabel);
            eventTitleLayout.Children.Add(titleEntryFrame);

            Frame eventTitleFrame = new Frame
            {
                Content = eventTitleLayout,
                Padding = 0,
                BackgroundColor = Color.FromHex("376467"),
                CornerRadius = 5
            };

            return eventTitleFrame;
        }

        private Frame creatEventDescriptionFrame()
        {
            StackLayout eventDescriptionLayout = new StackLayout();

            Label eventDescriptionLabel = new Label
            {
                Text = "Description:",
                TextColor = Color.White,
                FontSize = 20,
                Margin = new Thickness(5, 0, 0, 0),
            };

            descriptionEntry = new Editor
            {
                Text = ev.Beschreibung,
                BackgroundColor = Color.White,
                HeightRequest = 150
            };

            Frame descriptionEntryFrame = new Frame
            {
                Content = descriptionEntry,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5
            };

            eventDescriptionLayout.Children.Add(eventDescriptionLabel);
            eventDescriptionLayout.Children.Add(descriptionEntryFrame);

            Frame eventDescriptionFrame = new Frame
            {
                Content = eventDescriptionLayout,
                Padding = 0,
                BackgroundColor = Color.FromHex("376467"),
                CornerRadius = 5
            };

            return eventDescriptionFrame;
        }

        private Frame createEventIconFrame()
        {
            StackLayout eventIconLayout = new StackLayout();

            Label eventIconLabel = new Label
            {
                Text = "Image:",
                TextColor = Color.White,
                FontSize = 20,
                Margin = new Thickness(5, 0, 0, 0),
            };

            eventImage = new Image
            {
                Aspect = Aspect.AspectFit,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center
            };
            eventImage.Source = ev.Bild;
            eventImage.Scale = 0.5;
            var eventImageTapGestureRecognizer = new TapGestureRecognizer();
            eventImageTapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
            {
                // handle the tap
                OnEventImageClicked(sender, e);
            };
            eventImage.GestureRecognizers.Add(eventImageTapGestureRecognizer);
            eventImage.PropertyChanged += eventImageChanged;

            Frame imageFrame = new Frame
            {
                Content = eventImage,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5
            };

            eventIconLayout.Children.Add(eventIconLabel);
            eventIconLayout.Children.Add(imageFrame);


            Frame eventTitleFrame = new Frame
            {
                Content = eventIconLayout,
                Padding = 0,
                BackgroundColor = Color.FromHex("376467"),
                CornerRadius = 5
            };
            return eventTitleFrame;
        }

        private Frame createEventLocationFrame()
        {
            StackLayout eventLocationLayout = new StackLayout();

            Label eventLocationLabel = new Label
            {
                Text = "Location:",
                TextColor = Color.White,
                FontSize = 20,
                Margin = new Thickness(5, 0, 0, 0),
            };

            locationEntry = new Entry
            {
                Text = ev.Name,
                BackgroundColor = Color.White
            };

            Frame locationEntryFrame = new Frame
            {
                Content = locationEntry,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5
            };

            eventLocationLayout.Children.Add(eventLocationLabel);
            eventLocationLayout.Children.Add(locationEntryFrame);

            Frame eventLocationFrame = new Frame
            {
                Content = eventLocationLayout,
                Padding = 0,
                BackgroundColor = Color.FromHex("376467"),
                CornerRadius = 5
            };

            return eventLocationFrame;
        }

        private Frame createEventDateFrame()
        {
            StackLayout eventDateLayout = new StackLayout();

            Label eventDateLabel = new Label
            {
                Text = "Date:",
                TextColor = Color.White,
                FontSize = 20,
                Margin = new Thickness(5, 0, 0, 0),
            };

            string date = ev.Datum;
            int year;
            int month;
            int day;
            Int32.TryParse(date.Substring(0, 4), out year);
            Int32.TryParse(date.Substring(5, 2), out month);
            Int32.TryParse(date.Substring(8, 2), out day);

            DateTime dt = new DateTime(year, month, day);

            datePicker = new DatePicker
            {
                Format = "D",
                Date = dt,
                BackgroundColor = Color.White,
            };

            Frame datePickerFrame = new Frame
            {
                Content = datePicker,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5
            };

            eventDateLayout.Children.Add(eventDateLabel);
            eventDateLayout.Children.Add(datePickerFrame);

            Frame eventDateFrame = new Frame
            {
                Content = eventDateLayout,
                Padding = 0,
                BackgroundColor = Color.FromHex("376467"),
                CornerRadius = 5
            };

            return eventDateFrame;
        }

        private Frame createButtonsFrame()
        {
            StackLayout errorLabelLayout = new StackLayout
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            StackLayout buttonsLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Button cancelButton = new Button
            {
                Text = "Cancel",
                BackgroundColor = Color.White,
                TextColor = Color.FromHex(App.GetMenueColor()),
                FontAttributes = FontAttributes.Bold,
                WidthRequest = 150
            };
            cancelButton.Clicked += cancelButtonClicked;

            saveChangesButton = new Button
            {
                Text = "Save Changes",
                TextColor = Color.Black,
                IsEnabled = false,
                BackgroundColor = Color.LightGreen,
                FontAttributes = FontAttributes.Bold,
                WidthRequest = 150
            };
            saveChangesButton.Clicked += saveChanges;

            errorLabel = new Label
            {
                IsVisible = false,
                FontSize = 15,
                TextColor = Color.Red
            };

            buttonsLayout.Children.Add(cancelButton);
            buttonsLayout.Children.Add(saveChangesButton);

            errorLabelLayout.Children.Add(errorLabel);
            errorLabelLayout.Children.Add(buttonsLayout);

            Frame buttonsFrame = new Frame
            {
                Content = errorLabelLayout,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5
            };
            return buttonsFrame;
        }

        private void saveChanges(object sender, EventArgs e)
        {
            if (titleEntry.Text.Equals(""))
            {
                errorLabel.IsVisible = true;
                errorLabel.Text = "Please enter a name for the event!";
                return;
            }
            else if (descriptionEntry.Text.Equals(""))
            {
                errorLabel.IsVisible = true;
                errorLabel.Text = "Please enter a descripion for the event!";
                return;
            }
            else if (locationEntry.Text.Equals(""))
            {
                errorLabel.IsVisible = true;
                errorLabel.Text = "Please enter a location for the event!";
                return;
            }
            else if (datePicker.Date == null)
            {
                errorLabel.IsVisible = true;
                errorLabel.Text = "Please enter a date for the event!";
                return;
            }
            errorLabel.IsVisible = false;

            updateEventAndWriteToDatabase();

            List<Page> pages = Navigation.NavigationStack.ToList();
            // remove the old event page and the editing page
            Navigation.RemovePage(pages.ElementAt(pages.Count - 1));
            Navigation.RemovePage(pages.ElementAt(pages.Count - 2));

            Navigation.PushAsync(new SingleEventPage(ev));
        }

        private async void updateEventAndWriteToDatabase()
        {
            DateTime dt = datePicker.Date;
            string eventDate = String.Format("{0:yyyy-MM-dd}", dt);

            ev.Name = titleEntry.Text;
            ev.Datum = eventDate;
            ev.Beschreibung = descriptionEntry.Text;
            ev.Ort = locationEntry.Text;
            ev.Bild = eventImage.Source.ToString().Substring(6);

            try
            {
                await App.firebase.Child("Veranstaltungen").Child(ev.ID).PutAsync(ev);
            }
            catch (Exception)
            {
                DisplayAlert("Server Connection Failure", "Communication problems occurred while updating event!", "OK");
            }
        }
        async void OnEventImageClicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new ChooseEventPicturePopupPage(eventImage));
        }

        private void cancelButtonClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void textChangesMade(object sender, TextChangedEventArgs e)
        {
            DateTime dt = datePicker.Date;
            string eventDate = String.Format("{0:yyyy-MM-dd}", dt);
            if (!titleEntry.Text.Equals(ev.Name))
            {
                saveChangesButton.IsEnabled = true;
                return;
            }
            else if (!descriptionEntry.Text.Equals(ev.Beschreibung))
            {
                saveChangesButton.IsEnabled = true;
                return;
            }
            else if(!ev.Bild.Equals(eventImage.Source.ToString().Substring(6)))
            {
                saveChangesButton.IsEnabled = true;
                return;
            }
            else if (!locationEntry.Text.Equals(ev.Ort))
            {
                saveChangesButton.IsEnabled = true;
                return;
            }
            else if (!eventDate.Equals(ev.Datum))
            {
                saveChangesButton.IsEnabled = true;
                return;
            }
            saveChangesButton.IsEnabled = false;
        }
        private void dateChangesMade(object sender, DateChangedEventArgs e)
        {
            textChangesMade(null, null);
        }

        private void eventImageChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if(e.PropertyName.Equals("Source"))
                textChangesMade(null, null);
        }
    }
}