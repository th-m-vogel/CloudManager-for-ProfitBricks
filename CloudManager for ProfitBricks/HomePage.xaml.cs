using CloudManager_for_ProfitBricks.Common;
using CloudManager_for_ProfitBricks.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace CloudManager_for_ProfitBricks
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class HomePage : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public HomePage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            App myApp = (App)App.Current;
            myApp.CurrentCredential = new CredentialItem("No Credential Specified", "", "");
        }

        /// <summary>
        /// Populates the page with content passed during navigation. Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session. The state will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            var credentials = CredentialDataSource.GetCredentials();
            this.DefaultViewModel["Credentials"] = credentials;
            if (credentials.Count() > 0)
                { this.AccountControl.Visibility = Visibility.Collapsed; }
            else
                { this.AccountControl.Visibility = Visibility.Visible; }
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }


        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion


        private void AccountItem_Click(object sender, ItemClickEventArgs e)
        {
            var credential = ((CredentialItem)e.ClickedItem);
            App myApp = (App)App.Current;
            myApp.CurrentCredential = credential;
            this.Frame.Navigate(typeof(CloudRessourcePage), credential);
        }

        private async void AccountControlSaveButton_Click(object sender, RoutedEventArgs e)
        {
            Uri wsdluri = new Uri("https://api.profitbricks.com/1.2/?wsdl");

            HttpWebRequest authTestRequest = (HttpWebRequest)WebRequest.Create(wsdluri);
            authTestRequest.Credentials =  new NetworkCredential(AccountUser.Text,AccountPassword.Password) ;
            try
            {
                WebResponse authTestResponse = await authTestRequest.GetResponseAsync();
            }
            catch (WebException exeption)
            {
                this.AccountControlMessage.Visibility = Visibility.Visible;
                this.AccountControlMessage.Text = "Error connectig Webservice:"
                    + System.Environment.NewLine
                    + System.Environment.NewLine
                    + exeption.Message
                    + System.Environment.NewLine
                    + System.Environment.NewLine 
                    + "Account Information was not saved";
                return;
            }

            if (!this.AccountUser.IsEnabled && !this.AccountPassword.IsEnabled)
            {
                CredentialItem old = CredentialDataSource.GetCredentialByUser(this.AccountUser.Text);
                CredentialDataSource.RemoveCredential(old.Name, old.User, old.Password);
                CredentialDataSource.AddCredential(this.AccountName.Text, this.AccountUser.Text, this.AccountPassword.Password);
            }
            else
            {
                CredentialDataSource.AddCredential(this.AccountName.Text, this.AccountUser.Text, this.AccountPassword.Password);
                CredentialDataSource.GetCredentials();
            }
            this.AccountName.Text = this.AccountUser.Text = this.AccountPassword.Password = "" ;
            this.AccountName.IsEnabled = this.AccountUser.IsEnabled = this.AccountPassword.IsEnabled = true;
            this.AccountControlMessage.Visibility = Visibility.Collapsed;
            this.AccountControlMessage.Text = "";
            if (CredentialDataSource.GetCredentials().Count() > 0)
            {
                this.AccountControl.Visibility = Visibility.Collapsed;
            }
        }

        private void AccountControlChancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (CredentialDataSource.GetCredentials().Count() > 0)
            {
                this.AccountControl.Visibility = Visibility.Collapsed;
            }
            this.AccountName.Text = this.AccountUser.Text = this.AccountPassword.Password = "";
            this.AccountName.IsEnabled = this.AccountUser.IsEnabled = this.AccountPassword.IsEnabled = true;
        }

        private void EditAccountButton_Click(object sender, RoutedEventArgs e)
        {
            this.BottomAppBar.IsOpen = false;
            this.AccountControl.Visibility = Visibility.Visible;
            this.AccountControlChancelButton.Visibility = Visibility.Visible;
            var item = (CredentialItem)credentialGridView.SelectedItems.First();
            this.AccountName.Text = item.Name;
            this.AccountName.IsEnabled = false;
            this.AccountUser.Text = item.User;
            this.AccountPassword.Password = item.Password;
        }

        private void RenameAccountButton_Click(object sender, RoutedEventArgs e)
        {
            this.BottomAppBar.IsOpen = false;
            this.AccountControl.Visibility = Visibility.Visible;
            this.AccountControlChancelButton.Visibility = Visibility.Visible;
            var item = (CredentialItem)credentialGridView.SelectedItems.First();
            this.AccountName.Text = item.Name;
            this.AccountUser.Text = item.User;
            this.AccountUser.IsEnabled = false;
            this.AccountPassword.Password = item.Password;
            this.AccountPassword.IsEnabled = false;
        }

        private void DeleteAccountButton_Click(object sender, RoutedEventArgs e)
        {
            this.BottomAppBar.IsOpen = false;
            foreach (CredentialItem item in credentialGridView.SelectedItems )
            {
                CredentialDataSource.RemoveCredential(item.Name, item.User, item.Password);
            }
            
            if (CredentialDataSource.GetCredentials().Count() > 0)
            {
                this.AccountControl.Visibility = Visibility.Collapsed;
            }
            else 
            {
                this.AccountControl.Visibility = Visibility.Visible;
                this.AccountControlChancelButton.Visibility = Visibility.Collapsed;
            }

        }

        private void AddAccountButton_Click(object sender, RoutedEventArgs e)
        {
            this.AccountControl.Visibility = Visibility.Visible;
            this.AccountControlChancelButton.Visibility = Visibility.Visible;
            this.BottomAppBar.IsOpen = false;
        }

        private void CredentialGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (credentialGridView.SelectedItems.Count() > 0)
            {
                this.BottomAppBar.IsSticky = true;
                this.BottomAppBar.IsOpen = true;
                this.BottomAppBarAddButton.Visibility = Visibility.Collapsed;
                this.BottomAppBarDeleteButton.IsEnabled = true;
                this.BottomAppBarEditButton.IsEnabled = this.BottomAppBarRenameButton.IsEnabled = true;
                if (credentialGridView.SelectedItems.Count() > 1)
                {
                    this.BottomAppBarEditButton.IsEnabled = this.BottomAppBarRenameButton.IsEnabled = false;
                }
            }
            else 
            {
                this.BottomAppBar.IsSticky = false;
                this.BottomAppBarDeleteButton.IsEnabled = false;
                this.BottomAppBarEditButton.IsEnabled = this.BottomAppBarRenameButton.IsEnabled = false;
                this.BottomAppBarAddButton.Visibility = Visibility.Visible;
                this.BottomAppBar.IsOpen = false;
            }
        }

        private void AccountControl_ChangedText(object sender, TextChangedEventArgs e)
        {
            this.AccountControlSaveButton.IsEnabled = !string.IsNullOrWhiteSpace(AccountName.Text) && !string.IsNullOrWhiteSpace(AccountUser.Text) && !string.IsNullOrWhiteSpace(AccountPassword.Password);
        }

        private void AccountControl_ChangedPassword(object sender, RoutedEventArgs e)
        {
            this.AccountControlSaveButton.IsEnabled = !string.IsNullOrWhiteSpace(AccountName.Text) && !string.IsNullOrWhiteSpace(AccountUser.Text) && !string.IsNullOrWhiteSpace(AccountPassword.Password);
        }
    }
}
