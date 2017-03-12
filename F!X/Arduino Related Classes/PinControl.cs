using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maker.RemoteWiring;
using Microsoft.Maker.Serial;

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

        public PinControl(string VID, string PID, uint BaudRate){
            connection = new UsbSerial(VID,PID);
            baudRate = BaudRate;

            arduino = new RemoteDevice(connection);
            connection.begin(baudRate, SerialConfig.SERIAL_8N1);

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


    }
}
