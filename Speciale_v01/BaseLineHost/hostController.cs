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
        private static string FULLRESPONSESTRING = "";
        private static readonly HttpClient client = new HttpClient();
        private static string NAMEONTEST = "Error";

        public static void hostOfBaseLineTester()
        {
            VirtualMachineController tempVir = null;
            Boolean action = false;
            while (true)
            {

                tempVir = new VirtualMachineController();
                tempVir.startVirtualMachine("BaselineTest");
                Thread.Sleep(1980000);

                tempVir.poweroffVirtualMachine("BaselineTest");

                Thread.Sleep(5000);

                tempVir.restoreVirtualMachine("BaselineTest", "BLsnapshotStartUp");

                Thread.Sleep(10000);

            }
        }

        public static void getQuickHost()
        {
            string responseString = "";
            try
            {
                responseString = client.GetStringAsync("http://192.168.8.102/v1/index.php/getquickhost").Result;

            }
            catch (Exception)
            {

                throw;
            }

            FULLRESPONSESTRING = responseString;
        }

        private static async void postQuickPosted(string RAWNAMEONTEST)
        {
            NAMEONTEST = findNAMEONTEST(RAWNAMEONTEST);
            Console.WriteLine("1");
            var values = new Dictionary<string, string>
            {
                {"FileChangedOnHash", "0"},
                {"FileChangedOnWatcher", "0"},
                {"Active", "0" },
                {"RansomwareName",  NAMEONTEST}
            };

            Console.WriteLine("2");
            var content = new FormUrlEncodedContent(values);
            Console.WriteLine("3");
            var response = client.PostAsync("http://192.168.8.102/v1/index.php/postquickposted", content).Result;
            Console.WriteLine("4");
            var responseString = await response.Content.ReadAsByteArrayAsync();
            Console.WriteLine("5");
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
