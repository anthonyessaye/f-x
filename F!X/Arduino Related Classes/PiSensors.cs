using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Sensors.Dht;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;
using Windows.UI.Xaml;

namespace F_X.Arduino_Related_Classes
{
    class PiSensors
    {

        //Microsoft Sample Classes Below This
        private DispatcherTimer _dispatchTimer;
        private GpioPin _temperaturePin;
        private IDht _dhtInterface;
        private List<int> _retryCount;
        private DateTimeOffset _startedAt;
        private DeviceClient deviceClient;
        public int TotalAttempts { get; private set; }
        public float Temperature { get; private set; }
        public float Humidity { get; private set; }

        public PiSensors()
        {
            if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.IoT")
            {
                InittempSensor();

            }
          
        }


        private void InittempSensor()
        {
            // call the method to initialize variables and hardware components
            InitHardware();

            // set interval of timer to 1 second
            _dispatchTimer.Interval = TimeSpan.FromSeconds(1);

            // invoke a method at each tick (as per interval of your timer)
            _dispatchTimer.Tick += _dispatchTimer_Tick;

            // initialize pin (GPIO pin on which you have set your temperature sensor)
            _temperaturePin = GpioController.GetDefault().OpenPin(4, GpioSharingMode.Exclusive);

            // create instance of a DHT11 
            _dhtInterface = new Dht11(_temperaturePin, GpioPinDriveMode.Input);

            // start the timer
            _dispatchTimer.Start();

            // set start date time
            _startedAt = DateTimeOffset.Now;

            //Azure IoT Hub
            string iotHubUri = "mondayiothub1.azure-devices.net";
            string deviceId = "iot1";
            string deviceKey = "Dfdr5BaIf+0uJUMLa8YBcIe74fNpBvsQ7FayoQpRXXs=";

            deviceClient = DeviceClient.Create(iotHubUri,
                        AuthenticationMethodFactory.
                            CreateAuthenticationWithRegistrySymmetricKey(deviceId, deviceKey),
                        TransportType.Http1);
        }


        private void InitHardware()
        {
            _dispatchTimer = new DispatcherTimer();
            _temperaturePin = null;
            _dhtInterface = null;
            _retryCount = new List<int>();
            _startedAt = DateTimeOffset.Parse("1/1/1");
        }

        private async void _dispatchTimer_Tick(object sender, object e)
        {
            try
            {
                DhtReading reading = new DhtReading();

                int val = this.TotalAttempts;

                this.TotalAttempts++;

                reading = await _dhtInterface.GetReadingAsync().AsTask();

                _retryCount.Add(reading.RetryCount);
                //this.OnPropertyChanged(nameof(AverageRetriesDisplay));
                //this.OnPropertyChanged(nameof(TotalAttempts));
                //this.OnPropertyChanged(nameof(PercentSuccess));

                if (reading.IsValid) // if we are able to capture value, display those
                {
                    //this.TotalSuccess++;
                    this.Temperature = Convert.ToSingle(reading.Temperature);
                    this.Humidity = Convert.ToSingle(reading.Humidity);
                    //this.LastUpdated = DateTimeOffset.Now;
                    //this.OnPropertyChanged(nameof(SuccessRate));

                    var telemetryDataPoint = new
                    {
                        deviceId = "iot1",
                        temperature = Temperature.ToString(),
                        humidity = Humidity.ToString(),
                        date = DateTime.Now.ToString("dd-MM-yyyy"),
                        hours = DateTime.Now.Hour.ToString(),
                        minutes = DateTime.Now.Minute.ToString(),
                        seconds = DateTime.Now.Second.ToString()

                    };

                    var messageString = JsonConvert.SerializeObject(telemetryDataPoint);
                    var message = new Message(Encoding.ASCII.GetBytes(messageString));

                    await deviceClient.SendEventAsync(message);


                    Debug.WriteLine(message);


                }
                else // log if the reading is not in valid state
                {
                    Debug.WriteLine(string.Format("IsValid: {0}, RetryCount: {1}, TimedOut: {2}, Humidity: {3}, Temperature: {4}", reading.IsValid, reading.RetryCount, reading.TimedOut, reading.Humidity, reading.Temperature));
                }

                //this.OnPropertyChanged(nameof(LastUpdatedDisplay)); // show when the data was last updated
                //this.OnPropertyChanged(nameof(DateTimeDisplay));
            }
            catch (Exception ex) // log any exception that occurs
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public string getRoomData()
        {

            if (Windows.System.Profile.AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.IoT")
            {
                return "Current Room Temperature: " + Temperature + " ºC\nCurrent Room Humidity: " + Humidity + " %";

            }

            else
                return "Sensor Not Available";
            

           
        }

    }
}
