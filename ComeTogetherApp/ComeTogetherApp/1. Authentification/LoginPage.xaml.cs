using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComeTogetherApp._1._Authentification
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;
            buttonLoginin.IsEnabled = false;

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
                    var auth = await authProvider.SignInWithEmailAndPasswordAsync(emailEntry.Text.Replace(" ",String.Empty), passwordEntry.Text);

                    firebase = new FirebaseClient(App.GetServerAdress(), new FirebaseOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken)
                    });

                    await DisplayAlert("Token", auth.FirebaseToken, "OK");
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
                            DisplayAlert("Serverfehler", exception.ToString(), "OK");
                            //await DisplayAlert("Anmeldefehler", "Sie konnten nicht angemeldet werden!", "OK");
                            break;
                    }
                }

                /* Facebook Auth
                var facebookAccessToken = "<login with facebook and get oauth access token>";
                var auth = await authProvider.SignInWithOAuthAsync(FirebaseAuthType.Facebook, facebookAccessToken);
                */

                /*
                var dinos = await firebase.Child("dinosaurs").OnceAsync<Dinosaur>();

                foreach (var dino in dinos)
                {
                    System.Diagnostics.Debug.WriteLine($"{dino.Key} is {dino.Object.Height}m high.");
                }
                */


            }

            Application.Current.Properties["IsUserLoggedIn"] = true;
            App.LogInSwitch();

            buttonLoginin.IsEnabled = true;
            activityIndicator.IsRunning = false;
        }

        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;
            buttonSignUp.IsEnabled = false;

            //Greatings from Server
            //await App.Communicate("#001;~", this);

            App.NavigateToSignUp();

            buttonSignUp.IsEnabled = true;
            activityIndicator.IsRunning = false;
        }

        private void ServerAnswer(string protocol)
        {
            //DisplayAlert("Servermessage", protocol, "OK");                    //Output of the Server Answer

            List<string> list = new List<string>();
            list = protocol.Split(new char[] { ';' }).ToList();

            if (list.ElementAt(0).Equals("#103"))
            {
                switch (list.ElementAt(1))
                {
                    case "1":
                        int iD;
                        int.TryParse(list.ElementAt(2), out iD);
                        //App.SetUserID(iD);
                        //App.SetUsername(usernameEntry.Text);
                        //Application.Current.Properties["IsUserLoggedIn"] = true;
                        //App.LogInSwitch();
                        DisplayAlert("Login Erfolgreich!", "Hallo " + emailEntry.Text + "!", "OK");
                        break;
                    case "2":
                        //messageLabel.Text = "Login fehlgeschlagen, falsches Passwort oder falscher Benutzername!";
                        DisplayAlert("Login Fehlgeschlagen", "falsches Passwort oder falscher Benutzername!", "OK");
                        passwordEntry.Text = string.Empty;
                        emailEntry.Text = string.Empty;
                        break;
                    case "3":
                        //messageLabel.Text = "Login fehlgeschlagen, Serverproblem!";
                        DisplayAlert("Login Fehlgeschlagen", "Serverproblem!", "OK");
                        break;
                    default:
                        //messageLabel.Text = "Kommunikationsproblem, Undefinierte Antwort vom Server 1!";
                        DisplayAlert("Login Fehlgeschlagen", "Kommunikationsproblem, Undefinierte Antwort vom Server 1!", "OK");
                        break;
                }
            }
            else
            {
                messageLabel.Text = "Kommunikationsproblem, Undefinierte Antwort vom Server 2!";
            }
            //App.endpointConnection.closeConnection();
        }
    }
}