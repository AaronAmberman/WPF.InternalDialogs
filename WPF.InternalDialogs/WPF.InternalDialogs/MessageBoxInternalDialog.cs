﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
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

        private Canvas canvas;
        private Border innerBorder;
        private Button cancelButton;
        private Button closeButton;
        private Button okButton;
        private Button noButton;
        private Button yesButton;
        private Thumb titleThumb;
        private Grid resizeThumbContainer;
        private Thumb resizeThumb;

        private Size defaultSize = Size.Empty;
        private bool initialLayoutComplete;
        private bool isSizeAndPositionUserManaged;

        #endregion

        #region Properties

        /// <summary>Gets or sets the background for the button area.</summary>
        public Brush ButtonAreaBackground
        {
            get { return (Brush)GetValue(ButtonAreaBackgroundProperty); }
            set { SetValue(ButtonAreaBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ButtonAreaBackgroundProperty =
            DependencyProperty.Register("ButtonAreaBackground", typeof(Brush), typeof(MessageBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>gets or sets the style to use for the buttons in the message box.</summary>
        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(MessageBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the text to use for the Cancel button.</summary>
        public string ButtonTextCancel
        {
            get { return (string)GetValue(ButtonTextCancelProperty); }
            set { SetValue(ButtonTextCancelProperty, value); }
        }

        public static readonly DependencyProperty ButtonTextCancelProperty =
            DependencyProperty.Register("ButtonTextCancel", typeof(string), typeof(MessageBoxInternalDialog), new PropertyMetadata("Cancel"));

        /// <summary>Gets or sets the text to use for the No button.</summary>
        public string ButtonTextNo
        {
            get { return (string)GetValue(ButtonTextNoProperty); }
            set { SetValue(ButtonTextNoProperty, value); }
        }

        public static readonly DependencyProperty ButtonTextNoProperty =
            DependencyProperty.Register("ButtonTextNo", typeof(string), typeof(MessageBoxInternalDialog), new PropertyMetadata("No"));

        /// <summary>Gets or sets the text to use for the OK button.</summary>
        public string ButtonTextOk
        {
            get { return (string)GetValue(ButtonTextOkProperty); }
            set { SetValue(ButtonTextOkProperty, value); }
        }

        public static readonly DependencyProperty ButtonTextOkProperty =
            DependencyProperty.Register("ButtonTextOk", typeof(string), typeof(MessageBoxInternalDialog), new PropertyMetadata("OK"));

        /// <summary>Gets or sets the text to use for the Yes button.</summary>
        public string ButtonTextYes
        {
            get { return (string)GetValue(ButtonTextYesProperty); }
            set { SetValue(ButtonTextYesProperty, value); }
        }

        public static readonly DependencyProperty ButtonTextYesProperty =
            DependencyProperty.Register("ButtonTextYes", typeof(string), typeof(MessageBoxInternalDialog), new PropertyMetadata("Yes"));

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
        public Brush MessageBoxBackground
        {
            get { return (Brush)GetValue(MessageBoxBackgroundProperty); }
            set { SetValue(MessageBoxBackgroundProperty, value); }
        }

        public static readonly DependencyProperty MessageBoxBackgroundProperty =
            DependencyProperty.Register("MessageBoxBackground", typeof(Brush), typeof(MessageBoxInternalDialog), new PropertyMetadata(null));

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

        /// <summary>Gets or sets the default height of the resizable portion.</summary>
        public double MessageBoxDefaultHeight
        {
            get { return (double)GetValue(MessageBoxDefaultHeightProperty); }
            set { SetValue(MessageBoxDefaultHeightProperty, value); }
        }

        public static readonly DependencyProperty MessageBoxDefaultHeightProperty =
            DependencyProperty.Register("MessageBoxDefaultHeight", typeof(double), typeof(MovableResizableInternalDialog), new PropertyMetadata(double.NaN));

        /// <summary>Gets or sets the default width of the resizable portion.</summary>
        public double MessageBoxDefaultWidth
        {
            get { return (double)GetValue(MessageBoxDefaultWidthProperty); }
            set { SetValue(MessageBoxDefaultWidthProperty, value); }
        }

        public static readonly DependencyProperty MessageBoxDefaultWidthProperty =
            DependencyProperty.Register("MessageBoxDefaultWidth", typeof(double), typeof(MovableResizableInternalDialog), new PropertyMetadata(double.NaN));

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

        /// <summary>Gets or sets the content for the resize grip.</summary>
        /// <remarks>
        /// Resize Grip is 18x18 and the top left slightly overlays the bottom right of the resizable area. Plan your visuals accordingly. There 
        /// is also sometihng to know, the opacity for the whole resize grip area is .8 or 80% and on mouse over goes to 1.0 or 100%. This is so 
        /// we can generically achieve a mouse over look. Plan your visuals accordingly.
        /// </remarks>
        public object ResizeGripContent
        {
            get { return (object)GetValue(ResizeGripContentProperty); }
            set { SetValue(ResizeGripContentProperty, value); }
        }

        public static readonly DependencyProperty ResizeGripContentProperty =
            DependencyProperty.Register("ResizeGripContent", typeof(object), typeof(MessageBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the cursor for the resize gripper. Default is Cursors.NWSE.</summary>
        public Cursor ResizeGripCursor
        {
            get { return (Cursor)GetValue(ResizeGripCursorProperty); }
            set { SetValue(ResizeGripCursorProperty, value); }
        }

        public static readonly DependencyProperty ResizeGripCursorProperty =
            DependencyProperty.Register("ResizeGripCursor", typeof(Cursor), typeof(MessageBoxInternalDialog), new PropertyMetadata(Cursors.SizeNWSE));

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
        public Brush TitleAreaBackground
        {
            get { return (Brush)GetValue(TitleAreaBackgroundProperty); }
            set { SetValue(TitleAreaBackgroundProperty, value); }
        }

        public static readonly DependencyProperty TitleAreaBackgroundProperty =
            DependencyProperty.Register("TitleAreaBackground", typeof(Brush), typeof(MessageBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the cursor for the title area. Default is Cursors.SizeAll.</summary>
        public Cursor TitleCursor
        {
            get { return (Cursor)GetValue(TitleCursorProperty); }
            set { SetValue(TitleCursorProperty, value); }
        }

        public static readonly DependencyProperty TitleCursorProperty =
            DependencyProperty.Register("TitleCursor", typeof(Cursor), typeof(MessageBoxInternalDialog), new PropertyMetadata(Cursors.SizeAll));

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
            VisibilityProperty.OverrideMetadata(typeof(MessageBoxInternalDialog), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, VisibilityChangedCallback));
        }

        public MessageBoxInternalDialog()
        {
            LayoutUpdated += MessageBoxInternalDialog_LayoutUpdated;
            SizeChanged += MessageBoxInternalDialog_SizeChanged;
        }

        #endregion

        #region Methods

        private void MessageBoxInternalDialog_LayoutUpdated(object sender, EventArgs e)
        {
            if (canvas == null) return;
            if (innerBorder == null) return;
            if (Visibility == Visibility.Collapsed) return;

            if (!initialLayoutComplete)
            {
                PlaceDialogInitially();

                initialLayoutComplete = true;
            }

            PlaceResizer();
        }

        private void MessageBoxInternalDialog_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (canvas == null) return;
            if (innerBorder == null) return;
            if (Visibility == Visibility.Collapsed) return;
            if (!initialLayoutComplete) return;

            // if the user changed the position or size manually then no more automatic management of the dialog
            if (isSizeAndPositionUserManaged) EnsureInBounds();
            else SizeContent();
        }

        new private static void VisibilityChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MessageBoxInternalDialog instance = d as MessageBoxInternalDialog;

            if (instance == null) return;

            Visibility visibility = (Visibility)e.NewValue;

            // we will have Visible and Collapsed states, no Hidden
            if (visibility == Visibility.Hidden)
            {
                instance.SetValue(VisibilityProperty, Visibility.Collapsed);

                // kick out and let us setting the new value make the below logic run,
                // we'll leave our callback (this if will not be hit next callback)
                return;
            }

            if (visibility == Visibility.Visible)
            {
                instance.ValidateMinAndMax();
            }
            else // Collapsed
            {
                instance.initialLayoutComplete = false;
                instance.isSizeAndPositionUserManaged = false;

                // make sure reset the size of the message (fixes custom size set from user dragging)
                if (instance.innerBorder != null)
                {
                    instance.innerBorder.Width = double.NaN;
                    instance.innerBorder.Height = double.NaN;
                }
            }
        }

        private void PlaceDialogInitially()
        {
            double x = 0;
            double y = 0;
            double width = double.IsNaN(MessageBoxDefaultWidth) ? innerBorder.ActualWidth : MessageBoxDefaultWidth;
            double height = double.IsNaN(MessageBoxDefaultHeight) ? innerBorder.ActualHeight : MessageBoxDefaultHeight;

            defaultSize = new Size(width, height);

            // respect max
            if (HorizontalContentAlignment == HorizontalAlignment.Stretch)
                width = MessageBoxMaxWidth;

            // make sure we fit
            if (width > canvas.ActualWidth)
                width = canvas.ActualWidth - Padding.Left - Padding.Right;

            // make sure we aren't smaller than minimum
            if (width < MessageBoxMinWidth)
                width = MessageBoxMinWidth;

            if (HorizontalContentAlignment == HorizontalAlignment.Left)
                x = 0;
            else if (HorizontalContentAlignment == HorizontalAlignment.Right)
                x = canvas.ActualWidth - width - Padding.Right - Padding.Left;
            else if (HorizontalContentAlignment == HorizontalAlignment.Center ||
                     HorizontalContentAlignment == HorizontalAlignment.Stretch)
                x = (canvas.ActualWidth / 2) - (width / 2) - Padding.Left;

            if (x < 0) x = 0;

            // respect max
            if (VerticalContentAlignment == VerticalAlignment.Stretch)
                height = MessageBoxMaxHeight;

            // make sure we fit
            if (height > canvas.ActualHeight)
                height = canvas.ActualHeight - Padding.Left - Padding.Right;

            // make sure we aren't smaller than minimum
            if (height < MessageBoxMinHeight)
                height = MessageBoxMinHeight;

            if (VerticalContentAlignment == VerticalAlignment.Top)
                y = 0;
            else if (VerticalContentAlignment == VerticalAlignment.Bottom)
                y = canvas.ActualHeight - height - Padding.Bottom - Padding.Top;
            else if (VerticalContentAlignment == VerticalAlignment.Center ||
                     VerticalContentAlignment == VerticalAlignment.Stretch)
                y = (canvas.ActualHeight / 2) - (height / 2) - Padding.Top;

            if (y < 0) y = 0;

            innerBorder.Width = width;
            innerBorder.Height = height;

            Canvas.SetLeft(innerBorder, x);
            Canvas.SetTop(innerBorder, y);
        }

        private void EnsureInBounds()
        {
            double left = Canvas.GetLeft(innerBorder);
            double top = Canvas.GetTop(innerBorder);
            double right = left + innerBorder.ActualWidth + Padding.Left + Padding.Right;
            double bottom = top + innerBorder.ActualHeight + Padding.Top + Padding.Bottom;

            if (right > canvas.ActualWidth)
            {
                double newLeft = canvas.ActualWidth - innerBorder.ActualWidth - Padding.Left - Padding.Right;

                // make sure we aren't pushing left out of view
                if (newLeft < 0) newLeft = 0;

                Canvas.SetLeft(innerBorder, newLeft);
            }

            if (bottom > canvas.ActualHeight)
            {
                double newTop = canvas.ActualHeight - innerBorder.ActualHeight - Padding.Top - Padding.Bottom;

                // make sure we aren't pushing up out of view
                if (newTop < 0) newTop = 0;

                Canvas.SetTop(innerBorder, newTop);
            }
        }

        private void PlaceResizer()
        {
            double updatedX = Canvas.GetLeft(innerBorder);
            double updatedY = Canvas.GetTop(innerBorder);

            double width = innerBorder.ActualWidth;
            double height = innerBorder.ActualHeight;

            double resizerX = updatedX + width + Padding.Left - 5;
            double resizerY = updatedY + height + Padding.Top - 5;

            Canvas.SetLeft(resizeThumbContainer, resizerX);
            Canvas.SetTop(resizeThumbContainer, resizerY);
        }

        private void SizeContent()
        {
            double x = Canvas.GetLeft(innerBorder);
            double y = Canvas.GetTop(innerBorder);
            double width = canvas.ActualWidth - Padding.Left - Padding.Right;
            double height = canvas.ActualHeight - Padding.Top - Padding.Bottom;

            // make sure we fit
            if (width > canvas.ActualWidth) width = canvas.ActualWidth - Padding.Left - Padding.Right;
            if (width < MessageBoxMinWidth) width = MessageBoxMinWidth;

            if (HorizontalContentAlignment == HorizontalAlignment.Left)
            {
                if (width > defaultSize.Width) width = defaultSize.Width;
            }
            if (HorizontalContentAlignment == HorizontalAlignment.Right)
            {
                if (width > defaultSize.Width) width = defaultSize.Width;

                x = canvas.ActualWidth - width - Padding.Left - Padding.Right;
            }
            else if (HorizontalContentAlignment == HorizontalAlignment.Center)
            {
                if (width > defaultSize.Width) width = defaultSize.Width;

                x = (canvas.ActualWidth / 2) - (width / 2) - Padding.Left;
            }
            else if (HorizontalContentAlignment == HorizontalAlignment.Stretch)
            {
                // MessageBoxDefaultWidth is ignored if stretch, MessageBoxMaxWidth is not
                width = MessageBoxMaxWidth;

                // make sure the border fits into the canvas
                if (width + Padding.Left + Padding.Right > canvas.ActualWidth)
                    width = canvas.ActualWidth - Padding.Left - Padding.Right;

                x = (canvas.ActualWidth / 2) - (width / 2) - Padding.Left;
            }

            // ensure 0 at a minimum
            if (width >= canvas.ActualWidth - Padding.Left - Padding.Right)
                x = 0;

            // make sure we fit
            if (height > canvas.ActualHeight) height = canvas.ActualHeight - Padding.Top - Padding.Bottom;
            if (height < MessageBoxMinHeight) height = MessageBoxMinHeight;

            if (VerticalContentAlignment == VerticalAlignment.Top)
            {
                if (height > defaultSize.Height) height = defaultSize.Height;
            }
            if (VerticalContentAlignment == VerticalAlignment.Bottom)
            {
                if (height > defaultSize.Height) height = defaultSize.Height;

                y = canvas.ActualHeight - height - Padding.Top - Padding.Bottom;
            }
            else if (VerticalContentAlignment == VerticalAlignment.Center)
            {
                if (height > defaultSize.Height) height = defaultSize.Height;

                y = (canvas.ActualHeight / 2) - (height / 2) - Padding.Top;
            }
            else if (VerticalContentAlignment == VerticalAlignment.Stretch)
            {
                // MessageBoxDefaultHeight is ignored if stretch, MessageBoxMaxHeight is not
                height = MessageBoxMaxHeight;

                // make sure the border fits into the canvas
                if (height + Padding.Top + Padding.Bottom > canvas.ActualHeight)
                    height = canvas.ActualHeight - Padding.Top - Padding.Bottom;

                y = (canvas.ActualHeight / 2) - (height / 2) - Padding.Top;
            }

            // ensure 0 at a minimum
            if (height >= canvas.ActualHeight - Padding.Top - Padding.Bottom)
                y = 0;

            innerBorder.Width = width;
            innerBorder.Height = height;

            Canvas.SetLeft(innerBorder, x);
            Canvas.SetTop(innerBorder, y);
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
                titleThumb.DragDelta += TitleThumb_DragDelta;

            resizeThumbContainer = GetTemplateChild("PART_ResizeThumbContainer") as Grid;
            resizeThumb = GetTemplateChild("PART_ResizeThumb") as Thumb;

            if (resizeThumb != null)
                resizeThumb.DragDelta += ResizeThumb_DragDelta;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            SetValue(ResultProperty, MessageBoxResult.Cancel);
            SetValue(VisibilityProperty, Visibility.Collapsed);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            SetValue(ResultProperty, MessageBoxResult.Cancel);
            SetValue(VisibilityProperty, Visibility.Collapsed);
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            SetValue(ResultProperty, MessageBoxResult.OK);
            SetValue(VisibilityProperty, Visibility.Collapsed);
        }

        private void NoButton_Click(object sender, RoutedEventArgs e)
        {
            SetValue(ResultProperty, MessageBoxResult.No);
            SetValue(VisibilityProperty, Visibility.Collapsed);
        }

        private void YesButton_Click(object sender, RoutedEventArgs e)
        {
            SetValue(ResultProperty, MessageBoxResult.Yes);
            SetValue(VisibilityProperty, Visibility.Collapsed);
        }

        private void TitleThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (canvas == null) return;
            if (innerBorder == null) return;

            isSizeAndPositionUserManaged = true;

            double newX = Canvas.GetLeft(innerBorder) + e.HorizontalChange;
            double newY = Canvas.GetTop(innerBorder) + e.VerticalChange;

            if (newX > 0 && newX < canvas.ActualWidth - innerBorder.ActualWidth - Padding.Right - Padding.Left)
                Canvas.SetLeft(innerBorder, newX);

            if (newY > 0 && newY < canvas.ActualHeight - innerBorder.ActualHeight - Padding.Bottom - Padding.Top)
                Canvas.SetTop(innerBorder, newY);
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (canvas == null) return;
            if (innerBorder == null) return;

            isSizeAndPositionUserManaged = true;

            double xAdjust = innerBorder.ActualWidth + e.HorizontalChange;
            double yAdjust = innerBorder.ActualHeight + e.VerticalChange;

            if (xAdjust >= 0 && yAdjust >= 0)
            {
                // make sure we are within are min and max sizes
                if (xAdjust <= MessageBoxMinWidth) xAdjust = MessageBoxMinWidth;
                if (xAdjust >= MessageBoxMaxWidth) xAdjust = MessageBoxMaxWidth;

                if (yAdjust <= MessageBoxMinHeight) yAdjust = MessageBoxMinHeight;
                if (yAdjust >= MessageBoxMaxHeight) yAdjust = MessageBoxMaxHeight;

                double x = Canvas.GetLeft(innerBorder);
                double y = Canvas.GetTop(innerBorder);

                // make sure we are within the canvas area as well, no dragging off screen
                double totalX = x + xAdjust + Padding.Left + Padding.Right;
                double totalY = y + yAdjust + Padding.Top + Padding.Bottom;

                if (totalX > canvas.ActualWidth)
                    xAdjust = canvas.ActualWidth - x - Padding.Left - Padding.Right;

                if (totalY > canvas.ActualHeight)
                    yAdjust = canvas.ActualHeight - y - Padding.Top - Padding.Bottom;

                innerBorder.Width = xAdjust;
                innerBorder.Height = yAdjust;

                PlaceResizer();
            }
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
