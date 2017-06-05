using System;
using System.Collections.Generic;
using System.IO;
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

        static string fileToVirusNames = @"RansomwareList.txt";
        static string pathToFolders = @"C:\Speciale\Relevant Data";

        static void Main(string[] args)
        {

            //Read txt file with all virus name

            List<string> listOfRansomwareNames = VirusFileParser.parseTxtToList(fileToVirusNames);
            string ransomwareOutput = "";
            foreach (var item in listOfRansomwareNames)
            {
                Console.WriteLine(item.Substring(1, item.Length - 2));
                ransomwareName = item.Substring(1,item.Length-2);
                //ransomwareOutput = ServerCommunicator.returnDatabaseOutputForRansomware(databaseinputbase + databaseTester + middlepart + ransomwareName);
                
                StreamReader sr = new StreamReader(@"C:\Speciale\Relevant Data\output.txt");

                ransomwareOutput = sr.ReadLine();

                ServerOutputHandler.CreateReadableFileForRansomware(databaseTester,ransomwareName,ransomwareOutput,pathToFolders);
                
            }

            Console.ReadLine();
        }
    }
}

