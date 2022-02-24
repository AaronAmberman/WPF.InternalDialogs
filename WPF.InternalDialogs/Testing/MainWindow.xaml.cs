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
        }

        private void onDragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            // move the Thumb to the mouse position during the drag operation
            double yadjust = canvas.Height + e.VerticalChange;

            if (double.IsNaN(yadjust)) yadjust = canvas.ActualHeight + e.VerticalChange;

            double xadjust = canvas.Width + e.HorizontalChange;

            if (double.IsNaN(xadjust)) xadjust = canvas.ActualWidth + e.HorizontalChange;

            Debug.WriteLine($"X adjust: {xadjust} | Y adjust: {yadjust}");

            if ((xadjust >= 0) && (yadjust >= 0))
            {
                canvas.Width = xadjust;
                canvas.Height = yadjust;

                Canvas.SetLeft(myThumb, Canvas.GetLeft(myThumb) + e.HorizontalChange);
                Canvas.SetTop(myThumb, Canvas.GetTop(myThumb) + e.VerticalChange);

                Debug.WriteLine($"Size: {canvas.Width}, {canvas.Height}");
            }
        }

        private void onDragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            myThumb.Background = Brushes.Orange;
        }

        private void onDragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            myThumb.Background = Brushes.Blue;
        }

        private void Thumb_DragDelta(object sender, System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            // move the Thumb to the mouse position during the drag operation
            double yadjust = e.VerticalChange;
            double xadjust = e.HorizontalChange;

            Debug.WriteLine($"X adjust: {xadjust} | Y adjust: {yadjust}");

            Canvas.SetLeft(titleBorder, Canvas.GetLeft(titleBorder) + e.HorizontalChange);
            Canvas.SetTop(titleBorder, Canvas.GetTop(titleBorder) + e.VerticalChange);
        }

        private void Thumb_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            titleBorder.Background = Brushes.Blue;
        }

        private void Thumb_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            titleBorder.Background = Brushes.LightGray;
        }
    }
}
