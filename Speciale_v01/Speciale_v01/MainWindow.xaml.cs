﻿using System;
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

            //The current path is that of desktop
            string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            //Creates a watcher from the class CreateFileWatcher
            Watcher test = new Watcher();
            Console.WriteLine("The path is: " + path);

            //The file watcher now monitors the directory given and all of the subdirectories.
            test.CreateFileWatcher(path + "\\test");

            //FileCreator creates a file in every folder in a given directory
            FileCreator newFileCreator = new FileCreator();

            //Recursively creates a .txt file in the directory and every given subdirectory
            newFileCreator.CreateFileInEveryFolder(path + "\\test");

            //Instead of shutting down the program it waits for an input.
            Console.ReadLine();
        }
    }
}
