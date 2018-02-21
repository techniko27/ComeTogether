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
    public partial class EventsPage : ContentPage
    {
        Label resultsLabel;
        SearchBar searchBar;

        private ScrollView scroll;
        private StackLayout stack;
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
                SearchCommand = new Command(() => { resultsLabel.Text = "Result: " + searchBar.Text + " is what you asked for."; })
            };

            scroll= new ScrollView();

            stack = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(10, Device.OnPlatform(20, 0, 0), 10, 5)
            };
            stack.Children.Add(searchBar);
            stack.Children.Add(resultsLabel);
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
            Content = scroll;
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