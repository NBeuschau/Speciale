using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaseLineLogger
{
    class Program
    {
        //static string PATH = @"C:\Users\viruseater1";
        //The path used for the testing computer
        static string PATH = @"C:\Users\Baseline";

        //The path for the ransomwareDownloader
        static string RANSOMWAREDOWNLOADERPATH = @"C:\Software\RansomwareBaseDownloader\bin\Release\RansomwareBaseDownloader.exe";

        static void Main(string[] args)
        {
            baseLineTester();
        }

        //The controlling process
        public static void baseLineTester()
        {
            //Gets the name of the ransomware
            BaseLineLogger.setRansomwareDownloaderPath(RANSOMWAREDOWNLOADERPATH);
            BaseLineLogger.getBaseRansomware();
            Console.WriteLine(BaseLineLogger.getNAMEONTEST());

            //Starts the logwriter
            BaseLineLogger.LogWriter(PATH);

            //Posts the results to the server
            BaseLineLogger.postBasePosted();

            //Post to the server that the baseline has tested this ransomware
            BaseLineLogger.postBaseTested();
        }
    }
}
