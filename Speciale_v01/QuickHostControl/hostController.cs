using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuickHostControl
{
    class hostController
    {
        private static string FULLRESPONSESTRING = "";
        private static readonly HttpClient client = new HttpClient();
        private static string NAMEONTEST = "Error";

        public static void hostOfQuickTester()
        {
            while (true)
            {
                VirtualMachineController.startVirtualMachine("QuickTester");
                Thread.Sleep(10000);
                //Wait for QT post somehow
                getQuickHost();
                string temp = FULLRESPONSESTRING;
                Boolean action = false;
                int runs = 0;
                while (!action)
                {
                    getQuickHost();
                    if (!temp.Equals(FULLRESPONSESTRING))
                    {
                        break;
                    }
                    runs++;
                    Thread.Sleep(5000);
                    if (runs >= 60)
                    {
                        postQuickPosted(NAMEONTEST);
                        break;
                    }
                }


                VirtualMachineController.poweroffVirtualMachine("QuickTester");

                Thread.Sleep(8000);

                //TODO
                VirtualMachineController.restoreVirtualMachine("QuickTester", "QTsnapshot1");
            }
        }

        public static void getQuickHost()
        {
            var responseString = client.GetStringAsync("http://192.168.8.102/v1/index.php/getquickhost").Result;

            FULLRESPONSESTRING = responseString;
        }

        private static async void postQuickPosted(string NAMEONTEST)
        {
            var values = new Dictionary<string, string>
            {
                {"FileChangedOnHash", "0"},
                {"FileChangedOnWatcher", "0"},
                {"Active", "No" },
                {"RansomwareName",  NAMEONTEST}
            };

            var content = new FormUrlEncodedContent(values);

            var response = await client.PostAsync("http://192.168.8.102/v1/index.php/postquickposted", content);

            var responseString = await response.Content.ReadAsByteArrayAsync();
        }
    }
}
