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
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace F_X.AutomatedSession
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AutomatedPage : Page
    {


        SettingsQueries theSettings = new SettingsQueries();
        WeatherQuery theWeatherQuery = new WeatherQuery();
        ExtraPageGather theExtras = new ExtraPageGather();
        XDocument SettingsXML;
        string CityYouSelected;
        bool UnitTemperature;

        FTPDownloads pleaseDownload = new FTPDownloads();
        private XDocument NamesXML;

        PinControl theArduino = new PinControl("VID_2341", "PID_0001", 57600);
        string[] OriginalPinData;




        public async void onBoot()
        {
            await Task.Delay(200);

            try
            {
                SettingsXML = XDocument.Load(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("SettingsData.xml"));
            }

            catch (UnauthorizedAccessException e)
            {
                SettingsXML = XDocument.Load(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("SettingsData.xml"));
            }

            catch (FileNotFoundException e)
            {
                this.Frame.Navigate(typeof(LoginPage));
            }

            StorageFile ProfilePictureFile = await ApplicationData.Current.LocalFolder.GetFileAsync("profile.jpg");

            CityYouSelected = theSettings.getWeatherQuery();
            UnitTemperature = theSettings.isUnitTemperatureC();
            Weather theWeather = new Weather(CityYouSelected, UnitTemperature);

            MainPageInformation.TextAlignment = TextAlignment.Center;
            UsernameText.TextAlignment = TextAlignment.Center;
            DisplayNameText.TextAlignment = TextAlignment.Center;

            await Task.Delay(100);
            profilePicture.Source = new BitmapImage(new Uri(ProfilePictureFile.Path, UriKind.Absolute));

            MainPageInformation.Text = "Weather Forecast for " + CityYouSelected + ":\nMin: " + theWeatherQuery.getMinTemp() +
                                            "ºC\tMax: " + theWeatherQuery.getMaxTemp() + "ºC\nHumidity:\t" +
                                              theWeatherQuery.getHumidity() + "%";
            UsernameText.Text = "@" + theSettings.getUserQuery();
            DisplayNameText.Text = theSettings.getNameQuery();

            LedControl();
            

            //NamesXML = XDocument.Load(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("OutputNames.xml"));

            //var DataQuery = from r in NamesXML.Descendants("Output")
            //                select r;

            //for (int i = 0; i < 4; i++)
            //{
            //    XElement Data = DataQuery.ElementAt(i);
            //    OriginalPinData[i] = Data.Element("name").Value;
            //}


            await Task.Delay(1000);
            theExtras.LoadNews(NewsTextLine, new Uri("http://feeds.bbci.co.uk/news/world/rss.xml"));

            theArduino.UpdatingPinsThread(5);


        }

        public AutomatedPage()
        {
            this.InitializeComponent();
            onBoot();
        }

        public Frame AppFrame { get { return Content; } }

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


        private void LedControl()
        {
            TimeSpan period;
            period = TimeSpan.FromSeconds(5);


            ThreadPoolTimer PeriodicTimer = ThreadPoolTimer.CreatePeriodicTimer(async (source) =>
            {



                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High,
                   () =>
                   {
                       try
                       {
                           theArduino.SetPinNumber(6);
                           theArduino.ChangeState();
                       }
                       catch (Exception e)
                       {

                       }

                   }
                    );



            }, period);
        }
        private async void updateOnBoot()
        {
            pleaseDownload.getLatestOutputs();


            try
            {
                await Task.Delay(1000);
                NamesXML = XDocument.Load(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("OutputNames.xml"));

                var DataQuery = from r in NamesXML.Descendants("Output")
                                select r;



                await Task.Delay(2000);
                for (int i = 0; i < 4; i++)
                {
                    XElement Data = DataQuery.ElementAt(i);



                    if (OriginalPinData[i] != Data.Element("status").Value)
                    {
                        theArduino.SetPinNumber(Convert.ToByte(i + 1));
                        theArduino.ChangeState();

                    }


                }

            }
            catch (Exception e)
            {

            }

        }


    }
}
