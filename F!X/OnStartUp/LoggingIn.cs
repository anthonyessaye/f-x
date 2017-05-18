using F_X.InformationGathering;
using F_X.InformationQueries;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace F_X.OnStartUp
{
   public class LoggingIn
    {

        private Chilkat.Ftp2 ftp;
        public bool isFileDownloaded { get; set; }
        public bool isConnected { get; set; }

        private const string Server = "ftp.bodirectors.com";
        private string Email;
        private string Password;

        

        private StorageFile OutputFile;
        private StorageFile SettingsFile;
        private StorageFile ProfilePictureFile;





        public LoggingIn(string EmailIn, string PassIn)
        {
            Email = EmailIn;
            Password = PassIn;
            ftp = new Chilkat.Ftp2();


        }

  
        public async void ConnectAndGetLatest()
        {
            ftp.UnlockComponent("test");

            ftp.Hostname = "ftp.bodirectors.com";
            ftp.Username = Email + "@bodirectors.com";
            ftp.Password = Password;

            

            OutputFile = await ApplicationData.Current.LocalFolder.GetFileAsync("OutputNames.xml");
            SettingsFile = await ApplicationData.Current.LocalFolder.GetFileAsync("SettingsData.xml");
            ProfilePictureFile = await ApplicationData.Current.LocalFolder.GetFileAsync("profile.jpg");

            
            isFileDownloaded = await ftp.ConnectAsync();
            isFileDownloaded = await ftp.GetFileAsync("OutputNames.xml", OutputFile.Path);
            isFileDownloaded = await ftp.GetFileAsync("SettingsData.xml", SettingsFile.Path);
            isFileDownloaded = await ftp.GetFileAsync("profile.jpg", ProfilePictureFile.Path);

            isConnected = isFileDownloaded;

           
            await ftp.DisconnectAsync();


        }

       

    }
}
