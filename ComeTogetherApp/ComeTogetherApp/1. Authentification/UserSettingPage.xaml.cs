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
        Entry paypalMeEntry;

        public UserSettingPage()
        {
            InitializeComponent();

            Title = "Account";

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

            Label paypalMeLabel = new Label()
            {
                Text = "Paypal.me Link"
            };

            paypalMeEntry = new Entry()
            { 
                Margin = new Thickness(0, 0, 0, 20)
            };

            Button buttonResetPassword = new Button()
            {
                Text = "Reset Password",
                TextColor = Color.FromHex(App.GetMenueColor()),
                BackgroundColor = Color.White,
                Margin = new Thickness(0, 60, 0, 0)
            };
            buttonResetPassword.Clicked += OnButtonResetPasswordClicked;

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

            Button buttonDeleteAccount = new Button()
            {
                Text = "Delete Account",
                TextColor = Color.White,
                BackgroundColor = Color.Red,
                Margin = new Thickness(0, 0, 0, 0),

            };
            buttonDeleteAccount.Clicked += OnButtonDeleteAccount;

            stack.Children.Add(usernameLabel);
            stack.Children.Add(usernameEntry);
            stack.Children.Add(paypalMeLabel);
            stack.Children.Add(paypalMeEntry);
            stack.Children.Add(messageLabel);
            stack.Children.Add(buttonSaveChanges);
            stack.Children.Add(buttonResetPassword);
            stack.Children.Add(buttonDeleteAccount);

            ServerGetUser();
        }

        private async void OnButtonDeleteAccount(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Delete Account", "Do you want delete your account?", "Yes", "No");

            if(answer == true)
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(App.GetFirebaseApiKey()));

                try
                {
                    await authProvider.DeleteUser(await App.firebaseClientRefresh());

                    DisplayAlert("Sucess", "Your account is going to delete!", "Ok");

                    Application.Current.Properties["IsUserLoggedIn"] = false;
                    Device.BeginInvokeOnMainThread(() => App.LogInSwitch());

                }
                catch (FirebaseAuthException exception)
                {
                   
                }
            }
        }

        private async void ServerGetUser()
        {
            var users = await App.firebase.Child("users").OrderByKey().StartAt(App.GetUserID()).LimitToFirst(1).OnceAsync<User>();
            User user = users.ElementAt(0).Object;
            usernameEntry.Text = user.userName;
            paypalMeEntry.Text = user.PayPal_me_link;

            activityIndicatorSwitch();
        }

        async void OnButtonSaveChangesClicked(object sender, EventArgs e)
        {
            if (usernameEntry.Text == "")
            {
                messageLabel.Text = "Please enter a name for your username!";
                return;
            }
            else if (paypalMeEntry.Text != "" && paypalMeEntry.Text != null)
            {
                string paypalMeLinkString = paypalMeEntry.Text.ToLower();

                if (paypalMeLinkString.Contains("https://"))
                {
                    paypalMeLinkString = paypalMeLinkString.Substring(8);
                }

                string[] test = paypalMeLinkString.Split('/');


                if(!paypalMeLinkString.Contains("paypal.me/") || paypalMeLinkString.Length < 10 || (test.Length >= 3 && test[2] != ""))
                {
                    messageLabel.Text = "Your PayPal.Me Link is not correct!";
                    return;
                }
            }

            string paypalMeLink = paypalMeEntry.Text.Replace(" ", "");
            if (paypalMeLink.EndsWith("/"))
            {
                paypalMeLink = paypalMeLink.Remove(paypalMeLink.Length - 1);
            }

            activityIndicatorSwitch();
            try
            { 
                string userID = App.GetUserID();

                await App.firebase.Child("users").Child(userID).PutAsync(new User(usernameEntry.Text, App.GetEmail(), paypalMeLink));

                DisplayAlert("Success", "All changes saved!", "OK");
            }
            catch (Exception)
            {
                DisplayAlert("Server connection failure", "Communication problems occured while adding event", "OK");
            }
            activityIndicatorSwitch();
        }

        async void OnButtonResetPasswordClicked(object sender, EventArgs e)
        {
            //Login Request
            var authProvider = new FirebaseAuthProvider(new FirebaseConfig(App.GetFirebaseApiKey()));

            try
            {
                await authProvider.SendPasswordResetEmailAsync(App.GetEmail());

                DisplayAlert("Sucess", "Password reset mail is send to " + App.GetEmail(), "Ok");
            }
            catch (FirebaseAuthException exception)
            {
                
             
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
