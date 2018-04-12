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
            initLayout(ev);
        }

        private void initLayout(Event ev)
        {
            EventDetailsToDosPage toDosPage = new EventDetailsToDosPage(ev);
            EventDetailsMembersPage membersPage = new EventDetailsMembersPage(ev);
            EventDetailsGeneralPage generalPage = new EventDetailsGeneralPage(ev);

            Children.Add(toDosPage);
            Children.Add(membersPage);
            Children.Add(generalPage);

            CurrentPage = toDosPage;
        }

        private void initProperties()
        {
            Title = "Event Details";

            Color menuColor = Color.FromHex(App.GetMenueColor());
            BarBackgroundColor = menuColor;
            BarTextColor = Color.White;
        }
    }
}