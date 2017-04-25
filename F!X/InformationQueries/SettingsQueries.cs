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
    class SettingsQueries
    {
        private XDocument SettingsXML;
        private StorageFile settingsFile;

        private string City;
        private string TemperatureUnit { get; set; }
        private string fName;
        private string lName;

        private string userName;
        private string assistantName;
        public string assistantGender { get; set; }
        public string assistantStatus { get; set; }

        public SettingsQueries()
        {
            SetXML();
        }

        private async void SetXML()
        {
            try
            {
                SettingsXML = XDocument.Load(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("SettingsData.xml"));
                settingsFile = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("SettingsData.xml");
            }
            catch(Exception e)
            {

            }
        }

        public string getWeatherQuery()
        {
            var WeatherQuery = from r in SettingsXML.Descendants("weather")
                            select r;
            XElement weather = WeatherQuery.ElementAt(0);
            City = weather.Element("city").Value;
            TemperatureUnit = weather.Element("unit").Value;
            

            return City;

        }
        public string getNameQuery()
        {
            var NameQuery = from r in SettingsXML.Descendants("user")
                            select r;
            XElement name = NameQuery.ElementAt(0);
            fName = name.Element("firstname").Value;
            lName = name.Element("lastname").Value;

            return fName + " " + lName;

        }
        public string getUserQuery()
        {
            var userNameQuery = from r in SettingsXML.Descendants("username")
                            select r;
            XElement u_name = userNameQuery.ElementAt(0);
            userName = u_name.Element("user").Value;

            return userName;

        }
        public string getAssistantQuery()
        {
            var assistantQuery = from r in SettingsXML.Descendants("assistant")
                            select r;
            XElement name = assistantQuery.ElementAt(0);
            assistantName = name.Element("name").Value;
            assistantStatus = name.Element("alwayson").Value;
            assistantGender = name.Element("gender").Value;

            return assistantName;

        }
        public bool isAssistantAlwaysOn()
        {
            if (assistantStatus == "On")
                return true;
            else
                return false;
        }
        public bool isAssistantMale()
        {
            if (assistantGender == "Male")
                return false;
            else
                return true;
        }
        public bool isUnitTemperatureC()
        {
            if (TemperatureUnit == "C")
                return true;
            else
                return false;
        }

        private void setWeatherQuery(string city, bool Unit)
        {
            var WeatherQuery = from r in SettingsXML.Descendants("weather")
                            select r;
            XElement weatherelement = WeatherQuery.ElementAt(0);

            weatherelement.Element("city").Value = city;

            if (Unit == true)
                weatherelement.Element("unit").Value = "C";
            else weatherelement.Element("unit").Value = "F";

        }
        private void setNameQuery(string name)
        { 
            var NameQuery = from r in SettingsXML.Descendants("user")
                            select r;
            XElement nameelement = NameQuery.ElementAt(0);
            string[] names;
            names = name.Trim().Split(new char[] { ' ' }, 2);

            if (names.Length == 1)
            {
               fName = "";
                lName = names[0];
            }
            else
            {
                fName = names[0];
                lName = names[1];
            }

            nameelement.Element("firstname").Value = fName;
            nameelement.Element("lastname").Value = lName;

        }
        private void setUserQuery(string username)
        {
            var userNameQuery = from r in SettingsXML.Descendants("username")
                                select r;
            XElement u_name = userNameQuery.ElementAt(0);
            u_name.Element("user").Value = username;
        }
        private void setAssistantQuery(string AssistantName, bool isAlwaysOn, bool gender)
        {
            var assistantQuery = from r in SettingsXML.Descendants("assistant")
                                 select r;
            XElement name = assistantQuery.ElementAt(0);
            name.Element("name").Value = AssistantName;

            if (isAlwaysOn == true)
                name.Element("alwayson").Value = "On";
            else
                name.Element("alwayson").Value = "Off";

            if (gender == true)
                name.Element("gender").Value = "Female";
            else
                name.Element("gender").Value = "Male";
        }
        

        public  void saveSettings(string city,bool unit, string displayname, string user, string AssistantName,
                                    bool isAlwaysOn, bool Gender)
        {
            
            FileStream fileStream = new FileStream(settingsFile.Path, FileMode.Truncate);

           
            setWeatherQuery(city, unit);
            setNameQuery(displayname);
            setUserQuery(user);
            setAssistantQuery(AssistantName, isAlwaysOn, Gender);

            SettingsXML.Save(fileStream);
            fileStream.Dispose();
        }


        public void Dispose()
        {
            SettingsXML = null;
        }
    }
}
