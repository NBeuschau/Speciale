using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public MainWindow()
        {
            InitializeComponent();

            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string specialePath = "\\SpecialeTest";
            Watcher test = new Watcher();
            test.CreateFileWatcher(path);
            Console.WriteLine("Stien er: " + path + specialePath);

            test.CreateFileWatcher(path + specialePath);

            FileCreator newFileCreator = new FileCreator();

            newFileCreator.CreateFileInEveryFolder(path + specialePath);



        }
    }
}
