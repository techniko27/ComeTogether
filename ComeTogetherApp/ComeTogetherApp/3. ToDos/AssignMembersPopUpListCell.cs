﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace ComeTogetherApp
{
    class AssignMembersPopUpListCell : ViewCell
    {
        private User user;
        private Event ev;
        private AssignMemberToDoPopUp parentPopUp;

        StackLayout horizontalLayout;

        public AssignMembersPopUpListCell(Event ev, AssignMemberToDoPopUp parentPopUp)
        {
            this.ev = ev;
            this.parentPopUp = parentPopUp;

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

            horizontalLayout.Orientation = StackOrientation.Horizontal;
            horizontalLayout.VerticalOptions = LayoutOptions.CenterAndExpand;
            horizontalLayout.Padding = new Thickness(5, 10, 0, 0);

            horizontalLayout.Children.Add(userIcon);
            horizontalLayout.Children.Add(usernameLabel);

            Tapped += handleTap;
        }

        private async void handleTap(object sender, EventArgs e)
        {
            parentPopUp.assignMemberToToDo(user);
            await parentPopUp.Navigation.PopPopupAsync();
        }
    }
}
