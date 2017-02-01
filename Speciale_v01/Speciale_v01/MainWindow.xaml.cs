using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using Microsoft.Diagnostics.Tracing;
using Microsoft.Diagnostics.Tracing.Parsers;
using Microsoft.Diagnostics.Tracing.Parsers.Kernel;
using Microsoft.Diagnostics.Tracing.Session;

namespace Speciale_v01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public void addWriteEvent(FileIOReadWriteTraceData writeEvent)
        {
            string test = writeEvent.ProcessID.ToString();
        }
        public MainWindow()
        {
            InitializeComponent();



            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            Watcher test = new Watcher();
            Console.WriteLine("Stien er: " + path);
            test.CreateFileWatcher(path);

            test.CreateFileWatcher(path + "\\test");

            FileCreator newFileCreator = new FileCreator();

            newFileCreator.CreateFileInEveryFolder(path + "\\test");

            Console.ReadLine();
        }
    }
}
