using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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
        

        public async void onBoot()
        {

            SettingsXML = XDocument.Load(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("SettingsData.xml"));

            var CityQuery = from r in SettingsXML.Descendants("city")
                            select r;
            XElement city = CityQuery.ElementAt(0);
                TextBoxHomeCity.Text = city.Element("name").Value;
            
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
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationPane.IsPaneOpen = !NavigationPane.IsPaneOpen;
        }

        private async void OnSave_Click(object sender, RoutedEventArgs e)
        {
            StorageFile file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("SettingsData.xml");
            FileStream fileStream = new FileStream(file.Path, FileMode.Truncate);

            var CityQuery = from r in SettingsXML.Descendants("city")
                            select r;
            XElement city = CityQuery.ElementAt(0);

            city.Element("name").Value = TextBoxHomeCity.Text ;

            SettingsXML.Save(fileStream);
            fileStream.Dispose();

            // What this does basically is save the settings.
            // "file" is just the location of the xml data, while fileStream is a stream of that data.
            // A query is dont just to know what tags to update. then the stream is saved and filestream is disposed.
            // (closing the file after stream to be able to read after it).
           
        }

       
    }
}
