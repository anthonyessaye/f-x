﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace F_X.OnStartUp
{
    class LoggingIn
    {

        private Chilkat.Ftp2 ftp;
        public bool isFileDownloaded { get; set; }

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


            await ftp.ConnectAsync();
            isFileDownloaded = await ftp.GetFileAsync("OutputNames.xml", OutputFile.Path);
            isFileDownloaded = await ftp.GetFileAsync("SettingsData.xml", SettingsFile.Path);
            isFileDownloaded = await ftp.GetFileAsync("OutputNames.xml", ProfilePictureFile.Path);
            await ftp.DisconnectAsync();
        
        }


       

    }
}