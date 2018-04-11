using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComeTogetherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CostOverviewPage : ContentPage
    {
        public Event ev;

        private ActivityIndicator activityIndicator;

        private ObservableCollection<ToDo> eventMemberList;

        private StackLayout stackLayout;
        private Frame eventMemberCostFrame;
        private Frame transactionsFrame;
        private Button restartEventButton;

        public CostOverviewPage(Event ev)
        {
            InitializeComponent();

            Title = "Cost Overview";
            this.ev = ev;


            initLayout();

            activityIndicatorSwitch();
        }

        private void initLayout()
        {
            ScrollView scrollView = new ScrollView();

            activityIndicator = new ActivityIndicator()
            {
                Color = Color.Gray,
                IsRunning = true,
                WidthRequest = 80,
                HeightRequest = 80,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            stackLayout = createStackLayout();
            scrollView.Content = stackLayout;

            Content = scrollView;
        }
        private StackLayout createStackLayout()
        {
            StackLayout stackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                Padding = new Thickness(10, 10, 10, 10)
            };

            Label explanationLabel = new Label
            {
                Text = "If you have something to pay, just click on someone with a minus, to balance the incurred costs.",
                TextColor = Color.Black,
                FontSize = 15,
                Margin = new Thickness(0, 0, 0, 0),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand
            };
            stackLayout.Children.Add(explanationLabel);

            eventMemberCostFrame = createEventMemberCostFrame();
            transactionsFrame = createTransactionFrame();

            stackLayout.Children.Add(activityIndicator);

            restartEventButton = new Button
            {
                Text = "Restart Event",
                TextColor = Color.Black,
                BackgroundColor = Color.LightGreen,
                FontAttributes = FontAttributes.Bold
            };
            restartEventButton.Clicked += OnRestartEventButtonClicked;

            return stackLayout;
        }
        private Frame createEventMemberCostFrame()
        {
            StackLayout eventMemberCostLayout = new StackLayout();

            Label eventMemberCostLabel = createFrameHeaderLabel("Members and Costs");
            eventMemberCostLayout.Children.Add(eventMemberCostLabel);

            StackLayout eventMemberCostInnerLayout = new StackLayout();

            ScrollView eventMemberInnerScrollView = new ScrollView();
            eventMemberInnerScrollView.Content = eventMemberCostInnerLayout;
            eventMemberCostLayout.Children.Add(eventMemberInnerScrollView);

            for (int i = 0; i < 4; i++)
            {
                Frame eventMemberFrame = createListElementFrame();
                eventMemberCostInnerLayout.Children.Add(eventMemberFrame);
            }
            
            return new Frame
            {
                Content = eventMemberCostLayout,
                Padding = 4,
                BackgroundColor = Color.FromHex("376467"),
                CornerRadius = 5,
                HeightRequest = 200
            };
        }
        private Label createFrameHeaderLabel(String text)
        {
            return new Label
            {
                Text = text,
                TextColor = Color.White,
                FontSize = 20,
                Margin = new Thickness(5, 0, 0, 0),
            };
        }
        private Frame createListElementFrame()
        {
            StackLayout listViewFrameLayout = new StackLayout();

            Frame listViewFrame = new Frame
            {
                Content = listViewFrameLayout,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5,
                Padding = 1
            };

            StackLayout horizontalLableAndButtonLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, 0, 0, 0),
                HeightRequest = 35
            };
            var horizontalLayoutTapGestureRecognizer = new TapGestureRecognizer();
            horizontalLayoutTapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
            {
                // handle the tap
                OnCostButtonClicked(sender, e);
            };
            horizontalLableAndButtonLayout.GestureRecognizers.Add(horizontalLayoutTapGestureRecognizer);

            Label memberLabel = new Label
            {
                Text = "Member",
                TextColor = Color.White,
                FontSize = 20,
                Margin = new Thickness(0, 0, 0, 0),
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            horizontalLableAndButtonLayout.Children.Add(memberLabel);

            Button costButton = new Button()
            {
                Text = "-99€",
                BackgroundColor = Color.White,
                TextColor = Color.FromHex(App.GetMenueColor()),
                FontAttributes = FontAttributes.Bold,
                WidthRequest = 80,
                HeightRequest = 80,
                CornerRadius = 40,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            costButton.Clicked += OnCostButtonClicked;
            horizontalLableAndButtonLayout.Children.Add(costButton);

            listViewFrameLayout.Children.Add(horizontalLableAndButtonLayout);

            return listViewFrame;
        }
        private Frame createTransactionFrame()
        {
            StackLayout transactionLayout = new StackLayout();

            Label transactionLabel = createFrameHeaderLabel("Transactions");
            transactionLayout.Children.Add(transactionLabel);

            StackLayout transactionInnerLayout = new StackLayout();

            ScrollView transactionInnerScrollView = new ScrollView();
            transactionInnerScrollView.Content = transactionInnerLayout;
            transactionLayout.Children.Add(transactionInnerScrollView);

            for (int i = 0; i < 4; i++)
            {
                Frame transactionFrame = createTransactionElementFrame();
                transactionInnerLayout.Children.Add(transactionFrame);
            }

            return new Frame
            {
                Content = transactionLayout,
                Padding = 4,
                BackgroundColor = Color.FromHex("376467"),
                CornerRadius = 5,
                HeightRequest = 200
            };
        }
        private Frame createTransactionElementFrame()
        {
            StackLayout transactionFrameLayout = new StackLayout();

            Frame transactionFrame = new Frame
            {
                Content = transactionFrameLayout,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5,
                Padding = 1
            };

            Label transactionLabel = new Label
            {
                Text = "Transaktion",
                TextColor = Color.White,
                FontSize = 20,
                Margin = new Thickness(0, 0, 0, 0),
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            transactionFrameLayout.Children.Add(transactionLabel);

            return transactionFrame;
        }
        private void activityIndicatorSwitch()
        {
            if (activityIndicator.IsRunning)
            {
                activityIndicator.IsRunning = false;

                stackLayout.Children.Remove(activityIndicator);

                stackLayout.Children.Add(eventMemberCostFrame);
                stackLayout.Children.Add(transactionsFrame);

                if (ev.adminID.Equals(App.GetUserID()))
                    stackLayout.Children.Add(restartEventButton);
            }
            else
            {
                activityIndicator.IsRunning = true;

                if (ev.adminID.Equals(App.GetUserID()))
                    stackLayout.Children.Remove(restartEventButton);

                stackLayout.Children.Remove(transactionsFrame);
                stackLayout.Children.Remove(eventMemberCostFrame);

                stackLayout.Children.Add(activityIndicator);
            }
        }

        async void OnCostButtonClicked(object sender, EventArgs e)
        {
            await DisplayAlert("CostButton", "Clicked", "ok");
        }
        async void OnRestartEventButtonClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Restart Event", "has to be implemented, only possible, when no transaction is done", "ok");
        }
    }
}