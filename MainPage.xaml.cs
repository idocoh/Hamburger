using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PartyViewer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            List<Image> partyImages = new List<Image>();
            Image image1 = new Image();
            image1.Source = new BitmapImage(new Uri("ms-appx:///Assets/small.bmp"));
            Image image2 = new Image();
            image2.Source = new BitmapImage(new Uri("ms-appx:///Assets/medium.bmp"));
            Image image3 = new Image();
            image3.Source = new BitmapImage(new Uri("ms-appx:///Assets/large.bmp"));
            Image image4 = new Image();
            image4.Source = new BitmapImage(new Uri("ms-appx:///Assets/list.bmp"));
            partyImages.Add(image1);
            partyImages.Add(image2);
            partyImages.Add(image3);
            partyImages.Add(image4);
            PartyDisplay partyDisplay = new PartyDisplay();
            this.Content = partyDisplay;
            partyDisplay.Images = partyImages;

        }
    }
}
