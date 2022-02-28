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
                
            }
        }

        private void InternalDialogCloseButton(object sender, RoutedEventArgs e)
        {
            internalDialog.Visibility = Visibility.Collapsed;
        }

        private void MovableResizableCloseButton(object sender, RoutedEventArgs e)
        {
            mrid.Visibility = Visibility.Collapsed;
        }

        private void InternalDialogExample1(object sender, RoutedEventArgs e)
        {
            internalDialog.Visibility = Visibility.Visible;
        }

        private void InternalDialogExample2(object sender, RoutedEventArgs e)
        {
            internalDialog.HorizontalContentAlignment = HorizontalAlignment.Center;
            internalDialog.VerticalContentAlignment = VerticalAlignment.Center;
            internalDialog.BorderThickness = new Thickness(5);
            internalDialog.BorderBrush = Brushes.Red;
            internalDialog.Visibility = Visibility.Visible;
        }

        private void InternalDialogExample3(object sender, RoutedEventArgs e)
        {
            internalDialog.Background = new SolidColorBrush(new Color { A = 33, R = 0, G = 0, B = 0 });
            internalDialog.BorderThickness = new Thickness(10);
            internalDialog.BorderBrush = Brushes.Green;
            internalDialog.ContentPadding = new Thickness(50);
            internalDialog.CornerRadius = new CornerRadius(0);
            internalDialog.Padding = new Thickness(50);
            internalDialog.Foreground = Brushes.Green;
            internalDialog.Visibility = Visibility.Visible;
        }

        private void InternalDialogExample4(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                Debug.WriteLine($"Before : {i}");
            }

            internalDialog.Content = "Showing modal. Press escape to close.";
            internalDialog.IsModal = true;
            internalDialog.Visibility = Visibility.Visible;

            for (int i = 0; i < 10; i++)
            {
                Debug.WriteLine($"After : {i}");
            }
        }

        private void MessageBoxExample1(object sender, RoutedEventArgs e)
        {
            mbiDialog.Title = "Message Box Example 1";
            mbiDialog.Message = "This is a simple message box.";
            mbiDialog.MessageBoxImage = MessageBoxInternalDialogImage.Information;
            mbiDialog.MessageBoxButton = MessageBoxButton.OKCancel;
            mbiDialog.Visibility = Visibility.Visible;
        }

        private void MessageBoxExample2(object sender, RoutedEventArgs e)
        {
            mbiDialog.Title = "Message Box Example 2";
            mbiDialog.Message = "The user was asked a question. Something something question?";
            mbiDialog.MessageBoxImage = MessageBoxInternalDialogImage.Help;
            mbiDialog.MessageBoxButton = MessageBoxButton.YesNo;
            mbiDialog.Visibility = Visibility.Visible;
        }

        private void MessageBoxExample3(object sender, RoutedEventArgs e)
        {
            mbiDialog.Title = "Message Box Example 3";
            mbiDialog.TitleAreaBackground = Brushes.Green;
            mbiDialog.Message = "A security check was successful.";
            mbiDialog.MessageBoxImage = MessageBoxInternalDialogImage.SecurityOK;
            mbiDialog.MessageBoxButton = MessageBoxButton.OK;
            mbiDialog.ButtonAreaBackground = Brushes.ForestGreen;
            mbiDialog.Visibility = Visibility.Visible;
        }

        private void MessageBoxExample4(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                Debug.WriteLine($"Before : {i}");
            }

            mbiDialog.Title = "Message Box Example 4";
            mbiDialog.TitleAreaBackground = Brushes.Red;
            mbiDialog.Message = "Some security critical thing occurred. Tell the user. Add modal-ness so code executes after we close.";
            mbiDialog.MessageBoxImage = MessageBoxInternalDialogImage.SecurityCritical;
            mbiDialog.MessageBoxButton = MessageBoxButton.YesNoCancel;
            mbiDialog.MessageBoxBackground = Brushes.Orange;
            mbiDialog.ButtonAreaBackground = Brushes.Red;
            mbiDialog.IsModal = true;
            mbiDialog.Visibility = Visibility.Visible;

            for (int i = 0; i < 10; i++)
            {
                Debug.WriteLine($"After : {i}");
            }
        }

        private void InputBoxExample1(object sender, RoutedEventArgs e)
        {
            ibid.Input = "Feed in input.";
            ibid.InputBoxMessage = "Some message to the user";
            ibid.Visibility = Visibility.Visible;
        }

        private void InputBoxExample2(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                Debug.WriteLine($"Before : {i}");
            }

            ibid.Input = "";
            ibid.InputBoxMessage = "This is blocking input.";
            ibid.IsModal = true;
            ibid.Visibility = Visibility.Visible;

            for (int i = 0; i < 10; i++)
            {
                Debug.WriteLine($"After : {i}");
            }
        }

        private void MovableResizableExample1(object sender, RoutedEventArgs e)
        {
            mrid.Visibility = Visibility.Visible;
        }

        private void MovableResizableExample2(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                Debug.WriteLine($"Before : {i}");
            }

            mrid.IsModal = true;
            mrid.Visibility = Visibility.Visible;

            for (int i = 0; i < 10; i++)
            {
                Debug.WriteLine($"After : {i}");
            }
        }

        private void ProgressInternalDialogExample1(object sender, RoutedEventArgs e)
        {
            pid2.Visibility = Visibility.Visible;
        }

        private void ProgressInternalDialogExample2(object sender, RoutedEventArgs e)
        {
            pid.ProgressDialogMessage = "This is a normal progress bar. Max: 100, Min:0, Value 75.";
            pid.ProgressValue = 75.0;
            pid.Visibility = Visibility.Visible;
        }
    }
}
