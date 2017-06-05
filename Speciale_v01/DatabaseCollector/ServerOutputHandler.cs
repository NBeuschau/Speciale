using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseCollector
{
    class ServerOutputHandler
    {
        internal static void CreateReadableFileForRansomware(string databaseTester, string ransomwareName, string ransomwareOutput, string path)
        {
            string firstColon = "";
            string secondColon = "";
            int firstColonPos = 0;
            int secondColonPos = 0;
            string data = "";

            List<string> serverOutput = new List<string>();

            for (int i = 0; i < ransomwareOutput.Length -1; i++)
            {
                if(firstColonPos == 0)
                {
                    firstColon = ransomwareOutput.Substring(i, 1);
                }
                else if(secondColonPos == 0)
                {
                    secondColon = ransomwareOutput.Substring(i, 1);
                }
                else
                {

                }
                if(firstColon == "\"")
                {
                    firstColonPos = i;
                    firstColon = "";
                }
                if (secondColon == "\"")
                {
                    secondColonPos = i;
                    secondColon = "";
                }

                if(firstColonPos != 0 && secondColonPos != 0)
                {

                    if (data.Equals("listFilemonObservations"))
                    {
                        data = ransomwareOutput.Substring(firstColonPos + 1, secondColonPos - firstColonPos - 1);
                        data = fixFileMonObservations(data);
                        serverOutput.Add(data);
                    }
                    else
                    {
                        data = ransomwareOutput.Substring(firstColonPos+1, secondColonPos - firstColonPos -1);
                        serverOutput.Add(data);
                    }

                    firstColonPos = 0;
                    secondColonPos = 0;
                }
                
            }


            string filePath = path + @"\" + databaseTester + @"\" + ransomwareName + ".txt";
            Console.WriteLine(filePath);
            Console.WriteLine(path);
            if (!File.Exists(filePath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    foreach (var item in serverOutput)
                    {
                        sw.WriteLine(item);
                    }
                }
            }
        }

        private static string fixFileMonObservations(string data)
        {
            int dataLength = data.Length;
            for (int i = 0; i < dataLength-5; i++)
            {
                if (data.Substring(i, 5).Equals("-2017"))
                {
                    data = data.Substring(0, i - 6) + "*" + data.Substring(i - 5);
                }

            }
            return data;
        }
    }
}
