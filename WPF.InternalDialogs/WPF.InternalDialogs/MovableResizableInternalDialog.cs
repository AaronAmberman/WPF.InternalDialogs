using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace WPF.InternalDialogs
{
    /// <summary>A movable and resizable internal dialog that can display whatever content. This class cannot be inherited.</summary>
    [TemplatePart(Name = "PART_Canvas", Type = typeof(Canvas))]
    [TemplatePart(Name = "PART_InnerBorder", Type = typeof(Border))]
    [TemplatePart(Name = "PART_CloseButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_TitleThumb", Type = typeof(Thumb))]
    [TemplatePart(Name = "PART_ResizeThumbContainer", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_ResizeThumb", Type = typeof(Thumb))]
    public sealed class MovableResizableInternalDialog : InternalDialog
    {
        #region Fields

        private Canvas canvas;
        private Border innerBorder;
        private Button closeButton;
        private Thumb titleThumb;
        private Grid resizeThumbContainer;
        private Thumb resizeThumb;

        private bool initialLayoutComplete;

        #endregion

        #region Properties

        /// <summary>Gets or sets the background for the answer area.</summary>
        public SolidColorBrush AnswerAreaBackground
        {
            get 
            {
                VerifyDisposed();

                return (SolidColorBrush)GetValue(AnswerAreaBackgroundProperty);
            }
            set 
            {
                VerifyDisposed();

                SetValue(AnswerAreaBackgroundProperty, value);
            }
        }

        public static readonly DependencyProperty AnswerAreaBackgroundProperty =
            DependencyProperty.Register("AnswerAreaBackground", typeof(SolidColorBrush), typeof(MovableResizableInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the answer area content.</summary>
        public object AnswerAreaContent
        {
            get 
            {
                VerifyDisposed();

                return (object)GetValue(AnswerAreaContentProperty);
            }
            set 
            {
                VerifyDisposed();

                SetValue(AnswerAreaContentProperty, value);
            }
        }

        public static readonly DependencyProperty AnswerAreaContentProperty =
            DependencyProperty.Register("AnswerAreaContent", typeof(object), typeof(MovableResizableInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the style to use for the close button at the top right.</summary>
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
            DependencyProperty.Register("CloseButtonStyle", typeof(Style), typeof(MovableResizableInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the background for the content part of the movable resizable internal dialog. Not the same as Background.</summary>
        public SolidColorBrush ContentBackground
        {
            get 
            {
                VerifyDisposed();

                return (SolidColorBrush)GetValue(ContentBackgroundProperty);
            }
            set 
            {
                VerifyDisposed();

                SetValue(ContentBackgroundProperty, value);
            }
        }

        public static readonly DependencyProperty ContentBackgroundProperty =
            DependencyProperty.Register("ContentBackground", typeof(SolidColorBrush), typeof(MovableResizableInternalDialog), new PropertyMetadata(null));

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
            DependencyProperty.Register("KeepDialogCenteredOnContainerResize", typeof(bool), typeof(MovableResizableInternalDialog), new PropertyMetadata(false));

        /// <summary>Gets or sets the default height of the resizable portion.</summary>
        public double ResizableDefaultHeight
        {
            get
            {
                VerifyDisposed();

                return (double)GetValue(ResizableDefaultHeightProperty);
            }
            set 
            {
                VerifyDisposed();

                SetValue(ResizableDefaultHeightProperty, value);
            }
        }

        public static readonly DependencyProperty ResizableDefaultHeightProperty =
            DependencyProperty.Register("ResizableDefaultHeight", typeof(double), typeof(MovableResizableInternalDialog), new PropertyMetadata(double.NaN));

        /// <summary>Gets or sets the default width of the resizable portion.</summary>
        public double ResizableDefaultWidth
        {
            get 
            {
                VerifyDisposed();

                return (double)GetValue(ResizableDefaultWidthProperty);
            }
            set 
            {
                VerifyDisposed();

                SetValue(ResizableDefaultWidthProperty, value); 
            }
        }

        public static readonly DependencyProperty ResizableDefaultWidthProperty =
            DependencyProperty.Register("ResizableDefaultWidth", typeof(double), typeof(MovableResizableInternalDialog), new PropertyMetadata(double.NaN));

        /// <summary>Gets or sets the movable resizable internal dialog maximum height. Default is 600.0.</summary>
        public double ResizableMaxHeight
        {
            get 
            {
                VerifyDisposed();

                return (double)GetValue(ResizableMaxHeightProperty);
            }
            set 
            {
                VerifyDisposed();

                SetValue(ResizableMaxHeightProperty, value);
            }
        }

        public static readonly DependencyProperty ResizableMaxHeightProperty =
            DependencyProperty.Register("ResizableMaxHeight", typeof(double), typeof(MovableResizableInternalDialog), new PropertyMetadata(600.0));

        /// <summary>Gets or sets the movable resizable internal dialog maximum width. Default is 800.0.</summary>
        public double ResizableMaxWidth
        {
            get 
            {
                VerifyDisposed();

                return (double)GetValue(ResizableMaxWidthProperty);
            }
            set 
            {
                VerifyDisposed();

                SetValue(ResizableMaxWidthProperty, value);
            }
        }

        public static readonly DependencyProperty ResizableMaxWidthProperty =
            DependencyProperty.Register("ResizableMaxWidth", typeof(double), typeof(MovableResizableInternalDialog), new PropertyMetadata(800.0));

        /// <summary>Gets or sets the movable resizable internal dialog minimum height. Default is 50.0.</summary>
        public double ResizableMinHeight
        {
            get
            {
                VerifyDisposed();

                return (double)GetValue(ResizableMinHeightProperty);
            }
            set 
            {
                VerifyDisposed();

                SetValue(ResizableMinHeightProperty, value);
            }
        }

        public static readonly DependencyProperty ResizableMinHeightProperty =
            DependencyProperty.Register("ResizableMinHeight", typeof(double), typeof(MovableResizableInternalDialog), new PropertyMetadata(50.0));

        /// <summary>Gets or sets the movable resizable internal dialog minimum width. default is 100.0.</summary>
        public double ResizableMinWidth
        {
            get
            {
                VerifyDisposed();

                return (double)GetValue(ResizableMinWidthProperty);
            }
            set 
            {
                VerifyDisposed();

                SetValue(ResizableMinWidthProperty, value);
            }
        }

        public static readonly DependencyProperty ResizableMinWidthProperty =
            DependencyProperty.Register("ResizableMinWidth", typeof(double), typeof(MovableResizableInternalDialog), new PropertyMetadata(100.0));

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
            DependencyProperty.Register("ResizeGripContent", typeof(object), typeof(MovableResizableInternalDialog), new PropertyMetadata(null));

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
            DependencyProperty.Register("ResizeGripCursor", typeof(Cursor), typeof(MovableResizableInternalDialog), new PropertyMetadata(Cursors.SizeNWSE));

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
            DependencyProperty.Register("ResizeGripVisibility", typeof(Visibility), typeof(MovableResizableInternalDialog), new PropertyMetadata(Visibility.Visible));

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
            DependencyProperty.Register("TitleAreaBackground", typeof(SolidColorBrush), typeof(MovableResizableInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the title content. The title content has IsHitTestVisible="False" set so the underlying Thumb can work.</summary>
        public object TitleContent
        {
            get 
            {
                VerifyDisposed();

                return (object)GetValue(TitleContentProperty);
            }
            set 
            {
                VerifyDisposed();

                SetValue(TitleContentProperty, value);
            }
        }

        public static readonly DependencyProperty TitleContentProperty =
            DependencyProperty.Register("TitleContent", typeof(object), typeof(MovableResizableInternalDialog), new PropertyMetadata(null));

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
            DependencyProperty.Register("TitleCursor", typeof(Cursor), typeof(MovableResizableInternalDialog), new PropertyMetadata(Cursors.SizeAll));

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
            DependencyProperty.Register("TitleHorizontalAlignment", typeof(HorizontalAlignment), typeof(MovableResizableInternalDialog),
                new PropertyMetadata(HorizontalAlignment.Left));

        #endregion

        #region Constructors

        static MovableResizableInternalDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MovableResizableInternalDialog), new FrameworkPropertyMetadata(typeof(MovableResizableInternalDialog)));
            VisibilityProperty.OverrideMetadata(typeof(MovableResizableInternalDialog), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, VisibilityChangedCallback));
        }

        public MovableResizableInternalDialog()
        {
            LayoutUpdated += MovableResizableInternalDialog_LayoutUpdated;
            SizeChanged += MovableResizableInternalDialog_SizeChanged;
        }

        #endregion

        #region Methods

        private void MovableResizableInternalDialog_LayoutUpdated(object? sender, EventArgs e)
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

        private void MovableResizableInternalDialog_SizeChanged(object sender, SizeChangedEventArgs e)
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
            MovableResizableInternalDialog instance = d as MovableResizableInternalDialog;

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
            innerBorder.Width = ResizableDefaultWidth;
            innerBorder.Height = ResizableDefaultHeight;

            // center input box
            double totalWidth = canvas.ActualWidth;
            double totalHeight = canvas.ActualHeight;

            double messageBoxWidth = innerBorder.ActualWidth;
            double messageBoxHeight = innerBorder.ActualHeight;

            if (!double.IsNaN(ResizableDefaultHeight))
                messageBoxHeight = ResizableDefaultHeight;

            if (!double.IsNaN(ResizableDefaultWidth))
                messageBoxWidth = ResizableDefaultWidth;

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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            SetValue(ResultProperty, MessageBoxResult.Cancel);
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
                if (xAdjust <= ResizableMinWidth) xAdjust = ResizableMinWidth;
                if (xAdjust >= ResizableMaxWidth) xAdjust = ResizableMaxWidth;
                               
                if (yAdjust <= ResizableMinHeight) yAdjust = ResizableMinHeight;
                if (yAdjust >= ResizableMaxHeight) yAdjust = ResizableMaxHeight;

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
                if (innerBorder.Width <= ResizableMinWidth)
                    newWidth = left + ResizableMinWidth - 5;

                if (innerBorder.Width >= ResizableMaxWidth)
                    newWidth = left + ResizableMaxWidth - 5;

                if (innerBorder.Height <= ResizableMinHeight)
                    newHeight = top + ResizableMinHeight - 5;

                if (innerBorder.Height >= ResizableMaxHeight)
                    newHeight = top + ResizableMaxHeight - 5;

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
            switch (ResizableMaxHeight)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "ResizableMaxHeight");
            }

            if (ResizableMaxHeight < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "ResizableMaxHeight");
            }

            switch (ResizableMaxWidth)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "ResizableMaxWidth");
            }

            if (ResizableMaxWidth < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "ResizableMaxWidth");
            }

            switch (ResizableMinHeight)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "ResizableMinHeight");
            }

            if (ResizableMinHeight < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "ResizableMinHeight");
            }

            switch (ResizableMinWidth)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "ResizableMinWidth");
            }

            if (ResizableMinWidth < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "ResizableMinWidth");
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposedValue)
            {
                if (disposing)
                {
                    LayoutUpdated -= MovableResizableInternalDialog_LayoutUpdated;
                    SizeChanged -= MovableResizableInternalDialog_SizeChanged;

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
