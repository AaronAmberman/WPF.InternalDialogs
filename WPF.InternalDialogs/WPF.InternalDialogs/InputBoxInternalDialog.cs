﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF.InternalDialogs
{
    /// <summary>A simple input box to gather basic user input.</summary>
    [TemplatePart(Name = "PART_Canvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_InnerBorder", Type = typeof(Border))]
    [TemplatePart(Name = "PART_CancelButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_CloseButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_OkButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_TitleThumb", Type = typeof(Thumb))]
    [TemplatePart(Name = "PART_ResizeThumbContainer", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_ResizeThumb", Type = typeof(Thumb))]
    public sealed class InputBoxInternalDialog : InternalDialog
    {
        #region Fields

        private Canvas? canvas;
        private Border? innerBorder;
        private Button? cancelButton;
        private Button? closeButton;
        private Button? okButton;
        private Thumb? titleThumb;
        private Grid? resizeThumbContainer;
        private Thumb? resizeThumb;

        private int hasBeenUpdatedCount = 0;

        #endregion

        #region Properties

        /// <summary>Gets or sets the background for the button area.</summary>
        public SolidColorBrush ButtonAreaBackground
        {
            get { return (SolidColorBrush)GetValue(ButtonAreaBackgroundProperty); }
            set { SetValue(ButtonAreaBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ButtonAreaBackgroundProperty =
            DependencyProperty.Register("ButtonAreaBackground", typeof(SolidColorBrush), typeof(InputBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>gets or sets the style to use for the buttons in the message box.</summary>
        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(InputBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>gets or sets the style to use for the close button at the top right.</summary>
        public Style CloseButtonStyle
        {
            get { return (Style)GetValue(CloseButtonStyleProperty); }
            set { SetValue(CloseButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty CloseButtonStyleProperty =
            DependencyProperty.Register("CloseButtonStyle", typeof(Style), typeof(InputBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the input to display in the text box portion of the input box.</summary>
        public string Input
        {
            get { return (string)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty InputProperty =
            DependencyProperty.Register("Input", typeof(string), typeof(InputBoxInternalDialog), new PropertyMetadata(string.Empty));

        /// <summary>Gets or sets whether or not the input text box accepts return. Default is false.</summary>
        public bool InputBoxAcceptsReturn
        {
            get { return (bool)GetValue(InputBoxAcceptsReturnProperty); }
            set { SetValue(InputBoxAcceptsReturnProperty, value); }
        }

        public static readonly DependencyProperty InputBoxAcceptsReturnProperty =
            DependencyProperty.Register("InputBoxAcceptsReturn", typeof(bool), typeof(InputBoxInternalDialog), new PropertyMetadata(false));

        /// <summary>Gets or sets whether or not the input text box accepts tabs. Default is false.</summary>
        public bool InputBoxAcceptsTab
        {
            get { return (bool)GetValue(InputBoxAcceptsTabProperty); }
            set { SetValue(InputBoxAcceptsTabProperty, value); }
        }

        public static readonly DependencyProperty InputBoxAcceptsTabProperty =
            DependencyProperty.Register("InputBoxAcceptsTab", typeof(bool), typeof(InputBoxInternalDialog), new PropertyMetadata(false));

        /// <summary>Gets or sets the background for the message box part of the message box internal dialog. Not the same as Background.</summary>
        public SolidColorBrush InputBoxBackground
        {
            get { return (SolidColorBrush)GetValue(InputBoxBackgroundProperty); }
            set { SetValue(InputBoxBackgroundProperty, value); }
        }

        public static readonly DependencyProperty InputBoxBackgroundProperty =
            DependencyProperty.Register("InputBoxBackground", typeof(SolidColorBrush), typeof(InputBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the message box maximum height. Default is 600.0.</summary>
        public double InputBoxMaxHeight
        {
            get { return (double)GetValue(InputBoxMaxHeightProperty); }
            set { SetValue(InputBoxMaxHeightProperty, value); }
        }

        public static readonly DependencyProperty InputBoxMaxHeightProperty =
            DependencyProperty.Register("InputBoxMaxHeight", typeof(double), typeof(InputBoxInternalDialog), new PropertyMetadata(600.0));

        /// <summary>Gets or sets the message box maximum width. Default is 800.0</summary>
        public double InputBoxMaxWidth
        {
            get { return (double)GetValue(InputBoxMaxWidthProperty); }
            set { SetValue(InputBoxMaxWidthProperty, value); }
        }

        public static readonly DependencyProperty InputBoxMaxWidthProperty =
            DependencyProperty.Register("InputBoxMaxWidth", typeof(double), typeof(InputBoxInternalDialog), new PropertyMetadata(800.0));

        /// <summary>Gets or sets the message box minimum height. Default is 50.0.</summary>
        public double InputBoxMinHeight
        {
            get { return (double)GetValue(InputBoxMinHeightProperty); }
            set { SetValue(InputBoxMinHeightProperty, value); }
        }

        public static readonly DependencyProperty InputBoxMinHeightProperty =
            DependencyProperty.Register("InputBoxMinHeight", typeof(double), typeof(InputBoxInternalDialog), new PropertyMetadata(50.0));

        /// <summary>Gets or sets the message box minimum width. default is 100.0.</summary>
        public double InputBoxMinWidth
        {
            get { return (double)GetValue(InputBoxMinWidthProperty); }
            set { SetValue(InputBoxMinWidthProperty, value); }
        }

        public static readonly DependencyProperty InputBoxMinWidthProperty =
            DependencyProperty.Register("InputBoxMinWidth", typeof(double), typeof(InputBoxInternalDialog), new PropertyMetadata(100.0));

        /// <summary>Gets or sets the message to display to the user.</summary>
        public string InputBoxMessage
        {
            get { return (string)GetValue(InputBoxMessageProperty); }
            set { SetValue(InputBoxMessageProperty, value); }
        }

        public static readonly DependencyProperty InputBoxMessageProperty =
            DependencyProperty.Register("InputBoxMessage", typeof(string), typeof(InputBoxInternalDialog), new PropertyMetadata(string.Empty));

        /// <summary>Gets or sets the brush for the gripper section at the bottom right.</summary>
        public SolidColorBrush ResizeGripBrush
        {
            get { return (SolidColorBrush)GetValue(ResizeGripBrushProperty); }
            set { SetValue(ResizeGripBrushProperty, value); }
        }

        public static readonly DependencyProperty ResizeGripBrushProperty =
            DependencyProperty.Register("ResizeGripBrush", typeof(SolidColorBrush), typeof(InputBoxInternalDialog), new PropertyMetadata(Brushes.White));

        /// <summary>Gets or sets the cursor for the resize gripper.</summary>
        public Cursor ResizeGripCursor
        {
            get { return (Cursor)GetValue(ResizeGripCursorProperty); }
            set { SetValue(ResizeGripCursorProperty, value); }
        }

        public static readonly DependencyProperty ResizeGripCursorProperty =
            DependencyProperty.Register("ResizeGripCursor", typeof(Cursor), typeof(InputBoxInternalDialog), new PropertyMetadata(Cursors.SizeNWSE));

        /// <summary>Gets or sets the visibility of the resize grip. Visible = resizing enabled, Collapsed/Hidden = resizing disabled.</summary>
        public Visibility ResizeGripVisibility
        {
            get { return (Visibility)GetValue(ResizeGripVisibilityProperty); }
            set { SetValue(ResizeGripVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ResizeGripVisibilityProperty =
            DependencyProperty.Register("ResizeGripVisibility", typeof(Visibility), typeof(InputBoxInternalDialog), new PropertyMetadata(Visibility.Visible));

        /// <summary>Gets or sets the title to the message box.</summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(InputBoxInternalDialog), new PropertyMetadata(string.Empty));

        /// <summary>Gets or sets the background for the title area.</summary>
        public SolidColorBrush TitleAreaBackground
        {
            get { return (SolidColorBrush)GetValue(TitleAreaBackgroundProperty); }
            set { SetValue(TitleAreaBackgroundProperty, value); }
        }

        public static readonly DependencyProperty TitleAreaBackgroundProperty =
            DependencyProperty.Register("TitleAreaBackground", typeof(SolidColorBrush), typeof(InputBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the cursor for the title area. Default is Cursors.SizeAll.</summary>
        public Cursor TitleCursor
        {
            get { return (Cursor)GetValue(TitleCursorProperty); }
            set { SetValue(TitleCursorProperty, value); }
        }

        public static readonly DependencyProperty TitleCursorProperty =
            DependencyProperty.Register("TitleCursor", typeof(Cursor), typeof(InputBoxInternalDialog), new PropertyMetadata(Cursors.SizeAll));

        /// <summary>Gets or sets the horizontal alignment of the title.</summary>
        public HorizontalAlignment TitleHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(TitleHorizontalAlignmentProperty); }
            set { SetValue(TitleHorizontalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty TitleHorizontalAlignmentProperty =
            DependencyProperty.Register("TitleHorizontalAlignment", typeof(HorizontalAlignment), typeof(InputBoxInternalDialog),
                new PropertyMetadata(HorizontalAlignment.Left));

        #endregion

        #region Constructors

        static InputBoxInternalDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InputBoxInternalDialog), new FrameworkPropertyMetadata(typeof(InputBoxInternalDialog)));
            VisibilityProperty.OverrideMetadata(typeof(InputBoxInternalDialog), new FrameworkPropertyMetadata(Visibility.Collapsed, VisibilityChangedCallback));
        }

        public InputBoxInternalDialog()
        {
            LayoutUpdated += InputBoxInternalDialog_LayoutUpdated;
        }

        private void InputBoxInternalDialog_LayoutUpdated(object? sender, EventArgs e)
        {
            /*
             * To make it so the message box appears in the middle we use LayoutUpdated because it 
             * fires frequently, we need to finese it so that it only gets us what we need then it 
             * stops messing with the UI, a certain number of passes accomplishes that goal. This 
             * centers when showing but then allows the user to move it freely afterwards (takes less 
             * than one second for all iterations to occur).
             * 
             * (this accounts for initial load and every show there after)
             * 
             * weird very specific bug:
             * If the window containing the InputBoxInternalDialog moved monitors before the 
             * InputBoxInternalDialog is shown for the first time then it always starts at 
             * the top left but only for the first show. It is fine every view afterwards. No 
             * matter what action is taken except for the thing that breaks dragging completely, 
             * which obviously we don't want. That thing is to comment out the counting of 
             * iterations of LayoutUpdated so it is constantly called when visible. This makes 
             * it so the message box it always centered and it cannot be moved/dragged. This 
             * quirk only occurs if the message box has not been shown yet and if moving monitors 
             * (this is even true if moving back to the original monitor) but only occurs on 
             * first render). It also shows the resize gripper at position 0,0 because it wasn't 
             * moved by our centering logic yet. 
             * 
             * The reason for this is because LayoutUpdated does not fire in this scenario. Not 
             * sure why the WPF framework doesn't fire LayoutUpdated in this scenario but without 
             * the event being called, sadly, our logic is not called. :(
             * 
             * another weird specific bug:
             * If the message box has not been shown yet and the window is smaller then the needed space
             * for the mesage box visually then it shows at the top left. It also shows the resize 
             * gripper at position 0,0 because it wasn't moved by our centering logic yet. 
             * 
             * The reason for this is because LayoutUpdated does not fire in this scenario. Not 
             * sure why the WPF framework doesn't fire LayoutUpdated in this scenario but without 
             * the event being called, sadly, our logic is not called. :( (same reason)
             * 
             * Both of these can also seem to occur (again, on first show) if the computer is running
             * slow or processing a heavy load. Seems rare even here.
             * 
             * potential solution:
             * Very quickly show the message box when your MainWindow loads then immediately hide it.
             * This is so the OnApplyTemplate can run and we can grab our runtime controls that make 
             * up our ControlTemplate (we'll the ones we use).
             */
            if (hasBeenUpdatedCount < 20)
            {
                CenterMessageBox();

                hasBeenUpdatedCount++;
            }
        }

        #endregion

        #region Methods

        new private static void VisibilityChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InputBoxInternalDialog? instance = d as InputBoxInternalDialog;

            if (instance == null) return;

            Visibility visibility = (Visibility)e.NewValue;

            // we will have Visible and Collapsed states, no Hidden
            if (visibility == Visibility.Hidden)
            {
                instance.Visibility = Visibility.Collapsed;

                // kick out and let us setting the new value make the below logic run,
                // we'll leave our callback (this if will not be hit next callback)
                return;
            }

            // call our base
            InternalDialog.VisibilityChangedCallback(instance, e);

            if (visibility == Visibility.Visible)
            {
                instance.ValidateMinAndMax();
            }
            else // Collapsed
            {
                // just reset our visual update counter
                instance.hasBeenUpdatedCount = 0;

                // make sure reset the size of the message (fixes custom size set from user dragging)
                if (instance.innerBorder != null)
                {
                    instance.innerBorder.Width = double.NaN;
                    instance.innerBorder.Height = double.NaN;
                }
            }
        }

        private void CenterMessageBox()
        {
            if (canvas == null) return;
            if (innerBorder == null) return;

            // if we are not visible then we do not need to manage our visuals, just leave
            if (Visibility == Visibility.Collapsed)
            {
                return;
            }

            // center message box
            double totalWidth = canvas.ActualWidth;
            double totalHeight = canvas.ActualHeight;

            double messageBoxWidth = innerBorder.ActualWidth;
            double messageBoxHeight = innerBorder.ActualHeight;

            double centerX = (totalWidth / 2) - (messageBoxWidth / 2);
            double centerY = (totalHeight / 2) - (messageBoxHeight / 2);

            Canvas.SetLeft(innerBorder, centerX);
            Canvas.SetTop(innerBorder, centerY);

            // we are going to move the resizer too (bottom right of message box)
            double resizerX = centerX + messageBoxWidth - 5;
            double resizerY = centerY + messageBoxHeight - 5;

            Canvas.SetLeft(resizeThumbContainer, resizerX);
            Canvas.SetTop(resizeThumbContainer, resizerY);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            canvas = GetTemplateChild("PART_Canvas") as Canvas;
            innerBorder = GetTemplateChild("PART_InnerBorder") as Border;

            cancelButton = GetTemplateChild("PART_CancelButton") as Button;
            closeButton = GetTemplateChild("PART_CloseButton") as Button;
            okButton = GetTemplateChild("PART_OkButton") as Button;

            if (cancelButton != null)
                cancelButton.Click += CancelButton_Click;

            if (closeButton != null)
                closeButton.Click += CloseButton_Click;

            if (okButton != null)
                okButton.Click += OkButton_Click;

            titleThumb = GetTemplateChild("PART_TitleThumb") as Thumb;

            if (titleThumb != null)
            {
                titleThumb.DragDelta += TitleThumb_DragDelta;
                titleThumb.DragStarted += TitleThumb_DragStarted;
                titleThumb.DragCompleted += TitleThumb_DragCompleted;
            }

            resizeThumbContainer = GetTemplateChild("PART_ResizeThumbContainer") as Grid;
            resizeThumb = GetTemplateChild("PART_ResizeThumb") as Thumb;

            if (resizeThumb != null)
            {
                resizeThumb.DragDelta += ResizeThumb_DragDelta;
                resizeThumb.DragStarted += ResizeThumb_DragStarted;
                resizeThumb.DragCompleted += ResizeThumb_DragCompleted;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            Visibility = Visibility.Collapsed;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Cancel;
            Visibility = Visibility.Collapsed;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            Visibility = Visibility.Collapsed;
        }

        private void TitleThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (innerBorder != null)
            {
                Canvas.SetLeft(innerBorder, Canvas.GetLeft(innerBorder) + e.HorizontalChange);
                Canvas.SetTop(innerBorder, Canvas.GetTop(innerBorder) + e.VerticalChange);

                // we are going to move the resizer too (bottom right of message box)
                double updatedX = Canvas.GetLeft(innerBorder);
                double updatedY = Canvas.GetTop(innerBorder);

                double width = innerBorder.ActualWidth;
                double height = innerBorder.ActualHeight;

                double resizerX = updatedX + width - 5;
                double resizerY = updatedY + height - 5;

                Canvas.SetLeft(resizeThumbContainer, resizerX);
                Canvas.SetTop(resizeThumbContainer, resizerY);
            }
        }

        private void TitleThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            // should we do something here?
        }

        private void TitleThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            if (canvas == null) return;

            // get visual data for spacial tracking
            Point? topLeft = canvas.PointToScreen(new Point(0, 0));
            if (topLeft == null) return;

            Point? bottomRight = new Point(topLeft.Value.X + canvas.ActualWidth, topLeft.Value.Y + canvas.ActualHeight);
            if (bottomRight == null) return;

            double left = canvas.PointToScreen(new Point(Canvas.GetLeft(innerBorder), 0.0)).X;
            double? width = innerBorder?.ActualWidth;

            if (width == null) return;

            double right = left + width.Value;
            double top = canvas.PointToScreen(new Point(0.0, Canvas.GetTop(innerBorder))).Y;
            double? height = innerBorder?.ActualHeight;

            if (height == null) return;

            double bottom = top + height.Value;

            Rect canvasOnScreen = new Rect(topLeft.Value, bottomRight.Value);

            // verify the user didn't drag the message box outside the view port, and if they did, move it a little bit back into view
            if (right <= canvasOnScreen.Left)
            {
                Canvas.SetLeft(innerBorder, canvas.PointFromScreen(new Point(canvasOnScreen.Left + 100 - width.Value, 0)).X);
            }

            if (left >= canvasOnScreen.Right)
            {
                Canvas.SetLeft(innerBorder, canvas.PointFromScreen(new Point(canvasOnScreen.Right - 100, 0)).X);
            }

            if (top <= canvasOnScreen.Top)
            {
                Canvas.SetTop(innerBorder, canvas.PointFromScreen(new Point(0, canvasOnScreen.Top)).Y);
            }

            if (bottom >= canvasOnScreen.Bottom)
            {
                Canvas.SetTop(innerBorder, canvas.PointFromScreen(new Point(0, canvasOnScreen.Bottom - height.Value)).Y);
            }
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (innerBorder == null) return;

            double xAdjust = innerBorder.Width + e.HorizontalChange;

            if (double.IsNaN(xAdjust)) xAdjust = innerBorder.ActualWidth + e.HorizontalChange;

            double yAdjust = innerBorder.Height + e.VerticalChange;

            if (double.IsNaN(yAdjust)) yAdjust = innerBorder.ActualHeight + e.VerticalChange;

            if (xAdjust >= 0 && yAdjust >= 0)
            {
                // make sure we are within are min and max sizes
                if (xAdjust <= InputBoxMinWidth) xAdjust = InputBoxMinWidth;
                if (xAdjust >= InputBoxMaxWidth) xAdjust = InputBoxMaxWidth;

                if (yAdjust <= InputBoxMinHeight) yAdjust = InputBoxMinHeight;
                if (yAdjust >= InputBoxMaxHeight) yAdjust = InputBoxMaxHeight;

                innerBorder.Width = xAdjust;
                innerBorder.Height = yAdjust;

                // we are going to move the resizer too (bottom right of message box)
                double left = Canvas.GetLeft(innerBorder);
                double top = Canvas.GetTop(innerBorder);

                double newWidth = left + innerBorder.Width - 5;
                double newHeight = top + innerBorder.Height - 5;

                // make sure we can only drag to the minimum and maximum size of the message box
                if (innerBorder.Width <= InputBoxMinWidth)
                {
                    newWidth = left + InputBoxMinWidth - 5;
                }

                if (innerBorder.Width >= InputBoxMaxWidth)
                {
                    newWidth = left + InputBoxMaxWidth - 5;
                }

                if (innerBorder.Height <= InputBoxMinHeight)
                {
                    newHeight = top + InputBoxMinHeight - 5;
                }

                if (innerBorder.Height >= InputBoxMaxHeight)
                {
                    newHeight = top + InputBoxMaxHeight - 5;
                }

                Canvas.SetLeft(resizeThumbContainer, newWidth);
                Canvas.SetTop(resizeThumbContainer, newHeight);
            }
        }

        private void ResizeThumb_DragStarted(object sender, DragStartedEventArgs e)
        {
            // should we do something here?
        }

        private void ResizeThumb_DragCompleted(object sender, DragCompletedEventArgs e)
        {
            // should we do something here?
        }

        /// <summary>
        /// Validates min and max are not 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN or are less than 0.0.
        /// </summary>
        private void ValidateMinAndMax()
        {
            switch (InputBoxMaxHeight)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "MessageBoxMaxHeight");
            }

            if (InputBoxMaxHeight < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "MessageBoxMaxHeight");
            }

            switch (InputBoxMaxWidth)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "MessageBoxMaxWidth");
            }

            if (InputBoxMaxWidth < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "MessageBoxMaxWidth");
            }

            switch (InputBoxMinHeight)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "MessageBoxMinHeight");
            }

            if (InputBoxMinHeight < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "MessageBoxMinHeight");
            }

            switch (InputBoxMinWidth)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "MessageBoxMinWidth");
            }

            if (InputBoxMinWidth < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "MessageBoxMinWidth");
            }
        }

        #endregion
    }
}
