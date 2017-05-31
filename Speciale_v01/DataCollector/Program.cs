using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCollector
{
    class Program
    {
        static string path1 = @"C:\Users\Baseline\Desktop";
        static string path2 = @"C:\Users\Baseline\Documents";
        static string path3 = @"C:\Users\Baseline\Downloads";
        static string path4 = @"C:\Users\Baseline\Videos";
        static string dataPath = @"C:\Users\Baseline\Desktop\BaselineFileData.txt";


        static void Main(string[] args)
        {
            Dictionary<string, FileObject> CollectedData = new Dictionary<string, FileObject>();
            Dictionary<string, FileObject> Direct1 = new Dictionary<string, FileObject>();
            Dictionary<string, FileObject> Direct2 = new Dictionary<string, FileObject>();
            Dictionary<string, FileObject> Direct3 = new Dictionary<string, FileObject>();
            Dictionary<string, FileObject> Direct4 = new Dictionary<string, FileObject>();


            IteratorThroughFiles temp1 = new IteratorThroughFiles();
            IteratorThroughFiles temp2 = new IteratorThroughFiles();
            IteratorThroughFiles temp3 = new IteratorThroughFiles();
            IteratorThroughFiles temp4 = new IteratorThroughFiles();

            Direct1 = temp1.iterator(path1);
            Direct2 = temp2.iterator(path2);
            Direct3 = temp3.iterator(path3);
            Direct4 = temp4.iterator(path4);

            Direct1.ToList().ForEach(x => CollectedData.Add(x.Key, x.Value));
            Direct2.ToList().ForEach(x => CollectedData.Add(x.Key, x.Value));
            Direct3.ToList().ForEach(x => CollectedData.Add(x.Key, x.Value));
            Direct4.ToList().ForEach(x => CollectedData.Add(x.Key, x.Value));

            Console.WriteLine("Test");
            foreach (var item in CollectedData)
            {
                Console.WriteLine(item.Value.DateCreated);
                Console.WriteLine(item.Value.LastAccessed);
                Console.WriteLine(item.Value.LastModified);
                Console.WriteLine(item.Value.Path);
                Console.WriteLine(item.Value.Size);
                Console.WriteLine();
            }

            string filePath = dataPath;
            if (!File.Exists(filePath))
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(filePath))
                {
                    foreach (var item in CollectedData)
                    {
                        sw.Write(item.Value.Path             + "?");
                        sw.Write(item.Value.Size             + "?");
                        sw.Write(item.Value.DateCreated      + "?");
                        sw.Write(item.Value.LastModified     + "?");
                        sw.WriteLine(item.Value.LastAccessed);
                    }
                }
            }


            Console.ReadLine();
        }
    }
}
