using System;
using System.Threading;

namespace nanoFramework.DALI
{
    public class DALIMaster
    {
        private DALIProtocol daliProtocol;

        /// <summary>
        /// Creates a driver for the DALI bus enabled board.
        /// </summary>
        /// <param name="TxPin">TX pin connected to the DALI bus board.</param>
        /// <param name="RxPin">RX pin connected to the DALI bus board.</param>
        public DALIMaster(int TxPin, int RxPin)
        {
            daliProtocol = new DALIProtocol(TxPin, RxPin);
        }

        //****************************************************************
        //  Arc power control commands
        //
        //****************************************************************

        /// <summary>
        /// Direct Arc Power Control
        /// </summary>        
        /// <param name="Address">YAAAAAA0, 64 short addresses 0 – 63 0AAAAAA0, 16 group addresses 0 – 15 100AAAA0, broadcast 11111110</param>
        /// <param name="PowerValue">power is an integer in the range 0..255, 0 - dims down to the "MIN LEVEL" with the actual fade time and switches off, 0xFF - means "STOP FADING"</param>

        public void ControlDirectArcPower(byte Address, byte PowerValue)
        {
            daliProtocol.TransmitContolCommand(Address, PowerValue);
        }

        /// <summary>
        /// Extinguish the lamp immediately without fading.
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        public void ControlOff(byte Address)
        {
            daliProtocol.TransmitContolCommand(Address , DALICommands.DALI_CMD_OFF);
        }

        /// <summary>
        /// Dim up for 200 ms (execution time) using the selected ‘FADE RATE’.
        /// No change if the arc power output is already at the "MAX LEVEL"
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        public void ControlUp(byte Address)
        {
            daliProtocol.TransmitContolCommand(Address, DALICommands.DALI_CMD_UP);
        }

        /// <summary>
        /// Dim down for 200 ms (execution time) using the selected ‘FADE RATE’.
        /// No change if the arc power output is already at the "MIN LEVEL"
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        public void ControlDown(byte Address)
        {
            daliProtocol.TransmitContolCommand(Address, DALICommands.DALI_CMD_DOWN);
        }

        /// <summary>
        /// Set the actual arc power level one step higher immediately without fading.
        /// No change if the arc power output is already at the "MAX LEVEL".
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        public void ControlStepUp(byte Address)
        {
            daliProtocol.TransmitContolCommand(Address, DALICommands.DALI_CMD_STEP_UP);
        }

        /// <summary>
        /// Set the actual arc power level one step lower immediately without fading.
        /// Lamps shall not be switched off via this command.
        /// No change if the arc power output is already at the "MIN LEVEL".
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        public void ControlStepDown(byte Address)
        {
            daliProtocol.TransmitContolCommand(Address, DALICommands.DALI_CMD_STEP_DOWN);
        }

        /// <summary>
        /// Set the actual arc power level to the "MAX LEVEL" without fading. If the lamp is off it shall be ignited with this command.
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        public void ControlRecallMaxLevel(byte Address)
        {
            daliProtocol.TransmitContolCommand(Address, DALICommands.DALI_CMD_MAX_LEVEL);
        }

        /// <summary>
        /// Set the actual arc power level to the "MIN LEVEL" without fading. If the lamp is off it shall be ignited with this command.
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        public void ControlRecallMinLevel(byte Address)
        {
            daliProtocol.TransmitContolCommand(Address, DALICommands.DALI_CMD_MIN_LEVEL);
        }

        /// <summary>
        /// Set the actual arc power level one step lower immediately without fading.
        /// If the actual arc power level is already at the "MIN LEVEL", the lamp shall be switched off by this command.
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        public void ControlStepDownAndOff(byte Address)
        {
            daliProtocol.TransmitContolCommand(Address, DALICommands.DALI_CMD_STEP_DOWN_AND_OFF);
        }

        /// <summary>
        /// Set the actual arc power level one step higher immediately without fading.
        /// If the lamp is switched off, the lamp shall be ignited with this command and shall be set to the "MIN LEVEL".
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        public void ControlOnAndStepUp(byte Address)
        {
            daliProtocol.TransmitContolCommand(Address, DALICommands.DALI_CMD_ON_AND_STEP_UP);
        }

