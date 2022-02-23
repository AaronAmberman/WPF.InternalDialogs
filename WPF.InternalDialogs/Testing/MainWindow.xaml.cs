﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using WPF.InternalDialogs;

namespace Testing
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (internalDialog.Visibility == Visibility.Collapsed)
                {
                    internalDialog.Visibility = Visibility.Visible;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!internalDialog.BlockUntilReturned)
            {
                internalDialog.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            internalDialog.Result = MessageBoxResult.OK;
            internalDialog.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (internalDialog.BlockUntilReturned)
            {
                for (int i = 0; i < 10; i++)
                {
                    Debug.WriteLine(i);
                }

                internalDialog.Visibility = Visibility.Visible;

                for (int i = 0; i < 10; i++)
                {
                    Debug.WriteLine(i);
                }

                Debug.WriteLine(internalDialog.Result);
            }
        }
    }
}
