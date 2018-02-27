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
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        public SignUpPage()
        {
            InitializeComponent();
        }

        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;
            buttonSignUp.IsEnabled = false;

            if (usernameEntry.Text == null || passwordEntry.Text == null || passwordEntry2.Text == null ||
                emailEntry.Text == null) //forenameEntry.Text == null || secondnameEntry.Text == null || 
            {
                //await DisplayAlert("Eingabefehler", "Eingaben nicht vollständig!", "OK");
                messageLabel.Text = "Eingaben nicht vollständig!";
            }
            else if (usernameEntry.Text.Length < 3)
            {
                //await DisplayAlert("Eingabefehler", "Passwörter nicht identisch!", "OK");
                messageLabel.Text = "Benutzername muss mindestens drei Zeichen enthalten!";
            }
            else if (passwordEntry.Text.Length < 6)
            {
                //await DisplayAlert("Eingabefehler", "Passwörter nicht identisch!", "OK");
                messageLabel.Text = "Passwort muss mindestens 6 Zeichen enthalten!";
                passwordEntry.Text = string.Empty;
                passwordEntry2.Text = string.Empty;
            }
            else if (!passwordEntry.Text.Equals(passwordEntry2.Text))
            {
                //await DisplayAlert("Eingabefehler", "Passwörter nicht identisch!", "OK");
                messageLabel.Text = "Passwörter nicht identisch!";
                passwordEntry.Text = string.Empty;
                passwordEntry2.Text = string.Empty;
            }
            else if (!emailEntry.Text.Contains("@") || !emailEntry.Text.Contains(".") || emailEntry.Text.Length < 6)
            {
                //await DisplayAlert("Eingabefehler", "Email Adresse ungültig!", "OK");
                messageLabel.Text = "Email Adresse ungültig!";
            }
            else
            {
                //SignUp Request
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(App.GetFirebaseApiKey()));

                try
                {
                    var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(emailEntry.Text, passwordEntry.Text, usernameEntry.Text, true);

                    string userID = auth.User.LocalId;

                    auth = await authProvider.SignInWithEmailAndPasswordAsync(emailEntry.Text.Replace(" ", String.Empty), passwordEntry.Text);

                    FirebaseClient firebase = new FirebaseClient(App.GetServerAdress(), new FirebaseOptions
                    {
                        AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken)
                    });

                    await firebase.Child("users").Child(userID).PutAsync(new User(usernameEntry.Text, emailEntry.Text));

                    await DisplayAlert("Sign Up erfolgreich!", "Sie können sich jetzt einloggen", "OK");
                    Navigation.PopToRootAsync();
                }
                catch (FirebaseAuthException exception)
                {
                    switch (exception.Reason)
                    {
                        case AuthErrorReason.WeakPassword:
                            messageLabel.Text = "Passwort muss mindestens 6 Zeichen lang sein.";
                            passwordEntry.Text = string.Empty;
                            passwordEntry2.Text = string.Empty;
                            break;
                        case AuthErrorReason.EmailExists:
                            messageLabel.Text = "Mit dieser Email Adresse wurde bereits ein Konto angelegt.";
                            break;
                        case AuthErrorReason.InvalidEmailAddress:
                            messageLabel.Text = "Email Adresse ungültig.";
                            break;
                        case AuthErrorReason.InvalidProviderID:
                            messageLabel.Text = "InvlidProviderID";
                            break;
                        default:
                            //messageLabel.Text = "Kommunikationsproblem, Undefinierte Antwort vom Server!";
                            DisplayAlert("Serverfehler", exception.ToString(), "OK");
                            break;
                    }
                }
            }

            buttonSignUp.IsEnabled = true;
            activityIndicator.IsRunning = false;
        }
    }
}