        /// <summary>
        /// Set the actual arc power level to the value stored for scene XXXX using the actual fade time.
        /// If the ballast does not belong to scene XXXX, the arc power level remains unchanged.
        /// If the lamp is off, it shall be ignited with this command.
        /// If the value stored for scene XXXX is zero and the lamp is lit then the lamp shall be switched off by this command after the fade time.
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        /// <param name="SceneNumber">Scene number in the range 0..15</param>
        public void ControlGoToScene(byte Address, byte SceneNumber)
        {
            if (SceneNumber > 15) throw new Exception("Invalid value of scene number.");

            byte scene = (byte)(DALICommands.DALI_CMD_GO_TO_SCENE + SceneNumber);

            daliProtocol.TransmitContolCommand(Address, scene);
        }


        //****************************************************************
        //
        //      Configuration commands
        //
        // Every configuration command(32 – 128) shall be received a second time in the next 100 ms
        // before it shall be executed in order to increase the probability for a proper reception.No other
        // commands addressing the same ballast shall be received between these two commands,
        // otherwise these commands shall be ignored and the respective configuration sequence shall
        // be aborted.
        //
        //****************************************************************

        /// <summary>
        /// After the second reception of the command, the variables in the persistent memory shall be changed to their reset values.
        /// It is not guaranteed that any commands are received properly within the next 300 ms by a ballast acting on this command.
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        public void ConfigurationReset(byte Address)
        {
            daliProtocol.TransmitConfigurationCommand(Address, DALICommands.DALI_CMD_RESET);
            Thread.Sleep(350);
        }

        public void ConfigurationStoreActualLevelInTDtr(byte Address)
        {
            throw new Exception("Not implemented.");
        }

        public void ConfigurationStoreMaxLevel(byte Address)
        {
            throw new Exception("Not implemented.");
        }

        public void ConfigurationStoreMinLevel(byte Address)
        {
            throw new Exception("Not implemented.");
        }

        public void ConfigurationStoreSystemFailureLevel(byte Address)
        {
            throw new Exception("Not implemented.");
        }

        public void ConfigurationStorePowerOnLevell(byte Address)
        {
            throw new Exception("Not implemented.");
        }

        public void ConfigurationStoreFadeTime(byte Address)
        {
            throw new Exception("Not implemented.");
        }

        public void ConfigurationStoreFadeRate(byte Address)
        {
            throw new Exception("Not implemented.");
        }

        public void ConfigurationStoreScene(byte Address, byte Scene)
        {
            throw new Exception("Not implemented.");
        }

        public void ConfigurationRemoveFromScene(byte Address, byte Scene)
        {
            throw new Exception("Not implemented.");
        }

        public void ConfigurationStoreAddToGroup(byte Address, byte Group)
        {
            throw new Exception("Not implemented.");
        }

        public void ConfigurationRemoveFromGroup(byte Address, byte Group)
        {
            throw new Exception("Not implemented.");
        }

        /// <summary>
        /// Save the value in the DTR as new short address.
        /// The structure of the DTR shall be: XXXX XXXX = 0AAA AAA1 or 0xFF (1111 1111) ). 0xFF shall remove the short address.
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        public void ConfigurationStoreShortAddress(byte Address)
        {
            daliProtocol.TransmitConfigurationCommand(Address, DALICommands.DALI_CMD_STORE_DTR_SHORT_ADDRESS);                     
        }

        //****************************************************************
        //
        //      Query commands
        //
        //  Query commands shall be addressed to individual ballasts preferably.
        //  If addressed to groups  or broadcast the answers might be overlapped as all ballasts addressed will answer.
        //  Query commands shall be of the kind that they can be answered with "Yes", "No" or 8-bit information
        //  The answers shall be also bi-phase coded except for the answer "No":
        //  "Yes": 1111 1111
        //  "No": The ballast shall not react
        //  8-bit information: XXXX XXXX
        //
        //****************************************************************

        /// <summary>
        /// Retrieve a status byte from the ballast
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        /// <param name="ExpectingFramesOverlapping"> False - for the addressed device, True - If addressed to the groups or broadcast message. The answers might be overlapped as all ballasts addressed will answer.</param>

        /// <returns>
        /// 
        /// if ExpectingFramesOverlapping == False 
        /// 
        /// Bit 0: status of ballast; 0 = OK
        /// Bit 1: lamp failure; 0 = OK
        /// Bit 2: lamp arc power on; 0 = OFF
        /// Bit 3: limit error; 0 = "last requested power was OFF or was between MIN..MAX LEVEL"

