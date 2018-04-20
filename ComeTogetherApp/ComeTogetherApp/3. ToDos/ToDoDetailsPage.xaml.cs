using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Rg.Plugins.Popup.Extensions;
using Firebase.Database;
using Firebase.Database.Query;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComeTogetherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ToDoDetailsPage : ContentPage
    {
        private ToDo toDo;
        private Event ev;
        private const int FONT_SIZE = 17;

        private User assignedUser;
        private ObservableCollection<User> eventMemberList;
        public ObservableCollection<User> assignedCostsList;

        private ObservableCollection<User> removedPayingUsers;

        private StackLayout assignCostsContentLayout;
        private Frame assignCostsContentFrame;

        private AssignedMembersListCell assignedUserCell;

        private Entry costEntry;
        private Entry todoNameEntry;
        private Entry todoDescriptionEntry;
        private Entry todoPlaceEntry;
        private Picker statusPicker;

        public ToDoDetailsPage(ToDo toDo, Event ev, User assignedUser)
        {
            InitializeComponent();

            this.toDo = toDo;
            this.ev = ev;

            this.assignedUser = assignedUser;

            assignedCostsList = new ObservableCollection<User>();
            eventMemberList = new ObservableCollection<User>();
            removedPayingUsers = new ObservableCollection<User>();
            assignedCostsList.CollectionChanged += costAssigned;

            initProperties();
            initLayout();

            retrieveAssignedMembersFromServer(ev);
        }

        private void costAssigned(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            // add new users
            if(e.NewItems != null)
            {
                foreach (User item in e.NewItems)
                {
                    AssignedCostsEntry costEntry = new AssignedCostsEntry(this);
                    costEntry.BindingContext = item;
                    assignCostsContentLayout.Children.Insert(0, costEntry);
                    if (removedPayingUsers.Contains(item))
                        removedPayingUsers.Remove(item);
                }
            }
            // remove old users
            if(e.OldItems != null)
            {
                foreach (User user in e.OldItems)
                {
                    AssignedCostsEntry entryToRemove = null;
                    foreach (AssignedCostsEntry entry in assignCostsContentLayout.Children)
                    {
                        if (entry.user.Equals(user))
                        {
                            entryToRemove = entry;
                        }
                    }
                    if (entryToRemove != null)
                    {
                        assignCostsContentLayout.Children.Remove(entryToRemove);
                        removedPayingUsers.Add(user);
                    }
                }
            }
            // adjust height of encompassing frame according to the number of entries in the list
            int newHeight = assignCostsContentLayout.Children.Count * 40;
            newHeight = newHeight > 200 ? newHeight : 200;
            assignCostsContentFrame.HeightRequest = newHeight;
        }

        private void initProperties()
        {
            Title = "ToDo Details";
            BackgroundColor = Color.White;
        }

        private void initLayout()
        {
            ScrollView scrollView = new ScrollView();
            StackLayout stackLayout = createStackLayout();
            scrollView.Content = stackLayout;

            Content = scrollView;
        }
        private StackLayout createStackLayout()
        {
            StackLayout stackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                Padding = new Thickness(10, 10, 10, 5)
            };

            Frame overviewFrame = createOverviewFrame();
            Frame assignMembersFrame = createAssignMembersFrame();
            Frame currentCostsFrame = createCurrentCostsFrame();
            Frame assignCostsFrame = createAssignCostsFrame();

            stackLayout.Children.Add(overviewFrame);
            stackLayout.Children.Add(assignMembersFrame);
            stackLayout.Children.Add(currentCostsFrame);
            stackLayout.Children.Add(assignCostsFrame);

            return stackLayout;
        }
        private Frame createOverviewFrame()
        {
            StackLayout overviewLayout = new StackLayout();

            // The header with the title and additional button on the right
            StackLayout overviewHeaderLayout = createOverviewHeaderLayout();

            // The actual content with the toDo info
            Frame overviewContentFrame = createOverviewContentFrame();

            overviewLayout.Children.Add(overviewHeaderLayout);
            overviewLayout.Children.Add(overviewContentFrame);

            // Put it together in a single frame
            Frame overviewFrame = createInfoFrame(overviewLayout);

            return overviewFrame;
        }

        private Frame createAssignMembersFrame()
        {
            StackLayout assignMembersLayout = new StackLayout();

            // The header with the title and additional button on the right
            StackLayout assignMembersHeaderLayout = createAssignMembersHeaderLayout();

            // The actual content with the member info
            Frame assignMembersContentFrame = createAssignMembersContentFrame();

            assignMembersLayout.Children.Add(assignMembersHeaderLayout);
            assignMembersLayout.Children.Add(assignMembersContentFrame);

            // Put it together in a single frame
            Frame assignMembersFrame = createInfoFrame(assignMembersLayout);

            return assignMembersFrame;
        }

        private Frame createCurrentCostsFrame()
        {
            StackLayout currentCostsLayout = new StackLayout();

            // The header with the title and additional button on the right
            StackLayout currentCostsHeaderLayout = createCurrentCostsHeaderLayout();

            // The actual content with the current cost info
            Frame currentCostsContentFrame = createCurrentCostsContentFrame();

            currentCostsLayout.Children.Add(currentCostsHeaderLayout);
            currentCostsLayout.Children.Add(currentCostsContentFrame);

            // Put it together in a single frame
            Frame currentCostsFrame = createInfoFrame(currentCostsLayout);

            return currentCostsFrame;
        }
        private Frame createAssignCostsFrame()
        {
            StackLayout assignCostsLayout = new StackLayout();

            // The header with the title and additional button on the right
            StackLayout assignCostsHeaderLayout = createAssignCostsHeaderLayout();

            // The actual content with the cost info
            Frame assignCostsContentFrame = createAssignCostsContentFrame();

            assignCostsLayout.Children.Add(assignCostsHeaderLayout);
            assignCostsLayout.Children.Add(assignCostsContentFrame);

            // Put it together in a single frame
            Frame assignCostsFrame = createInfoFrame(assignCostsLayout);

            return assignCostsFrame;
        }

        private StackLayout createOverviewHeaderLayout()
        {
            StackLayout overviewHeaderLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, 0, 0, 0),
                HeightRequest = 30
            };

            Label infoLabel = new Label
            {
                Text = "ToDo Information:",
                FontSize = 20,
                TextColor = Color.White,
                Margin = new Thickness(10, 0, 0, 0),
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            overviewHeaderLayout.Children.Add(infoLabel);

            StackLayout buttonLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                Margin = new Thickness(0, 0, 0, 0),
                HeightRequest = 30,
                HorizontalOptions = LayoutOptions.EndAndExpand
            };

            Image saveImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "speichern_weis.png",
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Scale = 0.75
            };

            Image infoImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "drei_punkte_weiss.png",
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Scale = 0.75
            };
            buttonLayout.Children.Add(saveImage);
            buttonLayout.Children.Add(infoImage);
            overviewHeaderLayout.Children.Add(buttonLayout);

            var saveImageTapGestureRecognizer = new TapGestureRecognizer();
            saveImageTapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
            {
                saveChanges();
            };
            saveImage.GestureRecognizers.Add(saveImageTapGestureRecognizer);

            var infoImageTapGestureRecognizer = new TapGestureRecognizer();
            infoImageTapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
            {
                // handle the tap
                OnInfoImageClicked(sender, e);
            };
            infoImage.GestureRecognizers.Add(infoImageTapGestureRecognizer);

            return overviewHeaderLayout;
        }

        private StackLayout createAssignMembersHeaderLayout()
        {
            StackLayout assignMembersHeaderLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, 0, 0, 0),
                HeightRequest = 30
            };

            Label assignMemberLabel = new Label
            {
                Text = "Assigned Member:",
                FontSize = 20,
                TextColor = Color.White,
                Margin = new Thickness(10, 0, 0, 0),
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            assignMembersHeaderLayout.Children.Add(assignMemberLabel);

            Image plusImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "kreis_plus_weiss.png",
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Scale = 0.75
            };
            assignMembersHeaderLayout.Children.Add(plusImage);
            var infoImageTapGestureRecognizer = new TapGestureRecognizer();
            infoImageTapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
            {
                // handle the tap
                OnAssignMemberClicked(sender, e);
            };
            plusImage.GestureRecognizers.Add(infoImageTapGestureRecognizer);

            return assignMembersHeaderLayout;
        }

        private StackLayout createCurrentCostsHeaderLayout()
        {
            StackLayout currentCostsHeaderLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, 0, 0, 0),
                HeightRequest = 30
            };

            Label currentCostsLabel = new Label
            {
                Text = "Total Cost: " + toDo.Kosten.ToString(),
                FontSize = 20,
                TextColor = Color.White,
                Margin = new Thickness(10, 0, 0, 0),
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            currentCostsHeaderLayout.Children.Add(currentCostsLabel);

            return currentCostsHeaderLayout;
        }

        private StackLayout createAssignCostsHeaderLayout()
        {
            StackLayout horizontalStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, 0, 0, 0),
                HeightRequest = 30
            };

            Label assignCostLabel = new Label
            {
                Text = $"Paying Members:",
                FontSize = 20,
                TextColor = Color.White,
                Margin = new Thickness(10, 0, 0, 0),
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            horizontalStackLayout.Children.Add(assignCostLabel);

            Image plusImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "kreis_plus_weiss.png",
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Scale = 0.75
            };
            horizontalStackLayout.Children.Add(plusImage);
            var infoImageTapGestureRecognizer = new TapGestureRecognizer();
            infoImageTapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
            {
                // handle the tap
                OnAssignCostClicked(sender, e);
            };
            plusImage.GestureRecognizers.Add(infoImageTapGestureRecognizer);

            return horizontalStackLayout;
        }

        private Frame createOverviewContentFrame()
        {
            StackLayout infoLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
            };

            todoNameEntry = new Entry
            {
                Placeholder = "Name...",
                Text = toDo.Name,
                TextColor = Color.White,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                FontSize = FONT_SIZE
            };

            todoDescriptionEntry = new Entry
            {
                Placeholder = "Description...",
                Text = toDo.Beschreibung,
                TextColor = Color.White,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                FontSize = FONT_SIZE
            };

            todoPlaceEntry = new Entry
            {
                Placeholder = "Place...",
                Text = toDo.Ort,
                TextColor = Color.White,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                FontSize = FONT_SIZE
            };

            statusPicker = new Picker
            {
                Title = "Status",
                TextColor = Color.White,
                BackgroundColor = Color.FromHex(App.GetMenueColor())
            };

            statusPicker.Items.Add("In Progress");
            statusPicker.Items.Add("Completed");

            statusPicker.SelectedItem = toDo.Status;

            int year;
            Int32.TryParse(toDo.Datum.Substring(0, 4), out year);
            int month;
            Int32.TryParse(toDo.Datum.Substring(5, 2), out month);
            int day;
            Int32.TryParse(toDo.Datum.Substring(8, 2), out day);

            DateTime dt = new DateTime(year, month, day);
            DatePicker todoDateEntry = new DatePicker()
            {
                Format = "D",
                TextColor = Color.White,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                Date = dt,
            };

            infoLayout.Children.Add(todoNameEntry);
            infoLayout.Children.Add(todoDescriptionEntry);
            infoLayout.Children.Add(todoPlaceEntry);
            infoLayout.Children.Add(todoDateEntry);
            infoLayout.Children.Add(statusPicker);

            Frame overviewContentFrame = new Frame
            {
                Content = infoLayout,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5,
                Padding = 5
            };
            return overviewContentFrame;
        }

        private Frame createAssignMembersContentFrame()
        {
            StackLayout assignMembersContentLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
            };

            assignedUserCell = new AssignedMembersListCell(ev);
            assignedUserCell.BindingContext = assignedUser;

            assignMembersContentLayout.Children.Add(assignedUserCell);

            Frame assignMembersContentFrame = new Frame
            {
                Content = assignMembersContentLayout,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5,
                Padding = 5
            };
            return assignMembersContentFrame;
        }

        private Frame createCurrentCostsContentFrame()
        {
            StackLayout currentCostsContentLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
            };

            costEntry = new Entry
            {
                Placeholder = "Cost in €",
                Text = toDo.Kosten.ToString(),
                TextColor = Color.White,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                FontSize = 20
            };
            currentCostsContentLayout.Children.Add(costEntry);

            Frame currentCostsContentFrame = new Frame
            {
                Content = currentCostsContentLayout,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5,
                Padding = 5
            };
            return currentCostsContentFrame;
        }

        private Frame createAssignCostsContentFrame()
        {
            assignCostsContentLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                BackgroundColor = Color.FromHex(App.GetMenueColor())
            };

            assignCostsContentFrame = new Frame
            {
                Content = assignCostsContentLayout,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5,
                Padding = 5,
                HeightRequest = 200
            };
            return assignCostsContentFrame;
        }

        private Frame createInfoFrame(StackLayout layout)
        {
            return new Frame
            {
                Content = layout,
                Padding = 0,
                BackgroundColor = Color.FromHex("376467"),
                CornerRadius = 5
            };
        }

        public void assignMemberToToDo(User user)
        {
            assignedUser = user;
            toDo.OrganisatorID = assignedUser.ID;
            assignedUserCell.BindingContext = assignedUser;
        }

        async void OnAssignMemberClicked(object sender, EventArgs e)
        {
            await Navigation.PushPopupAsync(new AssignMemberToDoPopUp(ev, toDo, this));
        }
        async void OnAssignCostClicked(object sender, EventArgs e)
        {
           await Navigation.PushPopupAsync(new AssignCostToDoPopUp(eventMemberList, assignedCostsList));
        }

        async void OnInfoImageClicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("ToDo Options", "Cancel", null, "Delete ToDo");
            switch (action)
            {
                case "Delete ToDo":
                    bool answer = await DisplayAlert("Are you sure?", $"Do you really want to delete this ToDo?", "Yes", "No");
                    if (answer)
                        deleteToDo();
                    break;
                default:
                    break;
            }
        }

        private async void retrieveAssignedMembersFromServer(Event ev)
        {
            String eventID = ev.ID;
            String toDoID = toDo.ID;

            try
            {
                var payingUsersInToDo = await App.firebase.Child("ToDo_Benutzer").Child(toDoID).OnceAsync<string>();

                foreach (FirebaseObject<string> e in payingUsersInToDo)
                {
                    string userID = e.Key;
                    var userQuery = await App.firebase.Child("users").OrderByKey().StartAt(userID).LimitToFirst(1).OnceAsync<User>();

                    User user = userQuery.ElementAt(0).Object;
                    user.ID = userID;

                    assignedCostsList.Add(user);
                }

                var usersInEvent = await App.firebase.Child("Veranstaltung_Benutzer").Child(eventID).OnceAsync<string>();

                foreach (FirebaseObject<string> e in usersInEvent)
                {
                    string userID = e.Key;
                    var userQuery = await App.firebase.Child("users").OrderByKey().StartAt(userID).LimitToFirst(1).OnceAsync<User>();
                    User user = userQuery.ElementAt(0).Object;
                    user.ID = userID;
                    eventMemberList.Add(user);
                }
            }
            catch (Exception e)
            {
                await DisplayAlert("Server connection failure", "Communication problems occured while querying", "OK");
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        private async void saveChanges()
        {
            LoadingPopUp loadingPopUp = new LoadingPopUp();
            await Navigation.PushPopupAsync(loadingPopUp);
            try
            {
                foreach (User user in assignedCostsList)
                {
                    await App.firebase.Child("ToDo_Benutzer").Child(toDo.ID).Child(user.ID).PutAsync<string>(user.userName);
                    await App.firebase.Child("Benutzer_ToDo").Child(user.ID).Child(ev.ID).Child(toDo.ID).Child("isPaying").PutAsync<string>("true");
                }

                foreach (User user in removedPayingUsers)
                {
                    await App.firebase.Child("ToDo_Benutzer").Child(toDo.ID).Child(user.ID).DeleteAsync();
                    await App.firebase.Child("Benutzer_ToDo").Child(user.ID).Child(ev.ID).Child(toDo.ID).Child("isPaying").PutAsync<string>("false");
                }

                int cost;
                Int32.TryParse(costEntry.Text, out cost);
                toDo.Kosten = cost;
                toDo.Beschreibung = todoDescriptionEntry.Text;
                toDo.Name = todoNameEntry.Text;
                toDo.Status = statusPicker.SelectedItem.ToString();

                await App.firebase.Child("ToDos").Child(toDo.ID).PutAsync(toDo);

                await Navigation.PopPopupAsync();

                var navStack = Navigation.NavigationStack;

                Navigation.RemovePage(navStack.ElementAt(navStack.Count - 2));
                await Navigation.PushAsync(new SingleEventPage(ev));

                Navigation.RemovePage(navStack.ElementAt(navStack.Count - 2));
            }
            catch (Exception e)
            {
                await DisplayAlert("Server Connection Failure", "Communication problems occurred while saving changes!", "OK");
                System.Diagnostics.Debug.WriteLine(e);
            }
        }

        private async void deleteToDo()
        {
            LoadingPopUp loadingPopUp = new LoadingPopUp();
            await Navigation.PushPopupAsync(loadingPopUp);
            try
            {
                foreach (User user in assignedCostsList)
                {
                    await App.firebase.Child("ToDo_Benutzer").Child(toDo.ID).Child(user.ID).DeleteAsync();
                    await App.firebase.Child("Benutzer_ToDo").Child(user.ID).Child(ev.ID).Child(toDo.ID).DeleteAsync();
                }

                foreach (User user in removedPayingUsers)
                {
                    await App.firebase.Child("ToDo_Benutzer").Child(toDo.ID).Child(user.ID).DeleteAsync();
                    await App.firebase.Child("Benutzer_ToDo").Child(user.ID).Child(ev.ID).Child(toDo.ID).DeleteAsync();
                }

                await App.firebase.Child("ToDos").Child(toDo.ID).DeleteAsync();

                await Navigation.PopPopupAsync();

                var navStack = Navigation.NavigationStack;

                Navigation.RemovePage(navStack.ElementAt(navStack.Count - 2));
                await Navigation.PushAsync(new SingleEventPage(ev));

                Navigation.RemovePage(navStack.ElementAt(navStack.Count - 2));
            }
            catch (Exception e)
            {
                await DisplayAlert("Server Connection Failure", "Communication problems occurred while deleting the ToDo!", "OK");
                System.Diagnostics.Debug.WriteLine(e);
            }
        }
    }
}