using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Hamburger.UI.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ColorPicker : UserControl
    {
        public Colors SelectedColor { get; set; }

        public delegate void ColorChangedEventHandler(object sender, SolidColorBrush e);

        public event ColorChangedEventHandler ColorChanged;

        public ColorPicker()
        {
            this.InitializeComponent();
            loadcolors();
        }

        public void loadcolors()
        {
            List<ColorInfo> myColors = new List<ColorInfo>();
            myColors.Add(new ColorInfo("Red", Colors.Red));
            myColors.Add(new ColorInfo("Blue", Colors.Blue));
            myColors.Add(new ColorInfo("Yellow", Colors.Yellow));
            myColors.Add(new ColorInfo("Green", Colors.Green));
            myColors.Add(new ColorInfo("Orange", Colors.Orange));
            myColors.Add(new ColorInfo("Purple", Colors.Purple));
            colorList.ItemsSource = myColors;
        }

        private void colorList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ColorChanged != null)
            {
                ColorChanged(this, (SolidColorBrush)(((ColorInfo)e.AddedItems[0]).ColorBrush));
            }
        }
    }
    public class ColorInfo
    {
        public ColorInfo(string ColorName, Color Color)
        {
            this.Color = ColorName;
            this.ColorBrush = new SolidColorBrush(Color);

        }
        public string Color
        {
            get;
            set;
        }
        public Brush ColorBrush
        {
            get;
            set;
        }
    }
}