        /// Bit 4: fade ready; 0 = ready, 1 = running
        /// Bit 5: reset state? 0 = NO
        /// Bit 6: missing short address? 0 = NO
        /// Bit 7: power failure? 0 = "RESET" or an arc power control command has
        /// 
        /// if ExpectingFramesOverlapping == True 
        /// 
        ///    If any data in response packets  - 0xFF 
        ///    If no packets - 0x00
        /// </returns>
        public byte QueryStatus(byte Address, bool ExpectingFramesOverlapping=false)
        {
            daliProtocol.TransmitConfigurationCommand(Address, DALICommands.DALI_CMD_QUERY_STATUS);
            
            byte returnValue=0;

            if(!daliProtocol.QueryCommand(out returnValue, ExpectingFramesOverlapping))
            {
                returnValue=0;
            }

            return returnValue;
        }

        /// <summary>
        /// Ask if there is a ballast with the given address that is able to communicate.
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        /// <param name="ExpectingFramesOverlapping"> False - for the addressed device, True - If addressed to the groups or broadcast message. The answers might be overlapped as all ballasts addressed will answer.</param>
        /// <returns>0xFF - Yes, otherwise - No </returns>
        public byte QueryBallast(byte Address, bool ExpectingFramesOverlapping = false)
        {
            daliProtocol.TransmitConfigurationCommand(Address, DALICommands.DALI_CMD_QUERY_BALLAST);

            byte returnValue = 0;

            if (!daliProtocol.QueryCommand(out returnValue, ExpectingFramesOverlapping))
            {
                returnValue = 0;
            }

            return returnValue;
        }

        /// <summary>
        /// Ask if there is a lamp problem at the given address.
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        /// <param name="ExpectingFramesOverlapping"> False - for the addressed device, True - If addressed to the groups or broadcast message. The answers might be overlapped as all ballasts addressed will answer.</param>
        /// <returns>0xFF - Yes, otherwise - No </returns>
        public byte QueryLampFailure(byte Address, bool ExpectingFramesOverlapping = false)
        {
            daliProtocol.TransmitConfigurationCommand(Address, DALICommands.DALI_CMD_QUERY_LAMP_FAILURE);

            byte returnValue = 0;

            if (!daliProtocol.QueryCommand(out returnValue, ExpectingFramesOverlapping))
            {
                returnValue = 0;
            }

            return returnValue;
        }

        /// <summary>
        /// Ask if there is a lamp operating at the given address.
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        /// <param name="ExpectingFramesOverlapping"> False - for the addressed device, True - If addressed to the groups or broadcast message. The answers might be overlapped as all ballasts addressed will answer.</param>
        /// <returns>0xFF - Yes, otherwise - No </returns>
        public byte QueryLampPowerOn(byte Address, bool ExpectingFramesOverlapping = false)
        {
            daliProtocol.TransmitConfigurationCommand(Address, DALICommands.DALI_CMD_QUERY_LAMP_POWER_ON);

            byte returnValue = 0;

            if (!daliProtocol.QueryCommand(out returnValue, ExpectingFramesOverlapping))
            {
                returnValue = 0;
            }

            return returnValue;
        }

        /// <summary>
        /// Ask if the last requested arc power level could not be met because
        /// it was above the MAX LEVEL or below the MIN LEVEL.
        /// (Power level of 0 is always "OFF" and is not an error.)
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        /// <param name="ExpectingFramesOverlapping"> False - for the addressed device, True - If addressed to the groups or broadcast message. The answers might be overlapped as all ballasts addressed will answer.</param>
        /// <returns>0xFF - Yes, otherwise - No </returns>
        public byte QueryLimitError(byte Address, bool ExpectingFramesOverlapping = false)
        {
            daliProtocol.TransmitConfigurationCommand(Address, DALICommands.DALI_CMD_QUERY_LIMIT_ERROR);

            byte returnValue = 0;

            if (!daliProtocol.QueryCommand(out returnValue, ExpectingFramesOverlapping))
            {
                returnValue = 0;
            }

            return returnValue;
        }

        /// <summary>
        /// Ask if the ballast is in "RESET STATE".        
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        /// <param name="ExpectingFramesOverlapping"> False - for the addressed device, True - If addressed to the groups or broadcast message. The answers might be overlapped as all ballasts addressed will answer.</param>
        /// <returns>0xFF - Yes, otherwise - No </returns>
        public byte QueryResetState(byte Address, bool ExpectingFramesOverlapping = false)
        {
            daliProtocol.TransmitConfigurationCommand(Address, DALICommands.DALI_CMD_QUERY_LIMIT_ERROR);

            byte returnValue = 0;

            if (!daliProtocol.QueryCommand(out returnValue, ExpectingFramesOverlapping))
            {
                returnValue = 0;
            }

            return returnValue;
        }

