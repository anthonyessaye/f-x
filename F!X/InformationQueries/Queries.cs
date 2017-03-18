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
    class Queries
    {
        private XDocument SettingsXML;
        public XElement City { get; set; }

        public Queries() {
        }

        // THIS CLASS DOES NOT WORK YET,I HAVE CREATED IT
        // SO LATER ON WE CAN REMOVE THE QUERIES FROM ALL
        // THE OTHER PAGES AND STACK THEM HERE.

        public async void QueryForCity()
        {
            SettingsXML = XDocument.Load(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("SettingsData.xml"));
            var CityQuery = from r in SettingsXML.Descendants("city")
                            select r;
            City = CityQuery.ElementAt(0);
            
        }

        public string GetCity() {
            QueryForCity();
            return City.Element("name").Value;
        }

        public async void SetCity()
        {
            StorageFile file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("SettingsData.xml");
            FileStream fileStream = new FileStream(file.Path, FileMode.Truncate);

            SettingsXML.Save(fileStream);
            fileStream.Dispose();
        }



    }
}
