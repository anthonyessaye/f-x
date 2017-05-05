
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
        PinControlReally theArduino = new PinControlReally();
        PinControl theComponent = new PinControl("FixHelp", 115200);

        FTPDownloads pleaseDownload = new FTPDownloads();



        public async void onBoot()

        {
            

            theNameBoxes = new TextBox[] { OutputOneName, OutputTwoName, OutputThreeName,
                                           OutputFourName};

            theToggles = new ToggleButton[] { OutputOneToggle, OutputTwoToggle, OutputThreeToggle,
                                              OutputFourToggle};

            statusText.TextAlignment = TextAlignment.Center;

            updateOnBoot();


           for(int i = 0; i < 15; i++)
            {
                await Task.Delay(1000);
                if(theComponent.isBtAvailable)
                {
                    ComponentSection.Visibility = Visibility.Visible;
                    break;
                }

                else
                {
                    statusText.Text = "No Components Available";
                }
            }

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
           
            if (OutputOneToggle.IsChecked == true)
            {
                OutputOneToggle.Content = "On";
                theArduino.SetPin(0, "On", ref NamesXML);
            }

            else
            {
                OutputOneToggle.Content = "Off";
                theArduino.SetPin(0, "Off", ref NamesXML);

            }


        }
        private void OutputTwoToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (OutputTwoToggle.IsChecked == true)
            {
                OutputTwoToggle.Content = "On";
                theArduino.SetPin(1, "On", ref NamesXML);
            }

            else
            {
                OutputTwoToggle.Content = "Off";
                theArduino.SetPin(1, "Off", ref NamesXML);

            }

        }
        private void OutputThreeToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (OutputThreeToggle.IsChecked == true)
            {
                OutputThreeToggle.Content = "On";
                theArduino.SetPin(2, "On", ref NamesXML);

            }
            else
            {
                OutputThreeToggle.Content = "Off";
                theArduino.SetPin(2, "Off", ref NamesXML);

            }
         
        }
        private void OutputFourToggle_Checked(object sender, RoutedEventArgs e)
        {
            if (OutputFourToggle.IsChecked == true)
            {
                OutputFourToggle.Content = "On";
                theArduino.SetPin(3, "On", ref NamesXML);

            }
            else
            {
                OutputFourToggle.Content = "Off";
                theArduino.SetPin(3, "Off", ref NamesXML);

            }

        }


        private async void updateOnBoot()
        {
            pleaseDownload.getLatestOutputs();

            NamesXML = XDocument.Load(await ApplicationData.Current.LocalFolder.OpenStreamForReadAsync("OutputNames.xml"));

            try
            {
                statusText.Text = "Flipping some buttons now :)";

                for (int i = 0; i < 4; i++)
                {
                    theNameBoxes[i].Text = theArduino.GetPinName(i,ref NamesXML);
                    

                    if (theArduino.ControlIsON(i,ref NamesXML) == true)
                    {
                        if (i == 0)
                        {
                            OutputOneToggle.IsChecked = true;
                            OutputOneToggle_Click(OutputOneToggle, new RoutedEventArgs());
                        }

                        if (i == 1)
                        {
                            OutputTwoToggle.IsChecked = true;
                            OutputTwoToggle_Checked(OutputTwoToggle, new RoutedEventArgs());
                        }

                        if (i == 2)
                        {
                            OutputThreeToggle.IsChecked = true;
                            OutputThreeToggle_Checked(OutputThreeToggle, new RoutedEventArgs());
                        }

                        if (i == 3)
                        {
                            OutputFourToggle.IsChecked = true;
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

        

        private void OutputOneName_LostFocus(object sender, RoutedEventArgs e)
        {
            theArduino.updateText(0, OutputOneName.Text, ref NamesXML);
        }
        private void OutputTwoName_LostFocus(object sender, RoutedEventArgs e)
        {
            theArduino.updateText(1, OutputTwoName.Text, ref NamesXML);
        }
        private void OutputThreeName_LostFocus(object sender, RoutedEventArgs e)
        {
            theArduino.updateText(2, OutputThreeName.Text, ref NamesXML);
        }
        private void OutputFourName_LostFocus(object sender, RoutedEventArgs e)
        {
            theArduino.updateText(3, OutputFourName.Text, ref NamesXML);
        }
    }
}
