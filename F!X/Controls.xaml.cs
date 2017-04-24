
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;
using Microsoft.Maker.Firmata;
using F_X.Arduino_Related_Classes;
using System.Xml;
using System.Xml.Linq;
using Windows.Storage;
using F_X.InformationGathering;
using System.Threading.Tasks;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace F_X
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Controls : Page
    {

        //VID: 2341
        // PID: 0001


        //  PinControl theArduino = new PinControl("VID_2341", "PID_0243", 57600); Changed PID for new arduino

        

        // PinControl is a class i created to minimize code in this section, but basically
        // it configures connection to arduino and has some basic functions.
        // This settings must be moved to each page if we need the assistant to control the settings.
        // or optionally we can pass the values from a page to another

        XDocument NamesXML;
        TextBox[] theNameBoxes;
        ToggleButton[] theToggles;
        bool isFocused = true;
        FTPDownloads pleaseDownload = new FTPDownloads();
        PinControl theArduino = new PinControl("VID_2341", "PID_0001", 57600);

        public async void onBoot()

        {
            theNameBoxes = new TextBox[] { OutputOneName, OutputTwoName, OutputThreeName,
                                           OutputFourName};

            theToggles = new ToggleButton[] { OutputOneToggle, OutputTwoToggle, OutputThreeToggle,
                                              OutputFourToggle};

            statusText.TextAlignment = TextAlignment.Center;

            updateOnBoot();



            // theArduino.UpdatingPinsThreadAndGui(theNameBoxes, theToggles, statusText, 2);

            // This function contains two arrays for UI Elements and the function that updates them every given amount of time

        }

        public Controls()
        {
            this.InitializeComponent();

            onBoot();

           

            // connection = new BluetoothSerial( "MLT-BT05");
            //connection.begin(115200, SerialConfig.SERIAL_8N1);
            // I left these here for bluetooth connection later on.



           // Temp_Txt.Text = "The room temperature is: " + theArduino.CalculateTemperature() + " °C";
           // Humidity_Text.Text = "The room humidity is: " + HumidityReading + "%";

        }


        private void OnConnectionEstablished()
        {
            var action = Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, new Windows.UI.Core.DispatchedHandler(() =>
            {
            }));
            

        }


        private void Home_Checked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
        private void Assistant_Checked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(PersonalAssistant.AssistantPage));
        }
        private void Controls_Checked(object sender, RoutedEventArgs e)
        {
             this.Frame.Navigate(typeof(Controls));
        }
        private void Settings_Checked(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Settings));
        }
        private void Logout_Checked(object sender, RoutedEventArgs e)
        {
            
            this.Frame.Navigate(typeof(LoginPage));
        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationPane.IsPaneOpen = !NavigationPane.IsPaneOpen;
        }



        //need to update all the buttons wuth new code from output one
        // UPDATE - THIS IS FIXED
        private void OutputOneToggle_Click(object sender, RoutedEventArgs e)
        {
            theArduino.SetPinNumber(1);
            if (OutputOneToggle.IsChecked == true)
            {
                OutputOneToggle.Content = "On";
            }

            else
            {
                OutputOneToggle.Content = "Off";
            }
            theArduino.ChangeState();
        }
        private void OutputTwoToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (OutputTwoToggle.IsChecked == true)
                OutputTwoToggle.Content = "On";

            else
                OutputTwoToggle.Content = "Off";
    
            theArduino.SetPinNumber(2);
            theArduino.ChangeState();
        }
        private void OutputThreeToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (OutputThreeToggle.IsChecked == true)
                OutputThreeToggle.Content = "On";

            else
                OutputThreeToggle.Content = "Off";
         
            theArduino.SetPinNumber(3);
            theArduino.ChangeState();
        }
        private void OutputFourToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (OutputFourToggle.IsChecked == true)
                OutputFourToggle.Content = "On";
         
            else
                OutputFourToggle.Content = "Off";
          
            theArduino.SetPinNumber(4);
            theArduino.ChangeState();
        }


        private async  void updateOnBoot()
        {
            pleaseDownload.getLatestOutputs();
            

            try
            {
                await Task.Delay(1000);
                NamesXML = XDocument.Load(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("OutputNames.xml"));
                statusText.Text = "Reading New Settings";
                var DataQuery = from r in NamesXML.Descendants("Output")
                                select r;

                statusText.Text = "Flipping some buttons now :)";

                await Task.Delay(2000);
                for (int i = 0; i < theNameBoxes.Length; i++)
                {
                    XElement Data = DataQuery.ElementAt(i);
                    theNameBoxes[i].Text = Data.Element("name").Value;

                    
                    if (theToggles[i].Content.ToString() != Data.Element("status").Value)
                    {
                        if (i == 0)
                        {
                            OutputOneToggle.IsChecked = true;
                            OutputOneToggle.Content = "On";
                            OutputOneToggle_Click(OutputOneToggle, new RoutedEventArgs());
                        }

                        if (i == 1)
                        {
                            OutputTwoToggle.IsChecked = true;
                            OutputTwoToggle.Content = "On";
                            OutputTwoToggle_Checked(OutputTwoToggle, new RoutedEventArgs());
                        }

                        if (i == 2)
                        {
                            OutputThreeToggle.IsChecked = true;
                            OutputThreeToggle.Content = "On";
                            OutputThreeToggle_Checked(OutputThreeToggle, new RoutedEventArgs());
                        }

                        if (i == 3)
                        {
                            OutputFourToggle.IsChecked = true;
                            OutputFourToggle.Content = "On";
                            OutputFourToggle_Checked(OutputFourToggle, new RoutedEventArgs());
                        }

                    }


                }
                statusText.Text = "Controls are Up-to-date";
            }
            catch (Exception e)
            {

            }

        }

      

        
    }
}
