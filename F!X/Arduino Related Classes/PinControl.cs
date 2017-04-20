using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;
using Windows.Storage;
using System.Threading;
using System.Xml.Linq;
using System.IO;
using Windows.System.Threading;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Core;

namespace F_X.Arduino_Related_Classes
{
    class PinControl
    {
        private byte pinNumber { get; set; }
        private PinState LOWorHIGH { get; set; }
        private UsbSerial connection { get; set; }
        //IStream connection;
        public RemoteDevice arduino { get; set; }
        private uint baudRate { get; set; }

        private Chilkat.Ftp2 ftp;
        private bool isFileAvailable;


        public TimeSpan period { get; set; }
        private XDocument NamesXML;

        private AutoResetEvent _refreshWaiter = new AutoResetEvent(true);
        private string Email;
        private string Password;

        private string[] OriginalStates;
        private const string Server = "ftp.bodirectors.com";

        public PinControl(string VID, string PID, uint BaudRate)
        {
            connection = new UsbSerial(VID, PID);
            baudRate = BaudRate;

            arduino = new RemoteDevice(connection);
            connection.begin(baudRate, SerialConfig.SERIAL_8N1);

            ftp = new Chilkat.Ftp2();



        }

        public void ChangeState()
        {
            var state = arduino.digitalRead(pinNumber);
            var nextState = (state == PinState.HIGH) ? PinState.LOW : PinState.HIGH;
            arduino.digitalWrite(pinNumber, nextState);
        }


        public void SetPinNumber(byte PinNumber)
        {
            PinNumber += 4;
            pinNumber = PinNumber;

        }

        // this function is currently not working i think analog read is wrong but ill need to debug
        // more to find out using the hardware.

        public int CalculateTemperature()
        {

            double Temp_reading = arduino.analogRead("A0");
            double voltage = Temp_reading * 5;
            voltage /= 1024;
            double temperatureC = (voltage - 0.5) / 10;

            return Convert.ToInt32(temperatureC);

        }


        private void ConnectingToFtp()
        {
            ftp.UnlockComponent("test");

            ftp.Hostname = "ftp.bodirectors.com";
            ftp.Username = (App.Current as App).Email + "@bodirectors.com";
            ftp.Password = (App.Current as App).Password;

        }

        public async void getOriginalStates()
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("OutputNames.xml");
            NamesXML = XDocument.Load(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("OutputNames.xml"));

            var DataQuery = from r in NamesXML.Descendants("Output")
                            select r;

            for (int i = 0; i < 4; i++)
            {
                XElement Data = DataQuery.ElementAt(i);
                OriginalStates[i] = Data.Element("status").Value;
                
            }


        }

        public async void UpdatingPinsThread(int TimerInSeconds)
        {
            ConnectingToFtp();

            getOriginalStates();

            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("OutputNames.xml");

            if (period == null)
                period = TimeSpan.FromSeconds(TimerInSeconds);


            ThreadPoolTimer PeriodicTimer = ThreadPoolTimer.CreatePeriodicTimer(async (source) =>
            {
                try
                {

                    await ftp.ConnectAsync();
                    isFileAvailable = await ftp.GetFileAsync("OutputNames.xml", file.Path);
                    await ftp.DisconnectAsync();

                    NamesXML = XDocument.Load(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("OutputNames.xml"));

                    var DataQuery = from r in NamesXML.Descendants("Output")
                                    select r;

                    for (int i = 0; i < 4; i++)
                    {
                        XElement Data = DataQuery.ElementAt(i);
                        if (OriginalStates[i] != Data.Value)
                        {
                            SetPinNumber(Convert.ToByte(i + 1));
                            ChangeState();
                        }

                    }
                }

                catch
                {

                }
            }, period);


        }

        public async void UpdatingPinsThreadAndGui(TextBox[] NamesToUpdate, ToggleButton[] TogglesToUpdate, TextBlock Status, int TimerInSeconds)
        {


            StorageFile file = await ApplicationData.Current.LocalFolder.GetFileAsync("OutputNames.xml");
        

            Status.Text = "Connecting to FTP...";
            ConnectingToFtp();


            period = TimeSpan.FromSeconds(TimerInSeconds);


            ThreadPoolTimer PeriodicTimer = ThreadPoolTimer.CreatePeriodicTimer(async (source) =>
            {

                await ftp.ConnectAsync();
                isFileAvailable = await ftp.GetFileAsync("OutputNames.xml", file.Path);
                await ftp.DisconnectAsync();

                  

                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.High,
                    async () =>
                    {
                        try
                        {
                            NamesXML = XDocument.Load(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("OutputNames.xml"));
                            Status.Text = "Reading New Settings";
                            var DataQuery = from r in NamesXML.Descendants("Output")
                                            select r;

                            Status.Text = "Flipping some buttons now :)";
                            TimeSpan.FromSeconds(1);
                            for (int i = 0; i < NamesToUpdate.Length; i++)
                            {
                                XElement Data = DataQuery.ElementAt(i);
                                NamesToUpdate[i].Text = Data.Element("name").Value;

                                SetPinNumber(Convert.ToByte(i + 1));
                                ChangeState();

                            }
                            Status.Text = "Controls are Up-to-date";
                        }
                        catch (Exception e)
                        {
                           
                        }

                    }
                    );
               


            }, period);
            
        }

    }
}
