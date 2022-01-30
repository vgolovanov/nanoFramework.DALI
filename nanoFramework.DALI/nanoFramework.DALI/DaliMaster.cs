using nanoFramework.Hardware.Esp32;
using System;
using System.Device.I2c;
using System.Diagnostics;

namespace nanoFramework.DALI
{
    public class DaliMaster
    {
        private readonly I2cDevice Wire;

        public DaliMaster()
        {
            Configuration.SetPinFunction(Gpio.IO22, DeviceFunction.I2C1_CLOCK);
         //   Configuration.SetPinFunction(Gpio.IO22, DeviceFunction.I2C1_DATA);

            Configuration.SetPinFunction(Gpio.IO21, DeviceFunction.I2C1_DATA);
          //  Configuration.SetPinFunction(Gpio.IO21, DeviceFunction.I2C1_CLOCK);
        }

        public DaliMaster(byte I2cBus = 1, byte ModuleAddress = 0x20)
        {

        }

        public byte ScanI2cBus(int I2cBus)
        {
            byte numDev = 0;

            Configuration.SetPinFunction(Gpio.IO21, DeviceFunction.I2C1_DATA);
            Configuration.SetPinFunction(Gpio.IO22, DeviceFunction.I2C1_CLOCK);          
           
            

            var buffer = new byte[6];
            //var scanI2cDevice1 = I2cDevice.Create(new I2cConnectionSettings(1, 0X20, I2cBusSpeed.FastMode));
            //var result11 = scanI2cDevice1.WriteByte(0xF0);
            //var result21 = scanI2cDevice1.Read(buffer);

            for (byte address = 1; address < 127; address++)
            {


                var scanI2cDevice = I2cDevice.Create(new I2cConnectionSettings(I2cBus, address, I2cBusSpeed.StandardMode));
              
                  
                
                var result = scanI2cDevice.WriteRead(new byte[] { 0xF0 }, buffer);

                //var result = scanI2cDevice.WriteByte(0xF0);
                //var result2 = scanI2cDevice.Read(buffer);
                


                //Wire.requestFrom(address, 1);
                //     scanI2cDevice.

                // Debug.WriteLine(address + " " + result.Status.ToString());
                //Debug.WriteLine(address + " " + buffer[0].ToString()+":" + buffer[1].ToString() + ":" + buffer[2].ToString() + ":" + buffer[3].ToString() + ":" + buffer[4].ToString() + ":" + buffer[5].ToString());
                Debug.WriteLine(address + " " + buffer[0].ToString() + ":" + buffer[1].ToString() + ":" + buffer[2].ToString() + ":" + buffer[3].ToString() + ":" + buffer[4].ToString() + ":" + buffer[5].ToString());

                if (result.Status==I2cTransferStatus.FullTransfer || result.Status == I2cTransferStatus.PartialTransfer)
                {
                    numDev++;
                }
            }

            return numDev;
        }
    }
}
