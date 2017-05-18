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
using F_X.Arduino_Related_Classes;
using F_X.Extras;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace F_X
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        SettingsQueries theSettings = new SettingsQueries();
        ExtraPageGather theExtras = new ExtraPageGather();
        XDocument SettingsXML;

        Weather theWeather;
        string CityYouSelected;
        bool UnitTemperature;
        string UnitTemperatureString;

        PinControl theArduino = new PinControl("VID_2341", "PID_0243", 57600);


        


        public async void onBoot()
        {



            try
            {
                SettingsXML = XDocument.Load(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("SettingsData.xml"));
                StorageFile ProfilePictureFile = await ApplicationData.Current.LocalFolder.GetFileAsync("profile.jpg");


                for (int i = 0; i < 2; i++)
                {
                    await Task.Delay(1000);

                    CityYouSelected = theSettings.getWeatherQuery();
                    UnitTemperature = theSettings.isUnitTemperatureC();

                    theWeather = new Weather(CityYouSelected, UnitTemperature);
                    WeatherQuery theWeatherQuery = new WeatherQuery();
                    if (UnitTemperature == true)
                        UnitTemperatureString = "°C";
                    else
                        UnitTemperatureString = "°F";



                    MainPageInformation.TextAlignment = TextAlignment.Center;
                    UsernameText.TextAlignment = TextAlignment.Center;
                    DisplayNameText.TextAlignment = TextAlignment.Center;

                    await Task.Delay(100);
                    profilePicture.Source = new BitmapImage(new Uri(ProfilePictureFile.Path, UriKind.Absolute));


                    MainPageInformation.Text = "Weather Forecast for " + CityYouSelected + ":\nMin: " + theWeatherQuery.getMinTemp() +
                                                    UnitTemperatureString + "\tMax: " + theWeatherQuery.getMaxTemp() + UnitTemperatureString + "\nHumidity:\t" +
                                                      theWeatherQuery.getHumidity() + "%";

                    UsernameText.Text = "@" + theSettings.getUserQuery();
                    DisplayNameText.Text = theSettings.getNameQuery();

                    theExtras.LoadNews(NewsTextLine, new Uri("http://feeds.bbci.co.uk/news/world/rss.xml"));
                }
            }

            catch (Exception e)
            {
                onBoot();

            }

            //This should get location from SettingsData.xml but i was waiting to finish the
            // queries class. Now its hard coded to beirut. ---- DONE
            // ------------ NVM now it gets location from queries. --------------------------//
            //Options for weather we need to add= Celsius or Fahrenhiet. ---- DONE
            // Metric or imperial measurement. --- DONE

            // The xml has a lot of data we can use

            //theArduino.UpdatingPinsThread(1);

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

        private void Extras_Checked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ExtrasPage));
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
