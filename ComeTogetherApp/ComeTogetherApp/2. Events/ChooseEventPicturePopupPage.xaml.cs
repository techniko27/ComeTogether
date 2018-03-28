using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComeTogetherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChooseEventPicturePopupPage : PopupPage
    {
        private Image parrentImage;
        public ChooseEventPicturePopupPage(Image parrentImage)
        {
            InitializeComponent();

            this.parrentImage = parrentImage;

            StackLayout stack = new StackLayout
            {
                //BackgroundColor = Color.FromHex(App.GetMenueColor()),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                //Padding = new Thickness(100, 100, 100, 100),
                //Margin = new Thickness(50, 50, 50, 50),
                Padding = 1,
                HeightRequest = 400,
                WidthRequest = 240
            };

            Grid grid = new Grid()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                ColumnSpacing = 1,
                RowSpacing = 1
            };


            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100, GridUnitType.Absolute) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120, GridUnitType.Absolute) });      //Width of the Colums not implemented because of scree rotation issues
            Image event_default = new Image()
            {
                Aspect = Aspect.AspectFit,
                //Scale = 0.9,
                Source = "event_default.png"
            };
            var event_defaultTapGestureRecognizer = new TapGestureRecognizer();
            event_defaultTapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
            {
                // handle the tap
                OnEventImageClicked(sender, e, event_default.Source.ToString());
            };  
            event_default.GestureRecognizers.Add(event_defaultTapGestureRecognizer);
            grid.Children.Add(event_default, 0, 0);

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120, GridUnitType.Absolute) });      //Width of the Colums not implemented because of scree rotation issues
            Image event_essen = new Image()
            {
                Aspect = Aspect.AspectFit,
                //Scale = 0.9,
                Source = "event_essen.png"
            };
            grid.Children.Add(event_essen, 1, 0);
            var event_essenTapGestureRecognizer = new TapGestureRecognizer();
            event_essenTapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
            {
                // handle the tap
                OnEventImageClicked(sender, e, event_essen.Source.ToString());
            };
            event_essen.GestureRecognizers.Add(event_essenTapGestureRecognizer);


            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100, GridUnitType.Absolute) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120, GridUnitType.Absolute) });      //Width of the Colums not implemented because of scree rotation issues
            Image event_hochzeit = new Image()
            {
                Aspect = Aspect.AspectFit,
                //Scale = 0.9,
                Source = "event_hochzeit.png"
            };
            grid.Children.Add(event_hochzeit, 0, 1);
            var event_hochzeitTapGestureRecognizer = new TapGestureRecognizer();
            event_hochzeitTapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
            {
                // handle the tap
                OnEventImageClicked(sender, e, event_hochzeit.Source.ToString());
            };
            event_hochzeit.GestureRecognizers.Add(event_hochzeitTapGestureRecognizer);

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120, GridUnitType.Absolute) });      //Width of the Colums not implemented because of scree rotation issues
            Image event_silvester = new Image()
            {
                Aspect = Aspect.AspectFit,
                //Scale = 0.9,
                Source = "event_silvester.png"
            };
            grid.Children.Add(event_silvester, 1, 1);
            var event_silvesterTapGestureRecognizer = new TapGestureRecognizer();
            event_silvesterTapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
            {
                // handle the tap
                OnEventImageClicked(sender, e, event_silvester.Source.ToString());
            };
            event_silvester.GestureRecognizers.Add(event_silvesterTapGestureRecognizer);


            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100, GridUnitType.Absolute) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120, GridUnitType.Absolute) });      //Width of the Colums not implemented because of scree rotation issues
            Image event_urlaub = new Image()
            {
                Aspect = Aspect.AspectFit,
                //Scale = 0.9,
                Source = "event_urlaub.png"
            };
            grid.Children.Add(event_urlaub, 0, 2);
            var event_urlaubTapGestureRecognizer = new TapGestureRecognizer();
            event_urlaubTapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
            {
                // handle the tap
                OnEventImageClicked(sender, e, event_urlaub.Source.ToString());
            };
            event_urlaub.GestureRecognizers.Add(event_urlaubTapGestureRecognizer);


            stack.Children.Add(grid);
            Content = stack;
        }
        async void OnEventImageClicked(object sender, EventArgs e, string imageString)
        {
            parrentImage.Source = imageString.Substring(6);
            await Navigation.PopPopupAsync();
        }
        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }
    }
}