using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComeTogetherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EnterJoinCodePopupPage : PopupPage
    {
        private Entry joinCodeEntry;
        public EnterJoinCodePopupPage()
        {
            InitializeComponent();

            StackLayout stack = new StackLayout
            {
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                //Padding = new Thickness(100, 100, 100, 100),
                //Margin = new Thickness(50, 50, 50, 50),
                Padding = 10,
                HeightRequest = 100,
                WidthRequest = 300
            };
            joinCodeEntry = new Entry
            {
                Text = "Enter Join-Code here",
                FontSize = 15,
                TextColor = Color.White,
            };
            joinCodeEntry.Focused += OnJoinCodeEntryClicked;
            Button enterJoinCodeButton = new Button()
            {
                Text = "Join Event",
                BackgroundColor = Color.White,
                TextColor = Color.FromHex(App.GetMenueColor()),
                FontAttributes = FontAttributes.Bold,
            };
            enterJoinCodeButton.Clicked += OnEnterJoinCodeButtonClicked;
            
            stack.Children.Add(joinCodeEntry);
            stack.Children.Add(enterJoinCodeButton);

            Content = stack;
            //BackgroundColor = Color.Aqua;

            //Versuch um BackgrondClick zu bekommen
            CloseWhenBackgroundIsClicked = true;
            HasSystemPadding = true;

            Padding = 30;
        }
        async void OnEnterJoinCodeButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PopPopupAsync();
            //TODO Serverconnection
        }
        async void OnJoinCodeEntryClicked(object sender, EventArgs e)
        {
            joinCodeEntry.Text = "";
        }

        // ### Overrided methods which can prevent closing a popup page ###

        // Invoked when a hardware back button is pressed
        protected override bool OnBackButtonPressed()
        {
            // Return true if you don't want to close this popup page when a back button is pressed
            return base.OnBackButtonPressed();
        }

        // Invoked when background is clicked
        protected override bool OnBackgroundClicked()
        {
            // Return false if you don't want to close this popup page when a background of the popup page is clicked
            return base.OnBackgroundClicked();
        }
    }
}