using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace HelloWorld
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DispatcherTimer timer = new DispatcherTimer();
        public MainPage()
        {
            this.InitializeComponent();

            foreach(object scrollviewer in flipView.Items.ToList())
            {
                Image pic = new Image();
                pic.Source = ((Image)((ScrollViewer)scrollviewer).Content).Source;
                pic.Opacity = 0.3;
                locationBar.Children.Add(pic);
            }

            ScrollViewer firstScroll = (ScrollViewer)flipView.Items.First();
            ScrollViewer lastScroll = (ScrollViewer)flipView.Items.Last();
            Image firstPic = new Image();
            Image lastPic = new Image();
            firstPic.Source = ((Image)firstScroll.Content).Source;
            lastPic.Source = ((Image)lastScroll.Content).Source;
            ScrollViewer scrvw1 = new ScrollViewer();
            ScrollViewer scrvw2 = new ScrollViewer();
            scrvw1.Content = firstPic;
            scrvw2.Content = lastPic;
            scrvw1.ZoomMode = ZoomMode.Enabled;
            scrvw2.ZoomMode = ZoomMode.Enabled;
            flipView.Items.Add(scrvw1);
            List<object> items = flipView.Items.ToList();
            items.Insert(0, scrvw2);
            flipView.ItemsSource = items;
            flipView.SelectedIndex = 1;
            Debug.WriteLine(flipView.Items.ToString());
        }

        private void flipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            timer.Stop();
            locationBar.Visibility = Visibility.Visible;
            timer.Tick += CollapseLocationBar;
            timer.Interval = new TimeSpan(0, 0, 3);
            timer.Start();
            int index = ((FlipView)sender).SelectedIndex;
            if (index == 0)
            {
                ((FlipView)sender).SelectedIndex = ((FlipView)sender).Items.Count - 2;
            }
            if (index == ((FlipView)sender).Items.Count - 1)
            {
                ((FlipView)sender).SelectedIndex = 1;
            }
            if(locationBar.Children.Count >= index && index > 0)
            {
                foreach(Image pic in locationBar.Children)
                {
                    pic.Opacity = 0.3;
                }
                locationBar.Children.ElementAt(index - 1).Opacity = 1;
            }
            if(index == 0 || index == 2)
            {
                ((ScrollViewer)flipView.Items.ElementAt(flipView.Items.Count - 1)).ZoomToFactor(((ScrollViewer)flipView.Items.ElementAt(1)).ZoomFactor);
            }
            if (index == flipView.Items.Count - 2 || index == 0)
            {
                ((ScrollViewer)flipView.Items.ElementAt(0)).ZoomToFactor(((ScrollViewer)flipView.Items.ElementAt(flipView.Items.Count - 2)).ZoomFactor);
            }
            //UpdateLayout();
        }

        void CollapseLocationBar(object sender, object e)
        {
            locationBar.Visibility = Visibility.Collapsed;
        }

    }
}
