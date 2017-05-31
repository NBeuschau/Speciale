using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCollector
{
    class Program
    {
        static string databaseinputbase = "http://192.168.8.102/v1/index.php/getdata";
        static string databaseTester = "hp1";
        static string middlepart = "?RansomwareName=";
        static string ransomwareName = "Vipsana2";

        private static readonly HttpClient client = new HttpClient();

        static void Main(string[] args)
        {
          
            var responseString = client.GetStringAsync(databaseinputbase+databaseTester+middlepart+ransomwareName).Result;

            Console.WriteLine(responseString);
            Console.ReadLine();
        }


    }
}
}
