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
    class ToDoListCell : StackLayout
    {
        private ToDo toDo;
        private Event ev;
        private EventDetailsToDosPage toDosPage;

        public ToDoListCell(EventDetailsToDosPage toDosPage)
        {
            this.toDosPage = toDosPage;
            ev = toDosPage.ev;
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
            toDoNameLabel.VerticalOptions = LayoutOptions.CenterAndExpand;

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

            Orientation = StackOrientation.Horizontal;
            VerticalOptions = LayoutOptions.CenterAndExpand;
            Padding = new Thickness(5, 0, 5, 0);
            BackgroundColor = Color.White;
            HeightRequest = 40;

            Children.Add(toDoNameLabel);
            Children.Add(statusLabel);

            var tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += toDoTapped;

            GestureRecognizers.Add(tapGestureRecognizer);
        }


        private async void toDoTapped(object sender, EventArgs e)
        {
            var assignedUserQuery = await App.firebase.Child("users").OrderByKey().StartAt(toDo.OrganisatorID).LimitToFirst(1).OnceAsync<User>();
            User assignedUser = assignedUserQuery.ElementAt(0).Object;
            assignedUser.ID = toDo.OrganisatorID;

            await toDosPage.Navigation.PushAsync(new ToDoDetailsPage(toDo, ev, assignedUser));
        }
    }
}
