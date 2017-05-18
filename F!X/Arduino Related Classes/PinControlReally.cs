using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;
using F_X.InformationGathering;
using System.IO;

namespace F_X.Arduino_Related_Classes
{
    class PinControlReally : PinControl
    {
       
      
        FTPDownloads pleaseDownload = new FTPDownloads();
        Chilkat.Ftp2 ftp = new Chilkat.Ftp2();

        bool isFileAvailable;
        StorageFile file;


        public PinControlReally() : base ("VID_2341", "PID_0001", 57600)
        {
            getFileLocation();
        }

        private async void getFileLocation()
        {
            file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync("OutputNames.xml");

        }

        private async void UploadFile()
        {
            ftp.UnlockComponent("test");

            ftp.Hostname = "ftp.bodirectors.com";
            ftp.Username = (App.Current as App).Email + "@bodirectors.com";
            ftp.Password = (App.Current as App).Password;



            await ftp.ConnectAsync();
           
            isFileAvailable = await ftp.PutFileAsync(file.Path, "OutputNames.xml");
            await ftp.DisconnectAsync();

            isFileAvailable = false;
        }

        private void SaveChanges(ref XDocument ControlsXML)
        {
            FileStream fileStream = new FileStream(file.Path, FileMode.Truncate);


            ControlsXML.Save(fileStream);
            fileStream.Dispose();
        }

        private void updateStateXML(int ToggleIndex, string ONorOFF, ref XDocument ControlsXML)
        {
            var DataQuery = from r in ControlsXML.Descendants("Output")
                                select r;
            XElement Data = DataQuery.ElementAt(ToggleIndex);
            Data.Element("status").Value = ONorOFF;

            SaveChanges(ref ControlsXML);
             UploadFile();

        }

        public void updateText(int ToggleIndex,string Text, ref XDocument ControlsXML)
        {
            var DataQuery = from r in ControlsXML.Descendants("Output")
                            select r;
            XElement Data = DataQuery.ElementAt(ToggleIndex);
            Data.Element("name").Value = Text;

            SaveChanges(ref ControlsXML);
            UploadFile();
        }

        public void ChangeControlState(byte PinNumb)
        {
            this.SetPinNumber(PinNumb);
            this.ChangeState();
        }

        private async void LoadControlsXML()
        {
            try
            {

            }
            catch(Exception e)
            {
                await Task.Delay(1000);
                LoadControlsXML();
            }
        }

        public  bool ControlIsON(int PinNumb, ref XDocument ControlsXML)
        {
            
            var DataQuery = from r in ControlsXML.Descendants("Output")
                            select r;
            XElement Data = DataQuery.ElementAt(PinNumb);
            if (Data.Element("status").Value == "On") return true;
            else return false;            
        }

        public string GetPinName(int PinNumb, ref XDocument ControlsXML)
        {
           
            var DataQuery = from r in ControlsXML.Descendants("Output")
                            select r;
            XElement Data = DataQuery.ElementAt(PinNumb);
            return Data.Element("name").Value;
        }

        public int FindPinName(string IncomText, ref XDocument ControlsXML)
        {
          
            var DataQuery = from r in ControlsXML.Descendants("Output")
                            select r;
            for (int x = 0; x < 4; x++)      //should change the hardcoded value 4
            {
                XElement Data = DataQuery.ElementAt(x);
                if (IncomText.Contains(Data.Element("name").Value)) return x;
            }
            return -1;
        }

        public void SetPin(int ToggleIndex, string ONorOFF,ref XDocument ControlsXML)
        {
            int toByte = ToggleIndex + 1;
            var DataQuery = from r in ControlsXML.Descendants("Output")
                            select r;
            XElement Data = DataQuery.ElementAt(ToggleIndex);
            if (Data.Element("status").Value != ONorOFF)
            {
                ChangeControlState((byte)toByte);
                updateStateXML(ToggleIndex, ONorOFF, ref ControlsXML);
            }
        }
    }
}
