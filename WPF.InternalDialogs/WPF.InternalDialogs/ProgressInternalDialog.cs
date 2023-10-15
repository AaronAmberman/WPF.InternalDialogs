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

        private Canvas canvas;
        private Border innerBorder;
        private Button closeButton;
        private Thumb titleThumb;
        private Grid resizeThumbContainer;
        private Thumb resizeThumb;

        private Size defaultSize = Size.Empty;
        private bool initialLayoutComplete;
        private bool isSizeAndPositionUserManaged;

        #endregion

        #region Properties

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
            DependencyProperty.Register("CloseButtonStyle", typeof(Style), typeof(ProgressInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the style for the progress bar in the progress dialog.</summary>
        public Style ProgressBarStyle
        {
            get
            {
                VerifyDisposed();

                return (Style)GetValue(ProgressBarStyleProperty); 
            }
            set 
            {
                VerifyDisposed();

                SetValue(ProgressBarStyleProperty, value);
            }
        }

        public static readonly DependencyProperty ProgressBarStyleProperty =
            DependencyProperty.Register("ProgressBarStyle", typeof(Style), typeof(ProgressInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the background for the input box part of the input box internal dialog. Not the same as Background.</summary>
        public Brush ProgressDialogBackground
        {
            get
            {
                VerifyDisposed();

                return (Brush)GetValue(ProgressDialogBackgroundProperty);
            }
            set 
            {
                VerifyDisposed();

                SetValue(ProgressDialogBackgroundProperty, value);
            }
        }

        public static readonly DependencyProperty ProgressDialogBackgroundProperty =
            DependencyProperty.Register("ProgressDialogBackground", typeof(Brush), typeof(ProgressInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the default height of the resizable portion.</summary>
        public double ProgressDialogDefaultHeight
        {
            get 
            {
                VerifyDisposed();

                return (double)GetValue(ProgressDialogDefaultHeightProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(ProgressDialogDefaultHeightProperty, value);
            }
        }

        public static readonly DependencyProperty ProgressDialogDefaultHeightProperty =
            DependencyProperty.Register("ProgressDialogDefaultHeight", typeof(double), typeof(MovableResizableInternalDialog), new PropertyMetadata(double.NaN));

        /// <summary>Gets or sets the default width of the resizable portion.</summary>
        public double ProgressDialogDefaultWidth
        {
            get 
            {
                VerifyDisposed();

                return (double)GetValue(ProgressDialogDefaultWidthProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(ProgressDialogDefaultWidthProperty, value);
            }
        }

        public static readonly DependencyProperty ProgressDialogDefaultWidthProperty =
            DependencyProperty.Register("ProgressDialogDefaultWidth", typeof(double), typeof(MovableResizableInternalDialog), new PropertyMetadata(double.NaN));

        /// <summary>Gets or sets the input box maximum height. Default is 600.0.</summary>
        public double ProgressDialogMaxHeight
        {
            get
            {
                VerifyDisposed();

                return (double)GetValue(ProgressDialogMaxHeightProperty); 
            }
            set 
            {
                VerifyDisposed();

                SetValue(ProgressDialogMaxHeightProperty, value);
            }
        }

        public static readonly DependencyProperty ProgressDialogMaxHeightProperty =
            DependencyProperty.Register("ProgressDialogMaxHeight", typeof(double), typeof(ProgressInternalDialog), new PropertyMetadata(600.0));

        /// <summary>Gets or sets the input box maximum width. Default is 800.0</summary>
        public double ProgressDialogMaxWidth
        {
            get 
            {
                VerifyDisposed();

                return (double)GetValue(ProgressDialogMaxWidthProperty);
            }
            set 
            {
                VerifyDisposed();

                SetValue(ProgressDialogMaxWidthProperty, value);
            }
        }

        public static readonly DependencyProperty ProgressDialogMaxWidthProperty =
            DependencyProperty.Register("ProgressDialogMaxWidth", typeof(double), typeof(ProgressInternalDialog), new PropertyMetadata(800.0));

        /// <summary>Gets or sets the input box minimum height. Default is 50.0.</summary>
        public double ProgressDialogMinHeight
        {
            get 
            {
                VerifyDisposed();

                return (double)GetValue(ProgressDialogMinHeightProperty);
            }
            set 
            {
                VerifyDisposed();

                SetValue(ProgressDialogMinHeightProperty, value);
            }
        }

        public static readonly DependencyProperty ProgressDialogMinHeightProperty =
            DependencyProperty.Register("ProgressDialogMinHeight", typeof(double), typeof(ProgressInternalDialog), new PropertyMetadata(50.0));

        /// <summary>Gets or sets the input box minimum width. default is 100.0.</summary>
        public double ProgressDialogMinWidth
        {
            get 
            {
                VerifyDisposed();

                return (double)GetValue(ProgressDialogMinWidthProperty);
            }
            set 
            {
                VerifyDisposed();

                SetValue(ProgressDialogMinWidthProperty, value);
            }
        }

        public static readonly DependencyProperty ProgressDialogMinWidthProperty =
            DependencyProperty.Register("ProgressDialogMinWidth", typeof(double), typeof(ProgressInternalDialog), new PropertyMetadata(100.0));

        /// <summary>Gets or sets the message to display to the user.</summary>
        public string ProgressDialogMessage
        {
            get 
            {
                VerifyDisposed();

                return (string)GetValue(ProgressDialogMessageProperty);
            }
            set 
            {
                VerifyDisposed();

                SetValue(ProgressDialogMessageProperty, value);
            }
        }

        public static readonly DependencyProperty ProgressDialogMessageProperty =
            DependencyProperty.Register("ProgressDialogMessage", typeof(string), typeof(ProgressInternalDialog), new PropertyMetadata(string.Empty));

        /// <summary>Gets or sets the progress value.</summary>
        public double ProgressValue
        {
            get 
            {
                VerifyDisposed();

                return (double)GetValue(ProgressValueProperty);
            }
            set 
            {
                VerifyDisposed();

                SetValue(ProgressValueProperty, value);
            }
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
            DependencyProperty.Register("ResizeGripContent", typeof(object), typeof(ProgressInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the cursor for the resize gripper.</summary>
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
            DependencyProperty.Register("ResizeGripCursor", typeof(Cursor), typeof(ProgressInternalDialog), new PropertyMetadata(Cursors.SizeNWSE));

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
            DependencyProperty.Register("ResizeGripVisibility", typeof(Visibility), typeof(ProgressInternalDialog), new PropertyMetadata(Visibility.Visible));

        /// <summary>Gets or sets the title to the input box.</summary>
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
            DependencyProperty.Register("Title", typeof(string), typeof(ProgressInternalDialog), new PropertyMetadata(string.Empty));

        /// <summary>Gets or sets the background for the title area.</summary>
        public Brush TitleAreaBackground
        {
            get 
            {
                VerifyDisposed();

                return (Brush)GetValue(TitleAreaBackgroundProperty);
            }
            set 
            {
                VerifyDisposed();

                SetValue(TitleAreaBackgroundProperty, value);
            }
        }

        public static readonly DependencyProperty TitleAreaBackgroundProperty =
            DependencyProperty.Register("TitleAreaBackground", typeof(Brush), typeof(ProgressInternalDialog), new PropertyMetadata(null));

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
            DependencyProperty.Register("TitleCursor", typeof(Cursor), typeof(ProgressInternalDialog), new PropertyMetadata(Cursors.SizeAll));

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
            SizeChanged += ProgressInternalDialog_SizeChanged;
        }

        #endregion

        #region Methods

        private void ProgressInternalDialog_LayoutUpdated(object? sender, EventArgs e)
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

        private void ProgressInternalDialog_SizeChanged(object sender, SizeChangedEventArgs e)
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
            ProgressInternalDialog instance = d as ProgressInternalDialog;

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
            double width = double.IsNaN(ProgressDialogDefaultWidth) ? innerBorder.ActualWidth : ProgressDialogDefaultWidth;
            double height = double.IsNaN(ProgressDialogDefaultHeight) ? innerBorder.ActualHeight : ProgressDialogDefaultHeight;

            defaultSize = new Size(width, height);

            // respect max
            if (HorizontalContentAlignment == HorizontalAlignment.Stretch)
                width = ProgressDialogMaxWidth;

            // make sure we fit
            if (width > canvas.ActualWidth)
                width = canvas.ActualWidth - Padding.Left - Padding.Right;

            // make sure we aren't smaller than minimum
            if (width < ProgressDialogMinWidth)
                width = ProgressDialogMinWidth;

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
                height = ProgressDialogMaxHeight;

            // make sure we fit
            if (height > canvas.ActualHeight)
                height = canvas.ActualHeight - Padding.Left - Padding.Right;

            // make sure we aren't smaller than minimum
            if (height < ProgressDialogMinHeight)
                height = ProgressDialogMinHeight;

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
            if (width < ProgressDialogMinWidth) width = ProgressDialogMinWidth;

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
                // ProgressDialogDefaultWidth is ignored if stretch, ProgressDialogMaxWidth is not
                width = ProgressDialogMaxWidth;

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
            if (height < ProgressDialogMinHeight) height = ProgressDialogMinHeight;

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
                // ProgressDialogDefaultHeight is ignored if stretch, ProgressDialogMaxHeight is not
                height = ProgressDialogMaxHeight;

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

            closeButton = GetTemplateChild("PART_CloseButton") as Button;

            if (closeButton != null)
                closeButton.Click += CloseButton_Click;

            titleThumb = GetTemplateChild("PART_TitleThumb") as Thumb;

            if (titleThumb != null)
            {
                titleThumb.DragDelta += TitleThumb_DragDelta;
            }

            resizeThumbContainer = GetTemplateChild("PART_ResizeThumbContainer") as Grid;
            resizeThumb = GetTemplateChild("PART_ResizeThumb") as Thumb;

            if (resizeThumb != null)
            {
                resizeThumb.DragDelta += ResizeThumb_DragDelta;
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
                if (xAdjust <= ProgressDialogMinWidth) xAdjust = ProgressDialogMinWidth;
                if (xAdjust >= ProgressDialogMaxWidth) xAdjust = ProgressDialogMaxWidth;

                if (yAdjust <= ProgressDialogMinHeight) yAdjust = ProgressDialogMinHeight;
                if (yAdjust >= ProgressDialogMaxHeight) yAdjust = ProgressDialogMaxHeight;

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
            switch (ProgressDialogMaxHeight)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "ProgressDialogMaxHeight");
            }

            if (ProgressDialogMaxHeight < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "ProgressDialogMaxHeight");
            }

            switch (ProgressDialogMaxWidth)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "ProgressDialogMaxWidth");
            }

            if (ProgressDialogMaxWidth < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "ProgressDialogMaxWidth");
            }

            switch (ProgressDialogMinHeight)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "ProgressDialogMinHeight");
            }

            if (ProgressDialogMinHeight < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "ProgressDialogMinHeight");
            }

            switch (ProgressDialogMinWidth)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "ProgressDialogMinWidth");
            }

            if (ProgressDialogMinWidth < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "ProgressDialogMinWidth");
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposedValue)
            {
                if (disposing)
                {
                    LayoutUpdated -= ProgressInternalDialog_LayoutUpdated;
                    SizeChanged -= ProgressInternalDialog_SizeChanged;

                    if (closeButton != null)
                        closeButton.Click -= CloseButton_Click;

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
