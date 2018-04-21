using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup.Extensions;
using ZXing.Mobile;
using ZXing.Net.Mobile.Forms;

namespace ComeTogetherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventsPage : ContentPage
    {
        SearchBar searchBar;

        private ScrollView scroll;
        public StackLayout stack;
        private Grid grid;
        private ActivityIndicator activityIndicator;
        public List<Event> eventList;

        public EventsPage()
        {
            InitializeComponent();

            Title = "Events";

            eventList = new List<Event>();
            //eventList.Add(new Event("","","Add New Event","", "kreis_plus_schwarz.png", "0", "null",""));

            searchBar = new SearchBar
            {
                Placeholder = "Search events...",
                Margin = new Thickness(0, 0, 10, 0),
                WidthRequest = 300

            };
            searchBar.TextChanged += SearchBar_TextChanged;

            Frame addNewEventButton = createAddEventButton();

            scroll = new ScrollView();

            stack = new StackLayout
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Padding = new Thickness(2, 2, 2, 2)
            };

            StackLayout topHorizontalLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal
            };

            topHorizontalLayout.Children.Add(searchBar);
            topHorizontalLayout.Children.Add(addNewEventButton);

            stack.Children.Add(topHorizontalLayout);

            scroll.Content = stack;

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

        private Frame createAddEventButton()
        {
            Image plusImage = new Image
            {
                Aspect = Aspect.AspectFit,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            plusImage.Source = "kreis_plus_weiss.png";


            Frame addEventButtonFrame = new Frame
            {
                Content = plusImage,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                HorizontalOptions = LayoutOptions.EndAndExpand,
                CornerRadius = 5,
                Padding = 7,
                WidthRequest = 40,
                HeightRequest = 30,
            };


            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += addEventButtonTapped;

            addEventButtonFrame.GestureRecognizers.Add(tapGestureRecognizer);

            return addEventButtonFrame;
        }

        private async void addEventButtonTapped(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("", "Cancel", null, "Add New Event", "Enter Joincode", "Scan Joincode");
            Debug.WriteLine("Action: " + action);
            switch (action)
            {
                case "Add New Event":
                    await Navigation.PushAsync(new AddNewEventPage(this));
                    break;
                case "Enter Joincode":
                    await Navigation.PushPopupAsync(new EnterJoinCodePopupPage(this));
                    break;
                case "Scan Joincode":
                    useScanPage();
                    break;
                default:

                    break;
            }
        }

        private async void ServerGetEvents()
        {
            try
            {

                var benutzer_Events = await App.firebase.Child("Benutzer_Veranstaltung").Child(App.GetUserID).OnceAsync<string>();
                
                foreach (var eventID in benutzer_Events)
                {
                    var ev = await App.firebase.Child("Veranstaltungen").OrderByKey().StartAt(eventID.Key).LimitToFirst(1).OnceAsync<Event>();
                    eventList.Add(ev.ElementAt(0).Object);
                }

                /*
                var events = await App.firebase.Child("Veranstaltungen").OrderByKey().StartAt("1").OnceAsync<Event>();
                //await firebase.Child("Veranstaltungen").Child("6").PutAsync(new Event("lala", "lolo", "lili", "lsls"));
                
                foreach (FirebaseObject<Event> e in events)
                {
                    System.Diagnostics.Debug.WriteLine($"{e.Key} is {e.Object.Name}");
                    e.Object.ID = e.Key;
                    eventList.Add(e.Object);
                }
                */

                eventList = eventList.OrderBy(o => o.Datum).ToList();       //Order List by Date
                buildGrid(eventList);                                       //build Grid on Eventpage with data from Server List
            }
            catch (Exception e)
            {
                await DisplayAlert("Server connection failure", "Communication problems occured while querying", "OK");
                System.Diagnostics.Debug.WriteLine(e);
            }

            activityIndicatorSwitch();
        }

        public void buildGrid(List<Event> gridList)
        {
            grid = new Grid()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                ColumnSpacing = 6,
                RowSpacing = 6
            };
            stack.Children.Add(grid);                                                       //add new grid

            int c = 0;                                  //List Counter
            int r = gridList.Count % 2;                 //List is odd (ungerade)
            for (int i = 0; i < (gridList.Count/2+r); i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(200, GridUnitType.Absolute) });

                for (int j = 0; j < 2; j++)
                {
                    if (gridList.Count == 1)
                    {
                        grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(170, GridUnitType.Absolute) });      //Width of the Colums not implemented because of scree rotation issues
                    }

                    if (gridList.Count <= c)
                    {
                        return;
                    }

                    EventButton eventButton = new EventButton(gridList[c], this);

                    Frame eventButtonFrame = new Frame
                    {
                        Content = eventButton,
                        BackgroundColor = Color.FromHex(App.GetMenueColor()),
                        CornerRadius = 15,
                        Padding = 0
                    };

                    grid.Children.Add(eventButtonFrame, j, i);
                    c++;
                }
            }
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            List<Event> searchList = new List<Event>();

            foreach (Event oneEvent in eventList)
            {
                if (oneEvent.Name.ToLower().Contains(searchBar.Text.ToLower()))
                {
                    searchList.Add(oneEvent);
                }
            }
            stack.Children.RemoveAt(stack.Children.IndexOf(stack.Children.Last()));         //Remove the last Grid
            buildGrid(searchList);                                                          //build new Grid
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
                DefaultOverlayTopText = "Scan the Joincode",
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
            foreach (var eventPageEvent in eventList)
            {
                //Check if event allready exist for this user
                if (eventPageEvent.ID == eventID)
                {
                    await DisplayAlert("Note", "You are already member of the event named " + eventPageEvent.Name, "OK");
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
                await DisplayAlert("Incorrect Joincode", "Event could not be found with ID " + eventID, "OK");
                return;
            }
            try
            {
                await App.firebase.Child("Veranstaltung_Benutzer").Child(eventID).Child(App.GetUserID()).PutAsync<string>(App.GetUsername());
                await App.firebase.Child("Benutzer_Veranstaltung").Child(App.GetUserID()).Child(eventID).PutAsync<string>(ev.ElementAt(0).Object.Name);

                eventList.Add(ev.ElementAt(0).Object);
                stack.Children.RemoveAt(stack.Children.IndexOf(stack.Children.Last()));         //Remove the last Grid from EventsPage
                buildGrid(eventList);

                await DisplayAlert("Success", "Event " + ev.ElementAt(0).Object.Name + " added", "OK");
            }
            catch (Exception)
            {
                await DisplayAlert("Server connection failure", "Communication problems occured while adding event", "OK");
            }
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