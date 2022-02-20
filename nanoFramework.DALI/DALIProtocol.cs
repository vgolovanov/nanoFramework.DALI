using nanoFramework.Hardware.Esp32.Rmt;
using System;

namespace nanoFramework.DALI
{
    internal class DALIProtocol
    {
        const byte clockDivider = 80; //1us clock( 80Mhz / 80 ) = 1Mhz
        const ushort duration = 416;   //416ms for half bit
        const ushort thresold = 7000; //7ms based on 1us clock
        const byte filter = 10; //filter out 10us / noise
     
        ReceiverChannel rxChannel;
        TransmitterChannel txChannel;

        RmtCommand txPulse1;
        RmtCommand txPulse0;

        private int txPin;
        private int rxPin;

        private int rmtCmdIndex = 0;

        public DALIProtocol(int TxPin, int RxPin)
        {
            txPin = TxPin;
            rxPin = RxPin;

            txPulse1 = new RmtCommand(duration, false, duration, true);
            txPulse0 = new RmtCommand(duration, true, duration, false);

            SetTxChannel();
            SetRxChannel();
        }

        private void SetTxChannel()
        {
            txChannel = new TransmitterChannel(txPin);  //TX channel initialization on txPin
            txChannel.ClockDivider = clockDivider; //1us clock( 80Mhz / 80 ) = 1Mhz
            txChannel.CarrierEnabled = false;
            txChannel.IdleLevel = true;

            while (rmtCmdIndex < 17)                    //creating the buffer for 1 start bit + 16 bits (2 bytes). In tootal 17 bits
            {
                txChannel.AddCommand(new RmtCommand(duration, true, duration, false));    //fill the buffer with 0 bits
                rmtCmdIndex++;
            }
        }
        private void SetRxChannel()
        {
            rxChannel = new ReceiverChannel(rxPin);     //RX channel initialization on rxPin
            rxChannel.ClockDivider = clockDivider;                // 1us clock ( 80Mhz / 80 ) = 1Mhz
            rxChannel.EnableFilter(true, filter);           //filter out 10us / noise             
            rxChannel.SetIdleThresold(thresold);              //7ms based on 1us clock
            rxChannel.ReceiveTimeout = new TimeSpan(0, 0, 0, 0, 30);    //RX timeout 30ms
        }

        public void TransmitContolCommand(byte Address, byte Command)
        {
            rmtCmdIndex = 0;
            SetBit(1); //Start bit
            SetByte(Address); // 1st byte
            SetByte(Command); // 2nd byte
            txChannel.Send(true); // send packet and wait for the transmission end
        }

        public void TransmitConfigurationCommand(byte Address, byte Command)
        {
            rmtCmdIndex = 0;
            SetBit(1); //Start bit
            SetByte(Address); // 1st byte
            SetByte(Command); // 2nd byte       
            txChannel.Send(true); // send packet and wait for the transmission end           
            txChannel.Send(true); // duplucate the packet within 100 ms
        }

        public bool QueryCommand(out byte ResponseValue, bool ExpectingOverlapping = false)
        {
            bool Success = false;
            rmtCmdIndex = 0;

            RmtCommand[] impulses;
            rxChannel.Start(true);
            impulses = rxChannel.GetAllItems();

            int boolIndex = 0;
            bool[] packet = new bool[21];
            int tempResponseValue = 0;
            ResponseValue = 0;

            if (impulses != null)
            {
                Success = true;

                //In case we expecting that possible response packets overlapping from different devices or possible collisions,
                //we can't guarantee exact response value
                //but at least we can confirm that some devices responded and returning Success=true but ResponseValue just 0xFF 
                if (ExpectingOverlapping)
                {
                    ResponseValue = 0xFF;
                    return Success;
                }

                foreach (var impulse in impulses)
                {
                    if (impulse.Duration0 < duration)
                    {
                        //append one false value
                        packet[boolIndex] = false;
                        boolIndex++;
                    }
                    else
                    {
                        //append two false values
                        packet[boolIndex] = false;
                        boolIndex++;
                        packet[boolIndex] = false;
                        boolIndex++;
                    }

                    if (impulse.Duration1 < duration)
                    {
                        ///append one true value
                        packet[boolIndex] = true;
                        boolIndex++;
                    }
                    else
                    {
                        //append two true values
                        packet[boolIndex] = true;
                        boolIndex++;
                        packet[boolIndex] = true;
                        boolIndex++;
                    }
                }

                byte i = 8;     //packet length from 0, start bit then higher to lower bits
                boolIndex = 0;  //index to read all bits

                //decoding Manchester coding
                while (boolIndex < 18)
                {
                    if (packet[boolIndex] == false && packet[boolIndex + 1] == true) tempResponseValue |= (1 << i);                  
                    boolIndex += 2;
                    i--;
                }

                ResponseValue = (byte)tempResponseValue;
            }

            rxChannel.Stop();

            return Success;
        }

            
        private void SetByte(byte Packet)
        {
            for (int i = 7; i >= 0; i--)
            {
                SetBit((Packet >> i) & 1);  //after start bit is higher bit 
            }
        }

        private void SetBit(int Bit)
        {
            if (Bit == 1)
            {
                txChannel[rmtCmdIndex].Level0 = false;
                txChannel[rmtCmdIndex].Level1 = true;
            }
            else
            {
                txChannel[rmtCmdIndex].Level0 = true;
                txChannel[rmtCmdIndex].Level1 = false;
            }
            rmtCmdIndex++;
        }
    }
}
