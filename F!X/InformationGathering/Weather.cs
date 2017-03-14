using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        private string CurrentUrl;

        public Weather(string WeatherLoc)
        {
            Location = WeatherLoc;
            CurrentUrl = "http://api.openweathermap.org/data/2.5/weather?q=" + Location + "&mode=xml&units=imperial&APPID=" + API_KEY;
        }

        

    }
}
