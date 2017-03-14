
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

        XDocument NamesXML = XDocument.Load(@"Database/OutputNames.xml");
        
        
        public Controls()
        {
            this.InitializeComponent();

            TextBox[] AllOutputs = new TextBox[] { OutputOneName, OutputTwoName, OutputThreeName,
                                                   OutputFourName,OutputFiveName,OutputSixName,
                                                   OutputSevenName,OutputEightName}; // Array for the 8 output names
       
            // connection = new BluetoothSerial( "MLT-BT05");
            //connection.begin(115200, SerialConfig.SERIAL_8N1);
            // I left these here for bluetooth connection later on.


            var NamesQuery = from r in NamesXML.Descendants("Output")
                              select r;

            for (int i = 0; i < 8; i++)
            {
                XElement Names = NamesQuery.ElementAt(i);
                AllOutputs[i].Text = Names.Element("name").Value;
            } // This "for" loop reads values from XML Database for the names of OUTPUTS

            //TO-DO ------- Write and save to XML and clean code


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

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationPane.IsPaneOpen = !NavigationPane.IsPaneOpen;
        }

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

        //need to update all the buttons wuth new code from output one
        private void OutputTwoToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (OutputTwoToggle.IsChecked == true)
            {
                OutputTwoToggle.Content = "On";
            }

            else
            {
                OutputTwoToggle.Content = "Off";
            }
        }
        private void OutputThreeToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (OutputThreeToggle.IsChecked == true)
            {
                OutputThreeToggle.Content = "On";
            }

            else
            {
                OutputThreeToggle.Content = "Off";
            }
        }
        private void OutputFourToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (OutputFourToggle.IsChecked == true)
            {
                OutputFourToggle.Content = "On";
            }

            else
            {
                OutputFourToggle.Content = "Off";
            }
        }
        private void OutputFiveToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (OutputFiveToggle.IsChecked == true)
            {
                OutputFiveToggle.Content = "On";
            }

            else
            {
                OutputFiveToggle.Content = "Off";
            }
        }
        private void OutputSixToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (OutputSixToggle.IsChecked == true)
            {
                OutputSixToggle.Content = "On";
            }

            else
            {
                OutputSixToggle.Content = "Off";
            }
        }
        private void OutputSevenToggle_Checked(object sender, RoutedEventArgs e)
        {
            theArduino.SetPinNumber(7);
            theArduino.ChangeState();

            if (OutputSevenToggle.IsChecked == true)
                OutputSevenToggle.Content = "On";
            else
                OutputSevenToggle.Content = "Off"; 
            
        }
        private void OutputEightToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (OutputEightToggle.IsChecked == true)
            {
                OutputEightToggle.Content = "On";
            }

            else
            {
                OutputEightToggle.Content = "Off";
            }
        }





    }
}
