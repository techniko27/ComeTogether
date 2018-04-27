﻿using System;
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
                IconSource = "events_icon.png",
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
                IconSource = "contacts_schwarz.png",
                TargetType = typeof(UserSettingPage)
            });
            /*
            masterPageItems.Add(new MasterPageItem
            {
                Title = "Settings",
                IconSource = "todo_schwarz.png",
                TargetType = typeof(EventsPage)
            });
            */
            masterPageItems.Add(new MasterPageItem
            {
                Title = "LogOff",
                IconSource = "logoff_schwarz.png",
                TargetType = typeof(LoginPage)
            });

            listView.ItemsSource = masterPageItems;
        }
    }
}