using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Animation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Hamburger__menu_swipe_in
{

	public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
			InitializeHamburgerMenu();
			trnslttrnsfrmMenuTop.X = mainPageWidth;
			trnslttrnsfrmMenuBottom.X = mainPageWidth;
			pgMainPage.ManipulationMode = ManipulationModes.TranslateX;
		}

		#region "Hamburger menu"

		double stckpnlMenuWidth = 210;
        double mainPageWidth = Window.Current.Bounds.Width;
		double InitialManipulationPointX;
		bool HamburgerMenuOpen = false;
		bool HamburgerMenuOpening = false;

		private void InitializeHamburgerMenu()
		{
			shMenu1.From = +stckpnlMenuWidth;
			shMenu2.From = +stckpnlMenuWidth;
			hdMenu1.To = +stckpnlMenuWidth;
			hdMenu2.To = +stckpnlMenuWidth;
			stckpnlMenuTop.Width = stckpnlMenuWidth;
			stckpnlMenuBottom.Width = stckpnlMenuWidth;
		}

		private void bttnHamburgerMenu_Tapped(object sender, TappedRoutedEventArgs e)
		{
			if (HamburgerMenuOpen)
			{
				HideMenu();
			}
			else
			{
				ShowMenu();
			}
		}

		private void MenuHome_Tapped(object sender, TappedRoutedEventArgs e)
		{
			txtblckMenuTapped.Text = "Home";
			HideMenu();
		}

		private void MenuAdd_Tapped(object sender, TappedRoutedEventArgs e)
		{
			txtblckMenuTapped.Text = "Add";
			HideMenu();
		}

		private void MenuPrivacy_Tapped(object sender, TappedRoutedEventArgs e)
		{
			txtblckMenuTapped.Text = "Privacy";
			HideMenu();
		}

		private void MenuTerms_Tapped(object sender, TappedRoutedEventArgs e)
		{
			txtblckMenuTapped.Text = "Terms";
			HideMenu();
		}

		private void MenuAbout_Tapped(object sender, TappedRoutedEventArgs e)
		{
			txtblckMenuTapped.Text = "About";
			HideMenu();
		}

		private void grdPageOverlay_Tapped(object sender, TappedRoutedEventArgs e)
		{
			HideMenu();
		}

		private async void ShowMenu()
		{
			//grdPageOverlay.Visibility = Visibility.Visible;
			await strbrdShowMenu.BeginAsync();
			HamburgerMenuOpen = true;
		}

		private async void HideMenu()
		{
			HamburgerMenuOpen = false;
			//grdPageOverlay.Visibility = Visibility.Collapsed;
			await strbrdHideMenu.BeginAsync();
		}

		private void pgMainPage_ManipulationStarted(object sender, ManipulationStartedRoutedEventArgs e)
		{
			InitialManipulationPointX = e.Position.X;
            HamburgerMenuOpening = HamburgerMenuOpen ? false : InitialManipulationPointX > mainPageWidth-stckpnlMenuWidth ? true : false;
		}

		private void pgMainPage_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
		{
            var HamburgerMenuClosing = HamburgerMenuOpen && InitialManipulationPointX < e.Position.X;

            if (HamburgerMenuOpening || HamburgerMenuClosing)
			{
				HamburgerMenuOpening = true;
				if (e.Position.X > mainPageWidth - stckpnlMenuWidth + 1)
				{
					Point currentpoint = e.Position;
					trnslttrnsfrmMenuTop.X = e.Position.X > mainPageWidth - stckpnlMenuWidth ? e.Position.X - stckpnlMenuWidth  : mainPageWidth - stckpnlMenuWidth;
                    trnslttrnsfrmMenuBottom.X = e.Position.X > mainPageWidth - stckpnlMenuWidth ? e.Position.X - stckpnlMenuWidth : mainPageWidth - stckpnlMenuWidth;
                }
            }
		} 

		private async void pgMainPage_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
		{
			if (HamburgerMenuOpening || (HamburgerMenuOpen && InitialManipulationPointX < e.Position.X))
			{
				double X = e.Position.X > mainPageWidth - stckpnlMenuWidth ? e.Position.X : mainPageWidth - stckpnlMenuWidth;
				if (X < mainPageWidth - stckpnlMenuWidth / 2)
				{
					shMenu1.From = -stckpnlMenuWidth + X;
					shMenu2.From = -stckpnlMenuWidth + X;
					await strbrdShowMenu.BeginAsync();
					shMenu1.From = -stckpnlMenuWidth;
					shMenu2.From = -stckpnlMenuWidth;
					//grdPageOverlay.Visibility = Visibility.Visible;
					HamburgerMenuOpen = true;
				}
				else	
				{	
					//grdPageOverlay.Visibility = Visibility.Collapsed;
					hdMenu1.From = X - stckpnlMenuWidth;
					hdMenu2.From = X - stckpnlMenuWidth;
					await strbrdHideMenu.BeginAsync();
					hdMenu1.From = 0;
					hdMenu2.From = 0;
					HamburgerMenuOpen = false;
				}
			}
			HamburgerMenuOpening = false;
		}

		#endregion
	}

	public static class StoryboardExtensions
	{
		public static Task BeginAsync(this Storyboard storyboard)
		{
			try
			{
				TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
				if (storyboard == null)
					tcs.SetException(new ArgumentNullException());
				else
				{
					EventHandler<object> onComplete = null;
					onComplete = (s, e) => {
						storyboard.Completed -= onComplete;
						tcs.SetResult(true);
					};
					storyboard.Completed += onComplete;
					storyboard.Begin();
				}
				return tcs.Task;
			}
			catch
			{
				return null;
			}
		}

	}                                                                   // Extension for an asynchrone storyboard animation

}
