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
using ZXing.Common;
using ZXing.QrCode;
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
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(30, 0, 30, 0),
                Margin = new Thickness(10, 10, 10, 10)
            };
            Label joynCodeLabel = new Label
            {
                Text = "Joyn-Code: "+ev.ID,
                FontSize = 15,
                TextColor = Color.Black
            };
            Button shareEventButton = new Button()
            {
                Text = "Invite via Message",
                BackgroundColor = Color.White,
                TextColor = Color.FromHex(App.GetMenueColor()),
                FontAttributes = FontAttributes.Bold
            };
            shareEventButton.Clicked += OnshareEventButtonClicked;
            image = new Image();

            stack.Children.Add(joynCodeLabel);
            stack.Children.Add(shareEventButton);
            stack.Children.Add(image);

            scroll.Content = stack;
            Content = scroll;
        }
        public void CreateBarcode()
        {
            /*
            var barcodeWriter = new BarcodeWriterGeneric()
            {
                Format = ZXing.BarcodeFormat.QR_CODE,
                Options = new ZXing.Common.EncodingOptions
                {
                    Width = 300,
                    Height = 50,
                    Margin = 30
                }
            };
            
            //barcodeWriter.Renderer = new PixelDataRenderer();
            var bitmap = barcodeWriter.Encode("sfheu");
            
            var stream = new MemoryStream(bitmapBytes);
            
            bitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);  // this is the diff between iOS and Android
            stream.Position = 0;
            
            ImageSource imageSource = ImageSource.FromStream(() => {
                MemoryStream ms = new MemoryStream(bitmapBytes);
                ms.Position = 0;
                return ms;
            });
            
            image.Source = imageSource;
            */
            /*
            image.Source = ImageSource.FromStream(() =>
            {
                var writer = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new EncodingOptions
                    {
                        Height = 200,
                        Width = 600
                    }
                };

                var bitmapBytes = writer.Write("Encode this to QRCode");
                MemoryStream ms = new MemoryStream(bitmapBytes);
                ms.Position = 0;
                return ms;
            });
            */
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
                Title = "ComeTogether Joyn-Code",
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