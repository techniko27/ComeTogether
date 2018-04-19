using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComeTogetherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventDetailsToDosPage : ContentPage
    {
        private ObservableCollection<ToDo> ownToDosList;
        private ObservableCollection<ToDo> otherToDosList;
        private ObservableCollection<ToDo> completedToDosList;

        private ActivityIndicator activityIndicator;
        private SearchBar memberSearchBar;

        private StackLayout stackLayout;
        private Frame ownToDosFrame;
        private Frame otherToDosFrame;
        private Frame completedToDosFrame;

        private ListView ownToDosListView;
        private ListView otherToDosListView;
        private ListView completedToDosListView;

        public Event ev;

        public EventDetailsToDosPage(Event ev)
        {
            InitializeComponent();

            this.ev = ev;

            initProperties();
            initLayout(ev);

            retrieveToDosFromServer(ev);
        }

        private async void retrieveToDosFromServer(Event ev)
        {
            String eventID = ev.ID;

            try
            {
                var toDosInEvent = await App.firebase.Child("Veranstaltung_ToDo").Child(eventID).OnceAsync<string>();
                var ownToDosInEvent = await App.firebase.Child("Benutzer_ToDo").Child(App.GetUserID()).Child(eventID).OnceAsync<object>();

                foreach (FirebaseObject<object> tD in ownToDosInEvent)
                {
                    string toDoID = tD.Key;
                    var toDoQuery = await App.firebase.Child("ToDos").OrderByKey().StartAt(toDoID).LimitToFirst(1).OnceAsync<ToDo>();
                    ToDo toDo = toDoQuery.ElementAt(0).Object;
                    toDo.ID = toDoID;

                    if (!toDo.Status.Equals("Completed"))
                        ownToDosList.Add(toDo);
                    else
                        completedToDosList.Add(toDo);
                }

                foreach (FirebaseObject<string> tD in toDosInEvent)
                {
                    string toDoID = tD.Key;
                    if (isInOwnToDoList(toDoID))
                        continue;
                    var toDoQuery = await App.firebase.Child("ToDos").OrderByKey().StartAt(toDoID).LimitToFirst(1).OnceAsync<ToDo>();
                    ToDo toDo = toDoQuery.ElementAt(0).Object;

                    if (!toDo.Status.Equals("Completed"))
                        otherToDosList.Add(toDo);
                    else
                        completedToDosList.Add(toDo);
                }
                activityIndicatorSwitch();
            }
            catch (Exception e)
            {
                await DisplayAlert("Server Connection Failure", "Communication problems occured while querying!", "OK");
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        private bool isInOwnToDoList(string toDoID)
        {
            foreach(ToDo toDo in ownToDosList)
            {
                if (toDo.ID.Equals(toDoID))
                    return true;
            }
            return false;
        }

        private void initProperties()
        {
            Title = "ToDos";
        }

        private void initLayout(Event ev)
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

            stackLayout = createStackLayout(ev);
            scrollView.Content = stackLayout;

            Content = scrollView;
        }

        private StackLayout createStackLayout(Event ev)
        {
            StackLayout stackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                Padding = new Thickness(10, 10, 10, 5)
            };

            memberSearchBar = new SearchBar
            {
                Placeholder = "Search ToDos..."
            };

           // memberSearchBar.TextChanged += searchBarTextChanged;

            Frame searchbarFrame = new Frame
            {
                Content = memberSearchBar,
                BackgroundColor = Color.LightGray,
                CornerRadius = 5,
                Padding = new Thickness(5, 0, 5, 10)
            };

            ownToDosFrame = createOwnToDosFrame();
            otherToDosFrame = createOtherToDosFrame();
            completedToDosFrame = createCompletedToDosFrame();

            stackLayout.Children.Add(searchbarFrame);
            stackLayout.Children.Add(activityIndicator);

            return stackLayout;
        }

        private Frame createOwnToDosFrame()
        {
            StackLayout ownToDosLayout = new StackLayout();

            Label ownToDosLabel = createFrameHeaderLabel("My ToDos:");

            ownToDosList = new ObservableCollection<ToDo>();
            ownToDosListView = createToDosListView(ownToDosList);

            Frame ownToDosListFrame = createListViewFrame(ownToDosListView);

            ownToDosLayout.Children.Add(ownToDosLabel);
            ownToDosLayout.Children.Add(ownToDosListFrame);

            Frame ownToDosFrame = createToDosFrame(ownToDosLayout);

            return ownToDosFrame;
        }

        private Frame createOtherToDosFrame()
        {
            StackLayout otherToDosLayout = new StackLayout();

            Label otherToDosLabel = createFrameHeaderLabel("Other ToDos:");

            otherToDosList = new ObservableCollection<ToDo>();
            otherToDosListView = createToDosListView(otherToDosList);

            Frame otherToDosListFrame = createListViewFrame(otherToDosListView);

            otherToDosLayout.Children.Add(otherToDosLabel);
            otherToDosLayout.Children.Add(otherToDosListFrame);

            Frame otherToDosFrame = createToDosFrame(otherToDosLayout);

            return otherToDosFrame;
        }

        private Frame createCompletedToDosFrame()
        {
            StackLayout completedToDosLayout = new StackLayout();

            Label completedToDosLabel = createFrameHeaderLabel("Completed ToDos:");

            completedToDosList = new ObservableCollection<ToDo>();
            completedToDosListView = createToDosListView(completedToDosList);

            Frame completedToDosListFrame = createListViewFrame(completedToDosListView);

            completedToDosLayout.Children.Add(completedToDosLabel);
            completedToDosLayout.Children.Add(completedToDosListFrame);

            Frame completedToDosFrame = createToDosFrame(completedToDosLayout);

            return completedToDosFrame;
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

        private ListView createToDosListView(ObservableCollection<ToDo> itemSource)
        {
            ListView memberList = new ListView
            {
                ItemsSource = itemSource,
                ItemTemplate = new DataTemplate(() =>
                {
                    return new ToDoListCell(this);
                }),
                Margin = new Thickness(0, 0, 0, 10),
                BackgroundColor = Color.White,
                SeparatorColor = Color.LightSlateGray
            };
            return memberList;
        }

        private Frame createListViewFrame(ListView listView)
        {
            ScrollView scrollableList = new ScrollView
            {
                Content = listView
            };
            Frame whiteListFrame = new Frame
            {
                Content = scrollableList,
                BackgroundColor = Color.White,
                CornerRadius = 5,
                Padding = 0
            };

            Frame listViewFrame = new Frame
            {
                Content = whiteListFrame,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5,
                Padding = 5
            };
            return listViewFrame;
        }

        private Frame createToDosFrame(StackLayout toDoLayout)
        {
            return new Frame
            {
                Content = toDoLayout,
                Padding = 0,
                BackgroundColor = Color.FromHex("376467"),
                CornerRadius = 5,
                HeightRequest = 200
            };
        }

        private void activityIndicatorSwitch()
        {
            if (activityIndicator.IsRunning)
            {
                activityIndicator.IsRunning = false;

                stackLayout.Children.Remove(activityIndicator);

                stackLayout.Children.Add(ownToDosFrame);
                stackLayout.Children.Add(otherToDosFrame);
                stackLayout.Children.Add(completedToDosFrame);
            }
            else
            {
                activityIndicator.IsRunning = true;

                stackLayout.Children.Remove(ownToDosFrame);
                stackLayout.Children.Remove(otherToDosFrame);
                stackLayout.Children.Remove(completedToDosFrame);

                stackLayout.Children.Add(activityIndicator);
            }
        }
    }
}