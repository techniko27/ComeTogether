using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComeTogetherApp._1._Authentification
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
            //await DisplayAlert("Sign Up erfolgreich!", "Sie können sich jetzt einloggen", "OK");
            //Navigation.PopToRootAsync();

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
                usernameEntry.Text = string.Empty;
            }
            else if (passwordEntry.Text.Length < 3)
            {
                //await DisplayAlert("Eingabefehler", "Passwörter nicht identisch!", "OK");
                messageLabel.Text = "Passwort muss mindestens drei Zeichen enthalten!";
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
                emailEntry.Text = string.Empty;
            }
            else
            {
                //SignUp Request
                //App.endpointConnection.SetProtocolFunction(this.ServerAnswer);
                //await App.Communicate("#104;" + usernameEntry.Text + ";" + secondnameEntry.Text + ";" + forenameEntry.Text + ";" + passwordEntry.Text + ";" + emailEntry.Text.ToLower() + ";0", this);
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(App.GetFirebaseApiKey()));

                try
                {
                    var auth = await authProvider.CreateUserWithEmailAndPasswordAsync(emailEntry.Text, passwordEntry.Text, usernameEntry.Text, true);
                }
                catch (Exception exception)
                {
                    await DisplayAlert("Fehler", exception.ToString(), "OK");
                    throw;
                }
            }

            buttonSignUp.IsEnabled = true;
            activityIndicator.IsRunning = false;
        }

        async void ServerAnswer(string protocol)
        {
            //await DisplayAlert("Servermessage", protocol, "OK");                      //Output of the Server Answer

            List<string> list = new List<string>();
            //string [] test  = protocol.Split(';');
            list = protocol.Split(';').ToList();

            //App.endpointConnection.closeConnection();

            if (list.ElementAt(0).Equals("#105"))
            {
                if (list.ElementAt(1).Equals("1"))
                {
                    await DisplayAlert("Sign Up erfolgreich!", "Sie können sich jetzt einloggen", "OK");
                    Navigation.PopToRootAsync();

                }
                else if (list.ElementAt(1).Equals("2"))
                {
                    messageLabel.Text = "Sign Up fehlgeschlagen, Benutzername wird bereits verwendet!";
                    usernameEntry.Text = string.Empty;
                }
                else if (list.ElementAt(1).Equals("3"))
                {
                    messageLabel.Text = "Sign Up fehlgeschlagen, Emailadresse wird bereits verwendet!";
                }
                else if (list.ElementAt(1).Equals("4"))
                {
                    messageLabel.Text = "Sign Up fehlgeschlagen, Serverproblem!";
                }
                else
                {
                    messageLabel.Text = "Kommunikationsproblem, Undefinierte Antwort vom Server 1!";
                }
            }
            else
            {
                messageLabel.Text = "Kommunikationsproblem, Undefinierte Antwort vom Server 2!";
            }
        }
    }
}