
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
using Windows.System.Threading;
using Windows.UI.Core;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace F_X
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Controls : Page
    {

        PinControl theArduino = new PinControl("VID_2341", "PID_0243",57600);
        // PinControl is a class i created to minimize code in this section, but basically
        // it configures connection to arduino and has some basic functions.
        // This settings must be moved to each page if we need the assistant to control the settings.
        // or optionally we can pass the values from a page to another

        XDocument NamesXML;
        TextBox[] theNameBoxes;
        ToggleButton[] theToggles;
        bool isFocused = true;

        private void onBoot()
        {
            theNameBoxes = new TextBox[] { OutputOneName, OutputTwoName, OutputThreeName,
                                           OutputFourName, OutputFiveName, OutputSixName,
                                           OutputSevenName, OutputEightName};

            theToggles = new ToggleButton[] { OutputOneToggle, OutputTwoToggle, OutputThreeToggle,
                                              OutputFourToggle, OutputFiveToggle, OutputSixToggle,
                                              OutputSevenToggle, OutputEightToggle};


            theArduino.UpdatingPinsThreadAndGui(theNameBoxes, theToggles, statusText,10);

            // This function contains two arrays for UI Elements and the function that updates them every given amount of time

        }
       

                public Controls()
        {
            this.InitializeComponent();
            onBoot();
            


            // connection = new BluetoothSerial( "MLT-BT05");
            //connection.begin(115200, SerialConfig.SERIAL_8N1);
            // I left these here for bluetooth connection later on.




            int HumidityReading = theArduino.arduino.analogRead("A1") / 100;
            

            Temp_Txt.Text = "The room temperature is: " + theArduino.CalculateTemperature() + " °C";
            Humidity_Text.Text = "The room humidity is: " + HumidityReading + "%";

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
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationPane.IsPaneOpen = !NavigationPane.IsPaneOpen;
        }

        public void OutputOneToggle_Click(object sender, RoutedEventArgs e)
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

        //need to update all the buttons wuth new code from output one
        // UPDATE - THIS IS FIXED
        public void OutputTwoToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (OutputTwoToggle.IsChecked == true)
                OutputTwoToggle.Content = "On";

            else
                OutputTwoToggle.Content = "Off";
    
            theArduino.SetPinNumber(2);
            theArduino.ChangeState();
        }
        public void OutputThreeToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (OutputThreeToggle.IsChecked == true)
                OutputThreeToggle.Content = "On";

            else
                OutputThreeToggle.Content = "Off";
         
            theArduino.SetPinNumber(3);
            theArduino.ChangeState();
        }
        public void OutputFourToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (OutputFourToggle.IsChecked == true)
                OutputFourToggle.Content = "On";
         
            else
                OutputFourToggle.Content = "Off";
          
            theArduino.SetPinNumber(4);
            theArduino.ChangeState();
        }
        public void OutputFiveToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (OutputFiveToggle.IsChecked == true)
                OutputFiveToggle.Content = "On";

            else
                OutputFiveToggle.Content = "Off";
          
            theArduino.SetPinNumber(5);
            theArduino.ChangeState();
        }
        public void OutputSixToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (OutputSixToggle.IsChecked == true)
                OutputSixToggle.Content = "On";

            else
                OutputSixToggle.Content = "Off";
            
            theArduino.SetPinNumber(6);
            theArduino.ChangeState();
        }
        public void OutputSevenToggle_Checked(object sender, RoutedEventArgs e)
        {
            theArduino.SetPinNumber(7);
            theArduino.ChangeState();

            if (OutputSevenToggle.IsChecked == true)
                OutputSevenToggle.Content = "On";
            else
                OutputSevenToggle.Content = "Off"; 
            
        }
        public void OutputEightToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (OutputEightToggle.IsChecked == true)
                OutputEightToggle.Content = "On";
           
            else
                OutputEightToggle.Content = "Off";

            theArduino.SetPinNumber(8);
            theArduino.ChangeState();
        }





    }
}
