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
using ImportExportDesktopApp.Pages;

namespace ImportExportDesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Frame _mainFrame;
        public MainWindow()
        {
            InitializeComponent();
            _mainFrame = (Frame)this.FindName("MainFrame");
            //if (processingScreen == null)
            //{
            //    processingScreen = new ProcessingScreen();
            //}
            _mainFrame.Navigate(new ProcessingPage());
        }

        private void page_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void page_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
