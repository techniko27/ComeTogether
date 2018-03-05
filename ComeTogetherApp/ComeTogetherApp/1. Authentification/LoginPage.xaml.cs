using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComeTogetherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();

            scrollView.BackgroundColor = Color.FromHex(App.GetMenueColor());
            buttonLoginin.TextColor = Color.FromHex(App.GetMenueColor());
            buttonLoginin.Margin = new Thickness(50, 20, 50, 0);
            buttonSignUp.TextColor = Color.FromHex(App.GetMenueColor());
            buttonSignUp.Margin = new Thickness(50, 0, 50, 0);

            buttonResetPassword.TextColor = Color.FromHex(App.GetMenueColor());
            buttonResetPassword.Margin = new Thickness(50, 0 , 50, 0);
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;
            buttonLoginin.IsEnabled = false;
            string firebaseToken = null;

            if (emailEntry.Text == null)
            {
                //await DisplayAlert("Eingabefehler", "Emailadresse eingeben!", "OK");
                messageLabel.Text = "Emailadresse eingeben!";
            }
            else if (passwordEntry.Text == null)
            {
                //await DisplayAlert("Eingabefehler", "Passwort eingeben!", "OK");
                messageLabel.Text = "Passwort eingeben!";
            }
            else if (!emailEntry.Text.Contains("@") || !emailEntry.Text.Contains(".") || emailEntry.Text.Length < 6)
            {
                //await DisplayAlert("Eingabefehler", "Email Adresse ungültig!", "OK");
                messageLabel.Text = "Email Adresse ungültig!";
            }
            else
            {
                //Login Request
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(App.GetFirebaseApiKey()));

                FirebaseClient firebase = null;

                try
                {
                    var auth = await authProvider.SignInWithEmailAndPasswordAsync(emailEntry.Text.Replace(" ", String.Empty), passwordEntry.Text);

                    firebase = new FirebaseClient(App.GetServerAdress(), new FirebaseOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken)
                    });

                    //await DisplayAlert("Token", auth.FirebaseToken, "OK");
                    firebaseToken = auth.FirebaseToken;

                    if (firebaseToken != null)
                    {
                        Application.Current.Properties["IsUserLoggedIn"] = true;
                        App.SetEmail(emailEntry.Text.Replace(" ", String.Empty));
                        App.SetPassword(passwordEntry.Text);
                        App.SetUserID(auth.User.LocalId);
                        App.LogInSwitch();
                    }
                }
                catch (FirebaseAuthException exception)
                {
                    switch (exception.Reason)
                    {
                        case AuthErrorReason.WrongPassword:
                            messageLabel.Text = "Passwort ist falsch.";
                            passwordEntry.Text = string.Empty;
                            break;
                        case AuthErrorReason.UserDisabled:
                            messageLabel.Text = "Benutzer deaktiviert.";
                            break;
                        case AuthErrorReason.UnknownEmailAddress:
                            messageLabel.Text = "Email Adresse unbekannt.";
                            break;
                        case AuthErrorReason.InvalidProviderID:
                            messageLabel.Text = "InvalidProviderID";
                            break;
                        default:
                            //messageLabel.Text = "Kommunikationsproblem, Undefinierte Antwort vom Server!";
                            DisplayAlert("Server connection failure", "Communication problems occured while login", "OK");
                            //await DisplayAlert("Anmeldefehler", "Sie konnten nicht angemeldet werden!", "OK");
                            break;
                    }
                }
            }

            buttonLoginin.IsEnabled = true;
            activityIndicator.IsRunning = false;
        }

        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;
            buttonSignUp.IsEnabled = false;

            App.NavigateToSignUp();

            buttonSignUp.IsEnabled = true;
            activityIndicator.IsRunning = false;
        }

        void OnResetPasswordButtonClicked(object sender, EventArgs e)
        {
            DisplayAlert("Clicked Action","You have clicked Forget Password", "OK");
        }
    }
}