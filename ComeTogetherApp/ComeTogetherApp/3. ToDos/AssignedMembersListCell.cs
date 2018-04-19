using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Firebase.Database.Query;
using System.Diagnostics.Contracts;

namespace ComeTogetherApp
{
    class AssignedMembersListCell : StackLayout
    {
        private User user;
        private Event ev;

        public AssignedMembersListCell(Event ev)
        {
            this.ev = ev;
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


            Orientation = StackOrientation.Horizontal;
            VerticalOptions = LayoutOptions.CenterAndExpand;
            Padding = new Thickness(5, 10, 0, 0);
            BackgroundColor = Color.FromHex(App.GetMenueColor());
            HeightRequest = 30;

            Children.Clear();

            Children.Add(userIcon);
            Children.Add(usernameLabel);
        }
    }
}
