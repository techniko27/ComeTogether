using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComeTogetherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CostOverviewPage : ContentPage
    {
        public Event ev;

        private ActivityIndicator activityIndicator;

        private ObservableCollection<User> eventMemberList;
        private ObservableCollection<Transaction> eventTransactionsList;

        private StackLayout stackLayout;
        private Frame eventMemberCostFrame;
        private Frame transactionsFrame;
        private Button restartEventButton;
        private StackLayout eventMemberCostInnerLayout;
        private StackLayout transactionInnerLayout;

        private int reloadCount;
        private bool pageIsDisappeared;
        public CostOverviewPage(Event ev)
        {
            InitializeComponent();

            Title = "Cost Overview";
            this.ev = ev;

            pageIsDisappeared = false;
            this.Disappearing += pageOnDisappearing;

            eventTransactionsList = new ObservableCollection<Transaction>();

            initLayout();

            retrieveMemberFromServer();
            retrieveTransactionsFromServer(true);

            continuousRetrieveTransactionsFromServer(3000);
        }

        private async void retrieveMemberFromServer()
        {
            String eventID = ev.ID;

            eventMemberList = new ObservableCollection<User>();

            try
            {
                var usersInEvent = await App.firebase.Child("Veranstaltung_Benutzer").Child(eventID).OnceAsync<string>();

                foreach (FirebaseObject<string> e in usersInEvent)
                {
                    string userID = e.Key;
                    var userQuery =
                        await App.firebase.Child("users").OrderByKey().StartAt(userID).LimitToFirst(1).OnceAsync<User>();
                    User user = userQuery.ElementAt(0).Object;
                    user.ID = userID;
                    eventMemberList.Add(user);
                    System.Diagnostics.Debug.WriteLine($"Name of {userID} is {user.userName}");
                }

                updateMemberCostFrame();
            }
            catch (Exception e)
            {
                await DisplayAlert("Server Connection Failure", "Communication problems occured while querying!", "OK");
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        private void updateMemberCostFrame()
        {
            for (int i = 0; i < eventMemberList.Count; i++)
            {
                Frame eventMemberFrame = createListElementFrame(eventMemberList[i]);
                eventMemberCostInnerLayout.Children.Add(eventMemberFrame);
            }
        }
        private async Task<bool> retrieveTransactionsFromServer(bool firstRun)
        {
            String eventID = ev.ID;

            ObservableCollection<Transaction> newEventTransactionsList = new ObservableCollection<Transaction>();

            try
            {
                var transactionsInEvent = await App.firebase.Child("Transaktionen").Child(eventID).OnceAsync<Transaction>();

                foreach (FirebaseObject<Transaction> transaction in transactionsInEvent)
                {
                    var senderInfos = await App.firebase.Child("users").OrderByKey().StartAt(transaction.Object.sender).LimitToFirst(1).OnceAsync<User>();
                    transaction.Object.senderName = senderInfos.ElementAt(0).Object.userName;

                    var receiverInfos = await App.firebase.Child("users").OrderByKey().StartAt(transaction.Object.receiver).LimitToFirst(1).OnceAsync<User>();
                    transaction.Object.receiverName = receiverInfos.ElementAt(0).Object.userName;

                    newEventTransactionsList.Add(transaction.Object);
                }

                if (firstRun)
                {
                    eventTransactionsList = newEventTransactionsList;

                    updateTransactionFrame();
                    activityIndicatorSwitch();
                }
                else if (eventTransactionsList.Count != newEventTransactionsList.Count)
                {
                    Navigation.InsertPageBefore(new CostOverviewPage(ev), Navigation.NavigationStack[Navigation.NavigationStack.Count - 1]);
                    await Navigation.PopAsync();
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("Server Connection Failure", "Communication problems occured while querying!", "OK");
                System.Diagnostics.Debug.WriteLine(e);
            }

            return true;
        }
        private void updateTransactionFrame()
        {
            for (int i = 0; i < eventTransactionsList.Count; i++)
            {
                Frame transactionFrame = createTransactionElementFrame(eventTransactionsList[i]);
                transactionInnerLayout.Children.Add(transactionFrame);
            }
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
                Text =
                    "If you have something to pay, just click on someone with a minus, to balance the incurred costs.",
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

            eventMemberCostInnerLayout = new StackLayout();

            ScrollView eventMemberInnerScrollView = new ScrollView();
            eventMemberInnerScrollView.Content = eventMemberCostInnerLayout;
            eventMemberCostLayout.Children.Add(eventMemberInnerScrollView);

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

        private Frame createListElementFrame(User member)
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
                OnCostButtonClicked(sender, e, member);
            };
            horizontalLableAndButtonLayout.GestureRecognizers.Add(horizontalLayoutTapGestureRecognizer);

            Label memberLabel = new Label
            {
                Text = member.userName,
                TextColor = Color.White,
                FontSize = 20,
                Margin = new Thickness(0, 0, 0, 0),
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            horizontalLableAndButtonLayout.Children.Add(memberLabel);

            Button costButton = new Button()
            {
                Text = "...",
                BackgroundColor = Color.White,
                TextColor = Color.FromHex(App.GetMenueColor()),
                FontAttributes = FontAttributes.Bold,
                WidthRequest = 80,
                HeightRequest = 80,
                CornerRadius = 40,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };
            costButton.Clicked += (object sender, EventArgs e) =>
            {
                // handle the tap
                OnCostButtonClicked(sender, e, member);
            };
            horizontalLableAndButtonLayout.Children.Add(costButton);

            try
            {
                Task<int> callTask = Task.Run(() => EventCostCalculator.getPersonalCost(ev, App.GetUserID(), new Label(), costButton));
            }
            catch (Exception)
            {
                costButton.Text = "?";
            }

            listViewFrameLayout.Children.Add(horizontalLableAndButtonLayout);

            return listViewFrame;
        }
        private Frame createTransactionFrame()
        {
            StackLayout transactionLayout = new StackLayout();

            Label transactionLabel = createFrameHeaderLabel("Transactions");
            transactionLayout.Children.Add(transactionLabel);

            transactionInnerLayout = new StackLayout();

            ScrollView transactionInnerScrollView = new ScrollView();
            transactionInnerScrollView.Content = transactionInnerLayout;
            transactionLayout.Children.Add(transactionInnerScrollView);

            return new Frame
            {
                Content = transactionLayout,
                Padding = 4,
                BackgroundColor = Color.FromHex("376467"),
                CornerRadius = 5,
                HeightRequest = 200
            };
        }
        private Frame createTransactionElementFrame(Transaction transaction)
        {
            StackLayout transactionFrameLayout = new StackLayout();

            Frame transactionFrame = new Frame
            {
                Content = transactionFrameLayout,
                BackgroundColor = Color.LightGreen,
                CornerRadius = 5,
                Padding = 1
            };


            Label transactionLabel = new Label
            {
                Text = transaction.senderName + " sent " + transaction.receiverName + " " + transaction.amount + "€ by " + transaction.type,
                TextColor = Color.Black,
                FontSize = 15,
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

                if (ev.adminID.Equals(App.GetUserID()) && eventTransactionsList.Count == 0)
                    stackLayout.Children.Add(restartEventButton);
            }
            else
            {
                activityIndicator.IsRunning = true;

                if (ev.adminID.Equals(App.GetUserID()) && eventTransactionsList.Count == 0)
                    stackLayout.Children.Remove(restartEventButton);

                stackLayout.Children.Remove(transactionsFrame);
                stackLayout.Children.Remove(eventMemberCostFrame);

                stackLayout.Children.Add(activityIndicator);
            }
        }

        async void OnCostButtonClicked(object sender, EventArgs e, User member)
        {
            string action;
            action = await DisplayActionSheet("Payment Options", "Cancel", null, "PayPal","Cash");
            switch (action)
            {
                case "PayPal":
                    
                    break;
                case "Cash":

                    break;
                default:
                    break;
            }
        }
        async void OnRestartEventButtonClicked(object sender, EventArgs e)
        {
            try
            {
                await App.firebase.Child("Veranstaltungen").Child(ev.ID).Child("Status").PutAsync<string>("start");
            }
            catch (Exception)
            {
                await DisplayAlert("Server connection failure", "Communication problems occured while querying", "OK");
                System.Diagnostics.Debug.WriteLine(e);
            }

            await Navigation.PushAsync(new EventsPage
            {
                Title = "Events"
            });
            clearNavigationStack();
        }
        private void clearNavigationStack()
        {
            var navigationPages = Navigation.NavigationStack.ToList();
            foreach (var page in navigationPages)
            {
                Navigation.RemovePage(page);
            }
        }

        private async void continuousRetrieveTransactionsFromServer(int ms)
        {
            await Task.Delay(ms);                   //only for wait until the first retrieveTransactionsFromServer(true); ist finished
            while (!pageIsDisappeared)
            {
                //await Task.Delay(ms);                                               //not really needed anymore, because of await and continuous reading from database
                await retrieveTransactionsFromServer(false);
                System.Diagnostics.Debug.WriteLine("Refresh" + reloadCount);
                reloadCount++;
            }

            System.Diagnostics.Debug.WriteLine("Refresh End");
        }
        async void pageOnDisappearing(object sender, EventArgs e)
        {
            pageIsDisappeared = true;
        }
    }
}