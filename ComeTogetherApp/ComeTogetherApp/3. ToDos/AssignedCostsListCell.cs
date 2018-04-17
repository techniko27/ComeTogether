using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Firebase.Database.Query;

namespace ComeTogetherApp
{
    class AssignedCostsListCell : ViewCell
    {
        private User user;
        private Event ev;

        StackLayout horizontalLayout;

        public AssignedCostsListCell(Event ev)
        {
            this.ev = ev;

            horizontalLayout = new StackLayout();

            View = horizontalLayout;
            View.BackgroundColor = Color.FromHex(App.GetMenueColor());
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

            horizontalLayout.Orientation = StackOrientation.Horizontal;
            horizontalLayout.VerticalOptions = LayoutOptions.CenterAndExpand;
            horizontalLayout.Padding = new Thickness(5, 10, 0, 0);

            horizontalLayout.Children.Add(userIcon);
            horizontalLayout.Children.Add(usernameLabel);
            // add option button (three dots) to all users if the current user is the admin
            // add it also for the current user in the list so they can leave if they want to
            if (App.GetUserID().Equals(ev.adminID) || App.GetUserID().Equals(user.ID))
            {
                horizontalLayout.Children.Add(threeDots);
            }
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
            //tapGestureRecognizer.Tapped += threeDotsTappedEvent;

            threeDots.GestureRecognizers.Add(tapGestureRecognizer);

            return threeDots;
        }
    }
}
