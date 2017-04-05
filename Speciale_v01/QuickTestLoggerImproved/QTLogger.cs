using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace QuickTestLoggerImproved
{
    class QTLogger
    {
        private static int INTERVALFORLOOP = 20000;
        private static int MINUTESOFLOGGING = 1;
        private static string NAMEONTEST = "test";
        private static Boolean MONITORSTATUS = true;

        private static string fileChangedOnWatcher = "0";
        private static readonly HttpClient client = new HttpClient();


        public static Boolean LogWriter(string PATH1, string PATH2, string PATH3, string PATH4)
        {

            //Find the start timestamp
            DateTime startTimeStamp = DateTime.Now;
            QTFileMon fileWatcher1 = new QTFileMon();
            QTFileMon fileWatcher2 = new QTFileMon();
            QTFileMon fileWatcher3 = new QTFileMon();
            QTFileMon fileWatcher4 = new QTFileMon();

            //var fw1 = new Thread(() => fileWatcher1.CreateFileWatcher(PATH1));
            //fw1.Start();
            //
            //var fw2 = new Thread(() => fileWatcher2.CreateFileWatcher(PATH2));
            //fw2.Start();
            //
            //var fw3 = new Thread(() => fileWatcher3.CreateFileWatcher(PATH3));
            //fw3.Start();

            fileWatcher1.CreateFileWatcher(PATH1);

            fileWatcher2.CreateFileWatcher(PATH2);

            fileWatcher3.CreateFileWatcher(PATH3);

            fileWatcher4.CreateFileWatcher(PATH4);


            //Find the name of the test


            //Take a hash of the files at the end
            Boolean activity = false;

            Dictionary<DateTime, string> fileMonChanges = null;

            DateTime endTimeStamp = DateTime.Now;

            while (!activity)
            {
                Thread.Sleep(5000);

                if(fileWatcher1.getFilemonChanges().Count() >= 5 
                    || fileWatcher2.getFilemonChanges().Count() >= 5
                    || fileWatcher3.getFilemonChanges().Count() >= 5
                    || fileWatcher4.getFilemonChanges().Count() >= 5)
                {
                    Console.WriteLine("One of the four has five or more encounters");
                    activity = true;
                }

            }

            string filePath = PATH1 + "\\RansomwareLog.txt";
            if (!File.Exists(filePath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(NAMEONTEST);
                    sw.WriteLine(MONITORSTATUS);
                    sw.WriteLine(startTimeStamp.ToString());
                    sw.WriteLine(endTimeStamp.ToString());
                   // sw.WriteLine(fileMonChanges.Count);
                   // foreach (var item in fileMonChanges)
                   // {
                   //     sw.WriteLine(item.Value + ", " + item.Key.ToString("dd/MM/yyyy HH:mm:ss.fff"));
                   // }
                }
            }
            return true;
        }

        public static async void postQuickPosted()
        {
            var values = new Dictionary<string, string>
            {
                {"FileChangedOnHash", "0"},
                {"FileChangedOnWatcher", fileChangedOnWatcher},
                {"Active", "1" },
                {"RansomwareName",  NAMEONTEST}
            };

            var content = new FormUrlEncodedContent(values);
            var response = client.PostAsync("http://192.168.8.102/v1/index.php/postquickposted", content).Result;
            var responseString = await response.Content.ReadAsByteArrayAsync();
        }

        public static async void getQuickHost()
        {
            var responseString = client.GetStringAsync("http://192.168.8.102/v1/index.php/getquickhost").Result;

            NAMEONTEST = findNAMEONTEST(responseString);
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
        
        public static string getNAMEONTEST()
        {
            return NAMEONTEST;
        }
    }
}
