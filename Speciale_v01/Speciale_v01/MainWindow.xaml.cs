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

namespace Speciale_v01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string path = @"C:\Speciale\Test";

            Watcher test = new Watcher();
            Console.WriteLine("Stien er: " + path);
            test.CreateFileWatcher(path);

            test.CreateFileWatcher(path);

            FileCreator newFileCreator = new FileCreator();

            //newFileCreator.CreateFileInEveryFolder(path + "\\test");

            Console.ReadLine();
        }
    }
}
