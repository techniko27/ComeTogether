using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComeTogetherApp
{
    class LogOut : ContentPage
    {
        public LogOut()
        {
            Application.Current.Properties["IsUserLoggedIn"] = false;
            Device.BeginInvokeOnMainThread(() => App.LogInSwitch());
        }
    }
}