        /// <summary>
        /// Ask if the ballast has no short address    
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        /// <param name="ExpectingFramesOverlapping"> False - for the addressed device, True - If addressed to the groups or broadcast message. The answers might be overlapped as all ballasts addressed will answer.</param>
        /// <returns>0xFF - Yes, otherwise - No </returns>
        public byte QueryMissingShortAddress(byte Address, bool ExpectingFramesOverlapping = false)
        {
            daliProtocol.TransmitConfigurationCommand(Address, DALICommands.DALI_CMD_QUERY_MISS_SHORT_ADDRESS);

            byte returnValue = 0;

            if (!daliProtocol.QueryCommand(out returnValue, ExpectingFramesOverlapping))
            {
                returnValue = 0;
            }

            return returnValue;
        }

        public byte QueryVersionNumber(byte Address, bool ExpectingFramesOverlapping = false)
        {
            throw new Exception("Not implemented.");        
        }

        public byte QueryContentDTR(byte Address, bool ExpectingFramesOverlapping = false)
        {
            throw new Exception("Not implemented.");
        }

        public byte QueryDeviceType(byte Address, bool ExpectingFramesOverlapping = false)
        {
            throw new Exception("Not implemented.");
        }

        public byte QueryPhysicalMinimumLevel(byte Address, bool ExpectingFramesOverlapping = false)
        {
            throw new Exception("Not implemented.");
        }

        public byte QueryPowerFailure(byte Address, bool ExpectingFramesOverlapping = false)
        {
            throw new Exception("Not implemented.");
        }

        //***************************************************************************************************
        // Extended commands
        //***************************************************************************************************
        public byte QueryContentDTR1(byte Address, bool ExpectingFramesOverlapping = false)
        {
            throw new Exception("Not implemented.");
        }

        public byte QueryContentDTR2(byte Address, bool ExpectingFramesOverlapping = false)
        {
            throw new Exception("Not implemented.");
        }

        public byte QueryOperatingMode(byte Address, bool ExpectingFramesOverlapping = false)
        {
            throw new Exception("Not implemented.");
        }

        public byte QueryLightSourceType(byte Address, bool ExpectingFramesOverlapping = false)
        {
            throw new Exception("Not implemented.");
        }

        //***************************************************************************************************
        // End of extended commands
        //***************************************************************************************************

        public byte QueryActualLevel(byte Address, bool ExpectingFramesOverlapping = false)
        {
            throw new Exception("Not implemented.");
        }

        public byte QueryMaxLevel(byte Address, bool ExpectingFramesOverlapping = false)
        {
            throw new Exception("Not implemented.");
        }

        public byte QueryMinLevel(byte Address, bool ExpectingFramesOverlapping = false)
        {
            throw new Exception("Not implemented.");
        }

        public byte QueryPowerOnLevel(byte Address, bool ExpectingFramesOverlapping = false)
        {
            throw new Exception("Not implemented.");
        }

        public byte QuerySystemFailureLevel(byte Address, bool ExpectingFramesOverlapping = false)
        {
            throw new Exception("Not implemented.");
        }

        public byte QueryFadeTimeFadeRate(byte Address, bool ExpectingFramesOverlapping = false)
        {
            throw new Exception("Not implemented.");
        }

        public byte QuerySceneLevel(byte Address, bool ExpectingFramesOverlapping = false)
        {
            throw new Exception("Not implemented.");
        }

        public byte QueryGroupsZeroToSeven(byte Address, bool ExpectingFramesOverlapping = false)
        {
            throw new Exception("Not implemented.");
        }

        public byte QueryGroupsEightToFifteen(byte Address, bool ExpectingFramesOverlapping = false)
        {
            throw new Exception("Not implemented.");
        }

        /// <summary>
        /// The 8 high bits of the random address.
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        /// <param name="ExpectingFramesOverlapping"> False - for the addressed device, True - If addressed to the groups or broadcast message. The answers might be overlapped as all ballasts addressed will answer.</param>
        /// <returns>The 8 high bits of the random address.</returns>
        public byte QueryRandomAddressH(byte Address, bool ExpectingFramesOverlapping = false)
        {
            daliProtocol.TransmitConfigurationCommand(Address, DALICommands.DALI_CMD_QUERY_RANDOM_ADDRH);

            byte returnValue = 0;

            if (!daliProtocol.QueryCommand(out returnValue, ExpectingFramesOverlapping))
            {
                returnValue = 0;
            }

            return returnValue;
        }

