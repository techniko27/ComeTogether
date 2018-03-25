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
    class ToDoListCell : ViewCell
    {
        private ToDo toDo;

        StackLayout horizontalLayout;

        public ToDoListCell()
        {
            horizontalLayout = new StackLayout();

            View = horizontalLayout;
            View.BackgroundColor = Color.FromHex(App.GetMenueColor());
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            if (BindingContext == null)
                return;
            this.toDo = (ToDo) BindingContext;

            Label toDoNameLabel = new Label();
            toDoNameLabel.SetBinding(Label.TextProperty, "Name");
            toDoNameLabel.TextColor = Color.Black;
            toDoNameLabel.FontSize = 18;

            Label statusLabel = new Label
            {
                FontSize = 12,
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Start
            };
            statusLabel.SetBinding(Label.TextProperty, "Status");
            if (toDo.Status.Equals("Done"))
                statusLabel.TextColor = Color.Green;

            horizontalLayout.Orientation = StackOrientation.Horizontal;
            horizontalLayout.VerticalOptions = LayoutOptions.CenterAndExpand;
            horizontalLayout.Padding = new Thickness(5, 10, 0, 0);

            horizontalLayout.Children.Add(toDoNameLabel);
            horizontalLayout.Children.Add(statusLabel);
        }
    }
}
