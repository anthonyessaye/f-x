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
using Windows.UI.Popups;



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

        
        

        public Settings()
        {
            this.InitializeComponent();
            onBoot();

        }
        private void Home_Checked(object sender, RoutedEventArgs e)
        {
            CheckChanges();
            this.Frame.Navigate(typeof(MainPage));
        }
        private void Assistant_Checked(object sender, RoutedEventArgs e)
        {
            CheckChanges();
            this.Frame.Navigate(typeof(PersonalAssistant.AssistantPage));
        }
        private void Controls_Checked(object sender, RoutedEventArgs e)
        {
            CheckChanges();
            this.Frame.Navigate(typeof(Controls));
        }
        private void Settings_Checked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Settings));
        }
        private void Logout_Checked(object sender, RoutedEventArgs e)
        {
            CheckChanges();
            this.Frame.Navigate(typeof(LoginPage));
        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationPane.IsPaneOpen = !NavigationPane.IsPaneOpen;
        }

        private void OnSave_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
     
        }

        private async void OnCopy_Click(object sender, RoutedEventArgs e)
        {
            // Get the logical root folder for all external storage devices.
            StorageFolder externalDevices = Windows.Storage.KnownFolders.RemovableDevices;

            // Get the first child folder, which represents the SD card.
            StorageFolder sdCard = (await externalDevices.GetFoldersAsync()).FirstOrDefault();

            if (sdCard != null)
            {
                // An SD card is present and the sdCard variable now contains a reference to it.
                string Data;

                var mainDialog = new Windows.UI.Popups.MessageDialog("This will prompt you to save your credentials on the SD card\n" +
                                                                           "Please choose the main partition of the SD card\n" +
                                                                           "Doing so will set up your main hub automatically");
              
                await mainDialog.ShowAsync();

                XmlDocument CredXML = new XmlDocument();
                XmlElement user = (XmlElement)CredXML.AppendChild(CredXML.CreateElement("User"));

                user.SetAttribute("Email", (App.Current as App).Email);
                user.SetAttribute("Password", (App.Current as App).Password);
                Data = CredXML.OuterXml;
             

                var savePicker = new Windows.Storage.Pickers.FileSavePicker();
                savePicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.ComputerFolder;

                // Dropdown of file types the user can save the file as
                savePicker.FileTypeChoices.Add("XML File", new List<string>() { ".xml" });

                // Default file name if the user does not type one in or select a file to replace
                savePicker.SuggestedFileName = "UserCred";

                
                Windows.Storage.StorageFile SavedFile = await savePicker.PickSaveFileAsync();
                
                if (SavedFile != null)
                {
                 
                    Windows.Storage.CachedFileManager.DeferUpdates(SavedFile);
                    await Windows.Storage.FileIO.WriteTextAsync(SavedFile, Data);
                    Windows.Storage.Provider.FileUpdateStatus status = await Windows.Storage.CachedFileManager.CompleteUpdatesAsync(SavedFile);


                    if (status == Windows.Storage.Provider.FileUpdateStatus.Complete)
                    {
                        var messageDialog = new Windows.UI.Popups.MessageDialog("File " + SavedFile.Name + " was saved.\nNow turn off the main hub insert SD Card and you are ready to go!");
                        await messageDialog.ShowAsync();
                    }
                    else
                    {
                        var messageDialog = new Windows.UI.Popups.MessageDialog("File " + SavedFile.Name + " couldn't be saved.");
                        await messageDialog.ShowAsync();
                    }
                }
                else
                {
                    var messageDialog = new Windows.UI.Popups.MessageDialog("Operation cancelled.");
                    await messageDialog.ShowAsync();
                }
            }
            else
            {
                var messageDialog = new Windows.UI.Popups.MessageDialog("No SD card available");
                await messageDialog.ShowAsync();
            }

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


            StorageFile Settingsfile = null;

            UploadConnectionStatus.Text = "Uploading Latest File To Server";
            await ftp.ConnectAsync();

            Settingsfile = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("SettingsData.xml");
            isFileAvailable = await ftp.PutFileAsync(Settingsfile.Path, "SettingsData.xml");
           
           
            await ftp.DisconnectAsync();

            UploadConnectionStatus.Text = "File Uploaded\n\n";
            isFileAvailable = false;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
          
        }

        private async void SaveSettings()
        {
            DisableUI();

            theSettings.saveSettings(TextBoxHomeCity.Text, TSTemperatureUnit.IsOn, TextBoxDisplayedName.Text, TextBoxAccountName.Text,
                                         TextBoxAssistantName.Text, TSAssistantAlwaysON.IsOn, TSAssistantGender.IsOn);

            await UploadFile();

            EnableUI();

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

            CopyBtn.IsEnabled = false;
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

            CopyBtn.IsEnabled = true;
            SaveBtn.IsEnabled = true;
        }

        private bool IsThereChanges()
        {
           if( TextBoxHomeCity.Text != theSettings.getWeatherQuery())return true;
           if(TextBoxAccountName.Text != theSettings.getUserQuery())return true;
           if( TextBoxDisplayedName.Text != theSettings.getNameQuery()) return true;
           if( TextBoxAssistantName.Text != theSettings.getAssistantQuery()) return true;


           if( TSAssistantAlwaysON.IsOn != theSettings.isAssistantAlwaysOn()) return true;
           if( TSAssistantGender.IsOn != theSettings.isAssistantMale()) return true;
            if (TSTemperatureUnit.IsOn != theSettings.isUnitTemperatureC()) return true;

            else return false;
        }

        private async void CheckChanges()
        {
            
            if (IsThereChanges() == true)
            {
                MessageDialog messageDialog = new MessageDialog("Do you want to save the changes?");
                messageDialog.Commands.Add(new UICommand("Yes") { Id = 0 });
                messageDialog.Commands.Add(new UICommand("No") { Id = 1 });
                messageDialog.DefaultCommandIndex = 0;
                messageDialog.CancelCommandIndex = 1;

                var result = await messageDialog.ShowAsync();
                if ((int)result.Id == 0)
                {
                    SaveSettings();
                }
                else return;
            }
            else return;
        }

    }
}
