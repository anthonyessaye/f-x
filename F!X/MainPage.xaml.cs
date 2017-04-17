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
using F_X.InformationQueries;
using System.Xml.Linq;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace F_X
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        SettingsQueries theSettings = new SettingsQueries();
        WeatherQuery theWeatherQuery = new WeatherQuery();
        XDocument SettingsXML;
        string CityYouSelected;

       


        public async void onBoot()
        {

            SettingsXML = XDocument.Load(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("SettingsData.xml"));
            StorageFile ProfilePictureFile = await ApplicationData.Current.LocalFolder.GetFileAsync("profile.jpg");

          



            CityYouSelected = theSettings.getCityQuery();
            Weather theWeather = new Weather(CityYouSelected);

            MainPageInformation.TextAlignment = TextAlignment.Center;
            UsernameText.TextAlignment = TextAlignment.Center;
            DisplayNameText.TextAlignment = TextAlignment.Center;


            profilePicture.Source = new BitmapImage(new Uri(ProfilePictureFile.Path, UriKind.Absolute));
            MainPageInformation.Text = "Weather Forecast for " + CityYouSelected + ":\nMin: " + theWeatherQuery.getMinTemp() +
                                            "ºC\tMax: " + theWeatherQuery.getMaxTemp() + "ºC";
            UsernameText.Text = "@" + theSettings.getUserQuery();
            DisplayNameText.Text = theSettings.getNameQuery();
            //This should get location from SettingsData.xml but was waiting to finish the
            // queries class. Now its hard coded to beirut.
            // ------------ NVM not it gets location from queries. --------------------------//
                                                        //Options for weather we need to add= Celsius or Fahrenhiet.
                                                        // Metric or imperial measurement.

                                                        // The xml has a lot of data we can use



        }


        public MainPage()
        {
            this.InitializeComponent();
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
        private void Logout_Checked(object sender, RoutedEventArgs e)
        {

            this.Frame.Navigate(typeof(LoginPage));
        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationPane.IsPaneOpen = !NavigationPane.IsPaneOpen;
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            
           

            
        }


    }
}
