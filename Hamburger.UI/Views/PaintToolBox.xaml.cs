
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Hamburger.UI.Views
{
    public sealed partial class PaintToolBox : UserControl
    {
        public Brush SelectedColor { get; set; } = new SolidColorBrush(Colors.Black);
        public PaintToolBox()
        {
            this.InitializeComponent();
        }

        private void ColorPickButton_Click(object sender, RoutedEventArgs e)
        {
            if (ColorPicker.Visibility == Visibility.Visible)
            {
                ColorPicker.Visibility = Visibility.Collapsed;
            }
            else
            {
                ColorPicker.Visibility = Visibility.Visible;
                ColorPicker.Focus(FocusState.Programmatic);
            }
        }

        private void ColorPicker_LostFocus(object sender, RoutedEventArgs e)
        {
            ColorPicker.Visibility = Visibility.Collapsed;
        }

        private void ColorPicker_ColorChanged(object sender, Brush e)
        {
            SelectedColor = e;
            ColorPickButton.Foreground = e;
        }

        private void FreeHandButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
