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
        Cost cost;
        StackLayout horizontalLayout;

        public AssignedCostsListCell()
        {
            horizontalLayout = new StackLayout();

            View = horizontalLayout;
            View.VerticalOptions = LayoutOptions.FillAndExpand;
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext == null)
                return;
            this.cost = (Cost)BindingContext;

            Label descriptionLabel = new Label();
            descriptionLabel.SetBinding(Label.TextProperty, "Description");
            descriptionLabel.TextColor = Color.Black;
            descriptionLabel.FontSize = 18;
            descriptionLabel.VerticalOptions = LayoutOptions.FillAndExpand;

            Label costLabel = new Label();
            costLabel.Text = $"{cost.Costs}€";
            costLabel.TextColor = Color.OrangeRed;
            costLabel.FontSize = 20;
            costLabel.HorizontalOptions = LayoutOptions.EndAndExpand;

            horizontalLayout.Orientation = StackOrientation.Horizontal;
            horizontalLayout.VerticalOptions = LayoutOptions.StartAndExpand;
            horizontalLayout.Padding = new Thickness(5, 10, 0, 0);
            horizontalLayout.BackgroundColor = Color.FromHex(App.GetMenueColor());

            horizontalLayout.Children.Add(descriptionLabel);
            horizontalLayout.Children.Add(costLabel);
        }
    }
}
