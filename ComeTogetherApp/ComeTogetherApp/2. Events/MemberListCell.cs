using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComeTogetherApp
{
    public class MemberListCell : ViewCell
    {
        public MemberListCell()
        {
            StackLayout horizontalLayout = new StackLayout();

            Image userIcon = new Image { Aspect = Aspect.AspectFit, VerticalOptions = LayoutOptions.Start };
            userIcon.Source = "benutzer.png";
            userIcon.AnchorX = 0;
            userIcon.AnchorY = 0;
            userIcon.Scale = 0.7;

            Label usernameLabel = new Label();
            usernameLabel.SetBinding(Label.TextProperty, "userName");
            usernameLabel.TextColor = Color.Black;
            usernameLabel.FontSize = 18;

            Image threeDots = new Image { Aspect = Aspect.AspectFit, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.EndAndExpand };
            threeDots.Source = "drei_punkte_schwarz.png";
            threeDots.AnchorX = 0;
            threeDots.AnchorY = 0;
            threeDots.Scale = 0.7;

            horizontalLayout.Orientation = StackOrientation.Horizontal;
            horizontalLayout.VerticalOptions = LayoutOptions.CenterAndExpand;
            horizontalLayout.Padding = new Thickness(5, 10, 0, 0);

            horizontalLayout.Children.Add(userIcon);
            horizontalLayout.Children.Add(usernameLabel);
            horizontalLayout.Children.Add(threeDots);
            View = horizontalLayout;
            View.BackgroundColor = Color.FromHex(App.GetMenueColor());
        }
    }
}
