using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Firebase.Database.Query;

namespace ComeTogetherApp
{
    class AssignedCostsEntry : StackLayout
    {
        private User user;

        public AssignedCostsEntry()
        {
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext == null)
                return;
            this.user = (User)BindingContext;

            Image userIcon = new Image { Aspect = Aspect.AspectFit, VerticalOptions = LayoutOptions.Start };
            userIcon.Source = "benutzer.png";
            userIcon.AnchorX = 0;
            userIcon.AnchorY = 0;
            userIcon.Scale = 0.7;

            Label usernameLabel = new Label();
            if (!App.GetUserID().Equals(user.ID))
            {
                usernameLabel.SetBinding(Label.TextProperty, "userName");
            }
            else
            {
                usernameLabel.Text = "You";
            }
            usernameLabel.TextColor = Color.Black;
            usernameLabel.FontSize = 18;

            Image threeDots = createThreeDotsButton();

            Orientation = StackOrientation.Horizontal;
            VerticalOptions = LayoutOptions.CenterAndExpand;
            Padding = new Thickness(5, 10, 0, 0);
            BackgroundColor = Color.FromHex(App.GetMenueColor());
            HeightRequest = 30;

            Children.Add(userIcon);
            Children.Add(usernameLabel);
            Children.Add(threeDots);
        }

        private Image createThreeDotsButton()
        {
            Image threeDots = new Image { Aspect = Aspect.AspectFit, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.EndAndExpand };
            threeDots.Source = "drei_punkte_schwarz.png";
            threeDots.AnchorX = 0;
            threeDots.AnchorY = 0;
            if (Device.RuntimePlatform == Device.iOS)
            {
                threeDots.AnchorX = 0.4; //bei iOS 0.5
                threeDots.AnchorY = 0.4; //bei iOS 0.5
            }
            threeDots.Scale = 0.7;

            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += threeDotsTapped;

            threeDots.GestureRecognizers.Add(tapGestureRecognizer);

            return threeDots;
        }

        private void threeDotsTapped(object sender, EventArgs e)
        {
            
        }
    }
}
