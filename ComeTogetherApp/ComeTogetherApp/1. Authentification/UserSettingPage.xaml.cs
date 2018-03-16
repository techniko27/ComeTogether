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
        private Label messageLabel;
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
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(30, 0, 30, 0),
                Margin = new Thickness(10, 10, 10, 10)
                //VerticalOptions = LayoutOptions.Fill,
                //HorizontalOptions = LayoutOptions.Fill,
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
                Text = "Benutzername"
            };

            usernameEntry = new Entry()
            {
                //HeightRequest = 50,
                Margin = new Thickness(0, 0, 0, 20)
            };

            Button buttonResetPassword = new Button()
            {
                Text = "Reset Password",
                TextColor = Color.FromHex(App.GetMenueColor()),
                BackgroundColor = Color.White,
                Margin = new Thickness(0, 0, 0, 0)
            };

            messageLabel = new Label
            {
                FontSize = 20,
                TextColor = Color.Red
            };

            Button buttonSaveChanges = new Button()
            {
                Text = "Save Changes",
                TextColor = Color.FromHex(App.GetMenueColor()),
                BackgroundColor = Color.White,
                Margin = new Thickness(0, 0, 0, 0),

            };
            buttonSaveChanges.Clicked += OnButtonSaveChangesClicked;

            stack.Children.Add(usernameLabel);
            stack.Children.Add(usernameEntry);
            stack.Children.Add(messageLabel);
            stack.Children.Add(buttonResetPassword);
            stack.Children.Add(buttonSaveChanges);

            ServerGetUser();
        }

        private async void ServerGetUser()
        {
            var users = await App.firebase.Child("users").OrderByKey().StartAt(App.GetUserID()).LimitToFirst(1).OnceAsync<User>();
            User user = users.ElementAt(0).Object;
            usernameEntry.Text = user.userName;

            activityIndicatorSwitch();
        }

        async void OnButtonSaveChangesClicked(object sender, EventArgs e)
        {
            if (usernameEntry.Text == "")
            {
                messageLabel.Text = "Please enter a name for your username!";
            } 
            else 
            {
                try
                { 
                    //await firebase.Child("users").Child(userID).PutAsync(new User(usernameEntry.Text, emailEntry.Text));
                    //firebase.database().ref().update(updates);
   
                    string userID = App.GetUserID();

                    await App.firebase.Child("users").Child(userID).PutAsync(new User(usernameEntry.Text, App.GetEmail()));

                    DisplayAlert("Success", "All changes save!", "OK");
                }
                catch (Exception)
                {
                    DisplayAlert("Server connection failure", "Communication problems occured while adding event", "OK");
                }
                
   
            }

        }

        private void update()
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
