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


            //byte opa = 0;

            ////0010



            ////    daliMaster.TransmitContolCommand(DALICommands.DALI_CMD_TERMINATE, 0);
            //daliMaster.TransmitConfigurationCommand(0xFF, DALICommands.DALI_CMD_RESET);

            //daliMaster.TransmitContolCommand(0xFF, DALICommands.DALI_CMD_OFF);
            ////Thread.Sleep(5000);
            ////daliMaster.TransmitContolCommand(0xFF, DALICommands.DALI_CMD_MAX_LEVEL);
            //Thread.Sleep(300);

            //var data = daliMaster.TransmitQueryCommand (0xFF, DALICommands.DALI_CMD_QUERY_BALLAST);
            ////Thread.Sleep(5000);

            var daliMaster = new DaliMaster(19, 18);

            //daliMaster.TransmitContolCommand(DALICommands.DALI_CMD_TERMINATE, 0x00);
            //Thread.Sleep(300);
            //daliMaster.TransmitContolCommand(DALICommands.DALI_CMD_RESET, 0x00);
            //Thread.Sleep(10);
            //daliMaster.TransmitContolCommand(DALICommands.DALI_CMD_RESET, 0x00);
            //Thread.Sleep(300);
            //daliMaster.TransmitContolCommand(0xFF, DALICommands.DALI_CMD_OFF);
            //Thread.Sleep(300);

            daliMaster.TransmitContolCommand(0xFF, DALICommands.DALI_CMD_RESET);
            Thread.Sleep(30);
            daliMaster.TransmitContolCommand(0xFF, DALICommands.DALI_CMD_RESET);
            Thread.Sleep(300);
            daliMaster.TransmitContolCommand(0xFF, DALICommands.DALI_CMD_OFF);
            Thread.Sleep(300);
            daliMaster.TransmitContolCommand(DALICommands.DALI_CMD_INITIALISE ,0x00);
            Thread.Sleep(30);
            daliMaster.TransmitContolCommand(DALICommands.DALI_CMD_INITIALISE, 0x00);
            Thread.Sleep(300);

          //  daliMaster.TransmitContolCommand(DALICommands.DALI_CMD_RANDOMISE,0xFF);
            daliMaster.TransmitContolCommand(DALICommands.DALI_CMD_RANDOMISE, 0x00);
            Thread.Sleep(30);
            daliMaster.TransmitContolCommand(DALICommands.DALI_CMD_RANDOMISE, 0x00);
            Thread.Sleep(30);

            var data = daliMaster.TransmitQueryCommand(0xFF, DALICommands.DALI_CMD_QUERY_BALLAST);
        }        
    }
}
