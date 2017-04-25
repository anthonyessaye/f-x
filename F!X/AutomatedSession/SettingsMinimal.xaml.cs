using F_X.Arduino_Related_Classes;
using F_X.InformationGathering;
using F_X.InformationQueries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace F_X.AutomatedSession
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 



    public sealed partial class SettingsMinimal : Page
    {

        XDocument SettingsXML;
        Chilkat.Ftp2 ftp = new Chilkat.Ftp2();

        SettingsQueries theSettings = new SettingsQueries();

        bool isFileAvailable;
        bool success;
        string InfoText;


       

        public async void onBoot()
        {
            DisableUI();
            await DownloadFile();


            TextBoxHomeCity.Text = theSettings.getWeatherQuery();
            TextBoxAccountName.Text = theSettings.getUserQuery();
            TextBoxDisplayedName.Text = theSettings.getNameQuery();
            TextBoxAssistantName.Text = theSettings.getAssistantQuery();


            TSAssistantAlwaysON.IsOn = theSettings.isAssistantAlwaysOn();
            TSAssistantGender.IsOn = theSettings.isAssistantMale();
            TSTemperatureUnit.IsOn = theSettings.isUnitTemperatureC();

            //theSettings.Dispose();


            SettingsXML = XDocument.Load(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("SettingsData.xml"));

            EnableUI();
        }


        public SettingsMinimal()
        {
            this.InitializeComponent();
            onBoot();
        }



        private void Home_Checked(object sender, RoutedEventArgs e)
        {


            this.Frame.Navigate(typeof(AutomatedPage));
        }
      
        private void Settings_Checked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(SettingsMinimal));
        }
        private void Logout_Checked(object sender, RoutedEventArgs e)
        {

            this.Frame.Navigate(typeof(LoginPage));
        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationPane.IsPaneOpen = !NavigationPane.IsPaneOpen;
        }

        private async void OnSave_Click(object sender, RoutedEventArgs e)
        {
            DisableUI();

            theSettings.saveSettings(TextBoxHomeCity.Text,TSTemperatureUnit.IsOn, TextBoxDisplayedName.Text, TextBoxAccountName.Text,
                                         TextBoxAssistantName.Text, TSAssistantAlwaysON.IsOn, TSAssistantGender.IsOn);

            await UploadFile();

            EnableUI();

        }



        private async Task DownloadFile()
        {
            await Task.Delay(200);

            InfoText = DownloadConnectionStatus.Text;
            ftp.UnlockComponent("test");

            ftp.Hostname = "ftp.bodirectors.com";
            ftp.Username = (App.Current as App).Email + "@bodirectors.com";
            ftp.Password = (App.Current as App).Password;

            StorageFile file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("SettingsData.xml");

            DownloadConnectionStatus.Text = "Getting latest file from server.";
            await ftp.ConnectAsync();
            isFileAvailable = await ftp.GetFileAsync("SettingsData.xml", file.Path);
            await ftp.DisconnectAsync();

            DownloadConnectionStatus.Text = "File Downlaoded\n\n";
            isFileAvailable = false;

        }
        private async Task UploadFile()
        {
            ftp.UnlockComponent("test");

            ftp.Hostname = "ftp.bodirectors.com";
            ftp.Username = (App.Current as App).Email + "@bodirectors.com";
            ftp.Password = (App.Current as App).Password;

            DownloadConnectionStatus.Visibility = Visibility.Collapsed;

            StorageFile file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("SettingsData.xml");

            UploadConnectionStatus.Text = "Uploading Latest File To Server";
            await ftp.ConnectAsync();
            isFileAvailable = await ftp.PutFileAsync(file.Path, "SettingsData.xml");

            await ftp.DisconnectAsync();

            UploadConnectionStatus.Text = "File Uploaded\n\n";
            isFileAvailable = false;
        }




        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

        }

        private void DisableUI()
        {
            TextBoxAccountName.IsEnabled = false;
            TextBoxDisplayedName.IsEnabled = false;

            TextBoxHomeCity.IsEnabled = false;
            TSEnableLocation.IsEnabled = false;
            TSTemperatureUnit.IsEnabled = false;

            TextBoxAssistantName.IsEnabled = false;
            TSAssistantAlwaysON.IsEnabled = false;
            TSAssistantGender.IsEnabled = false;

            SaveBtn.IsEnabled = false;
        }

        private void EnableUI()
        {
            TextBoxAccountName.IsEnabled = true;
            TextBoxDisplayedName.IsEnabled = true;

            TextBoxHomeCity.IsEnabled = true;
            TSEnableLocation.IsEnabled = true;
            TSTemperatureUnit.IsEnabled = true;

            TextBoxAssistantName.IsEnabled = true;
            TSAssistantAlwaysON.IsEnabled = true;
            TSAssistantGender.IsEnabled = true;

            SaveBtn.IsEnabled = true;
        }

    }
}

