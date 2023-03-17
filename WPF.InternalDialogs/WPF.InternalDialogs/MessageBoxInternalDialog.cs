using System;
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

        private bool initialLayoutComplete;

        #endregion

        #region Properties

        /// <summary>Gets or sets the background for the button area.</summary>
        public SolidColorBrush ButtonAreaBackground
        {
            get 
            {
                VerifyDisposed();

                return (SolidColorBrush)GetValue(ButtonAreaBackgroundProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(ButtonAreaBackgroundProperty, value);
            }
        }

        public static readonly DependencyProperty ButtonAreaBackgroundProperty =
            DependencyProperty.Register("ButtonAreaBackground", typeof(SolidColorBrush), typeof(MessageBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>gets or sets the style to use for the buttons in the message box.</summary>
        public Style ButtonStyle
        {
            get
            {
                VerifyDisposed();

                return (Style)GetValue(ButtonStyleProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(ButtonStyleProperty, value);
            }
        }

        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(MessageBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the text to use for the Cancel button.</summary>
        public string ButtonTextCancel
        {
            get 
            {
                VerifyDisposed();

                return (string)GetValue(ButtonTextCancelProperty);
            }
            set 
            {
                VerifyDisposed();

                SetValue(ButtonTextCancelProperty, value);
            }
        }

        public static readonly DependencyProperty ButtonTextCancelProperty =
            DependencyProperty.Register("ButtonTextCancel", typeof(string), typeof(MessageBoxInternalDialog), new PropertyMetadata("Cancel"));

        /// <summary>Gets or sets the text to use for the No button.</summary>
        public string ButtonTextNo
        {
            get
            {
                VerifyDisposed();

                return (string)GetValue(ButtonTextNoProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(ButtonTextNoProperty, value);
            }
        }

        public static readonly DependencyProperty ButtonTextNoProperty =
            DependencyProperty.Register("ButtonTextNo", typeof(string), typeof(MessageBoxInternalDialog), new PropertyMetadata("No"));

        /// <summary>Gets or sets the text to use for the OK button.</summary>
        public string ButtonTextOk
        {
            get 
            {
                VerifyDisposed();

                return (string)GetValue(ButtonTextOkProperty);
            }
            set 
            {
                VerifyDisposed();

                SetValue(ButtonTextOkProperty, value); 
            }
        }

        public static readonly DependencyProperty ButtonTextOkProperty =
            DependencyProperty.Register("ButtonTextOk", typeof(string), typeof(MessageBoxInternalDialog), new PropertyMetadata("OK"));

        /// <summary>Gets or sets the text to use for the Yes button.</summary>
        public string ButtonTextYes
        {
            get 
            {
                VerifyDisposed();

                return (string)GetValue(ButtonTextYesProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(ButtonTextYesProperty, value);
            }
        }

        public static readonly DependencyProperty ButtonTextYesProperty =
            DependencyProperty.Register("ButtonTextYes", typeof(string), typeof(MessageBoxInternalDialog), new PropertyMetadata("Yes"));

        /// <summary>gets or sets the style to use for the close button at the top right.</summary>
        public Style CloseButtonStyle
        {
            get 
            {
                VerifyDisposed();

                return (Style)GetValue(CloseButtonStyleProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(CloseButtonStyleProperty, value);
            }
        }

        public static readonly DependencyProperty CloseButtonStyleProperty =
            DependencyProperty.Register("CloseButtonStyle", typeof(Style), typeof(MessageBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets whether or not the dialog is re-centered when the container is resized (not the dialog itself).</summary>
        public bool KeepDialogCenteredOnContainerResize
        {
            get
            {
                VerifyDisposed();

                return (bool)GetValue(KeepDialogCenteredOnContainerResizeProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(KeepDialogCenteredOnContainerResizeProperty, value);
            }
        }

        public static readonly DependencyProperty KeepDialogCenteredOnContainerResizeProperty =
            DependencyProperty.Register("KeepDialogCenteredOnContainerResize", typeof(bool), typeof(MessageBoxInternalDialog), new PropertyMetadata(false));

        /// <summary>Gets or sets the message to display in the dialog.</summary>
        public string Message
        {
            get 
            {
                VerifyDisposed();

                return (string)GetValue(MessageProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(MessageProperty, value);
            }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(MessageBoxInternalDialog), new PropertyMetadata(string.Empty));

        /// <summary>Gets or sets the background for the message box part of the message box internal dialog. Not the same as Background.</summary>
        public SolidColorBrush MessageBoxBackground
        {
            get 
            {
                VerifyDisposed();

                return (SolidColorBrush)GetValue(MessageBoxBackgroundProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(MessageBoxBackgroundProperty, value);
            }
        }

        public static readonly DependencyProperty MessageBoxBackgroundProperty =
            DependencyProperty.Register("MessageBoxBackground", typeof(SolidColorBrush), typeof(MessageBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the message box buttons shown.</summary>
        public MessageBoxButton MessageBoxButton
        {
            get 
            {
                VerifyDisposed();

                return (MessageBoxButton)GetValue(MessageBoxButtonProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(MessageBoxButtonProperty, value);
            }
        }

        public static readonly DependencyProperty MessageBoxButtonProperty =
            DependencyProperty.Register("MessageBoxButton", typeof(MessageBoxButton), typeof(MessageBoxInternalDialog), new PropertyMetadata(MessageBoxButton.OK));

        /// <summary>Gets or sets the image for the message.</summary>
        public MessageBoxInternalDialogImage MessageBoxImage
        {
            get 
            {
                VerifyDisposed();

                return (MessageBoxInternalDialogImage)GetValue(MessageBoxImageProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(MessageBoxImageProperty, value);
            }
        }

        public static readonly DependencyProperty MessageBoxImageProperty =
            DependencyProperty.Register("MessageBoxImage", typeof(MessageBoxInternalDialogImage), typeof(MessageBoxInternalDialog),
                new PropertyMetadata(MessageBoxInternalDialogImage.None));

        /// <summary>Gets or sets the default height of the resizable portion.</summary>
        public double MessageBoxDefaultHeight
        {
            get 
            {
                VerifyDisposed();

                return (double)GetValue(MessageBoxDefaultHeightProperty); 
            }
            set 
            {
                VerifyDisposed();

                SetValue(MessageBoxDefaultHeightProperty, value);
            }
        }

        public static readonly DependencyProperty MessageBoxDefaultHeightProperty =
            DependencyProperty.Register("MessageBoxDefaultHeight", typeof(double), typeof(MovableResizableInternalDialog), new PropertyMetadata(double.NaN));

        /// <summary>Gets or sets the default width of the resizable portion.</summary>
        public double MessageBoxDefaultWidth
        {
            get 
            {
                VerifyDisposed();

                return (double)GetValue(MessageBoxDefaultWidthProperty); 
            }
            set 
            {
                VerifyDisposed();

                SetValue(MessageBoxDefaultWidthProperty, value);
            }
        }

        public static readonly DependencyProperty MessageBoxDefaultWidthProperty =
            DependencyProperty.Register("MessageBoxDefaultWidth", typeof(double), typeof(MovableResizableInternalDialog), new PropertyMetadata(double.NaN));

        /// <summary>Gets or sets the message box maximum height. Default is 600.0.</summary>
        public double MessageBoxMaxHeight
        {
            get 
            {
                VerifyDisposed();

                return (double)GetValue(MessageBoxMaxHeightProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(MessageBoxMaxHeightProperty, value);
            }
        }

        public static readonly DependencyProperty MessageBoxMaxHeightProperty =
            DependencyProperty.Register("MessageBoxMaxHeight", typeof(double), typeof(MessageBoxInternalDialog), new PropertyMetadata(600.0));

        /// <summary>Gets or sets the message box maximum width. Default is 800.0</summary>
        public double MessageBoxMaxWidth
        {
            get 
            {
                VerifyDisposed();

                return (double)GetValue(MessageBoxMaxWidthProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(MessageBoxMaxWidthProperty, value);
            }
        }

        public static readonly DependencyProperty MessageBoxMaxWidthProperty =
            DependencyProperty.Register("MessageBoxMaxWidth", typeof(double), typeof(MessageBoxInternalDialog), new PropertyMetadata(800.0));

        /// <summary>Gets or sets the message box minimum height. Default is 50.0.</summary>
        public double MessageBoxMinHeight
        {
            get 
            {
                VerifyDisposed();

                return (double)GetValue(MessageBoxMinHeightProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(MessageBoxMinHeightProperty, value);
            }
        }

        public static readonly DependencyProperty MessageBoxMinHeightProperty =
            DependencyProperty.Register("MessageBoxMinHeight", typeof(double), typeof(MessageBoxInternalDialog), new PropertyMetadata(50.0));

        /// <summary>Gets or sets the message box minimum width. default is 100.0.</summary>
        public double MessageBoxMinWidth
        {
            get 
            {
                VerifyDisposed();

                return (double)GetValue(MessageBoxMinWidthProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(MessageBoxMinWidthProperty, value);
            }
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
            get 
            {
                VerifyDisposed();

                return (object)GetValue(ResizeGripContentProperty); 
            }
            set
            {
                VerifyDisposed();

                SetValue(ResizeGripContentProperty, value);
            }
        }

        public static readonly DependencyProperty ResizeGripContentProperty =
            DependencyProperty.Register("ResizeGripContent", typeof(object), typeof(MessageBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the cursor for the resize gripper. Default is Cursors.NWSE.</summary>
        public Cursor ResizeGripCursor
        {
            get 
            {
                VerifyDisposed();

                return (Cursor)GetValue(ResizeGripCursorProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(ResizeGripCursorProperty, value);
            }
        }

        public static readonly DependencyProperty ResizeGripCursorProperty =
            DependencyProperty.Register("ResizeGripCursor", typeof(Cursor), typeof(MessageBoxInternalDialog), new PropertyMetadata(Cursors.SizeNWSE));

        /// <summary>Gets or sets the visibility of the resize grip. Visible = resizing enabled, Collapsed/Hidden = resizing disabled.</summary>
        public Visibility ResizeGripVisibility
        {
            get 
            {
                VerifyDisposed();

                return (Visibility)GetValue(ResizeGripVisibilityProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(ResizeGripVisibilityProperty, value);
            }
        }

        public static readonly DependencyProperty ResizeGripVisibilityProperty =
            DependencyProperty.Register("ResizeGripVisibility", typeof(Visibility), typeof(MessageBoxInternalDialog), new PropertyMetadata(Visibility.Visible));

        /// <summary>Gets or sets the title to the message box.</summary>
        public string Title
        {
            get 
            {
                VerifyDisposed();

                return (string)GetValue(TitleProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(TitleProperty, value);
            }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(MessageBoxInternalDialog), new PropertyMetadata(string.Empty));

        /// <summary>Gets or sets the background for the title area.</summary>
        public SolidColorBrush TitleAreaBackground
        {
            get 
            {
                VerifyDisposed();

                return (SolidColorBrush)GetValue(TitleAreaBackgroundProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(TitleAreaBackgroundProperty, value);
            }
        }

        public static readonly DependencyProperty TitleAreaBackgroundProperty =
            DependencyProperty.Register("TitleAreaBackground", typeof(SolidColorBrush), typeof(MessageBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the cursor for the title area. Default is Cursors.SizeAll.</summary>
        public Cursor TitleCursor
        {
            get 
            {
                VerifyDisposed();

                return (Cursor)GetValue(TitleCursorProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(TitleCursorProperty, value);
            }
        }

        public static readonly DependencyProperty TitleCursorProperty =
            DependencyProperty.Register("TitleCursor", typeof(Cursor), typeof(MessageBoxInternalDialog), new PropertyMetadata(Cursors.SizeAll));

        /// <summary>Gets or sets the horizontal alignment of the title.</summary>
        public HorizontalAlignment TitleHorizontalAlignment
        {
            get 
            {
                VerifyDisposed();

                return (HorizontalAlignment)GetValue(TitleHorizontalAlignmentProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(TitleHorizontalAlignmentProperty, value);
            }
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
                CenterMessageBox();

                initialLayoutComplete = true;
            }

            PlaceResizer();
        }

        private void MessageBoxInternalDialog_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (canvas == null) return;
            if (innerBorder == null) return;
            if (Visibility == Visibility.Collapsed) return;

            if (initialLayoutComplete && KeepDialogCenteredOnContainerResize)
            {
                CenterMessageBox();

                return;
            }

            EnsureVisibility();
            SizeContent();
        }

        new private static void VisibilityChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MessageBoxInternalDialog instance = d as MessageBoxInternalDialog;

            if (instance == null) return;

            instance.VerifyDisposed();

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
            // set the inner border to the default size
            innerBorder.Width = MessageBoxDefaultWidth;
            innerBorder.Height = MessageBoxDefaultHeight;

            // center input box
            double totalWidth = canvas.ActualWidth;
            double totalHeight = canvas.ActualHeight;

            double messageBoxWidth = innerBorder.ActualWidth;
            double messageBoxHeight = innerBorder.ActualHeight;

            if (!double.IsNaN(MessageBoxDefaultHeight))
                messageBoxHeight = MessageBoxDefaultHeight;

            if (!double.IsNaN(MessageBoxDefaultWidth))
                messageBoxWidth = MessageBoxDefaultWidth;

            double centerX = (totalWidth / 2) - (messageBoxWidth / 2);
            double centerY = (totalHeight / 2) - (messageBoxHeight / 2);

            Canvas.SetLeft(innerBorder, centerX);
            Canvas.SetTop(innerBorder, centerY);
        }

        private void EnsureVisibility()
        {
            double left = Canvas.GetLeft(innerBorder);
            double top = Canvas.GetTop(innerBorder);

            Point borderTopLeft = canvas.PointToScreen(new Point(left, top));

            Rect borderOnScreen = new Rect(borderTopLeft, new Size(innerBorder.ActualWidth, innerBorder.ActualHeight));
            Rect canvasOnScreen = new Rect(canvas.PointToScreen(new Point(0, 0)), new Size(canvas.ActualWidth, canvas.ActualHeight));

            // we only need to manage pushing the border if it is too far right or down
            if (borderOnScreen.Right > canvasOnScreen.Right)
            {
                double newLeft = canvasOnScreen.Right - innerBorder.ActualWidth;
                double newLeftActual = canvas.PointFromScreen(new Point(newLeft, 0)).X;

                // make sure we aren't pushing left out of view
                if (newLeft >= canvasOnScreen.Left)
                {
                    Canvas.SetLeft(innerBorder, newLeftActual);
                }
            }

            if (borderOnScreen.Bottom > canvasOnScreen.Bottom)
            {
                double newTop = canvasOnScreen.Bottom - innerBorder.ActualHeight;
                double newTopActual = canvas.PointFromScreen(new Point(0, newTop)).Y;

                // make sure we aren't pushing up out of view
                if (newTop >= canvasOnScreen.Top)
                {
                    Canvas.SetTop(innerBorder, newTopActual);
                }
            }
        }

        private void PlaceResizer()
        {
            double updatedX = Canvas.GetLeft(innerBorder);
            double updatedY = Canvas.GetTop(innerBorder);

            double width = innerBorder.ActualWidth;
            double height = innerBorder.ActualHeight;

            double resizerX = updatedX + width - 5;
            double resizerY = updatedY + height - 5;

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

            Rect canvasOnScreen = new Rect(canvas.PointToScreen(new Point(0, 0)), new Size(canvas.ActualWidth, canvas.ActualHeight));

            double newX = Canvas.GetLeft(innerBorder) + e.HorizontalChange;
            double newY = Canvas.GetTop(innerBorder) + e.VerticalChange;
            double newXOnScreen = canvas.PointToScreen(new Point(newX, 0)).X;
            double newYOnScreen = canvas.PointToScreen(new Point(0, newY)).Y;
            double newXRightOnScreen = newXOnScreen + innerBorder.ActualWidth;
            double newYBottomOnScreen = newYOnScreen + innerBorder.ActualHeight;

            if (newXOnScreen > canvasOnScreen.Left && newXRightOnScreen < canvasOnScreen.Right)
                Canvas.SetLeft(innerBorder, newX);

            if (newYOnScreen > canvasOnScreen.Top && newYBottomOnScreen < canvasOnScreen.Bottom)
                Canvas.SetTop(innerBorder, newY);
        }

        private void ResizeThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (canvas == null) return;
            if (innerBorder == null) return;

            Rect canvasOnScreen = new Rect(canvas.PointToScreen(new Point(0, 0)), new Size(canvas.ActualWidth, canvas.ActualHeight));

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

                // make sure we are with the canvas area as well, no dragging off screen
                double left = Canvas.GetLeft(innerBorder);
                double top = Canvas.GetTop(innerBorder);

                Point onScreenAdjusted = canvas.PointToScreen(new Point(left + xAdjust, top + yAdjust));

                if (onScreenAdjusted.X > canvasOnScreen.Right)
                    xAdjust = canvas.PointFromScreen(new Point(canvasOnScreen.Right - left, 0)).X;

                if (onScreenAdjusted.Y > canvasOnScreen.Bottom)
                    yAdjust = canvas.PointFromScreen(new Point(0, canvasOnScreen.Bottom - top)).Y;

                innerBorder.Width = xAdjust;
                innerBorder.Height = yAdjust;

                // we are going to move the resizer too (bottom right of input box)
                double newWidth = left + innerBorder.Width - 5;
                double newHeight = top + innerBorder.Height - 5;

                // make sure we can only drag to the minimum and maximum size of the input box
                if (innerBorder.Width <= MessageBoxMinWidth)
                    newWidth = left + MessageBoxMinWidth - 5;

                if (innerBorder.Width >= MessageBoxMaxWidth)
                    newWidth = left + MessageBoxMaxWidth - 5;

                if (innerBorder.Height <= MessageBoxMinHeight)
                    newHeight = top + MessageBoxMinHeight - 5;

                if (innerBorder.Height >= MessageBoxMaxHeight)
                    newHeight = top + MessageBoxMaxHeight - 5;

                Canvas.SetLeft(resizeThumbContainer, newWidth);
                Canvas.SetTop(resizeThumbContainer, newHeight);
            }
        }

        private void SizeContent()
        {
            //double canvasWidth = canvas.ActualWidth;
            //double canvasHeight = canvas.ActualHeight;
            //Rect canvasOnScreen = new Rect(canvas.PointToScreen(new Point(0, 0)), new Size(canvasWidth, canvasHeight));

            //Debug.WriteLine($"Canvas on screen: {canvasOnScreen}");

            //double borderWidth = innerBorder.ActualWidth;
            //double borderHeight = innerBorder.ActualHeight;
            //Rect borderOnScreen = new Rect(innerBorder.PointToScreen(new Point(0, 0)), new Size(borderWidth, borderHeight));

            //Debug.WriteLine($"Border on screen: {borderOnScreen}");
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

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposedValue)
            {
                if (disposing)
                {
                    LayoutUpdated -= MessageBoxInternalDialog_LayoutUpdated;
                    SizeChanged -= MessageBoxInternalDialog_SizeChanged;

                    if (cancelButton != null)
                        cancelButton.Click -= CancelButton_Click;

                    if (closeButton != null)
                        closeButton.Click -= CloseButton_Click;

                    if (okButton != null)
                        okButton.Click -= OkButton_Click;

                    if (noButton != null)
                        noButton.Click -= NoButton_Click;

                    if (yesButton != null)
                        yesButton.Click -= YesButton_Click;

                    if (titleThumb != null)
                    {
                        titleThumb.DragDelta -= TitleThumb_DragDelta;
                    }

                    if (resizeThumb != null)
                    {
                        resizeThumb.DragDelta -= ResizeThumb_DragDelta;
                    }
                }

                disposedValue = true;
            }
        }

        #endregion
    }
}
