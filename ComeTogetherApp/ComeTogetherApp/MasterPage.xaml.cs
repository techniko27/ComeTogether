using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComeTogetherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterPage : ContentPage
    {
        public ListView ListView { get { return listView; } }
        public String listColor;

        public MasterPage()
        {
            InitializeComponent();

            listView.BackgroundColor = Color.FromHex(App.GetMenueColor());

            var masterPageItems = new List<MasterPageItem>();

            masterPageItems.Add(new MasterPageItem
            {
                Title = "Events",
                IconSource = "rooms.png",
                TargetType = typeof(EventsPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Settings",
                IconSource = "todo.png",
                TargetType = typeof(EventsPage)
            });
            /*
            masterPageItems.Add(new MasterPageItem
            {
                Title = "lo",
                //IconSource = "contacts.png",
                TargetType = typeof(RoomPage),
            });
            */
            masterPageItems.Add(new MasterPageItem
            {
                //Title = App.GetUsername(),
                Title = "Account",
                IconSource = "contacts.png",
                TargetType = typeof(EventsPage)
            });
            masterPageItems.Add(new MasterPageItem
            {
                Title = "LogOff",
                IconSource = "logoff.png",
                TargetType = typeof(LoginPage)
            });

            listView.ItemsSource = masterPageItems;
        }
    }
}