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
    public class MemberListCell : ViewCell
    {
        private EventDetailsMembersPage memberPage;
        private User user;
        private Event ev;

        StackLayout horizontalLayout;

        public MemberListCell(Event ev, EventDetailsMembersPage memberPage)
        {
            this.memberPage = memberPage;
            this.ev = ev;

            horizontalLayout = new StackLayout();

            View = horizontalLayout;
            View.BackgroundColor = Color.FromHex(App.GetMenueColor());
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            this.user = (User) BindingContext;

            Image userIcon = new Image { Aspect = Aspect.AspectFit, VerticalOptions = LayoutOptions.Start };
            userIcon.Source = "benutzer.png";
            userIcon.AnchorX = 0;
            userIcon.AnchorY = 0;
            userIcon.Scale = 0.7;

            Label usernameLabel = new Label();
            if(!App.GetUserID().Equals(user.ID))
            {
                usernameLabel.SetBinding(Label.TextProperty, "userName");
            } else
            {
                usernameLabel.Text = "You";
            }
            usernameLabel.TextColor = Color.Black;
            usernameLabel.FontSize = 18;

            Label administratorLabel = new Label
            {
                Text = "(Administrator)",
                FontSize = 12,
                TextColor = Color.Gray,
                VerticalOptions = LayoutOptions.Center,
                VerticalTextAlignment = TextAlignment.Start
            };

            Image threeDots = createThreeDotsButton();

            horizontalLayout.Orientation = StackOrientation.Horizontal;
            horizontalLayout.VerticalOptions = LayoutOptions.CenterAndExpand;
            horizontalLayout.Padding = new Thickness(5, 10, 0, 0);

            horizontalLayout.Children.Add(userIcon);
            horizontalLayout.Children.Add(usernameLabel);
            if(ev.adminID.Equals(user.ID))
            {
                horizontalLayout.Children.Add(administratorLabel);
            } 
            // add option button (three dots) to all users if the current user is the admin
            // add it also for the current user in the list so they can leave if they want to
            if (App.GetUserID().Equals(ev.adminID) || App.GetUserID().Equals(user.ID))
            {
                horizontalLayout.Children.Add(threeDots);
            }
        }


        private Image createThreeDotsButton()
        {
            Contract.Ensures(Contract.Result<Image>() != null);
            Image threeDots = new Image { Aspect = Aspect.AspectFit, VerticalOptions = LayoutOptions.Center, HorizontalOptions = LayoutOptions.EndAndExpand };
            threeDots.Source = "drei_punkte_schwarz.png";
            threeDots.AnchorX = 0; //bei iOS 0.5
            threeDots.AnchorY = 0; //bei iOS 0.5
            threeDots.Scale = 0.7;

            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += threeDotsTappedEvent;

            threeDots.GestureRecognizers.Add(tapGestureRecognizer);

            return threeDots;
        }

        private async void threeDotsTappedEvent(object sender, EventArgs e)
        {
            string action;
            if (!App.GetUserID().Equals(user.ID))
            {
                action = await memberPage.DisplayActionSheet("Member Options", "Cancel", null, "Kick Member");
            } else
            {
                action = await memberPage.DisplayActionSheet("Member Options", "Cancel", null, "Leave Event");
            }
            switch (action)
            {
                case "Kick Member":
                    bool answer = await memberPage.DisplayAlert("Are you sure?", $"Do you really want to kick {user.userName}?", "Yes", "No");
                    if(answer)
                    {
                        removeUserFromEvent();
                        memberPage.eventMemberList.Remove(user);
                    }
                    break;
                case "Leave Event":
                    if (!App.GetUserID().Equals(ev.adminID))
                        answer = await memberPage.DisplayAlert("Are you sure?", $"Do you really want to leave this event?", "Yes", "No");
                    else
                        answer = await memberPage.DisplayAlert("Are you sure?", $"Do you really want to leave this event? As the administrator of this event, leaving it will fully delete the event.", "Yes", "No");
                    if (answer)
                    {
                        removeUserFromEvent();
                        memberPage.Navigation.PushAsync(new EventsPage());
                    }
                    break;
                default:
                    break;
            }
        }

        private async void removeUserFromEvent()
        {
            string eventID = ev.ID;

            // if the admin is leaving the event, dissolve the whole event
            if (App.GetUserID().Equals(ev.adminID))
            {
                foreach(User u in memberPage.eventMemberList)
                {
                    await App.firebase.Child("Benutzer_Veranstaltung").Child(u.ID).Child(eventID).DeleteAsync();
                }

                await App.firebase.Child("Veranstaltung_Benutzer").Child(eventID).DeleteAsync();
                await App.firebase.Child("Veranstaltungen").Child(eventID).DeleteAsync();

                return;
            }

            await App.firebase.Child("Benutzer_Veranstaltung").Child(App.GetUserID()).Child(eventID).DeleteAsync();
            await App.firebase.Child("Veranstaltung_Benutzer").Child(eventID).Child(App.GetUserID()).DeleteAsync();
        }
    }
}
