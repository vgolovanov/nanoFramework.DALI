using System;
using System.Diagnostics;
using System.Threading;

namespace nanoFramework.DALI.Samples
{
    public class Program
    {
        public static void Main()
        {
            Debug.WriteLine("Hello from nanoFramework!");

         
            // Browse our samples repository: https://github.com/nanoframework/samples
            // Check our documentation online: https://docs.nanoframework.net/
            // Join our lively Discord community: https://discord.gg/gCyBu8T

            var daliMaster = new DaliMaster();

          var i2cCount = daliMaster.ScanI2cBus(1);
        }
    }
}
