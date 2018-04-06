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

            List<string> pictureList = new List<string>();
            pictureList.Add("event_default.png");
            pictureList.Add("event_abschluss.png");
            pictureList.Add("event_essen.png");
            pictureList.Add("event_geburtstag.png");
            pictureList.Add("event_grillen.png");
            pictureList.Add("event_hochzeit.png");
            pictureList.Add("event_kino.png");
            pictureList.Add("event_party.png");
            pictureList.Add("event_silvester.png");
            pictureList.Add("event_sport_hantel.png");
            pictureList.Add("event_sport_fussball.png");
            pictureList.Add("event_theater.png");
            pictureList.Add("event_urlaub.png");
            pictureList.Add("event_zelten.png");

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

            int counter = 0;
            int r = pictureList.Count % 2;                 //List is odd (ungerade)
            for (int i = 0; i < (pictureList.Count / 2 + r); i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(120, GridUnitType.Absolute) });      //Width of the Colums not implemented because of scree rotation issues

                for (int j = 0; j < 2; j++)
                {
                    if (pictureList.Count == 1)
                    {
                        grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(100, GridUnitType.Absolute) });
                    }

                    if (pictureList.Count <= counter)
                    {
                        break;
                    }

                    Image eventImage = new Image()
                    {
                        Aspect = Aspect.AspectFit,
                        //Scale = 0.9,
                        Source = pictureList[counter]
                    };
                    var eventImageTapGestureRecognizer = new TapGestureRecognizer();
                    eventImageTapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
                    {
                        // handle the tap
                        OnEventImageClicked(sender, e, eventImage.Source.ToString());
                    };
                    eventImage.GestureRecognizers.Add(eventImageTapGestureRecognizer);
                    grid.Children.Add(eventImage, j, i);

                    counter++;
                }
            }

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