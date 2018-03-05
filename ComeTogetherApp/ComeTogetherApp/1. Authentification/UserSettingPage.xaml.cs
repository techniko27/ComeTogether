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
        public UserSettingPage()
        {
            InitializeComponent();
            scrollView.BackgroundColor = Color.FromHex(App.GetMenueColor());
            buttonSaveChanges.TextColor = Color.FromHex(App.GetMenueColor());
            buttonSaveChanges.BackgroundColor = Color.White;
            buttonSaveChanges.Margin = new Thickness(50, 0, 50, 0);
            buttonresetPasswort.TextColor = Color.FromHex(App.GetMenueColor());
            buttonresetPasswort.BackgroundColor = Color.White;
            buttonresetPasswort.Margin = new Thickness(50, 20, 50, 0);

            ServerGetUser();
        }

        //usernameEntry.Placeholder = await App.firebase.Child("Veranstaltungen").OrderByKey().StartAt("1").OnceAsync<Event>();
        private async void ServerGetUser() 
        {
            try
            {
                IReadOnlyCollection<FirebaseObject<User>> users = await App.firebase.Child("users").OnceAsync<User>();
                Func<FirebaseObject<User>, bool> predicateUserID = (FirebaseObject<User> userObject) => { return userObject.Key.Equals(App.GetUserID()); };
                User user = users.Where(predicateUserID).ElementAt(0).Object;
            } catch (Exception e)
            {

            }
        }

        private async void ServerSetUser(){
            
        }


    }
}
