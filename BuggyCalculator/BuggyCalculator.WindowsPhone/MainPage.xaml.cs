using BuggyCalculator.Common;
using BuggyCalculator.State;
using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace BuggyCalculator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        // Comfortable access to the application
        private readonly App app = Application.Current as App;

        private NavigationHelper navigationHelper;

        public MainPage()
        {
            Debug.WriteLine("Main: new instance");

            this.InitializeComponent();

            // This doesn't solve the general suspension problem
            // but this is an easy trick to just make sure
            // the calculator state is not reset when going
            // to the settings or about pages
            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// </summary>
        public Calculator ViewModel
        {
            get { return app.Calculator; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (e.PageState != null)
                Debug.WriteLine("Main: restoring from previous state");
            else
                Debug.WriteLine("Main: fresh init");
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            Debug.Assert(e.PageState != null);
            Debug.WriteLine("Main: saving state");
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void ButtonReset_Click(object sender, RoutedEventArgs e)
        {
            app.Calculator.HandleResetKey();
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            app.Calculator.HandleClearKey();
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            app.Calculator.HandleAddKey();
        }

        private void ButtonSubstract_Click(object sender, RoutedEventArgs e)
        {
            app.Calculator.HandleSubstructKey();
        }

        private void ButtonMultiply_Click(object sender, RoutedEventArgs e)
        {
            app.Calculator.HandleMultiplyKey();
        }

        private void ButtonDivide_Click(object sender, RoutedEventArgs e)
        {
            app.Calculator.HandleDivideKey();
        }

        private void ButtonResult_Click(object sender, RoutedEventArgs e)
        {
            app.Calculator.HandleResultKey();
        }

        private void Button0_Click(object sender, RoutedEventArgs e)
        {
            app.Calculator.HandleNumberKey(0);
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            app.Calculator.HandleNumberKey(1);
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            app.Calculator.HandleNumberKey(2);
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            app.Calculator.HandleNumberKey(3);
        }

        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            app.Calculator.HandleNumberKey(4);
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            app.Calculator.HandleNumberKey(5);
        }

        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            app.Calculator.HandleNumberKey(6);
        }

        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            app.Calculator.HandleNumberKey(7);
        }

        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            app.Calculator.HandleNumberKey(8);
        }

        private void Button9_Click(object sender, RoutedEventArgs e)
        {
            app.Calculator.HandleNumberKey(9);
        }

        private void ButtonPoint_Click(object sender, RoutedEventArgs e)
        {
            app.Calculator.HandlePointKey();
        }

        private void ButtonOpposite_Click(object sender, RoutedEventArgs e)
        {
            app.Calculator.HandleOppositeKey();
        }

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsPage));
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AboutPage));
        }
    }
}
