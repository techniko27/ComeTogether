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
        private ToDo toDo;
        private const int FONT_SIZE = 17;

        public ToDoDetailsPage(ToDo toDo)
        {
            InitializeComponent();

            this.toDo = toDo;

            initProperties();
            initLayout();
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

            Image infoImage = new Image
            {
                Aspect = Aspect.AspectFit,
                Source = "drei_punkte_weiss.png",
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.Start,
                Scale = 0.75
            };
            overviewHeaderLayout.Children.Add(infoImage);
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
                Text = "Assigned Members:",
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
                Text = "Total Cost:",
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
                Text = "Individual Costs:",
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

            Entry todoNameEntry = new Entry
            {
                Placeholder = "Name...",
                Text = toDo.Name,
                TextColor = Color.White,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                FontSize = FONT_SIZE
            };

            Entry todoDescriptionEntry = new Entry
            {
                Placeholder = "Description...",
                Text = toDo.Beschreibung + Environment.NewLine,
                TextColor = Color.White,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                FontSize = FONT_SIZE
            };

            Entry todoPlaceEntry = new Entry
            {
                Placeholder = "Place...",
                Text = toDo.Ort,
                TextColor = Color.White,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                FontSize = FONT_SIZE
            };

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

            Entry costEntry = new Entry
            {
                Placeholder = "Cost in €",
                Text = toDo.Kosten.ToString(),
                TextColor = Color.White,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                FontSize = FONT_SIZE
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
            StackLayout assignCostsContentLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Start,
            };

            Frame assignCostsContentFrame = new Frame
            {
                Content = assignCostsContentLayout,
                BackgroundColor = Color.FromHex(App.GetMenueColor()),
                CornerRadius = 5,
                Padding = 5
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