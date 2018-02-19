using System;
using Xamarin.Forms;

namespace ComeTogetherApp
{
    public partial class App : Application
    {
        protected static App app;
        public App()
        {
            InitializeComponent();

            app = this;

            MainPage = new LoginPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
            if (!Application.Current.Properties.ContainsKey("IsUserLoggedIn"))
            {
                Application.Current.Properties["IsUserLoggedIn"] = false;
                System.Diagnostics.Debug.WriteLine("First Appstart, initialize Current.Properties");
            }

            LogInSwitch();
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        public static void LogInSwitch()
        {
            System.Diagnostics.Debug.WriteLine("LogInSwitch");

            //App.IsUserLoggedIn = true;
            bool IsUserLoggedIn = Convert.ToBoolean(Application.Current.Properties["IsUserLoggedIn"]);
            if (!IsUserLoggedIn)
            {
                //app.MainPage = new NavigationPage(new tbfApp.LoginPage());

                //Navigation.PushModalAsync(new tbfApp.LoginPage());

                //app.MainPage = new tbfApp.LoginPage();
                app.MainPage = new NavigationPage(new LoginPage()
                {
                    Title = "Login",

                })
                {
                    BarBackgroundColor = Color.FromHex(App.GetMenueColor()), //#009acd
                    BarTextColor = Color.White,
                };
                //Navigation.PushModalAsync(new tbfApp.LoginPage());
            }
            else
            {
                app.MainPage = new ComeTogetherApp.MainPage();
            }
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
                Application.Current.Properties["menueColor"] = "009acd";
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

        public static int GetUserID()
        {
            if (!Application.Current.Properties.ContainsKey("userID"))
            {
                Application.Current.Properties["userID"] = 0;
                System.Diagnostics.Debug.WriteLine("First userID set");
            }
            return Convert.ToInt32(Application.Current.Properties["userID"]);
        }

        public static bool SetUserID(int newUserID)
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
    }
}
