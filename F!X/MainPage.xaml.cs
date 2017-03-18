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
using Windows.UI.Xaml.Navigation;
using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;
using F_X.PersonalAssistant;
using Windows.Storage;
using System.Reflection;
using System.Threading.Tasks;
using F_X.InformationGathering;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace F_X
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        CheckAndCreateDirectory ProjectFolders = new CheckAndCreateDirectory();

        public async void onBoot()
        {

            StorageFolder MainFolder = ApplicationData.Current.LocalFolder;

            //this onBoot function just copies files from here to the app installation folder
            StorageFile OutputNamesfile = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"OutputNames.xml"); // Path is file path
            StorageFile Settingsfile = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"SettingsData.xml"); // Path is file path


            if (await FileExistAsync(OutputNamesfile.Name) == false)
            {
                await OutputNamesfile.CopyAsync(MainFolder, OutputNamesfile.Name, NameCollisionOption.GenerateUniqueName);
                await Settingsfile.CopyAsync(MainFolder, Settingsfile.Name, NameCollisionOption.GenerateUniqueName);
            }

            Weather theWeather = new Weather("Beirut"); //This should get location from SettingsData.xml but was waiting to finish the
                                                        // queries class. Now its hard coded to beirut.

                                                        //Options for weather we need to add= Celsius or Fahrenhiet.
                                                        // Metric or imperial measurement.

                                                        // The xml has a lot of data we can use
        }


        public MainPage()
        {
            this.InitializeComponent();
            ProjectFolders.iMustCreate();

            onBoot();
           

        }

        public Frame AppFrame { get { return Content; } }

        private void Home_Checked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        private void Assistant_Checked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AssistantPage));
        }
        private void Controls_Checked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Controls));
        }
        private void Settings_Checked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Settings));
        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationPane.IsPaneOpen = !NavigationPane.IsPaneOpen;
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
