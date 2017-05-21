using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Shannon3Host
{
    class hostPocController
    {
        private static string nameOfMachine = "BaselineTest";
        private static string nameofStartUpSnapshot = "sh3startup";
        private static string FULLRESPONSESTRING = "";
        private static string NAMEONTEST = "Error";

        private static readonly HttpClient client = new HttpClient();

        //Every number here adds 5 seconds
        static int thresholdForRuntime = 80 * 12;

        //Hosts the baseline every 33 minute
        public static void hostOfPOCTester()
        {
            //Creates a virtualmachine controller
            VirtualMachineController tempVir = null;

            Boolean action = false;
            while (true)
            {
                //Instances a new virtual machine
                tempVir = new VirtualMachineController();

                //Starts up the machine
                tempVir.startVirtualMachine(nameOfMachine);

                Thread.Sleep(60000);

                getPocHP1Host();
                string temp = FULLRESPONSESTRING;

                Console.WriteLine(temp);

                int count = temp.Split(':').Length - 1;

                action = false;

                int runs = 0;

                Console.WriteLine(temp);

                while (!action)
                {
                    if (count > 1)
                    {
                        Console.WriteLine(temp);
                        Console.WriteLine(count);
                        getPocHP1Host();
                        if (!temp.Equals(FULLRESPONSESTRING))
                        {
                            Console.WriteLine("Shutting down virtual machine due to post message");
                            action = true;
                        }
                        runs++;
                        Thread.Sleep(5000);

                        if(runs >= thresholdForRuntime)
                        {
                            Console.WriteLine("Posting because no post has been made");
                            action = true;
                        }
                    }
                    else
                    {
                        Thread.Sleep(5000);
                        getPocHP1Host();
                        temp = FULLRESPONSESTRING;
                        count = temp.Split(':').Length - 1;
                    }
                }

                //Powers off the machine
                tempVir.poweroffVirtualMachine(nameOfMachine);
                Thread.Sleep(5000);

                //Restores the virtual machine to the original image
                tempVir.restoreVirtualMachine(nameOfMachine, nameofStartUpSnapshot);
                Thread.Sleep(10000);
            }
        }

        public static void getPocHP1Host()
        {
            string responseString = "";
            try
            {
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.ConnectionClose = true;
                responseString = client.GetStringAsync("http://192.168.8.102/v1/index.php/getsh3host").Result;

            }
            catch (Exception)
            {

                throw;
            }

            FULLRESPONSESTRING = responseString;
        }

        private static string findNAMEONTEST(string responsestring)
        {
            int i = 0;
            int j = 0;
            foreach (char c in responsestring)
            {
                if (i == 5)
                {
                    return responsestring.Substring(j, responsestring.Length - j - 4);
                }
                if (c.Equals('"'))
                {
                    i++;
                }
                j++;
            }

            return "Could Not Find";
        }
    }
}
