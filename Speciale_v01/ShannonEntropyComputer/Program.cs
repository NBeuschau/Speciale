using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShannonEntropyComputer
{
    class Program
    {
        static string pathToFiles = @"C:\Speciale\";
        static string txtFileNameBefore = @"beforeEncryption.txt";
        static string txtFileNameAfter = @"afterEncryption.txt";

        static void Main(string[] args)
        {
            Dictionary<string, string> beforeFiles = new Dictionary<string, string>();
            beforeFiles = parseTxTfile(pathToFiles, txtFileNameBefore);


            Dictionary<string, string> afterFiles = new Dictionary<string, string>();
            afterFiles = parseTxTfile(pathToFiles, txtFileNameAfter);

            Dictionary<string, ShannonObject> completeList = new Dictionary<string, ShannonObject>();
            double entropy = 0;
            foreach (var item in beforeFiles)
            {
                ShannonObject temp = new ShannonObject();
                temp.Path = item.Key;
                double.TryParse(item.Value, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out (entropy));
                temp.EntropyValue = entropy;
                temp.Type = getType(item.Key);
                completeList.Add(item.Key, temp);
            }

            Dictionary<string, string> afterFilesWithProperEnding = new Dictionary<string, string>();

            foreach (var item in afterFiles)
            {
                afterFilesWithProperEnding.Add(removeFun(item.Key), item.Value);
            }

            foreach (var item in afterFilesWithProperEnding)
            {
                if (completeList.ContainsKey(item.Key))
                {
                    double.TryParse(item.Value, System.Globalization.NumberStyles.Any, CultureInfo.InvariantCulture, out (entropy));
                    completeList[item.Key].EncryptedEntropyValue = entropy;
                }
                else
                {
                    Console.WriteLine("File " + item.Key + " does not exist");
                }
            }


            dataHandler(completeList);

            Console.ReadKey();
        }

        private static void dataHandler(Dictionary<string, ShannonObject> completeList)
        {
            double entTemp = 0;
            double encryptTemp = 0;
            int i = 0;
            foreach (var item in completeList.Values)
            {
                if (item.Type.Equals("pdf"))
                {
                    i++;
                    entTemp = entTemp + item.EntropyValue;
                    encryptTemp = encryptTemp + item.EncryptedEntropyValue;
                }
            }
            double averagePDFentropy = entTemp / i;
            double averagePDFencryptEntropy = encryptTemp / i;

            entTemp = 0;
            encryptTemp = 0;
            i = 0;
            foreach (var item in completeList.Values)
            {
                if (item.Type.Equals("docx"))
                {
                    i++;
                    entTemp = entTemp + item.EntropyValue;
                    encryptTemp = encryptTemp + item.EncryptedEntropyValue;
                }
            }

            double averageDOCXentropy = entTemp / i;
            double averageDOCXencryptEntropy = encryptTemp / i;

            entTemp = 0;
            encryptTemp = 0;
            i = 0;
            foreach (var item in completeList.Values)
            {
                if (item.Type.Equals("txt"))
                {
                    i++;
                    entTemp = entTemp + item.EntropyValue;
                    encryptTemp = encryptTemp + item.EncryptedEntropyValue;
                }
            }

            double averageTXTentropy = entTemp / i;
            double averageTXTencryptEntropy = encryptTemp / i;

            double zeroToOne = 0;
            double oneToTwo = 0;
            double twoToThree = 0;
            double threeToFour = 0;
            double fourToFive = 0;
            double fiveToSix = 0;
            double sixToSeven = 0;
            double sevenToEight = 0;
            double eightToNine = 0;
            double nineToNineOne = 0;
            double nineToNineTwo = 0;
            double nineToNineThree = 0;
            double nineToNineFour = 0;
            double nineToNineFive = 0;
            double nineToNineSix = 0;
            double nineToNineSeven = 0;
            double nineToNineEight = 0;
            double nineToNineNine = 0;
            double nineToNineNineNine = 0;
            double nineToNineNineNineNine = 0;
            double toOne = 0;

            int countzeroToOne = 0;
            int countoneToTwo = 0;
            int counttwoToThree = 0;
            int countthreeToFour = 0;
            int countfourToFive = 0;
            int countfiveToSix = 0;
            int countsixToSeven = 0;
            int countsevenToEight = 0;
            int counteightToNine = 0;
            int countnineToNineOne = 0;
            int countnineToNineTwo = 0;
            int countnineToNineThree = 0;
            int countnineToNineFour = 0;
            int countnineToNineFive = 0;
            int countnineToNineSix = 0;
            int countnineToNineSeven = 0;
            int countnineToNineEight = 0;
            int countnineToNineNine = 0;
            int countnineToNineNineNine = 0;
            int countnineToNineNineNineNine = 0;
            int counttoOne = 0;


            foreach (var item in completeList.Values)
            {
                if(item.EntropyValue < 0.1)
                {
                    zeroToOne = zeroToOne + item.EntropyValue - item.EncryptedEntropyValue;
                    countzeroToOne++;
                }
                else if (item.EntropyValue < 0.2)
                {
                    oneToTwo = oneToTwo + item.EntropyValue - item.EncryptedEntropyValue;
                    countoneToTwo++;
                }
                else if (item.EntropyValue < 0.3)
                {
                    twoToThree = twoToThree + item.EntropyValue - item.EncryptedEntropyValue;
                    counttwoToThree++;
                }
                else if (item.EntropyValue < 0.4)
                {
                    threeToFour = threeToFour + item.EntropyValue - item.EncryptedEntropyValue;
                    countthreeToFour++;
                }
                else if (item.EntropyValue < 0.5)
                {
                    fourToFive = fourToFive + item.EntropyValue - item.EncryptedEntropyValue;
                    countfourToFive++;
                }
                else if (item.EntropyValue < 0.6)
                {
                    fiveToSix = fiveToSix + item.EntropyValue - item.EncryptedEntropyValue;
                    countfiveToSix++;
                }
                else if (item.EntropyValue < 0.7)
                {
                    sixToSeven = sixToSeven + item.EntropyValue - item.EncryptedEntropyValue;
                    countsixToSeven++;
                }
                else if (item.EntropyValue < 0.8)
                {
                    sevenToEight = sevenToEight + item.EntropyValue - item.EncryptedEntropyValue;
                    countsevenToEight++;
                }
                else if (item.EntropyValue < 0.9)
                {
                    eightToNine = eightToNine + item.EntropyValue - item.EncryptedEntropyValue;
                    counteightToNine++;
                }
                else if (item.EntropyValue < 0.91)
                {
                    nineToNineOne = nineToNineOne + item.EntropyValue - item.EncryptedEntropyValue;
                    countnineToNineOne++;
                }
                else if (item.EntropyValue < 0.92)
                {
                    nineToNineTwo = nineToNineTwo + item.EntropyValue - item.EncryptedEntropyValue;
                    countnineToNineTwo++;
                }
                else if (item.EntropyValue < 0.93)
                {
                    nineToNineThree = nineToNineThree + item.EntropyValue - item.EncryptedEntropyValue;
                    countnineToNineThree++;
                }
                else if (item.EntropyValue < 0.94)
                {
                    nineToNineFour = nineToNineFour + item.EntropyValue - item.EncryptedEntropyValue;
                    countnineToNineFour++;
                }
                else if (item.EntropyValue < 0.95)
                {
                    nineToNineFive = nineToNineFive + item.EntropyValue - item.EncryptedEntropyValue;
                    countnineToNineFive++;
                }
                else if (item.EntropyValue < 0.96)
                {
                    nineToNineSix = nineToNineSix + item.EntropyValue - item.EncryptedEntropyValue;
                    countnineToNineSix++;
                }
                else if (item.EntropyValue < 0.97)
                {
                    nineToNineSeven = nineToNineSeven + item.EntropyValue - item.EncryptedEntropyValue;
                    countnineToNineSeven++;
                }
                else if (item.EntropyValue < 0.98)
                {
                    nineToNineEight = nineToNineEight + item.EntropyValue - item.EncryptedEntropyValue;
                    countnineToNineEight++;
                }
                else if (item.EntropyValue < 0.99)
                {
                    nineToNineNine = nineToNineNine + item.EntropyValue - item.EncryptedEntropyValue;
                    countnineToNineNine++;
                }
                else if (item.EntropyValue < 0.999)
                {
                    nineToNineNineNine = nineToNineNineNine + item.EntropyValue - item.EncryptedEntropyValue;
                    countnineToNineNineNine++;
                }
                else if (item.EntropyValue < 0.9999)
                {
                    nineToNineNineNineNine = nineToNineNineNineNine + item.EntropyValue - item.EncryptedEntropyValue;
                    countnineToNineNineNineNine++;
                }
                else if (item.EntropyValue < 1)
                {
                    toOne = toOne + item.EntropyValue - item.EncryptedEntropyValue;
                    counttoOne++;
                }
            }

            double averagezeroToOne              = zeroToOne / countzeroToOne;
            double averageoneToTwo               = oneToTwo / countoneToTwo;
            double averagetwoToThree             = twoToThree / counttwoToThree;
            double averagethreeToFour            = threeToFour / countthreeToFour;
            double averagefourToFive             = fourToFive / countfourToFive;
            double averagefiveToSix              = fiveToSix / countfiveToSix;
            double averagesixToSeven             = sixToSeven / countsixToSeven;
            double averagesevenToEight           = sevenToEight / countsevenToEight;
            double averageeightToNine            = eightToNine / counteightToNine;
            double averagenineToNineOne          = nineToNineOne / countnineToNineOne;
            double averagenineToNineTwo          = nineToNineTwo / countnineToNineTwo;
            double averagenineToNineThree        = nineToNineThree / countnineToNineThree;
            double averagenineToNineFour         = nineToNineFour / countnineToNineFour;
            double averagenineToNineFive         = nineToNineFive / countnineToNineFive;
            double averagenineToNineSix          = nineToNineSix / countnineToNineSix;
            double averagenineToNineSeven        = nineToNineSeven / countnineToNineSeven;
            double averagenineToNineEight        = nineToNineEight / countnineToNineEight;
            double averagenineToNineNine         = nineToNineNine / countnineToNineNine;
            double averagenineToNineNineNine     = nineToNineNineNine / countnineToNineNineNine;
            double averagenineToNineNineNineNine = nineToNineNineNineNine / countnineToNineNineNineNine;
            double averagetoOne                  = toOne / counttoOne;


            Console.WriteLine(averagePDFentropy);
            Console.WriteLine(averagePDFencryptEntropy);
            Console.WriteLine(averageDOCXentropy);
            Console.WriteLine(averageDOCXencryptEntropy);
            Console.WriteLine(averageTXTentropy);
            Console.WriteLine(averageTXTencryptEntropy);

            Console.WriteLine(averagezeroToOne);
            Console.WriteLine(averageoneToTwo              );
            Console.WriteLine(averagetwoToThree            );
            Console.WriteLine(averagethreeToFour           );
            Console.WriteLine(averagefourToFive            );
            Console.WriteLine(averagefiveToSix             );
            Console.WriteLine(averagesixToSeven            );
            Console.WriteLine(averagesevenToEight          );
            Console.WriteLine(averageeightToNine           );
            Console.WriteLine(averagenineToNineOne         );
            Console.WriteLine(averagenineToNineTwo         );
            Console.WriteLine(averagenineToNineThree       );
            Console.WriteLine(averagenineToNineFour        );
            Console.WriteLine(averagenineToNineFive        );
            Console.WriteLine(averagenineToNineSix         );
            Console.WriteLine(averagenineToNineSeven       );
            Console.WriteLine(averagenineToNineEight       );
            Console.WriteLine(averagenineToNineNine        );
            Console.WriteLine(averagenineToNineNineNine    );
            Console.WriteLine(averagenineToNineNineNineNine);
            Console.WriteLine(averagetoOne                 );


            string filePath = @"C:\Speciale\" + "\\textFile2.txt";
            if (!File.Exists(filePath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    sw.WriteLine(averagePDFentropy);
                    sw.WriteLine(averagePDFencryptEntropy);
                    sw.WriteLine(averageDOCXentropy);
                    sw.WriteLine(averageDOCXencryptEntropy);
                    sw.WriteLine(averageTXTentropy);
                    sw.WriteLine(averageTXTencryptEntropy);
                    sw.WriteLine(averagezeroToOne);
                    sw.WriteLine(averageoneToTwo);
                    sw.WriteLine(averagetwoToThree);
                    sw.WriteLine(averagethreeToFour);
                    sw.WriteLine(averagefourToFive);
                    sw.WriteLine(averagefiveToSix);
                    sw.WriteLine(averagesixToSeven);
                    sw.WriteLine(averagesevenToEight);
                    sw.WriteLine(averageeightToNine);
                    sw.WriteLine(averagenineToNineOne);
                    sw.WriteLine(averagenineToNineTwo);
                    sw.WriteLine(averagenineToNineThree);
                    sw.WriteLine(averagenineToNineFour);
                    sw.WriteLine(averagenineToNineFive);
                    sw.WriteLine(averagenineToNineSix);
                    sw.WriteLine(averagenineToNineSeven);
                    sw.WriteLine(averagenineToNineEight);
                    sw.WriteLine(averagenineToNineNine);
                    sw.WriteLine(averagenineToNineNineNine);
                    sw.WriteLine(averagenineToNineNineNineNine);
                    sw.WriteLine(averagetoOne);
                }
            }


            List<double> entropy = new List<double>();

            foreach (ShannonObject shOb in completeList.Values)
            {
                if((shOb.EntropyValue - shOb.EncryptedEntropyValue) > 0)
                {
                    Console.WriteLine("Hold the fuck up!");
                    Console.WriteLine("Path: " + shOb.Path + " Entropy: " + shOb.EntropyValue + " Encryptedtropy: " + shOb.EncryptedEntropyValue);
                }
                entropy.Add(shOb.EntropyValue - shOb.EncryptedEntropyValue);
            }

            List<string> stringEntropy = new List<string>();
            foreach (var item in entropy)
            {
                stringEntropy.Add(item.ToString());
            }


            FileWriter.listToTxt(pathToFiles, stringEntropy);
        }

        public static Dictionary<string, string> parseTxTfile(string pathToParse, string nameOfFile)
        {
            string line;
            string[] pairs = new string[2];
            Dictionary<string, string> hashedFilesReturn = new Dictionary<string, string>();
            System.IO.StreamReader file =
                new System.IO.StreamReader(pathToParse + "\\" + nameOfFile);
            while ((line = file.ReadLine()) != null)
            {
                pairs = line.Split('?');
                hashedFilesReturn.Add(pairs[0], pairs[1]);
            }
            file.Close();
            return hashedFilesReturn;
        }

        public static string getType(string fullPath)
        {
            string lastChars = fullPath.Substring(fullPath.Length - 5, 5);

            for (int i = 0; i < lastChars.Length; i++)
            {
                if (lastChars.Substring(i, 1).Equals("."))
                {
                    return lastChars.Substring(i+1);
                }
            }
            return "";
        }

        public static string removeFun(string fullPath)
        {
            if (fullPath.Substring(fullPath.Length - 4, 4).Equals(".fun"))
            {
                return fullPath.Remove(fullPath.Length - 4);
            }
            else
            {
                return fullPath;
            }
        }
    }

    class ShannonObject
    {
        public string Path { get; set; }
        public double EntropyValue { get; set; }
        public string Type { get; set; }
        public string Size { get; set; }
        public double EncryptedEntropyValue { get; set; }
    }

}
