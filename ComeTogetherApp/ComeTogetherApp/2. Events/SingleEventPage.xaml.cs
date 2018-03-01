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
    public partial class SingleEventPage : TabbedPage
    {
        public SingleEventPage(Event ev)
        {
            InitializeComponent();

            initProperties();

            Children.Add(new EventDetailsToDosPage(ev));
            Children.Add(new EventDetailsMembersPage(ev));
            Children.Add(new EventDetailsGeneralPage(ev));
        }

        private void initProperties()
        {
            Title = "Edit Event";

            Color menuColor = Color.FromHex(App.GetMenueColor());
            BackgroundColor = menuColor;
            BarBackgroundColor = menuColor;
            BarTextColor = Color.White;
        }
    }
}