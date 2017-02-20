using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Speciale_v01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static string PATH = @"C:\Speciale\Test\";
        static int INDEXER = 0;
        static string BACKINGNAME = "backingName";


        public MainWindow()
        {
            InitializeComponent();
            start();
        }

        public static void start()
        {
            FileCreator.CreateFileInEveryFolder(PATH);

            var fw = new Thread(() => FileMonitor.CreateFileWatcher(PATH));
            fw.Start();

            var pm = new Thread(() => Procmon.createProcmonBackingFile(PATH, BACKINGNAME + INDEXER));
            pm.Start();
        }

        public static void honeypotChange(string path)
        {
            Thread.Sleep(2000);
            Procmon.procmonTerminator();
            Thread.Sleep(10000);

            INDEXER++;
            var cpmbf = new Thread(() => Procmon.createProcmonBackingFile(PATH, BACKINGNAME + INDEXER));
            cpmbf.Start();

            Procmon.convertPMLfileToCSV(PATH, BACKINGNAME + (INDEXER - 1) + ".PML", "convertedFile" + (INDEXER - 1) + ".CSV");

            List<CSVfile> parsedData = CSVfile.CSVparser(PATH + "convertedFile" + (INDEXER - 1) + ".CSV");

            foreach (var item in parsedData)
            {
                if (!item.processName.Equals("Explorer.EXE"))
                {

                    Console.WriteLine("Process :" + item.processName + " has changed a honeypot");
                    Console.WriteLine("It has process ID: " + item.PID + "\n");
                }
            }
            //Dataanalysis
        }
        /*
        public void run()
        {
            //The current path is that of desktop
            //string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            

            //Creates a watcher from the class CreateFileWatcher
            FileMonitor test = new FileMonitor();
            Console.WriteLine("The path is: " + PATH);

            //The file watcher now monitors the directory given and all of the subdirectories.
            test.CreateFileWatcher(PATH);

            //FileCreator creates a file in every folder in a given directory
            FileCreator newFileCreator = new FileCreator();

            //Procmon has the ability to convert pml files to csv and to start process monitor
            Procmon newProcmonCreator = new Procmon();
            
            //
            // //Runs a cmd commando to start procmons backingfile. The file is indexed by the integer
            // //Converts the backingfile from pml to csv
            // newProcmonCreator.convertPMLfileToCSV(path, "backingName3.PML", "convertedCSVfile" + indexer + ".CSV");

            //Recursively creates a .txt file in the directory and every given subdirectory
            //newFileCreator.CreateFileInEveryFolder(path + "\\test");

            CSVfile newCSVfile = new CSVfile();
            //newCSVfile.CSVparser(@"C:\speciale\test\convertedCSVfile9.CSV");

            Procmon procmon = new Procmon();
            var t = new Thread(() => Procmon.createProcmonBackingFile(PATH, backingName));
            t.Start();
            
            //Instead of shutting down the program it waits for an input.
            Thread.Sleep(4000);
            
            Thread.Sleep(6000);
            var p = new Thread(() => Procmon.createProcmonBackingFile(PATH, backingName));
            p.Start();
            //var p = new Thread(() => Procmon.procmonTerminator());
            //p.Start();
            Console.ReadLine();
            

        }
        */
    }
}
