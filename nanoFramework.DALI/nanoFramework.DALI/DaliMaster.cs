using nanoFramework.Hardware.Esp32;
using nanoFramework.Hardware.Esp32.Rmt;
using System;
using System.Collections;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading;

namespace nanoFramework.DALI
{
    public class DaliMaster
    {
        ReceiverChannel rxChannel;
        TransmitterChannel txChannel;

        RmtCommand txPulse1;
        RmtCommand txPulse0;

        private int txPin;
        private int rxPin;

        private int rmtCmdIndex = 0;

        //         DALI commands have a length of 16 bits and two values in the data field: address and
        //         opcode[IEC62386 - 102].

        //        DALI-2 commands have a length of 24 bits and three values in the data field: address,
        //        instance byte and opcode[IEC62386 - 103].

        //         Answers have a length of 8 bits, a single value in the data field: the answer of the device
        //         [IEC62386 - 102]
   


        public DaliMaster(int TxPin, int RxPin)
        {
            txPin = TxPin;
            rxPin = RxPin;

            txPulse1 = new RmtCommand(416, false, 416, true);
            txPulse0 = new RmtCommand(416, true, 416, false);

            SetTxChannel();
        }

        private void SetTxChannel()
        {
            txChannel = new TransmitterChannel(txPin);
            txChannel.ClockDivider = 80;
            txChannel.CarrierEnabled = false;            
            txChannel.IdleLevel = true;

            while (rmtCmdIndex < 17)
            {
                txChannel.AddCommand(new RmtCommand(416, true, 416, false));
                rmtCmdIndex++;
            }        
        }

        private void SetRxChannel()
        {
            rxChannel = new ReceiverChannel(rxPin);
            rxChannel.ClockDivider = 80; // 1us clock ( 80Mhz / 80 ) = 1Mhz
          //  rxChannel.EnableFilter(true, 100); // filter out 100Us / noise 
        //    rxChannel.SetIdleThresold(400);  // 40ms based on 1us clock
            rxChannel.ReceiveTimeout = new TimeSpan(0, 0, 0, 0, 60);
        }

        public void TransmitContolCommand(byte Address,byte Command)
        {
            rmtCmdIndex = 0;       
            SetBit(1); //Start bit
            SetByte(Address);
            SetByte(Command);        
            txChannel.Send(true);


        }

        public void TransmitConfigurationCommand(byte Address, byte Command)
        {
            rmtCmdIndex = 0;       
            SetBit(1); //Start bit
            SetByte(Address);
            SetByte(Command);

            txChannel.Send(true);
            Thread.Sleep(10);
            txChannel.Send(true);
        }

        public byte TransmitQueryCommand(byte Address, byte Command)
        {
            rmtCmdIndex = 0;
            byte answerData = 0;     
            RmtCommand[] response = null;

            SetRxChannel();
            SetBit(1); //Start bit
            SetByte(Address);
            SetByte(Command);

            rxChannel.Start(true);
            txChannel.Send(true);

         //   Thread.Sleep(60);

            

            for (int count = 0; count < 5; count++)
			{
                response = rxChannel.GetAllItems();
                if (response != null)
                {
                    if (response.Length != 9)                        
                    {
                        Debug.WriteLine("Wrong!");
                    }
                    else
                    {
                        int respValue = 0;

                        for(byte i=1; i < 9; i++)
                        {
                            if (response[i].Level0 == false || response[i].Level1 == true)
                            {
                                respValue = respValue | (1 << (i - 1));
                            }                                
                        }                                              
                    }

                    break;
                }
                // Retry every 60 ms
                Thread.Sleep(60);
            }         
            rxChannel.Stop();
            return answerData;
        }
     
        private void SetByte(byte Packet)
        {
            for (int i = 7; i >= 0; i--)
            {
                SetBit((Packet >> i) & 1);
            }
        }

        private void SetBit(int Bit)
        {
            if (Bit==1)
            {              
                txChannel[rmtCmdIndex].Level0 = false;
                txChannel[rmtCmdIndex].Level1 = true;
                //txChannel[rmtCmdIndex] = txPulse1;
            }
            else
            {             
                txChannel[rmtCmdIndex].Level0 = true;
                txChannel[rmtCmdIndex].Level1 = false;
                // txChannel[rmtCmdIndex] = txPulse0;
            }

            rmtCmdIndex++;    
        }
    }
}
