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
            VirtualMachineController tempVir = null;
            Boolean action = false;
            while (true)
            {

                tempVir = new VirtualMachineController();
                tempVir.startVirtualMachine("QuickTester");
                Thread.Sleep(30000);
                //Wait for QT post somehow
                getQuickHost();
                string temp = FULLRESPONSESTRING;
                Console.Write(temp);
                int count = temp.Split(':').Length - 1;
                Console.Write(count);
                action = false;
                int runs = 0;
                while (!action)
                {
                    if (count > 1)
                    {
                        Console.WriteLine(temp);
                        Console.WriteLine(count);
                        getQuickHost();
                        if (!temp.Equals(FULLRESPONSESTRING))
                        {
                            Console.WriteLine("Breaking because of changes");
                            action = true;
                        }
                        runs++;
                        Thread.Sleep(5000);
                        if (runs >= 60)
                        {
                            Console.WriteLine("Posting soon");
                            postQuickPosted(temp);
                            action = true;
                        }
                    }
                    else
                    {
                        Thread.Sleep(5000);
                        getQuickHost();
                        temp = FULLRESPONSESTRING;
                        count = temp.Split(':').Length - 1;
                    }
                }



                tempVir.poweroffVirtualMachine("QuickTester");

                Thread.Sleep(5000);

                tempVir.restoreVirtualMachine("QuickTester", "QTsnapshotStartUp");

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
