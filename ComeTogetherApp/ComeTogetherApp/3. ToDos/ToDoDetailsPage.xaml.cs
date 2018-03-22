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
    public partial class ToDoDetailsPage : ContentPage
    {
        private ToDo todo;
        private int fontSize;
        public ToDoDetailsPage()
        {
            InitializeComponent();

            fontSize = 17;
            this.todo = new ToDo("Beschreibung...", "2017-12-06", "Name...", "15€", "status...", "Ort..");

            Title = "ToDo details";
            BackgroundColor = Color.White;

            ScrollView scrollView = new ScrollView();
            StackLayout stackLayout = createStackLayout();
            scrollView.Content = stackLayout;

            Content = scrollView;
        }

        private StackLayout createStackLayout()
        {
            StackLayout stackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(10, 10, 10, 5)
            };

            StackLayout overviewInfoLayout = createOverviewInfoLayout();
            Frame overviewInfoFrame = new Frame
            {
                Content = overviewInfoLayout,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5
            };
            stackLayout.Children.Add(overviewInfoFrame);

            StackLayout editingMemberLayout = createAssignMemberLayout();
            Frame editingMemberFrame = new Frame
            {
                Content = editingMemberLayout,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5
            };
            stackLayout.Children.Add(editingMemberFrame);

            StackLayout editingCostLayout = createEditingCostLayout();
            Frame editingCostFrame = new Frame
            {
                Content = editingCostLayout,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5
            };
            stackLayout.Children.Add(editingCostFrame);

            StackLayout assignCostLayout = createAssignCostLayout();
            Frame assignCostFrame = new Frame
            {
                Content = assignCostLayout,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5
            };
            stackLayout.Children.Add(assignCostFrame);

            return stackLayout;
        }

        private StackLayout createOverviewInfoLayout()
        {
            StackLayout infoLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(0, 0, 0, 0),
                Margin = new Thickness(-10, -10, -10, 5),
            };

            StackLayout horizontalStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, 0, 0, 0),
                HeightRequest = 30
            };

            Label infoLabel = new Label
            {
                Text = "ToDo Info",
                FontSize = 20,
                TextColor = Color.White,
                Margin = new Thickness(10, 0, 0, 0),
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            horizontalStackLayout.Children.Add(infoLabel);

            Image infoImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "drei_punkte_weiss.png",
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Scale = 0.75
            };
            horizontalStackLayout.Children.Add(infoImage);
            var infoImageTapGestureRecognizer = new TapGestureRecognizer();
            infoImageTapGestureRecognizer.Tapped += (object sender, EventArgs e) =>
            {
                // handle the tap
                OnInfoImageClicked(sender, e);
            };
            infoImage.GestureRecognizers.Add(infoImageTapGestureRecognizer);

            Frame horizontalFrame = new Frame()
            {
                Content = horizontalStackLayout,
                BackgroundColor = Color.FromHex("376467"),
                Margin = new Thickness(-10, -10, -10, 0),
                Padding = 0,
                CornerRadius = 5,
            };

            infoLayout.Children.Add(horizontalFrame);

            Entry todoNameEntry = new Entry
            {
                Placeholder = "Name",
                Text = todo.Name,
                TextColor = Color.White,
                FontSize = fontSize
            };

            Entry todoDescriptionEntry = new Entry
            {
                Placeholder = "Description",
                Text = todo.Beschreibung + Environment.NewLine,
                TextColor = Color.White,
                FontSize = fontSize
            };

            Entry todoPlaceEntry = new Entry
            {
                Placeholder = "Place",
                Text = "Place: " + todo.Ort,
                TextColor = Color.White,
                FontSize = fontSize
            };

            int year;
            Int32.TryParse(todo.Datum.Substring(0, 4), out year);
            int month;
            Int32.TryParse(todo.Datum.Substring(5, 2), out month);
            int day;
            Int32.TryParse(todo.Datum.Substring(8, 2), out day);

            DateTime dt = new DateTime(year, month, day);
            DatePicker todoDateEntry = new DatePicker()
            {
                Format = "D",
                TextColor = Color.White,
                Date = dt,
                //HeightRequest = 35,
            };

            infoLayout.Children.Add(todoNameEntry);
            //infoLayout.Children.Add(createSeparator(2));
            infoLayout.Children.Add(todoDescriptionEntry);
            //infoLayout.Children.Add(createSeparator(1));
            infoLayout.Children.Add(todoPlaceEntry);
            infoLayout.Children.Add(todoDateEntry);

            return infoLayout;
        }
        private static BoxView createSeparator(int height)
        {
            return new BoxView()
            {
                Color = Color.Black,
                WidthRequest = 1000,
                HeightRequest = height,
                HorizontalOptions = LayoutOptions.Center
            };
        }

        private StackLayout createAssignMemberLayout()
        {
            StackLayout assignMemberLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(0, 0, 0, 0),
                Margin = new Thickness(-10, -10, -10, 5),
            };

            StackLayout horizontalStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, 0, 0, 0),
                HeightRequest = 30
            };

            Frame horizontalFrame = new Frame()
            {
                Content = horizontalStackLayout,
                BackgroundColor = Color.FromHex("376467"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(-10, -10, -10, 0),
                Padding = 0,
                CornerRadius = 5,
            };
            assignMemberLayout.Children.Add(horizontalFrame);


            Label assignMemberLabel = new Label
            {
                Text = "Assign Member",
                FontSize = 20,
                TextColor = Color.White,
                Margin = new Thickness(10, 0, 0, 0),
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            horizontalStackLayout.Children.Add(assignMemberLabel);

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
                OnAssignMemberClicked(sender, e);
            };
            plusImage.GestureRecognizers.Add(infoImageTapGestureRecognizer);

            return assignMemberLayout;
        }

        private StackLayout createEditingCostLayout()
        {
            StackLayout editingCostLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(0, 0, 0, 0),
                Margin = new Thickness(-10, -10, -10, 5),
            };

            //costs incurred
            StackLayout horizontalStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, 0, 0, 0),
                HeightRequest = 30
            };

            Frame horizontalFrame = new Frame()
            {
                Content = horizontalStackLayout,
                BackgroundColor = Color.FromHex("376467"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(-10, -10, -10, 0),
                Padding = 0,
                CornerRadius = 5,
            };
            editingCostLayout.Children.Add(horizontalFrame);
            
            Label editingCostLabel = new Label
            {
                Text = "Costs incurred",
                FontSize = 20,
                TextColor = Color.White,
                Margin = new Thickness(10, 0, 0, 0),
                HorizontalOptions = LayoutOptions.StartAndExpand
            };
            horizontalStackLayout.Children.Add(editingCostLabel);

            Entry costEntry = new Entry
            {
                Placeholder = "Cost in €",
                Text = todo.Kosten,
                TextColor = Color.White,
                FontSize = fontSize
            };
            editingCostLayout.Children.Add(costEntry);

            return editingCostLayout;
        }

        private StackLayout createAssignCostLayout()
        {
            StackLayout assignCostLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
                Padding = new Thickness(0, 0, 0, 0),
                Margin = new Thickness(-10, -10, -10, 5),
            };

            StackLayout horizontalStackLayout = new StackLayout
            {
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(0, 0, 0, 0),
                HeightRequest = 30
            };

            Frame horizontalFrame = new Frame()
            {
                Content = horizontalStackLayout,
                BackgroundColor = Color.FromHex("376467"),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(-10, -10, -10, 0),
                Padding = 0,
                CornerRadius = 5,
            };
            assignCostLayout.Children.Add(horizontalFrame);


            Label assignCostLabel = new Label
            {
                Text = "Assign Cost",
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

            return assignCostLayout;
        }

        async void OnAssignMemberClicked(object sender, EventArgs e)
        {
            await DisplayAlert("EditMember", "", "Ok");
        }
        async void OnAssignCostClicked(object sender, EventArgs e)
        {
            await DisplayAlert("EditCost", "", "Ok");
        }

        async void OnInfoImageClicked(object sender, EventArgs e)
        {
            await DisplayAlert("InfoImage","","Ok");
        }
    }
}