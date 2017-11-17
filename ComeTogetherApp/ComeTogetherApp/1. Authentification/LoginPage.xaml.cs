using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            if (usernameEntry.Text == null)
            {
                await DisplayAlert("Eingabefehler", "Benutzername eingeben!", "OK");
            }
            else if (passwordEntry.Text == null)
            {
                await DisplayAlert("Eingabefehler", "Passwort eingeben!", "OK");
            }
            else
            {
                //Login Request
                //App.endpointConnection.SetProtocolFunction(this.ServerAnswer);
                //await App.Communicate("#102;" + usernameEntry.Text + ";" + passwordEntry.Text, this);
            }

            //Application.Current.Properties["IsUserLoggedIn"] = true;
            //App.LogInSwitch();

            buttonLoginin.IsEnabled = true;
            activityIndicator.IsRunning = false;
        }

        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            activityIndicator.IsRunning = true;
            buttonSignUp.IsEnabled = false;

            //Greatings from Server
            //await App.Communicate("#001;~", this);

            //App.NavigateToSignUp();

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
                        DisplayAlert("Login Erfolgreich!", "Hallo " + usernameEntry.Text + "!", "OK");
                        break;
                    case "2":
                        //messageLabel.Text = "Login fehlgeschlagen, falsches Passwort oder falscher Benutzername!";
                        DisplayAlert("Login Fehlgeschlagen", "falsches Passwort oder falscher Benutzername!", "OK");
                        passwordEntry.Text = string.Empty;
                        usernameEntry.Text = string.Empty;
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