using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace F_X.InformationGathering
{
    class FTPDownloads
    {

        private Chilkat.Ftp2 ftp;
        private bool isFileAvailable;


        private const string Server = "ftp.bodirectors.com";

        public FTPDownloads()
        {
            ftp = new Chilkat.Ftp2();
        }

        private void ConnectingToFtp()
        {
            ftp.UnlockComponent("test");

            ftp.Hostname = "ftp.bodirectors.com";
            ftp.Username = (App.Current as App).Email + "@bodirectors.com";
            ftp.Password = (App.Current as App).Password;

        }

        public async void getLatestOutputs()
        {

            
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("OutputNames.xml");

            await ftp.ConnectAsync();
            isFileAvailable = await ftp.GetFileAsync("OutputNames.xml", file.Path);
            await ftp.DisconnectAsync();

        }




    }
}
