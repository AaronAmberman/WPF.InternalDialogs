using System;
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
                //if (internalDialog.Visibility == Visibility.Collapsed)
                //{
                //    internalDialog.Visibility = Visibility.Visible;
                //}

                if (mbiDialog.Visibility == Visibility.Collapsed)
                {
                    mbiDialog.Visibility = Visibility.Visible;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //if (!internalDialog.IsModal)
            //{
            //    internalDialog.Visibility = Visibility.Visible;
            //}

            if (!mbiDialog.IsModal)
            {
                mbiDialog.Visibility = Visibility.Visible;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //internalDialog.Result = MessageBoxResult.OK;
            //internalDialog.Visibility = Visibility.Collapsed;

            mbiDialog.Result = MessageBoxResult.OK;
            mbiDialog.Visibility = Visibility.Collapsed;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //if (internalDialog.IsModal)
            //{
            //    for (int i = 0; i < 10; i++)
            //    {
            //        Debug.WriteLine(i);
            //    }

            //    internalDialog.Visibility = Visibility.Visible;

            //    for (int i = 0; i < 10; i++)
            //    {
            //        Debug.WriteLine(i);
            //    }

            //    Debug.WriteLine(internalDialog.Result);
            //}

            if (mbiDialog.IsModal)
            {
                for (int i = 0; i < 10; i++)
                {
                    Debug.WriteLine(i);
                }

                mbiDialog.Visibility = Visibility.Visible;

                for (int i = 0; i < 10; i++)
                {
                    Debug.WriteLine(i);
                }

                Debug.WriteLine(mbiDialog.Result);
            }
            else
            {
                mbiDialog.Message = "Some other message about your application. Some other message about your application. Some other message about your application. Some other message about your application. Some other message about your application. Some other message about your application. Some other message about your application. Some other message about your application. Some other message about your application. Some other message about your application. Some other message about your application. Some other message about your application.Some other message about your application.Some other message about your application. Some other message about your application. Some other message about your application.Some other message about your application.Some other message about your application.Some other message about your application.Some other message about your application.Some other message about your application.Some other message about your application.Some other message about your application.Some other message about your application.";

                mbiDialog.Visibility = Visibility.Visible;
            }
        }
    }
}
