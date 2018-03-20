using System;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;
using Xamarin.Forms;

namespace ComeTogetherApp
{
    public partial class App : Application
    {
        protected static App app;
        public static FirebaseClient firebase;
        public static Page activePage;

        public App()
        {
            InitializeComponent();

            app = this;

            activePage = new AppStartPage();
            MainPage = activePage;
        }

        protected override async void OnStart()
        {
            // Handle when your app starts
            if (!Application.Current.Properties.ContainsKey("IsUserLoggedIn"))
            {
                Application.Current.Properties["IsUserLoggedIn"] = false;
                System.Diagnostics.Debug.WriteLine("First Appstart, initialize Current.Properties");
            }

            await LogInSwitch();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override async void OnResume()
        {
            // Handle when your app resumes
            if (GetEmail().Length > 2)
            {
                await firebaseClientRefresh();
            }
        }

        public static async Task<string> firebaseClientRefresh()
        {
            try
            {
                var authProvider = new FirebaseAuthProvider(new FirebaseConfig(App.GetFirebaseApiKey()));
                var auth = await authProvider.SignInWithEmailAndPasswordAsync(GetEmail(), GetPassword());

                firebase = new FirebaseClient(App.GetServerAdress(), new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(auth.FirebaseToken)
                });
                return auth.FirebaseToken;
            }
            catch (Exception)
            {
                await activePage.DisplayAlert("Server connection failure", "Communication problems occured while authentication", "OK");
            }
            return "";
        }

        public static async Task<bool> LogInSwitch()
        {
            System.Diagnostics.Debug.WriteLine("LogInSwitch");

            //App.IsUserLoggedIn = true;
            bool IsUserLoggedIn = Convert.ToBoolean(Application.Current.Properties["IsUserLoggedIn"]);
            if (!IsUserLoggedIn)
            {
                Application.Current.Properties["Email"] = "";
                Application.Current.Properties["Password"] = "";

                //Navigation.PushModalAsync(new tbfApp.LoginPage());

                activePage = new NavigationPage(new LoginPage()
                {
                    Title = "Login",

                })
                {
                    BarBackgroundColor = Color.FromHex(App.GetMenueColor()), //#009acd
                    BarTextColor = Color.White,
                };
                app.MainPage = activePage;
            }
            else
            {
                await firebaseClientRefresh();

                activePage = new MainPage();
                app.MainPage = activePage;
            }
            return true;
        }

        public static void NavigateToSignUp()
        {
            app.MainPage.Navigation.PushAsync(new SignUpPage()
            {
                Title = "Sign up"
            });
        }

        public static String GetMenueColor()
        {
            if (!Application.Current.Properties.ContainsKey("menueColor"))
            {
                Application.Current.Properties["menueColor"] = "41BAC1";
                System.Diagnostics.Debug.WriteLine("First menueColor set");
            }
            return Convert.ToString(Application.Current.Properties["menueColor"]);
        }

        public static bool SetMenueColor(String newColor)
        {
            Application.Current.Properties["menueColor"] = newColor;
            return true;
        }

        public static String GetServerAdress()
        {
            if (!Application.Current.Properties.ContainsKey("serverAdress"))
            {
                //Application.Current.Properties["serverAdress"] = "https://cometogether-15b4.firebaseapp.com";
                Application.Current.Properties["serverAdress"] = "https://cometogether-15b4.firebaseio.com/";
                System.Diagnostics.Debug.WriteLine("First serverAdress set");
            }
            return Convert.ToString(Application.Current.Properties["serverAdress"]);
        }

        public static bool SetServerAdress(String newServerAdress)
        {
            Application.Current.Properties["serverAdress"] = newServerAdress;
            return true;
        }

        public static string GetUserID()
        {
            if (!Application.Current.Properties.ContainsKey("userID"))
            {
                Application.Current.Properties["userID"] = 0;
                System.Diagnostics.Debug.WriteLine("First userID set");
            }
            return Convert.ToString(Application.Current.Properties["userID"]);
        }

        public static bool SetUserID(string newUserID)
        {
            try
            {
                Application.Current.Properties["userID"] = newUserID;
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("USER ID IS NOT SET!!!!!!!!");
                //throw;
            }
            return true;
        }

        public static String GetUsername()
        {
            if (!Application.Current.Properties.ContainsKey("username"))
            {
                Application.Current.Properties["username"] = "Benutzername";
                System.Diagnostics.Debug.WriteLine("First username set");
            }
            return Convert.ToString(Application.Current.Properties["username"]);
        }
        public static bool SetUsername(String newUsername)
        {
            Application.Current.Properties["username"] = newUsername;
            return true;
        }
        public static String GetFirebaseApiKey()
        {
            //Firebase Api Key = Cloud Messaging -> Serverschlüssel
            //AAAAPcDUv78:APA91bEOSotN0eMtDs23FvMSmufB6htm11RsN-SCqewa31EIGvzhaFOilzVi3xrUcUGEluyVMCa2UvKv7vAvqt2q2qIh4WgrTtheXOEqkDEkbFiS27i5LZNORR1gHlnWqMdFsZCZCyEm
            if (!Application.Current.Properties.ContainsKey("FirebaseApiKey"))
            {
                Application.Current.Properties["FirebaseApiKey"] = 
                    "AIzaSyC5srcmE8bzhuaZLO04_BWUcSawJ9As9oA";
                System.Diagnostics.Debug.WriteLine("First FirebaseApiKey set");
            }
            return Convert.ToString(Application.Current.Properties["FirebaseApiKey"]);
        }
        public static bool SetFirebaseApiKey(String FirebaseApiKey)
        {
            Application.Current.Properties["FirebaseApiKey"] = FirebaseApiKey;
            return true;
        }
        /*Macht aktuell keinen Sinn das Token zu speichern, läuft innerhalb von einem Tag ab. Anstelle Email und Password speichern.
        public static String GetAuthToken()
        {
            if (!Application.Current.Properties.ContainsKey("AuthToken"))
            {
                Application.Current.Properties["AuthToken"] = "";
            }
            return Convert.ToString(Application.Current.Properties["AuthToken"]);
        }
        public static bool SetAuthToken(String newAuthToken)
        {
            Application.Current.Properties["AuthToken"] = newAuthToken;
            return true;
        }
        */
        public static String GetEmail()
        {
            if (!Application.Current.Properties.ContainsKey("Email"))
            {
                Application.Current.Properties["Email"] = "";
            }
            return Convert.ToString(Application.Current.Properties["Email"]);
        }
        public static bool SetEmail(String newEmail)
        {
            Application.Current.Properties["Email"] = newEmail;
            return true;
        }
        public static String GetPassword()
        {
            if (!Application.Current.Properties.ContainsKey("Password"))
            {
                Application.Current.Properties["Password"] = "";
            }
            return Convert.ToString(Application.Current.Properties["Password"]);
        }
        public static bool SetPassword(String newPassword)
        {
            Application.Current.Properties["Password"] = newPassword;
            return true;
        }
    }
}
