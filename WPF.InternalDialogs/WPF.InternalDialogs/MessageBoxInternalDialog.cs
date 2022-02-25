﻿using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WPF.InternalDialogs
{
    /// <summary>An internal dialog message box that is very similar to the normal message box. This class cannot be inherited.</summary>
    [TemplatePart(Name = "PART_Canvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_InnerBorder", Type = typeof(Border))]
    [TemplatePart(Name = "PART_CancelButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_CloseButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_OkButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_NoButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_YesButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_TitleThumb", Type = typeof(Thumb))]
    [TemplatePart(Name = "PART_ResizeThumbContainer", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_ResizeThumb", Type = typeof(Thumb))]
    public sealed class MessageBoxInternalDialog : InternalDialog
    {
        #region Fields

        private Canvas? canvas;
        private Border? innerBorder;
        private Button? cancelButton;
        private Button? closeButton;
        private Button? okButton;
        private Button? noButton;
        private Button? yesButton;
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
            DependencyProperty.Register("ButtonAreaBackground", typeof(SolidColorBrush), typeof(MessageBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>gets or sets the style to use for the buttons in the message box.</summary>
        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(MessageBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>gets or sets the style to use for the close button at the top right.</summary>
        public Style CloseButtonStyle
        {
            get { return (Style)GetValue(CloseButtonStyleProperty); }
            set { SetValue(CloseButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty CloseButtonStyleProperty =
            DependencyProperty.Register("CloseButtonStyle", typeof(Style), typeof(MessageBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the message to display in the dialog.</summary>
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(MessageBoxInternalDialog), new PropertyMetadata(string.Empty));

        /// <summary>Gets or sets the background for the message box part of the message box internal dialog. Not the same as Background.</summary>
        public SolidColorBrush MessageBoxBackground
        {
            get { return (SolidColorBrush)GetValue(MessageBoxBackgroundProperty); }
            set { SetValue(MessageBoxBackgroundProperty, value); }
        }

        public static readonly DependencyProperty MessageBoxBackgroundProperty =
            DependencyProperty.Register("MessageBoxBackground", typeof(SolidColorBrush), typeof(MessageBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the message box buttons shown.</summary>
        public MessageBoxButton MessageBoxButton
        {
            get { return (MessageBoxButton)GetValue(MessageBoxButtonProperty); }
            set { SetValue(MessageBoxButtonProperty, value); }
        }

        public static readonly DependencyProperty MessageBoxButtonProperty =
            DependencyProperty.Register("MessageBoxButton", typeof(MessageBoxButton), typeof(MessageBoxInternalDialog), new PropertyMetadata(MessageBoxButton.OK));

        /// <summary>Gets or sets the image for the message.</summary>
        public MessageBoxInternalDialogImage MessageBoxImage
        {
            get { return (MessageBoxInternalDialogImage)GetValue(MessageBoxImageProperty); }
            set { SetValue(MessageBoxImageProperty, value); }
        }

        public static readonly DependencyProperty MessageBoxImageProperty =
            DependencyProperty.Register("MessageBoxImage", typeof(MessageBoxInternalDialogImage), typeof(MessageBoxInternalDialog), 
                new PropertyMetadata(MessageBoxInternalDialogImage.None));

        /// <summary>Gets or sets the message box maximum height. Default is 600.0.</summary>
        public double MessageBoxMaxHeight
        {
            get { return (double)GetValue(MessageBoxMaxHeightProperty); }
            set { SetValue(MessageBoxMaxHeightProperty, value); }
        }

        public static readonly DependencyProperty MessageBoxMaxHeightProperty =
            DependencyProperty.Register("MessageBoxMaxHeight", typeof(double), typeof(MessageBoxInternalDialog), new PropertyMetadata(600.0));

        /// <summary>Gets or sets the message box maximum width. Default is 800.0</summary>
        public double MessageBoxMaxWidth
        {
            get { return (double)GetValue(MessageBoxMaxWidthProperty); }
            set { SetValue(MessageBoxMaxWidthProperty, value); }
        }

        public static readonly DependencyProperty MessageBoxMaxWidthProperty =
            DependencyProperty.Register("MessageBoxMaxWidth", typeof(double), typeof(MessageBoxInternalDialog), new PropertyMetadata(800.0));

        /// <summary>Gets or sets the message box minimum height. Default is 50.0.</summary>
        public double MessageBoxMinHeight
        {
            get { return (double)GetValue(MessageBoxMinHeightProperty); }
            set { SetValue(MessageBoxMinHeightProperty, value); }
        }

        public static readonly DependencyProperty MessageBoxMinHeightProperty =
            DependencyProperty.Register("MessageBoxMinHeight", typeof(double), typeof(MessageBoxInternalDialog), new PropertyMetadata(50.0));

        /// <summary>Gets or sets the message box minimum width. default is 100.0.</summary>
        public double MessageBoxMinWidth
        {
            get { return (double)GetValue(MessageBoxMinWidthProperty); }
            set { SetValue(MessageBoxMinWidthProperty, value); }
        }

        public static readonly DependencyProperty MessageBoxMinWidthProperty =
            DependencyProperty.Register("MessageBoxMinWidth", typeof(double), typeof(MessageBoxInternalDialog), new PropertyMetadata(100.0));

        /// <summary>Gets or sets the brush for the gripper section at the bottom right.</summary>
        public SolidColorBrush ResizeGripBrush
        {
            get { return (SolidColorBrush)GetValue(ResizeGripBrushProperty); }
            set { SetValue(ResizeGripBrushProperty, value); }
        }

        public static readonly DependencyProperty ResizeGripBrushProperty =
            DependencyProperty.Register("ResizeGripBrush", typeof(SolidColorBrush), typeof(MessageBoxInternalDialog), new PropertyMetadata(Brushes.White));

        /// <summary>Gets or sets the visibility of the resize grip. Visible = resizing enabled, Collapsed/Hidden = resizing disabled.</summary>
        public Visibility ResizeGripVisibility
        {
            get { return (Visibility)GetValue(ResizeGripVisibilityProperty); }
            set { SetValue(ResizeGripVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ResizeGripVisibilityProperty =
            DependencyProperty.Register("ResizeGripVisibility", typeof(Visibility), typeof(MessageBoxInternalDialog), new PropertyMetadata(Visibility.Visible));

        /// <summary>Gets or sets the title to the message box.</summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(MessageBoxInternalDialog), new PropertyMetadata(string.Empty));

        /// <summary>Gets or sets the background for the title area.</summary>
        public SolidColorBrush TitleAreaBackground
        {
            get { return (SolidColorBrush)GetValue(TitleAreaBackgroundProperty); }
            set { SetValue(TitleAreaBackgroundProperty, value); }
        }

        public static readonly DependencyProperty TitleAreaBackgroundProperty =
            DependencyProperty.Register("TitleAreaBackground", typeof(SolidColorBrush), typeof(MessageBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the horizontal alignment of the title.</summary>
        public HorizontalAlignment TitleHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(TitleHorizontalAlignmentProperty); }
            set { SetValue(TitleHorizontalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty TitleHorizontalAlignmentProperty =
            DependencyProperty.Register("TitleHorizontalAlignment", typeof(HorizontalAlignment), typeof(MessageBoxInternalDialog), 
                new PropertyMetadata(HorizontalAlignment.Left));

        #endregion

        #region Constructors

        static MessageBoxInternalDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageBoxInternalDialog), new FrameworkPropertyMetadata(typeof(MessageBoxInternalDialog)));
            VisibilityProperty.OverrideMetadata(typeof(MessageBoxInternalDialog), new FrameworkPropertyMetadata(Visibility.Collapsed, VisibilityChangedCallback));
        }

        public MessageBoxInternalDialog()
        {
            LayoutUpdated += MessageBoxInternalDialog_LayoutUpdated;
        }

        private void MessageBoxInternalDialog_LayoutUpdated(object? sender, EventArgs e)
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
             * If the window containing the MessageBoxInternalDialog moved monitors before the 
             * MessageBoxInternalDialog is shown for the first time then it always starts at 
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
            MessageBoxInternalDialog? instance = d as MessageBoxInternalDialog;

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
            double resizerX = centerX + messageBoxWidth - 10;
            double resizerY = centerY + messageBoxHeight - 10;

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
            noButton = GetTemplateChild("PART_NoButton") as Button;
            yesButton = GetTemplateChild("PART_YesButton") as Button;

            if (cancelButton != null)
                cancelButton.Click += CancelButton_Click;

            if (closeButton != null)
                closeButton.Click += CloseButton_Click;

            if (okButton != null)
                okButton.Click += OkButton_Click;

            if (noButton != null)
                noButton.Click += NoButton_Click;

            if (yesButton != null)
                yesButton.Click += YesButton_Click;

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
        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.No;
            Visibility = Visibility.Collapsed;
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Yes;
            Visibility = Visibility.Collapsed;
        }

        private void TitleThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            double yadjust = e.VerticalChange;
            double xadjust = e.HorizontalChange;

            if (innerBorder != null)
            {
                Canvas.SetLeft(innerBorder, Canvas.GetLeft(innerBorder) + e.HorizontalChange);
                Canvas.SetTop(innerBorder, Canvas.GetTop(innerBorder) + e.VerticalChange);

                // we are going to move the resizer too (bottom right of message box)
                double updatedX = Canvas.GetLeft(innerBorder);
                double updatedY = Canvas.GetTop(innerBorder);

                double width = innerBorder.ActualWidth;
                double height = innerBorder.ActualHeight;

                double resizerX = updatedX + width - 10;
                double resizerY = updatedY + height - 10;

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
                if (xAdjust <= MessageBoxMinWidth) xAdjust = MessageBoxMinWidth;
                if (xAdjust >= MessageBoxMaxWidth) xAdjust = MessageBoxMaxWidth;

                if (yAdjust <= MessageBoxMinHeight) yAdjust = MessageBoxMinHeight;
                if (yAdjust >= MessageBoxMaxHeight) yAdjust = MessageBoxMaxHeight;

                innerBorder.Width = xAdjust;
                innerBorder.Height = yAdjust;

                // we are going to move the resizer too (bottom right of message box)
                double left = Canvas.GetLeft(innerBorder);
                double top = Canvas.GetTop(innerBorder);

                double newWidth = left + innerBorder.Width - 10;
                double newHeight = top + innerBorder.Height - 10;

                // make sure we can only drag to the minimum and maximum size of the message box
                if (innerBorder.Width <= MessageBoxMinWidth)
                {
                    newWidth = left + MessageBoxMinWidth - 10;
                }

                if (innerBorder.Width >= MessageBoxMaxWidth)
                {
                    newWidth = left + MessageBoxMaxWidth - 10;
                }

                if (innerBorder.Height <= MessageBoxMinHeight)
                {
                    newHeight = top + MessageBoxMinHeight - 10;
                }

                if (innerBorder.Height >= MessageBoxMaxHeight)
                {
                    newHeight = top + MessageBoxMaxHeight - 10;
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
            switch (MessageBoxMaxHeight)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "MessageBoxMaxHeight");
            }

            if (MessageBoxMaxHeight < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "MessageBoxMaxHeight");
            }

            switch (MessageBoxMaxWidth)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "MessageBoxMaxWidth");
            }

            if (MessageBoxMaxWidth < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "MessageBoxMaxWidth");
            }

            switch (MessageBoxMinHeight)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "MessageBoxMinHeight");
            }

            if (MessageBoxMinHeight < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "MessageBoxMinHeight");
            }

            switch (MessageBoxMinWidth)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "MessageBoxMinWidth");
            }

            if (MessageBoxMinWidth < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "MessageBoxMinWidth");
            }
        }

        #endregion
    }
}
