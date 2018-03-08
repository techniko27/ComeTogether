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
            Image image = new Image { Aspect = Aspect.AspectFit, VerticalOptions = LayoutOptions.Start };
            image.Source = "benutzer_28x28.png";
            StackLayout cellWrapper = new StackLayout();
            StackLayout horizontalLayout = new StackLayout();
            Label usernameLabel = new Label();

            usernameLabel.SetBinding(Label.TextProperty, "userName");

            horizontalLayout.Orientation = StackOrientation.Horizontal;
            horizontalLayout.VerticalOptions = LayoutOptions.CenterAndExpand;
            horizontalLayout.Padding = new Thickness(5, 10, 0, 0);
            usernameLabel.TextColor = Color.Black;
            usernameLabel.FontSize = 15;

            horizontalLayout.Children.Add(image);
            horizontalLayout.Children.Add(usernameLabel);
            cellWrapper.Children.Add(horizontalLayout);
            View = cellWrapper;
            View.BackgroundColor = Color.FromHex(App.GetMenueColor());
        }
    }
}
