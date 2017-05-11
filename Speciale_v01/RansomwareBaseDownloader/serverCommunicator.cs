using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BaseLineRansomwareDownloader

{
    class serverCommunicator
    {
        static string NAMEONTEST = "";
        static string RANSOMWAREFILEPATH = "";
        private static readonly HttpClient client = new HttpClient();

        //Gets name of next ransomware
        public static void getBaseHost()
        {

            var responseString = client.GetStringAsync("http://192.168.8.102/v1/index.php/getbasehost").Result;
            NAMEONTEST = findNAMEONTEST(responseString);
            Console.WriteLine(NAMEONTEST);

        }

        //Parses the string from the server to the raw ransomware name
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

        //Downloads the ransomware from the server
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

        //Informs the server that the data has been downloaded, thus creating an empty 
        public static async void postBaseStarted()
        {
            var values = new Dictionary<string, string>
            {
                {"RansomwareName",  NAMEONTEST},
                {"Started", DateTime.Now.ToString() }
            };

            var content = new FormUrlEncodedContent(values);

            var response = client.PostAsync("http://192.168.8.102/v1/index.php/postbasestarted", content).Result;

            var responseString = await response.Content.ReadAsByteArrayAsync();
        }

        public static string getNAMEONTEST()
        {
            return NAMEONTEST;
        }

        //Sets the path for the downloaded ransomware, this is the desktop.
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
