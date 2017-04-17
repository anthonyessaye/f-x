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


        public WeatherQuery() {
            SetXML();
        }

        private async void SetXML()
        {
            WeatherXML = XDocument.Load(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("WeatherXML.xml"));
        }

        public string getMaxTemp()
        {
            
            
                var TempQuery = from r in WeatherXML.Descendants("temperature")
                                select r;
                XElement temperature = TempQuery.ElementAt(0);
                MaxTemperature = temperature.Attribute("max").Value;

                return MaxTemperature;

            
        }

        public string getMinTemp()
        {


            var TempQuery = from r in WeatherXML.Descendants("temperature")
                            select r;
            XElement temperature = TempQuery.ElementAt(0);
            MinTemperature = temperature.Attribute("min").Value;

            return MinTemperature;


        }

    }
}