        /// <summary>
        /// The 8 mid bits of the random address.
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        /// <param name="ExpectingFramesOverlapping"> False - for the addressed device, True - If addressed to the groups or broadcast message. The answers might be overlapped as all ballasts addressed will answer.</param>
        /// <returns>The 8 mid bits of the random address.</returns>
        public byte QueryRandomAddressM(byte Address, bool ExpectingFramesOverlapping = false)
        {
            daliProtocol.TransmitConfigurationCommand(Address, DALICommands.DALI_CMD_QUERY_RANDOM_ADDRM);

            byte returnValue = 0;

            if (!daliProtocol.QueryCommand(out returnValue, ExpectingFramesOverlapping))
            {
                returnValue = 0;
            }

            return returnValue;
        }

        /// <summary>
        /// The 8 low bits of the random address.
        /// </summary>
        /// <param name="Address">YAAAAAA1, 64 short addresses 0 – 63 0AAAAAA1, 16 group addresses 0 – 15 100AAAA1, broadcast 11111111</param>
        /// <param name="ExpectingFramesOverlapping"> False - for the addressed device, True - If addressed to the groups or broadcast message. The answers might be overlapped as all ballasts addressed will answer.</param>
        /// <returns>The 8 low bits of the random address.</returns>
        public byte QueryRandomAddressL(byte Address, bool ExpectingFramesOverlapping = false)
        {
            daliProtocol.TransmitConfigurationCommand(Address, DALICommands.DALI_CMD_QUERY_RANDOM_ADDRL);

            byte returnValue = 0;

            if (!daliProtocol.QueryCommand(out returnValue, ExpectingFramesOverlapping))
            {
                returnValue = 0;
            }

            return returnValue;
        }

        //****************************************************************
        //
        //      Special commands
        //
        //  Special Commands shall be broadcast and received by all ballasts.
        //  This means that the main address shall be 101C CCC1or 110C CCC1.
        //  CCCC is the significant "SPECIAL COMMAND".        
        //
        //****************************************************************

        public void SpecialTerminate()
        {
            daliProtocol.TransmitContolCommand(DALICommands.DALI_CMD_TERMINATE, 0);
        }

        public void SpecialStoreDTR(byte ValueDTR)
        {
            daliProtocol.TransmitContolCommand(DALICommands.DALI_CMD_STORE_DTR, ValueDTR);
        }

        //****************************************************************
        //
        //      Addressing commands
        //
        //****************************************************************

        /// <summary>
        /// This command shall start or re-trigger a timer for 15 minutes; the addressing commands shall only be processed within this period.
        /// All other commands shall still be processed during this period.
        /// This time period shall be aborted with the "Terminate" command.         
        /// </summary>
        /// <param name="Broadcast">If broadcast is True then all ballasts shall react.</param>        
        /// <param name="DeviceAddress">if Broadcast is False and address is an range of 0..63 then ballasts with the address supplied shall react and if address is 0xFF then ballasts without a short address shall react</param>        
        public void AddressingInitialise(bool Broadcast, byte DeviceAddress = 0xFF)
        {
            if (DeviceAddress < 0xFF)
            {
                if (DeviceAddress > 63)
                {
                    throw new Exception("Address must be in the range 0..63.");
                }
               
            }
            
            byte initAddress = 0xFF;

            if (Broadcast)
            {
                initAddress = 0;
            }
            else if (DeviceAddress != 0xFF)
            {
                initAddress =(byte)((DeviceAddress << 1) | 1);
            }

            daliProtocol.TransmitContolCommand(DALICommands.DALI_CMD_INITIALISE, initAddress);
        }

        /// <summary>
        /// The ballast shall generate a new 24-bit random address.
        /// The new  random address shall be available within a time period of 100ms.
        /// </summary>
        public void AddressingRandomise()
        {
            daliProtocol.TransmitConfigurationCommand(DALICommands.DALI_CMD_RANDOMISE, 0x00);
            Thread.Sleep(100);
        }

        /// <summary>
        /// The ballast shall compare its 24-bit random address with the combined search address stored in SearchAddrH, SearchAddrM and SearchAddrL.
        /// If its random address is smaller or equal to the search address and the ballast is not withdrawn then the ballast shall generate a query "YES".
        /// </summary>
        public void AddressingCompare()
        {            
            daliProtocol.TransmitContolCommand(DALICommands.DALI_CMD_COMPARE, 0x00);
        }

