using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace WPF.InternalDialogs
{
    /// <summary>A simple input box to gather basic user input. This class cannot be inherited.</summary>
    [TemplatePart(Name = "PART_Canvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_InnerBorder", Type = typeof(Border))]
    [TemplatePart(Name = "PART_CloseButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_TitleThumb", Type = typeof(Thumb))]
    [TemplatePart(Name = "PART_ResizeThumbContainer", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_ResizeThumb", Type = typeof(Thumb))]
    public sealed class ProgressInternalDialog : InternalDialog
    {
        #region Fields

        private Canvas? canvas;
        private Border? innerBorder;
        private Button? closeButton;
        private Thumb? titleThumb;
        private Grid? resizeThumbContainer;
        private Thumb? resizeThumb;

        private bool hasBeenUpdated = false;

        #endregion

        #region Properties

        /// <summary>gets or sets the style to use for the close button at the top right.</summary>
        public Style CloseButtonStyle
        {
            get { return (Style)GetValue(CloseButtonStyleProperty); }
            set { SetValue(CloseButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty CloseButtonStyleProperty =
            DependencyProperty.Register("CloseButtonStyle", typeof(Style), typeof(ProgressInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the style for the progress bar in the progress dialog.</summary>
        public Style ProgressBarStyle
        {
            get { return (Style)GetValue(ProgressBarStyleProperty); }
            set { SetValue(ProgressBarStyleProperty, value); }
        }

        public static readonly DependencyProperty ProgressBarStyleProperty =
            DependencyProperty.Register("ProgressBarStyle", typeof(Style), typeof(ProgressInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the background for the input box part of the input box internal dialog. Not the same as Background.</summary>
        public SolidColorBrush ProgressDialogBackground
        {
            get { return (SolidColorBrush)GetValue(ProgressDialogBackgroundProperty); }
            set { SetValue(ProgressDialogBackgroundProperty, value); }
        }

        public static readonly DependencyProperty ProgressDialogBackgroundProperty =
            DependencyProperty.Register("ProgressDialogBackground", typeof(SolidColorBrush), typeof(ProgressInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the input box maximum height. Default is 600.0.</summary>
        public double ProgressDialogMaxHeight
        {
            get { return (double)GetValue(ProgressDialogMaxHeightProperty); }
            set { SetValue(ProgressDialogMaxHeightProperty, value); }
        }

        public static readonly DependencyProperty ProgressDialogMaxHeightProperty =
            DependencyProperty.Register("ProgressDialogMaxHeight", typeof(double), typeof(ProgressInternalDialog), new PropertyMetadata(600.0));

        /// <summary>Gets or sets the input box maximum width. Default is 800.0</summary>
        public double ProgressDialogMaxWidth
        {
            get { return (double)GetValue(ProgressDialogMaxWidthProperty); }
            set { SetValue(ProgressDialogMaxWidthProperty, value); }
        }

        public static readonly DependencyProperty ProgressDialogMaxWidthProperty =
            DependencyProperty.Register("ProgressDialogMaxWidth", typeof(double), typeof(ProgressInternalDialog), new PropertyMetadata(800.0));

        /// <summary>Gets or sets the input box minimum height. Default is 50.0.</summary>
        public double ProgressDialogMinHeight
        {
            get { return (double)GetValue(ProgressDialogMinHeightProperty); }
            set { SetValue(ProgressDialogMinHeightProperty, value); }
        }

        public static readonly DependencyProperty ProgressDialogMinHeightProperty =
            DependencyProperty.Register("ProgressDialogMinHeight", typeof(double), typeof(ProgressInternalDialog), new PropertyMetadata(50.0));

        /// <summary>Gets or sets the input box minimum width. default is 100.0.</summary>
        public double ProgressDialogMinWidth
        {
            get { return (double)GetValue(ProgressDialogMinWidthProperty); }
            set { SetValue(ProgressDialogMinWidthProperty, value); }
        }

        public static readonly DependencyProperty ProgressDialogMinWidthProperty =
            DependencyProperty.Register("ProgressDialogMinWidth", typeof(double), typeof(ProgressInternalDialog), new PropertyMetadata(100.0));

        /// <summary>Gets or sets the message to display to the user.</summary>
        public string ProgressDialogMessage
        {
            get { return (string)GetValue(ProgressDialogMessageProperty); }
            set { SetValue(ProgressDialogMessageProperty, value); }
        }

        public static readonly DependencyProperty ProgressDialogMessageProperty =
            DependencyProperty.Register("ProgressDialogMessage", typeof(string), typeof(ProgressInternalDialog), new PropertyMetadata(string.Empty));

        /// <summary>Gets or sets the progress value.</summary>
        public double ProgressValue
        {
            get { return (double)GetValue(ProgressValueProperty); }
            set { SetValue(ProgressValueProperty, value); }
        }

        public static readonly DependencyProperty ProgressValueProperty =
            DependencyProperty.Register("ProgressValue", typeof(double), typeof(ProgressInternalDialog), new PropertyMetadata(0.0));

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
            DependencyProperty.Register("ResizeGripContent", typeof(object), typeof(ProgressInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the cursor for the resize gripper.</summary>
        public Cursor ResizeGripCursor
        {
            get { return (Cursor)GetValue(ResizeGripCursorProperty); }
            set { SetValue(ResizeGripCursorProperty, value); }
        }

        public static readonly DependencyProperty ResizeGripCursorProperty =
            DependencyProperty.Register("ResizeGripCursor", typeof(Cursor), typeof(ProgressInternalDialog), new PropertyMetadata(Cursors.SizeNWSE));

        /// <summary>Gets or sets the visibility of the resize grip. Visible = resizing enabled, Collapsed/Hidden = resizing disabled.</summary>
        public Visibility ResizeGripVisibility
        {
            get { return (Visibility)GetValue(ResizeGripVisibilityProperty); }
            set { SetValue(ResizeGripVisibilityProperty, value); }
        }

        public static readonly DependencyProperty ResizeGripVisibilityProperty =
            DependencyProperty.Register("ResizeGripVisibility", typeof(Visibility), typeof(ProgressInternalDialog), new PropertyMetadata(Visibility.Visible));

        /// <summary>Gets or sets the title to the input box.</summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ProgressInternalDialog), new PropertyMetadata(string.Empty));

        /// <summary>Gets or sets the background for the title area.</summary>
        public SolidColorBrush TitleAreaBackground
        {
            get { return (SolidColorBrush)GetValue(TitleAreaBackgroundProperty); }
            set { SetValue(TitleAreaBackgroundProperty, value); }
        }

        public static readonly DependencyProperty TitleAreaBackgroundProperty =
            DependencyProperty.Register("TitleAreaBackground", typeof(SolidColorBrush), typeof(ProgressInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the cursor for the title area. Default is Cursors.SizeAll.</summary>
        public Cursor TitleCursor
        {
            get { return (Cursor)GetValue(TitleCursorProperty); }
            set { SetValue(TitleCursorProperty, value); }
        }

        public static readonly DependencyProperty TitleCursorProperty =
            DependencyProperty.Register("TitleCursor", typeof(Cursor), typeof(ProgressInternalDialog), new PropertyMetadata(Cursors.SizeAll));

        /// <summary>Gets or sets the horizontal alignment of the title.</summary>
        public HorizontalAlignment TitleHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(TitleHorizontalAlignmentProperty); }
            set { SetValue(TitleHorizontalAlignmentProperty, value); }
        }

        public static readonly DependencyProperty TitleHorizontalAlignmentProperty =
            DependencyProperty.Register("TitleHorizontalAlignment", typeof(HorizontalAlignment), typeof(ProgressInternalDialog),
                new PropertyMetadata(HorizontalAlignment.Left));

        #endregion

        #region Constructors

        static ProgressInternalDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ProgressInternalDialog), new FrameworkPropertyMetadata(typeof(ProgressInternalDialog)));
            VisibilityProperty.OverrideMetadata(typeof(ProgressInternalDialog), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, VisibilityChangedCallback));
        }

        public ProgressInternalDialog()
        {
            LayoutUpdated += ProgressInternalDialog_LayoutUpdated;
        }

        private void ProgressInternalDialog_LayoutUpdated(object? sender, EventArgs e)
        {
            if (Visibility == Visibility.Collapsed) return;

            if (!hasBeenUpdated)
            {
                CenterMessageBox();

                hasBeenUpdated = true;
            }
        }

        #endregion

        #region Methods

        new private static void VisibilityChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ProgressInternalDialog? instance = d as ProgressInternalDialog;

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
                // just reset our visual update counter
                instance.hasBeenUpdated = false;

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

            // center input box
            double totalWidth = canvas.ActualWidth;
            double totalHeight = canvas.ActualHeight;

            double messageBoxWidth = innerBorder.ActualWidth;
            double messageBoxHeight = innerBorder.ActualHeight;

            double centerX = (totalWidth / 2) - (messageBoxWidth / 2);
            double centerY = (totalHeight / 2) - (messageBoxHeight / 2);

            Canvas.SetLeft(innerBorder, centerX);
            Canvas.SetTop(innerBorder, centerY);

            // we are going to move the resizer too (bottom right of input box)
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

            closeButton = GetTemplateChild("PART_CloseButton") as Button;

            if (closeButton != null)
                closeButton.Click += CloseButton_Click;

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

        private void TitleThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            if (innerBorder != null)
            {
                Canvas.SetLeft(innerBorder, Canvas.GetLeft(innerBorder) + e.HorizontalChange);
                Canvas.SetTop(innerBorder, Canvas.GetTop(innerBorder) + e.VerticalChange);

                // we are going to move the resizer too (bottom right of input box)
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

            // verify the user didn't drag the input box outside the view port, and if they did, move it a little bit back into view
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
                if (xAdjust <= ProgressDialogMinWidth) xAdjust = ProgressDialogMinWidth;
                if (xAdjust >= ProgressDialogMaxWidth) xAdjust = ProgressDialogMaxWidth;

                if (yAdjust <= ProgressDialogMinHeight) yAdjust = ProgressDialogMinHeight;
                if (yAdjust >= ProgressDialogMaxHeight) yAdjust = ProgressDialogMaxHeight;

                innerBorder.Width = xAdjust;
                innerBorder.Height = yAdjust;

                // we are going to move the resizer too (bottom right of input box)
                double left = Canvas.GetLeft(innerBorder);
                double top = Canvas.GetTop(innerBorder);

                double newWidth = left + innerBorder.Width - 5;
                double newHeight = top + innerBorder.Height - 5;

                // make sure we can only drag to the minimum and maximum size of the input box
                if (innerBorder.Width <= ProgressDialogMinWidth)
                {
                    newWidth = left + ProgressDialogMinWidth - 5;
                }

                if (innerBorder.Width >= ProgressDialogMaxWidth)
                {
                    newWidth = left + ProgressDialogMaxWidth - 5;
                }

                if (innerBorder.Height <= ProgressDialogMinHeight)
                {
                    newHeight = top + ProgressDialogMinHeight - 5;
                }

                if (innerBorder.Height >= ProgressDialogMaxHeight)
                {
                    newHeight = top + ProgressDialogMaxHeight - 5;
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
            switch (ProgressDialogMaxHeight)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "MessageBoxMaxHeight");
            }

            if (ProgressDialogMaxHeight < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "MessageBoxMaxHeight");
            }

            switch (ProgressDialogMaxWidth)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "MessageBoxMaxWidth");
            }

            if (ProgressDialogMaxWidth < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "MessageBoxMaxWidth");
            }

            switch (ProgressDialogMinHeight)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "MessageBoxMinHeight");
            }

            if (ProgressDialogMinHeight < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "MessageBoxMinHeight");
            }

            switch (ProgressDialogMinWidth)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "MessageBoxMinWidth");
            }

            if (ProgressDialogMinWidth < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "MessageBoxMinWidth");
            }
        }

        #endregion
    }
}
