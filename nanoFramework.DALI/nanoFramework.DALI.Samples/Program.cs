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
            var daliMaster = new DaliMaster(19,18);
            daliMaster.TransmitContolCommand(0xFF, DALICommands.DALI_CMD_OFF);
            Thread.Sleep(5000);
            daliMaster.TransmitContolCommand(0xFF, DALICommands.DALI_CMD_MAX_LEVEL);
            Thread.Sleep(5000);

            var data = daliMaster.TransmitQueryCommand (0xFF, DALICommands.DALI_CMD_QUERY_STATUS);
            Thread.Sleep(5000);
        }        
    }
}
