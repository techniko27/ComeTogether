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
    public partial class EventDetailsToDosPage : ContentPage
    {
        public EventDetailsToDosPage(Event ev)
        {
            InitializeComponent();

            initProperties();
        }

        private void initProperties()
        {
            Title = "ToDos";
            BackgroundColor = Color.FromHex(App.GetMenueColor());
        }
    }
}