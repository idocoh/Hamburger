using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace HamburgerChallenge
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            NavToFinancial(new object(), new RoutedEventArgs());
        }

        private void OpenPane(object sender, RoutedEventArgs e)
        {
            splitView.IsPaneOpen = !splitView.IsPaneOpen;
        }

        private void NavToFinancial(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(Financial));
            pageTitle.Text = "Financial";
            backButton.Visibility = Visibility.Collapsed;
            finPanel.Background = new SolidColorBrush(Colors.Blue);
            foodPanel.Background = new SolidColorBrush(Colors.Transparent);
        }
        private void NavToFood(object sender, RoutedEventArgs e)
        {
            frame.Navigate(typeof(Food));
            pageTitle.Text = "Food";
            backButton.Visibility = Visibility.Visible;
            foodPanel.Background = new SolidColorBrush(Colors.Blue);
            finPanel.Background = new SolidColorBrush(Colors.Transparent);
        }
        //In a real application, we would have a new function for the back button.
        //Can we please all agree that in this case, it's just not worth it, since
        //we get exactly the same functionality by reusing NavToFinancial?

        private void clearText(object sender, RoutedEventArgs e)
        {
            if (((TextBox)sender).Text.Equals("Search"))
            {
                ((TextBox)sender).Text = "";
            }
        }
    }
}
