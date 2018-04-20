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
    public partial class CashPaymentPage : ContentPage
    {
        public Event ev;
        public User member;
        private int sendAmount;
        private Task<int> personalCostCalculatingTask;

        private ActivityIndicator activityIndicator;
        private Frame frame;
        public CashPaymentPage(Event ev, User member, int sendAmount, Task<int> personalCostCalculatingTask)
        {
            InitializeComponent();
            
            Title = "Cash payment";

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
                Padding = new Thickness(10, 10, 10, 10)
            };

            frame = new Frame()
            {
                Content = stackLayout,
                Padding = 4,
                BackgroundColor = Color.FromHex("376467"),
                CornerRadius = 5,
                HeightRequest = 200,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            Label explanationLabel = new Label
            {
                Text = "I have sent " + sendAmount + "€ in cash to " + member.userName + ".",
                TextColor = Color.Black,
                FontSize = 30,
                Margin = new Thickness(0, 0, 0, 0),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };
            stackLayout.Children.Add(explanationLabel);

            Button confirmButton = new Button
            {
                Text = "Confirm",
                TextColor = Color.Black,
                BackgroundColor = Color.LightGreen,
                FontAttributes = FontAttributes.Bold
            };
            stackLayout.Children.Add(confirmButton);
            confirmButton.Clicked += OnconfirmButtonClicked;

            Content = frame;
        }
        async void OnconfirmButtonClicked(object sender, EventArgs e)
        {
            activityIndicatorSwitch();

            string transactionID = Guid.NewGuid().ToString("N");

            try
            {
                await App.firebase.Child("Transaktionen").Child(ev.ID).Child(transactionID).PutAsync<Transaction>(new Transaction(member.ID, App.GetUserID(), "Cash",sendAmount));
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
