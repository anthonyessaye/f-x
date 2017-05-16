using F_X.InformationGathering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace F_X.InformationQueries
{
    class WeatherQuery
    {

        private XDocument WeatherXML;
        private string MaxTemperature;
        private string MinTemperature;
        private string Humidity;


        public WeatherQuery() {
            SetXML();
        }

        private async void SetXML()
        {
            try
            {
                WeatherXML = XDocument.Load(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("WeatherXML.xml"));
            }
            catch(Exception e)
            {
                
            }
        }

        public string getMaxTemp()
        {

            try
            {
                var TempQuery = from r in WeatherXML.Descendants("temperature")
                                select r;
                XElement temperature = TempQuery.ElementAt(0);
                MaxTemperature = temperature.Attribute("max").Value;
            }

            catch (Exception e)
            {
                return "N/A";
            }

                return MaxTemperature;


            
        }

        public string getMinTemp()
        {

            try
            {
                var TempQuery = from r in WeatherXML.Descendants("temperature")
                                select r;
                XElement temperature = TempQuery.ElementAt(0);
                MinTemperature = temperature.Attribute("min").Value;
            }

            catch(Exception e)
            {
                return "N/A";
            }

            return MinTemperature;


        }

        public string getHumidity()
        {

            try
            {
                var HumQuery = from r in WeatherXML.Descendants("humidity")
                               select r;
                XElement HumidityElement = HumQuery.ElementAt(0);
                Humidity = HumidityElement.Attribute("value").Value;
            }

            catch(Exception e)
            {
                return "N/A";
            }

            return Humidity;


        }

    }
}
