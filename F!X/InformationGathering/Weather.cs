using Microsoft.Azure.Devices.Client;
using Sensors.Dht;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using Windows.Devices.Gpio;
using Windows.Storage;
using Windows.UI.Xaml;


namespace F_X.InformationGathering
{
    class Weather
    {
        // All the weather data is obtained from openweathermap.org
        // using their open source apis to generate an xml file with
        // weather info.

        // What this class should provide is the API_KEY for us to access
        // the weather server and extract info, an exact location.
        // Measurement system we need to use as well.

        private const string API_KEY = "ca181fbfc708eeab94347370c2b270db"; // this never changes, although we only get 60 calls/min 
                                                                           // for free
        public string Location { get; set; }
        public string UnitSystem { get; set; }

        private string CurrentUrl;
        private StorageFolder MainFolder = ApplicationData.Current.LocalFolder;
        private StorageFile WeatherXML;
        XDocument WeatherXMLdownload;


        public Weather(string WeatherLoc, bool Unit)
        {
            if (Unit == true) UnitSystem = "metric";
            else UnitSystem = "imperial";
            Location = WeatherLoc;
            CurrentUrl = "http://api.openweathermap.org/data/2.5/weather?q=" + Location + "&mode=xml&units=" + UnitSystem + "&APPID=" + API_KEY;

            CreateWeatherXML();

        }

        private async void CreateWeatherXML()
        {
            if (await FileExistAsync("WeatherXML.xml") == false)
            {
                await MainFolder.CreateFileAsync("WeatherXML.xml");
            }

            WeatherXML = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("WeatherXML.xml");


            

            FileStream fileStream = new FileStream(WeatherXML.Path, FileMode.Truncate);

            using (var httpclient = new HttpClient()) // client to copy url to file.
            {
                var response = await httpclient.GetAsync(CurrentUrl);
                WeatherXMLdownload = XDocument.Load(await response.Content.ReadAsStreamAsync());
                WeatherXMLdownload.Save(fileStream);
                fileStream.Dispose();
            }


        }

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