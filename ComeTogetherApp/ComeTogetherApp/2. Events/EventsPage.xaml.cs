using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComeTogetherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventsPage : ContentPage
    {
        Label resultsLabel;
        SearchBar searchBar;

        private ScrollView scroll;
        private StackLayout stack;
        private StackLayout stackButton;
        private Grid grid;
        private ActivityIndicator activityIndicator;

        public EventsPage()
        {
            InitializeComponent();

            resultsLabel = new Label
            {
                Text = "Result will appear here.",
                VerticalOptions = LayoutOptions.FillAndExpand,
                FontSize = 25
            };

            searchBar = new SearchBar
            {
                Placeholder = "Enter event to search",
                SearchCommand =
                    new Command(() => { resultsLabel.Text = "Result: " + searchBar.Text + " is what you asked for."; })
            };

            scroll = new ScrollView();

            stack = new StackLayout
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Padding = new Thickness(2, 2, 2, 2)
            };
            stack.Children.Add(searchBar);
            stack.Children.Add(resultsLabel);

            scroll.Content = stack;

            grid = new Grid()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                ColumnSpacing = 6,
                RowSpacing = 6
            };
            stack.Children.Add(grid);

            /*
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(250, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(250, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150, GridUnitType.Star) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150, GridUnitType.Star) });

            var topLeft = new Button() { Text = "Top Left" };
            var topRight = new Button() { Text = "Top Right" };
            var bottomLeft = new Button() { Text = "Bottom Left" };
            var bottomRight = new Button() { Text = "Bottom Right" };

            grid.Children.Add(topLeft, 0, 0);
            grid.Children.Add(topRight, 0, 1);
            grid.Children.Add(bottomLeft, 1, 0);
            grid.Children.Add(bottomRight, 1, 1);
            */
            
            for (int i = 0; i < 4; i++)
            {
                //grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(150, GridUnitType.Star) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(230, GridUnitType.Star) });

                for (int j = 0; j < 2; j++)
                {
                    var eventImage = new Image { Aspect = Aspect.AspectFit };
                    eventImage.Source = "256x256_in_app.png";
                    Label eventNameLabel = new Label
                    {
                        Text = "Eventname",
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        FontSize = 15
                    };
                    Label eventMembercountLabel = new Label
                    {
                        Text = "EventMemberCount",
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        FontSize = 15
                    };
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
                    {
                        // handle the tap
                        OnEventClicked(sender, e);
                    };
                    stackButton = new StackLayout
                    {
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Padding = new Thickness(2, 2, 2, 2),
                        BackgroundColor = Color.FromHex("41BAC1")
                        
                    };
                    stackButton.GestureRecognizers.Add(tapGestureRecognizer);
                    stackButton.Children.Add(eventImage);
                    stackButton.Children.Add(eventNameLabel);
                    stackButton.Children.Add(eventMembercountLabel);

                    grid.Children.Add(stackButton, j, i);
                }
            }

            activityIndicator = new ActivityIndicator()
            {
                Color = Color.Gray,
                IsRunning = true,
                WidthRequest = 80,
                HeightRequest = 80,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            Content = activityIndicator;

            ServerGetEvents();
        }

        private async void ServerGetEvents()
        {
            try
            {
                var events = await App.firebase.Child("Veranstaltungen").OrderByKey().StartAt("1").OnceAsync<Event>();
                //await firebase.Child("Veranstaltungen").Child("6").PutAsync(new Event("lala", "lolo", "lili", "lsls"));
                
                foreach (var e in events)
                {
                    System.Diagnostics.Debug.WriteLine($"{e.Key} is {e.Object.Name}");
                    resultsLabel.Text = e.Key;
                }

            }
            catch (Exception e)
            {
                await DisplayAlert("Server connection failure", "Communication problems occured while querying", "OK");
                System.Diagnostics.Debug.WriteLine(e);
            }

            activityIndicatorSwitch();
        }

        void OnEventClicked(object sender, EventArgs e)
        {
            DisplayAlert("Event", "Klicked", "OK");
        }

        private void activityIndicatorSwitch()
        {
            if (activityIndicator.IsRunning)
            {
                activityIndicator.IsRunning = false;

                Content = scroll;
            }
            else
            {
                activityIndicator.IsRunning = true;

                Content = activityIndicator;
            }
        }
    }
}