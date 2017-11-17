using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ComeTogetherApp._1._Authentification
{
    class LogOut
    {
        public LogOut()
        {
            Application.Current.Properties["IsUserLoggedIn"] = false;
            App.LogInSwitch();
            
        }
    }
}
