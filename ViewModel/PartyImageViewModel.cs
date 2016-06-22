using MesibaViewer.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

// Displays pictures of parties!

namespace MesibaViewer.ViewModel
{
    public class PartyImageViewModel
    {
        private DispatcherTimer timer = new DispatcherTimer();
        public int timeToDisplayLocationBar = 3;
        public double transparency = 0.3;
        public bool allowClickOnLocationBar = true;
        public int currentIndex = 0;
        private PartyImageModel partyImageModel;
        public List<object> listOfItems = new List<object>();

        public PartyImageViewModel()
        {
            timer.Tick += CollapseLocationBar;
            timer.Interval = new TimeSpan(0, 0, timeToDisplayLocationBar);
            partyImageModel = new PartyImageModel();
            LoadImages();

        }

        private void LoadImages ()
        {
            Image firstImage = new Image();
            Image lastImage = new Image();
            firstImage.Source = partyImageModel.Images.Last().Source;
            lastImage.Source = partyImageModel.Images.First().Source;
            
            listOfItems.Add(ConstructScrollViewer(firstImage));
            foreach (Image image in partyImageModel.Images)
            {
                listOfItems.Add(ConstructScrollViewer(image));
                Image previewPic = new Image();
                previewPic.Source = image.Source;
                previewPic.Opacity = transparency;
                previewPic.PointerPressed += NavigateToPic;
                //locationBar.Children.Add(previewPic);
            }
            listOfItems.Add(ConstructScrollViewer(lastImage));
        }

        internal ScrollViewer ConstructScrollViewer(Image image)
        {
            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.ZoomMode = ZoomMode.Enabled;
            scrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.Content = image;
            return scrollViewer;
        }

        public void SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Debug.WriteLine(currentIndex);
            ResetTimer();
            //ManageLocationBarSelected();
            ManageCyclicalNavigation();
        }

        void CollapseLocationBar(object sender, object e)
        {
           // locationBar.Visibility = Visibility.Collapsed;
        }

        private void ResetZoom(object sender, DoubleTappedRoutedEventArgs e)
        {
            ((ScrollViewer)listOfItems.ElementAt(currentIndex)).ZoomToFactor(1);
        }

        private void NavigateToPic(object sender, PointerRoutedEventArgs e)
        {
            if (allowClickOnLocationBar)
            {
                //currentIndex = locationBar.Children.IndexOf((UIElement)sender) + 1;
            }
        }

        internal void ResetTimer()
        {
            timer.Stop();
            //locationBar.Visibility = Visibility.Visible;
            timer.Start();
        }
        /*
        internal void ManageLocationBarSelected()
        {
            if (locationBar.Children.Count >= currentIndex && currentIndex > 0)
            {
                foreach (Image pic in locationBar.Children)
                {
                    pic.Opacity = transparency;
                }
                locationBar.Children.ElementAt(currentIndex - 1).Opacity = 1;
            }
        }
        */
        internal void ManageCyclicalNavigation()
        {
            if (currentIndex == 0)
            {
                currentIndex = listOfItems.Count - 2;
            }
            else if (currentIndex == listOfItems.Count - 1)
            {
                currentIndex = 1;
            }
            if (listOfItems.Count > 3)
            {
                ((ScrollViewer)listOfItems.ElementAt(listOfItems.Count - 1)).ZoomToFactor(((ScrollViewer)listOfItems.ElementAt(1)).ZoomFactor);
                ((ScrollViewer)listOfItems.ElementAt(listOfItems.Count - 1)).InvalidateScrollInfo();
                ((ScrollViewer)listOfItems.ElementAt(0)).ZoomToFactor(((ScrollViewer)listOfItems.ElementAt(listOfItems.Count - 2)).ZoomFactor);
                ((ScrollViewer)listOfItems.ElementAt(0)).InvalidateScrollInfo();
            }
        }

    }
}
