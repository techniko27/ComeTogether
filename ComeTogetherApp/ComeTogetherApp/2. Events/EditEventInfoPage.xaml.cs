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
    public partial class EditEventInfoPage : ContentPage
    {
        private Event ev;

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

            return stackLayout;
        }

        private Frame createEventTitleFrame()
        {
            StackLayout eventTitleLayout = new StackLayout();

            Label eventTitleLabel = new Label
            {
                Text = "Title:",
                TextColor = Color.White,
                FontSize = 20,
                Margin = new Thickness(5, 0, 0, 0),
            };

            Entry titleEntry = new Entry
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

            Editor descriptionEntry = new Editor
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

            Frame eventTitleFrame = new Frame
            {
                Content = eventIconLayout,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
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

            Entry locationEntry = new Entry
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

            DatePicker datePicker = new DatePicker
            {
                Format = "D",
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

            Button saveChangesButton = new Button
            {
                Text = "Save Changes",
                TextColor = Color.Black,
                BackgroundColor = Color.LightGreen,
                FontAttributes = FontAttributes.Bold,
                WidthRequest = 150
            };

            buttonsLayout.Children.Add(cancelButton);
            buttonsLayout.Children.Add(saveChangesButton);

            Frame buttonsFrame = new Frame
            {
                Content = buttonsLayout,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5
            };
            return buttonsFrame;
        }
    }
}