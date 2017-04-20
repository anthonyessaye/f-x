using F_X.InformationQueries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
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

    public sealed partial class Settings : Page
    {
        XDocument SettingsXML;
        Chilkat.Ftp2 ftp = new Chilkat.Ftp2();

        SettingsQueries theSettings = new SettingsQueries();

        bool isFileAvailable;
        bool success;
        string InfoText;

      

        public async void onBoot()
        {

            await DownloadFile();

            TextBoxHomeCity.Text = theSettings.getCityQuery();
            TextBoxAccountName.Text = theSettings.getUserQuery();
            TextBoxDisplayedName.Text = theSettings.getNameQuery();
            TextBoxAssistantName.Text = theSettings.getAssistantQuery();


            TSAssistantAlwaysON.IsOn = theSettings.isAssistantAlwaysOn();
            TSAssistantGender.IsOn = theSettings.isAssistantMale();

            //theSettings.Dispose();


            SettingsXML = XDocument.Load(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("SettingsData.xml"));
        }

        
        

        public Settings()
        {
            this.InitializeComponent();
            onBoot();

        }
        private void Home_Checked(object sender, RoutedEventArgs e)
        {


            this.Frame.Navigate(typeof(MainPage));
        }
        private void Assistant_Checked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PersonalAssistant.AssistantPage));
        }
        private void Controls_Checked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Controls));
        }
        private void Settings_Checked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Settings));
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
           

            theSettings.saveSettings(TextBoxHomeCity.Text,TextBoxDisplayedName.Text,TextBoxAccountName.Text,
                                         TextBoxAssistantName.Text, TSAssistantAlwaysON.IsOn,TSAssistantGender.IsOn);

            await UploadFile();
     
           
        }



        private async  Task DownloadFile()
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

    }
}
