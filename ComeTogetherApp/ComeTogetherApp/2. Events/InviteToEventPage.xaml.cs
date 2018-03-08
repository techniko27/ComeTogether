using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Share;
using Plugin.Share.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing;
using ZXing.Net.Mobile.Forms;
using ZXing.Rendering;


namespace ComeTogetherApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InviteToEventPage : ContentPage
    {
        private Event ev;

        public Image image;
        public byte[] bitmapBytes;
        public InviteToEventPage(Event ev)
        {
            InitializeComponent();

            this.ev = ev;

            Title = "Invite to event";
            BackgroundColor = Color.FromHex(App.GetMenueColor());

            ScrollView scroll = new ScrollView();

            StackLayout stack = new StackLayout
            {
                //VerticalOptions = LayoutOptions.StartAndExpand,
                //HorizontalOptions = LayoutOptions.StartAndExpand,
                //Padding = new Thickness(30, 0, 30, 0),
                Margin = new Thickness(20, 20, 20, 20)
            };
            Label joinCodeLabel = new Label
            {
                Text = "Join-Code: "+ev.ID,
                FontSize = 18,
                TextColor = Color.Black,
                FontAttributes = FontAttributes.Bold,
            };
            Button shareEventButton = new Button()
            {
                Text = "Invite via Message",
                BackgroundColor = Color.White,
                TextColor = Color.FromHex(App.GetMenueColor()),
                FontAttributes = FontAttributes.Bold,
                Margin = new Thickness(0,25,0,-20)
            };
            shareEventButton.Clicked += OnshareEventButtonClicked;
            ZXingBarcodeImageView barcodeImageView = new ZXingBarcodeImageView()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                BarcodeFormat = ZXing.BarcodeFormat.QR_CODE,
                BarcodeValue = ev.ID,
                BarcodeOptions = new ZXing.Common.EncodingOptions
                {
                    Width = 300,
                    Height = 300,
                    Margin = 0,
                }
            };

            stack.Children.Add(joinCodeLabel);
            stack.Children.Add(shareEventButton);
            stack.Children.Add(barcodeImageView);

            scroll.Content = stack;
            Content = scroll;
        }

        async void OnshareEventButtonClicked(object sender, EventArgs e)
        {
            ShareBlog();
        }

        public void ShareBlog()
        {
            if (!CrossShare.IsSupported)
                return;
            
            CrossShare.Current.Share(new ShareMessage
            {
                Title = "ComeTogether Join-Code",
                Text = "Checkout ComeTogether App and enter this code to participate in your friend eventplan:",
                Url = ev.ID
            },
            new ShareOptions
            {
                ChooserTitle = "Share your event with",
                ExcludedUIActivityTypes = new[] { ShareUIActivityType.Message, ShareUIActivityType.Mail,  }
            });
        }
    }
}