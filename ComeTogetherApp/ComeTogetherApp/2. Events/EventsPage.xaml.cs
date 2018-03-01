﻿using System;
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

namespace ComeTogetherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventsPage : ContentPage
    {
        SearchBar searchBar;

        private ScrollView scroll;
        private StackLayout stack;
        private Grid grid;
        private ActivityIndicator activityIndicator;
        private List<Event> eventList;

        public EventsPage()
        {
            InitializeComponent();

            eventList = new List<Event>();
            eventList.Add(new Event("","","Add new Event","", "kreis_plus_schwarz.png", "0"));

            searchBar = new SearchBar
            {
                Placeholder = "Enter event to search",
                //SearchCommand = new Command(() => createSearchList(searchBar.Text)),
                
            };
            searchBar.TextChanged += SearchBar_TextChanged;


            scroll = new ScrollView();

            stack = new StackLayout
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Padding = new Thickness(2, 2, 2, 2)
            };
            stack.Children.Add(searchBar);

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

        private async void ServerGetEvents()
        {
            try
            {
                var events = await App.firebase.Child("Veranstaltungen").OrderByKey().StartAt("1").OnceAsync<Event>();
                //await firebase.Child("Veranstaltungen").Child("6").PutAsync(new Event("lala", "lolo", "lili", "lsls"));
                
                foreach (FirebaseObject<Event> e in events)
                {
                    System.Diagnostics.Debug.WriteLine($"{e.Key} is {e.Object.Name}");
                    e.Object.ID = e.Key;
                    eventList.Add(e.Object);
                }
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

        private void buildGrid(List<Event> gridList)
        {
            grid = new Grid()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                ColumnSpacing = 6,
                RowSpacing = 6
            };
            stack.Children.Add(grid);                                                       //add new grid

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

            int c = 0;                                  //List Counter
            int r = gridList.Count % 2;                 //List is odd (ungerade)
            for (int i = 0; i < (gridList.Count/2+r); i++)
            {
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(260, GridUnitType.Absolute) });

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

                    grid.Children.Add(eventButton, j, i);
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