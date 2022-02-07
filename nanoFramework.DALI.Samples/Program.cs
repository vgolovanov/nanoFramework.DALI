using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using nanoFramework.DALI;

namespace nanoFramework.DALI.Samples
{
    public class Program
    {
        public static void Main()
        {
            
            var daliMaster = new DaliMaster(22, 21);

            byte response = 0;

            daliMaster.TransmitContolCommand(DALICommands.DALI_CMD_TERMINATE, 0x00);        
            daliMaster.TransmitConfigurationCommand(DALICommands.DALI_CMD_RESET, 0x00);       
            daliMaster.TransmitContolCommand(0xFF, DALICommands.DALI_CMD_OFF);

            //  daliMaster.TransmitContolCommand(0, DALICommands.DALI_CMD_Broadcast_DP);

            response = daliMaster.TransmitQueryCommand(0xFF, DALICommands.DALI_CMD_QUERY_STATUS);
            Debug.WriteLine("Response value:" + response.ToString());

            response = daliMaster.TransmitQueryCommand(0xFF, DALICommands.DALI_CMD_QUERY_BALLAST);
            Debug.WriteLine("Response value:" + response.ToString());

            response = daliMaster.TransmitQueryCommand(0xFF, DALICommands.DALI_CMD_QUERY_MISS_SA);
            Debug.WriteLine("Response value:" + response.ToString());

            daliMaster.TransmitContolCommand(0xFF, DALICommands.DALI_CMD_ON_AND_STEP_UP);
            daliMaster.TransmitContolCommand(0xFF, DALICommands.DALI_CMD_UP);
            daliMaster.TransmitContolCommand(0xFF, DALICommands.DALI_CMD_UP);
            daliMaster.TransmitContolCommand(0xFF, DALICommands.DALI_CMD_UP);
            daliMaster.TransmitContolCommand(0xFF, DALICommands.DALI_CMD_UP);
            daliMaster.TransmitContolCommand(0xFF, DALICommands.DALI_CMD_UP);
            daliMaster.TransmitContolCommand(0xFF, DALICommands.DALI_CMD_UP);
            daliMaster.TransmitContolCommand(0xFF, DALICommands.DALI_CMD_STEP_UP);
            daliMaster.TransmitContolCommand(0xFF, DALICommands.DALI_CMD_STEP_UP);
            daliMaster.TransmitContolCommand(0xFF, DALICommands.DALI_CMD_STEP_UP);
            daliMaster.TransmitContolCommand(0xFF, DALICommands.DALI_CMD_STEP_UP);
            

            daliMaster.TransmitConfigurationCommand(DALICommands.DALI_CMD_INITIALISE, 0x00);        
            daliMaster.TransmitConfigurationCommand(DALICommands.DALI_CMD_RANDOMISE, 0x00);
            
            //var data = daliMaster.TransmitQueryCommand(0xFF, DALICommands.DALI_CMD_QUERY_BALLAST);

            Thread.Sleep(100);

            daliMaster.TransmitContolCommand(DALICommands.DALI_CMD_SEARCHADDRH, 127);
            daliMaster.TransmitContolCommand(DALICommands.DALI_CMD_SEARCHADDRM, 255);
            daliMaster.TransmitContolCommand(DALICommands.DALI_CMD_SEARCHADDRL, 255);

            response = daliMaster.TransmitQueryCommand(DALICommands.DALI_CMD_COMPARE, 0);

            Debug.WriteLine("Response value:" + response.ToString());
         
        }        
    }
}
