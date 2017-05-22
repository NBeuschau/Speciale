using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShannonPOC
{
    class ShannonEntropy
    {
        private static Dictionary<string, double> savedEntropies = new Dictionary<string, double>();

        public Dictionary<string, double> getEntropyOfAllFilesInPath(string path)
        {
            string[] filesInDirectory = null;
            Console.WriteLine(path);

            //Check if it is possible to get the files in path, if not return findings
            try
            {
                filesInDirectory = Directory.GetFiles(path);
            }
            catch (Exception)
            {
                return savedEntropies;
            }

            //Takes the entropy of each file in directory
            FileInfo tempFil;
            foreach (string file in filesInDirectory)
            {
                tempFil = new FileInfo(file);
                savedEntropies.Add(file, CalculateEntropy(tempFil));
            }

            //Get every subdirectory in the given path
            var subDirectories = Directory.GetDirectories(path);

            //Iterates though the subdirectories
            foreach (var directory in subDirectories)
            {
                //Creates a string with the name of the subdirectory only
                string dirName = new DirectoryInfo(directory).Name;

                //Calls the function itself for every subdirectory
                getEntropyOfAllFilesInPath(path + "\\" + dirName);
            }

            return savedEntropies;
        }

        public double CalculateEntropy(FileInfo file)
        {
            //Set the range to 256
            int range = byte.MaxValue + 1;

            //Read the bytes of the file into a byte array
            //If the path is not a file but a directory it returns 0
            byte[] values;
            try
            {
                values = File.ReadAllBytes(file.FullName);
            }
            catch (Exception)
            {
                return -1;
            }

            //Make a long array the size of the range we are interested in
            long[] counts = new long[range];
            foreach (byte value in values)
            {
                //Count how many occurences there are of each byte
                counts[value] = counts[value] + 1;
            }

            double entropy = 0;

            //Adds the entropy of every single number in the values array together
            foreach (long count in counts)
            {
                if(count != 0)
                {
                    double probability = (double)count / values.LongLength;
                    entropy -= probability * Math.Log(probability, range);
                }
            }
            return entropy;
        }

        public static Dictionary<string, double> getSavedEntropies()
        {
            return savedEntropies;
        }

        public static void removeKeyFromSavedEntropies(string key)
        {
            if (savedEntropies.ContainsKey(key))
            {
                savedEntropies.Remove(key);
            }
            else
            {
                Console.WriteLine("Could not remove key "+ key +" since it does not exist in the list");
            }
        }

        public static void addKeyAndDoubleToSavedEntropies(string key, double value)
        {
            if (!savedEntropies.ContainsKey(key))
            {
                savedEntropies.Add(key, value);
            }
            else
            {
                Console.WriteLine("Could not add key " + key + " to the list since it is already there");
            }
        }
    }
}
