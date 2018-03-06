using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComeTogetherApp
{
    public partial class UserSettingPage : ContentPage
    {
        private ActivityIndicator activityIndicator;
        private ScrollView scroll;
        private StackLayout stack;
        Entry usernameEntry;

        public UserSettingPage()
        {
            InitializeComponent();
            // scrollView.BackgroundColor = Color.FromHex(App.GetMenueColor());
            // buttonSaveChanges.TextColor = Color.FromHex(App.GetMenueColor());
            // buttonSaveChanges.BackgroundColor = Color.White;
            // buttonSaveChanges.Margin = new Thickness(50, 0, 50, 0);


            scroll = new ScrollView
            {
                BackgroundColor = Color.FromHex(App.GetMenueColor())
            };

            stack = new StackLayout
            {
                VerticalOptions = LayoutOptions.Fill,
                HorizontalOptions = LayoutOptions.Fill,
                //Padding = new Thickness(2, 2, 2, 2)
            };
            scroll.Content = stack;


            activityIndicator = new ActivityIndicator()
            {
                Color = Color.Gray,
                IsRunning = true,
                WidthRequest = 80,
                HeightRequest = 80,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            Content = activityIndicator;

            Label usernameLabel = new Label()
            {
                Text ="Benutzername",
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            usernameEntry = new Entry()
            {
                HeightRequest = 50,
                VerticalOptions = LayoutOptions.StartAndExpand
            };

            Button buttonResetPassword = new Button()
            {
                Text = "Reset Password",
                TextColor = Color.FromHex(App.GetMenueColor()),
                VerticalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.White,
                //Margin = new Thickness(50, 20, 50, 0)
            };

            Button buttonSaveChanges = new Button()
            {
                Text = "Save Changes",
                TextColor = Color.FromHex(App.GetMenueColor()),
                VerticalOptions = LayoutOptions.StartAndExpand,
                BackgroundColor = Color.White,
                //Margin = new Thickness(50, 20, 50, 0)
            };

            stack.Children.Add(usernameLabel);
            stack.Children.Add(usernameEntry);
            stack.Children.Add(buttonResetPassword);
            stack.Children.Add(buttonSaveChanges);

            ServerGetUser();
        }

        //usernameEntry.Placeholder = await App.firebase.Child("Veranstaltungen").OrderByKey().StartAt("1").OnceAsync<Event>();
        private async void ServerGetUser() 
        {
            var users = await App.firebase.Child("users").OrderByKey().StartAt(App.GetUserID()).LimitToFirst(1).OnceAsync<User>();
            User user = users.ElementAt(0).Object;
            usernameEntry.Text = user.userName;

            activityIndicatorSwitch();
        }

        void saveChanges(object sender, System.EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void activityIndicatorSwitch()
        {
            if (activityIndicator.IsRunning)
            {
                activityIndicator.IsRunning = false;

                Content = scroll;
            }
            else
            {
                activityIndicator.IsRunning = true;

                Content = activityIndicator;
            }
        }
    }
}
