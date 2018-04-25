using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database.Query;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComeTogetherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PayPalPage : ContentPage
    {
        public Event ev;
        public User member;
        private double sendAmount;
        private Task<double> personalCostCalculatingTask;

        private ActivityIndicator activityIndicator;
        private Frame frame;
        public PayPalPage(Event ev, User member, double sendAmount, Task<double> personalCostCalculatingTask)
        {
            InitializeComponent();

            Title = "PayPal payment";
            BackgroundColor = Color.Black;

            this.ev = ev;
            this.member = member;
            this.sendAmount = sendAmount;
            this.personalCostCalculatingTask = personalCostCalculatingTask;

            initLayout();
        }
        private void initLayout()
        {
            activityIndicator = new ActivityIndicator()
            {
                Color = Color.Gray,
                IsRunning = false,
                WidthRequest = 80,
                HeightRequest = 80,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            StackLayout stackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                Padding = new Thickness(0, 0, 0, 0)
            };
            
            Grid grid = new Grid { RowSpacing = 1, ColumnSpacing = 1, };
            //grid.Padding = new Thickness(0, 5, 0, 5);
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(480) });
            stackLayout.Children.Add(grid);
            
            WebView browser = new WebView();                       //Use own WebView with own custom renderer
            browser.Source = "https://" + member.PayPal_me_link + "/" + sendAmount;
            grid.Children.Add(browser, 0, 0);

            StackLayout innerStackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                Padding = new Thickness(0, 0, 0, 0)
            };

            frame = new Frame()
            {
                Content = innerStackLayout,
                Padding = 0,
                BackgroundColor = Color.FromHex("376467"),
                CornerRadius = 5,
                HeightRequest = 80,
                WidthRequest = 300,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Label explanationLabel = new Label
            {
                Text = "I have sent " + sendAmount + "€ by PayPal.me to " + member.userName + ".",
                TextColor = Color.Black,
                FontSize = 15,
                Margin = new Thickness(0, 0, 0, 0),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };
            innerStackLayout.Children.Add(explanationLabel);

            Button confirmButton = new Button
            {
                Text = "Confirm",
                TextColor = Color.Black,
                BackgroundColor = Color.LightGreen,
                FontAttributes = FontAttributes.Bold
            };
            innerStackLayout.Children.Add(confirmButton);
            confirmButton.Clicked += OnconfirmButtonClicked;

            stackLayout.Children.Add(frame);

            Content = stackLayout;
        }
        async void OnconfirmButtonClicked(object sender, EventArgs e)
        {
            activityIndicatorSwitch();

            string transactionID = Guid.NewGuid().ToString("N");

            try
            {
                await App.firebase.Child("Transaktionen").Child(ev.ID).Child(transactionID).PutAsync<Transaction>(new Transaction(member.ID, App.GetUserID(), "PayPal.me", sendAmount));
            }
            catch (Exception)
            {
                await DisplayAlert("Server connection failure", "Communication problems occured while querying", "OK");
                System.Diagnostics.Debug.WriteLine(e);
            }

            Navigation.InsertPageBefore(new CostOverviewPage(ev, personalCostCalculatingTask), Navigation.NavigationStack[Navigation.NavigationStack.Count - 2]);
            Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count - 1]);
            await Navigation.PopAsync();
        }
        private void activityIndicatorSwitch()
        {
            if (activityIndicator.IsRunning)
            {
                activityIndicator.IsRunning = false;

                Content = frame;
            }
            else
            {
                activityIndicator.IsRunning = true;

                Content = activityIndicator;
            }
        }
    }
}