        /// <summary>
        /// The ballast that has a 24-bit random address equal to the combined search address stored in SearchAddrH, SearchAddrM and SearchAddrL 
        /// snall no longer respond to the compare command. This ballast shall not be excluded from the initialisation process.
        /// </summary>
        public void AddressingWithdraw()
        {
            daliProtocol.TransmitContolCommand(DALICommands.DALI_CMD_WITHDRAW, 0x00);
        }

        //***************************************************************************************************
        // Extended commands
        //***************************************************************************************************

        /// <summary>
        ///  Transmitted at 10 minute intervals by single master application controllers
        ///  (that cannot perform collision detection) to indicate their presence.  Ignored by control gear.
        /// </summary>
        public void AddressingPing()
        {
            daliProtocol.TransmitContolCommand(DALICommands.DALI_CMD_PING, 0x00);
        }
        //***************************************************************************************************
        // End of extended commands
        //***************************************************************************************************

        /// <summary>
        /// The 8 high bits of the search address.        
        /// </summary>
        /// <param name="AddressH">The 8 high bits of the search address</param>
        public void AddressingSearchaddrH(byte AddressH)
        {
            daliProtocol.TransmitContolCommand(DALICommands.DALI_CMD_SEARCHADDRH, AddressH);
        }

        /// <summary>
        /// The 8 mid bits of the search address.
        /// </summary>
        /// <param name="AddressM">The 8 mid bits of the search address</param>
        public void AddressingSearchaddrM(byte AddressM)
        {
            daliProtocol.TransmitContolCommand(DALICommands.DALI_CMD_SEARCHADDRM, AddressM);
        }

        /// <summary>
        /// The 8 low bits of the search address.
        /// </summary>
        /// <param name="AddressL">The 8 low bits of the search address</param>
        public void AddressingSearchaddrL(byte AddressL)
        {
            daliProtocol.TransmitContolCommand(DALICommands.DALI_CMD_SEARCHADDRL, AddressL);
        }

        /// <summary>
        /// "The ballast shall store the received 6-bit address as its short  address if it is selected.  It is selected if:
        ///  * the ballast's 24-bit random address is equal to the address in SearchAddrH, SearchAddrM and SearchAddrL
        ///  * physical selection has been detected (the lamp is electrically  disconnected after reception of command PhysicalSelection())         
        /// </summary>
        /// <param name="ShortAddress">Short address is an range of 0..63 If address is the 0xFF then the short address shall be deleted.</param>
        public void AddressingProgramShortAddress(byte ShortAddress)
        {
            if (ShortAddress > 63 || ShortAddress != 0xFF)
            {
                throw new Exception("Address must be in the range 0..63 or equal 0xFF.");
            }

            byte sendValue = 0xFF;

            if (ShortAddress != 0xFF)
            {
                sendValue = (byte)((ShortAddress << 1) | 1);
            }

            daliProtocol.TransmitContolCommand(DALICommands.DALI_CMD_PROG_SHORTADDRESS, sendValue);
        }

        /// <summary>
        /// The ballast shall give an answer "YES" if the received short
        /// address is equal to its own short address.
        /// </summary>
        /// <param name="ShortAddress"></param>
        /// <returns>0xFF if received short address is equal to its own short address, otherwise other value.</returns>        
        public byte AddressingVerifyShortAddresss(byte ShortAddress)
        {
            if (ShortAddress > 63)
            {
                throw new Exception("Address must be in the range 0..63 .");
            }

            byte sendValue = (byte)((ShortAddress << 1) | 1);

            daliProtocol.TransmitContolCommand(DALICommands.DALI_CMD_VERIFY_SHORTADDRESS, sendValue);

            byte returnValue=0;

            daliProtocol.QueryCommand(out returnValue, false);

            return returnValue;
        }

        /// <summary>
        /// The ballast shall send the short address if the random address is the same as the search
        /// address or the ballast is physically selected.
        /// </summary>
        /// <returns>Byte value is short address or 0xFF if if there is no short address stored.</returns>
        public byte AddressingQueryShortAddress()
        {
            daliProtocol.TransmitContolCommand(DALICommands.DALI_CMD_QUERY_SHORTADDRESS, 0);

            byte returnValue = 0;

            daliProtocol.QueryCommand(out returnValue, false);

            return (byte)(returnValue >> 1);
        }            
    }
}
