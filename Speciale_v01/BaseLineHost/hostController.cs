using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaseLineHost
{
    class hostController
    {
        //Hosts the baseline every 33 minute
        public static void hostOfBaseLineTester()
        {
            //Creates a virtualmachine controller
            VirtualMachineController tempVir = null;
            while (true)
            {
                //Instances a new virtual machine
                tempVir = new VirtualMachineController();

                //Starts up the machine
                tempVir.startVirtualMachine("BaselineTest");
                Thread.Sleep(3600000);

                //Powers off the machine
                tempVir.poweroffVirtualMachine("BaselineTest");
                Thread.Sleep(5000);

                //Restores the virtual machine to the original image
                tempVir.restoreVirtualMachine("BaselineTest", "BLsnapshotStartUp");
                Thread.Sleep(10000);
            }
        }
    }
}
