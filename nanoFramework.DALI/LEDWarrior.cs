using System;
using System.Device.I2c;
using System.Threading;

namespace nanoFramework.DALI
{
    public class LEDWarrior
    {
        private readonly I2cDevice Wire;
        private byte address;

        const byte BLANK_C = 0x00; // Easier to type

        // Specifically related to initialisation
        const byte RESET_C = 0x20;
        const byte TERMINATE_C = 0xA1;
        const byte INITIALISE_C = 0xA5;
        const byte RANDOMISE_C = 0xA7;
        const byte COMPARE_C = 0xA9;
        const byte SEARCHADDRH_C = 0xB1;
        const byte SEARCHADDRM_C = 0xB3;
        const byte SEARCHADDRL_C = 0xB5;
        const byte PROGRAMSHORT_C = 0xB7;
        const byte WITHDRAW_C = 0xAB;


        public LEDWarrior(byte I2cBus = 1, byte ModuleAddress = 0x23)
        {
            address = ModuleAddress;
            Wire = I2cDevice.Create(new I2cConnectionSettings(I2cBus, address, I2cBusSpeed.FastMode));
            clearStatusRegister();
            TransmitCommand(TERMINATE_C, BLANK_C);
            TransmitCommand(RESET_C, BLANK_C);
            TransmitCommand(RESET_C, BLANK_C);
            clearStatusRegister();
        }

        public byte TransmitCommand(byte cmd1, byte cmd2)
        {
            // Make sure the command register is clear before continuing
            clearStatusRegister();
        }

        private void clearStatusRegister()
        {
            byte  a, b;
            byte s = getStatus();
        }

        private byte getStatus()
        {
            // Wait until we have the correct number of bytes
            while (!getStatusRaw())
            {               
            }

            return Wire.ReadByte();
        }

        private bool getStatusRaw()
        {
            I2cTransferResult result;

            bool returnValue = false;

            int Counter = 1000;

            while (Counter!=0)
            {
                result =Wire.WriteByte(0x00);

                if (result.Status == I2cTransferStatus.FullTransfer)
                {
                    returnValue = true;
                    Counter = 0;
                }
                Thread.Sleep(1);
            }            
            return returnValue;
        }
    }
}
