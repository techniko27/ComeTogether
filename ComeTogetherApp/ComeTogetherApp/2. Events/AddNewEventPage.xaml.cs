using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ComeTogetherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddNewEventPage : ContentPage
    {
        public AddNewEventPage()
        {
            InitializeComponent();

            Title = "Add new event";

            ScrollView scroll = new ScrollView();

            StackLayout stack = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(2, 2, 2, 2)
            };
            Image eventImage = new Image { Aspect = Aspect.AspectFit, VerticalOptions = LayoutOptions.Start };
            eventImage.Source = "icon.png";
            Label eventNameLabel = new Label
            {
                Text = "Name",
                FontSize = 20,
                TextColor = Color.Black
            };
            Entry nameEntry = new Entry()
            {
                
            };
            Label eventDescriptionLabel = new Label
            {
                Text = "Description",
                FontSize = 20,
                TextColor = Color.Black
            };
            Entry descriptionEntry = new Entry()
            {

            };
            Label eventLocationLabel = new Label
            {
                Text = "Location",
                FontSize = 20,
                TextColor = Color.Black
            };
            Entry locationEntry = new Entry()
            {

            };
            Label eventDateLabel = new Label
            {
                Text = "Date",
                FontSize = 20,
                TextColor = Color.Black
            };
            DatePicker datePicker = new DatePicker
            {
                Format = "D",
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            scroll.Content = stack;
            stack.Children.Add(eventImage);
            stack.Children.Add(eventNameLabel);
            stack.Children.Add(nameEntry);
            stack.Children.Add(eventDescriptionLabel);
            stack.Children.Add(descriptionEntry);
            stack.Children.Add(eventLocationLabel);
            stack.Children.Add(locationEntry);
            stack.Children.Add(eventDateLabel);
            stack.Children.Add(datePicker);
            Content = scroll;
        }
    }
}