using F_X.Arduino_Related_Classes;
using F_X.InformationGathering;
using F_X.OnStartUp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Security.Credentials;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace F_X
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {

        private LoggingIn theLogin;


        private const string RESOURCE_NAME = "FIX";

        //  CheckAndCreateDirectory ProjectFolders = new CheckAndCreateDirectory();
        public async void onBoot()
        {
            StorageFolder MainFolder = ApplicationData.Current.LocalFolder;
            StorageFile OutputNamesfile = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"OutputNames.xml"); // Path is file path
            StorageFile Settingsfile = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"SettingsData.xml"); // Path is file path
            StorageFile profileFile = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"profile.jpg"); // Path is file path

            if (await FileExistAsync(OutputNamesfile.Name) == false)
            {
                await OutputNamesfile.CopyAsync(MainFolder, OutputNamesfile.Name, NameCollisionOption.GenerateUniqueName);
                await Settingsfile.CopyAsync(MainFolder, Settingsfile.Name, NameCollisionOption.GenerateUniqueName);
                await profileFile.CopyAsync(MainFolder, profileFile.Name, NameCollisionOption.GenerateUniqueName);
                Weather theWeather = new Weather("Beirut");
            }
            

        StatusText.TextAlignment = TextAlignment.Center;
        }
        

        public LoginPage()
        {
            
            this.InitializeComponent();
            onBoot();
            GetCredential();
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            var vault = new Windows.Security.Credentials.PasswordVault();



            theLogin = new LoggingIn(TextBoxUsername.Text, PassBoxLoginPass.Password);
            theLogin.ConnectAndGetLatest();

            if (CheckBoxRememberMe.IsChecked == true)
                SaveCredential(TextBoxUsername.Text, PassBoxLoginPass.Password);
            else if (CheckBoxRememberMe.IsChecked == false)
                RemoveCredential(TextBoxUsername.Text);

            

            await Task.Delay(5000);
            if (theLogin.isConnected)
            {
                (App.Current as App).Email = TextBoxUsername.Text;
                (App.Current as App).Password = PassBoxLoginPass.Password;

                this.Frame.Navigate(typeof(MainPage));

            }
            else
                StatusText.Text = "Something Went Wrong and We Couldn't\n Log You In";

        }



        private void SaveCredential(string userName, string password)
        {
            var vault = new PasswordVault();
            var credential = new PasswordCredential(RESOURCE_NAME, userName, password);

            // Permanently stores credential in the password vault.
            vault.Add(credential);
        }

        private void GetCredential()
        {
            string userName, password;

            var vault = new PasswordVault();
            try
            {
                var credential = vault.FindAllByResource(RESOURCE_NAME).FirstOrDefault();
                if (credential != null)
                {
                    // Retrieves the actual userName and password.
                    userName = credential.UserName;
                    password = vault.Retrieve(RESOURCE_NAME, userName).Password;

                    TextBoxUsername.Text = userName;
                    PassBoxLoginPass.Password = password;
                }
            }
            catch (Exception)
            {
                // If no credentials have been stored with the given RESOURCE_NAME, an exception
                // is thrown.
            }
        }

        private void RemoveCredential(string userName)
        {
            var vault = new PasswordVault();
            try
            {
                // Removes the credential from the password vault.
                vault.Remove(vault.Retrieve(RESOURCE_NAME, userName));
            }
            catch (Exception)
            {
                // If no credentials have been stored with the given RESOURCE_NAME, an exception
                // is thrown.
            }
        }



        // Function created just to return true or false in case of FileExist status instead of 
        // an IStorage status which cannot be converted to bool
        private async Task<bool> FileExistAsync(string filename)
        {
            IStorageFolder destination = Windows.Storage.ApplicationData.Current.LocalFolder;

            try
            {
                await destination.GetFileAsync(filename);
                return true;
            }
            catch (Exception NotExist)
            {
                return false;
            }
        }
    }
}
