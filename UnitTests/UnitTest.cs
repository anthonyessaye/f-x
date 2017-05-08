
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using F_X;
using Windows.Storage;
using F_X.OnStartUp;
using System.Threading.Tasks;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async void LoggingIn()
        {

            StorageFolder MainFolder = ApplicationData.Current.LocalFolder;
            StorageFile OutputNamesfile = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"OutputNames.xml"); // Path is file path
            StorageFile Settingsfile = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"SettingsData.xml"); // Path is file path
            StorageFile profileFile = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(@"profile.jpg"); // Path is file path

            if (await FileExistAsync(OutputNamesfile.Name) == false)
            {
                await OutputNamesfile.CopyAsync(MainFolder, OutputNamesfile.Name, NameCollisionOption.GenerateUniqueName);
              //  Weather theWeather = new Weather("Beirut", true);
            }
            if (await FileExistAsync(Settingsfile.Name) == false)
            {
                await Settingsfile.CopyAsync(MainFolder, Settingsfile.Name, NameCollisionOption.GenerateUniqueName);

            }
            if (await FileExistAsync(profileFile.Name) == false)
            {
                await profileFile.CopyAsync(MainFolder, profileFile.Name, NameCollisionOption.GenerateUniqueName);

            }


            LoggingIn theLogin;

            theLogin = new LoggingIn("test","theTest.");
            theLogin.ConnectAndGetLatest();
            

            for (int i = 0; i < 15; i++)
            {
                await Task.Delay(1000);

                if (theLogin.isConnected)
                {
                    Assert.IsTrue(theLogin.isConnected);
                    break;
                }
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
