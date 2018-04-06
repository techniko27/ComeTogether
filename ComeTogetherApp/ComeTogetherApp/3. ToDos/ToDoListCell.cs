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
        private Event ev;
        private EventDetailsToDosPage toDosPage;

        StackLayout horizontalLayout;

        public ToDoListCell(EventDetailsToDosPage toDosPage)
        {
            this.toDosPage = toDosPage;
            ev = toDosPage.ev;

            horizontalLayout = new StackLayout();

            View = horizontalLayout;
            //View.BackgroundColor = Color.White;
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
            toDoNameLabel.FontSize = 15;
            toDoNameLabel.VerticalOptions = LayoutOptions.FillAndExpand;

            Label statusLabel = new Label
            {
                FontSize = 13,
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Start,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            statusLabel.SetBinding(Label.TextProperty, "Status");
            if (toDo.Status.Equals("Completed"))
                statusLabel.TextColor = Color.Green;
            else if (toDo.Status.Equals("In Progress"))
                statusLabel.TextColor = Color.Orange;

            horizontalLayout.Orientation = StackOrientation.Horizontal;
            horizontalLayout.VerticalOptions = LayoutOptions.CenterAndExpand;
            horizontalLayout.Padding = new Thickness(5, 0, 5, 0);
            //horizontalLayout.BackgroundColor = Color.White;

            horizontalLayout.Children.Add(toDoNameLabel);
            horizontalLayout.Children.Add(statusLabel);

            Tapped += toDoTapped;
        }

        private void toDoTapped(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(toDo.ID);
            toDosPage.Navigation.PushAsync(new ToDoDetailsPage(toDo, ev));
        }
    }
}
