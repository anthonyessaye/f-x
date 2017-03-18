using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace F_X
{
    class CheckAndCreateDirectory
    {
       public string folderName { get; set; }
       public string XMLString { get; set; }
       public StorageFolder UniversalStorageFolder { get; set; }

        public CheckAndCreateDirectory()
        {
             folderName = "C:/F!X_UserData/";
             XMLString = "XML Files";
            


        }

        public async void iMustCreate()
        {

          //  if (ApplicationData.Current.LocalFolder.Path == null)
            //    await ApplicationData.Current.LocalFolder.CreateFolderAsync(XMLString);
            
        }


      

        public async void CopyFileToFolder(StorageFile filetoCopy, StorageFolder folder)

        {
            if (folder.TryGetItemAsync(filetoCopy.ToString()) == null)
            await filetoCopy.CopyAsync(folder);

        }

    }
}
