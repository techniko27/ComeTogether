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
        public SingleEventPage(String eventID)
        {
            InitializeComponent();

            initProperties();

            Title = "Single Event Page";
            Children.Add(new EventDetailsToDosPage());
            Children.Add(new EventDetailsMembersPage());
            Children.Add(new EventDetailsGeneralPage());
        }

        private void initProperties()
        {
            Color menuColor = Color.FromHex(App.GetMenueColor());
            BackgroundColor = menuColor;
            BarBackgroundColor = menuColor;
            BarTextColor = Color.White;
        }
    }
}