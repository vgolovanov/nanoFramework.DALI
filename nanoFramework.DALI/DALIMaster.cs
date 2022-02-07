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

            //txPulse1 = new RmtCommand(395, false, 440, true);
            //txPulse0 = new RmtCommand(440, true, 395, false);

            SetTxChannel();
            SetRxChannel();
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
            rxChannel.EnableFilter(true, 10); // filter out 100Us / noise             
            rxChannel.SetIdleThresold(1700);  // 40ms based on 1us clock
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

        public bool TransmitQueryCommand(byte Address, byte Command, out byte Response)
        {
            rmtCmdIndex = 0;
         
            RmtCommand[] response = null;
            
            bool ValidResonse = false;
            Response =0;
            SetBit(1); //Start bit
            SetByte(Address);
            SetByte(Command);

            txChannel.Send(false);
            
            Thread.Sleep(20);

            rxChannel.Start(true);

            for (int count = 0; count < 5; count++)
			{
                response = rxChannel.GetAllItems();
                if (response != null)
                {
                    if (response.Length != 9)                        
                    {
                        Debug.WriteLine("Wrong!" + response.Length.ToString());

                        foreach ( var item in response)
                        {
                            Debug.Write(item.Level0.ToString());
                            Debug.Write(item.Duration0.ToString());
                            Debug.Write(item.Level1.ToString());
                            Debug.Write(item.Duration1.ToString());
                            Debug.WriteLine("");
                        }

                        Response =0;

                        ValidResonse = false;
                    }
                    else
                    {
                        int respValue = 0;

                        for (byte i=1; i < 9; i++)
                        {
                            if (response[i].Level0 == false || response[i].Level1 == true)
                            {
                                respValue = respValue | (1 << (i - 1));
                            }                                
                        }

                        Response =(byte) respValue;

                        ValidResonse = true;
                    }

                   // break;
                }
                // Retry every 60 ms
                Thread.Sleep(60);
            }         
           
            rxChannel.Stop();
         
            return ValidResonse;
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
