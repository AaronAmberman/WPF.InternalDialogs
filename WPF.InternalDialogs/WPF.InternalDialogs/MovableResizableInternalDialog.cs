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
        private bool isSizeAndPositionUserManaged;

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
                PlaceDialogInitially();

                initialLayoutComplete = true;
            }

            PlaceResizer();
        }

        private void MovableResizableInternalDialog_SizeChanged(object sender, SizeChangedEventArgs e)
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
            double width = double.IsNaN(ResizableDefaultWidth) ? innerBorder.ActualWidth : ResizableDefaultWidth;
            double height = double.IsNaN(ResizableDefaultHeight) ? innerBorder.ActualHeight : ResizableDefaultHeight;

            if (HorizontalContentAlignment == HorizontalAlignment.Left)
                x = 0;
            else if (HorizontalContentAlignment == HorizontalAlignment.Right)
                x = canvas.ActualWidth - width - Padding.Right - Padding.Left;
            else if (HorizontalContentAlignment == HorizontalAlignment.Center)
                x = (canvas.ActualWidth / 2) - (width / 2) - Padding.Left;
            else if (HorizontalContentAlignment == HorizontalAlignment.Stretch)
            {
                // ResizableDefaultWidth is ignored if stretch, ResizableMaxWidth is not
                width = ResizableMaxWidth;

                // make sure the border fits into the canvas
                if (width > canvas.ActualWidth) width = canvas.ActualWidth - Padding.Left - Padding.Right;

                x = (canvas.ActualWidth / 2) - (width / 2) - Padding.Left;
            }

            if (VerticalContentAlignment == VerticalAlignment.Top)
                y = 0;
            else if (VerticalContentAlignment == VerticalAlignment.Bottom)
                y = canvas.ActualHeight - height - Padding.Bottom - Padding.Top;
            else if (VerticalContentAlignment == VerticalAlignment.Center)
                y = (canvas.ActualHeight / 2) - (height / 2) - Padding.Top;
            else if (VerticalContentAlignment == VerticalAlignment.Stretch)
            {
                // ResizableDefaultHeight is ignored if stretch, ResizableMaxHeight is not
                height = ResizableMaxHeight;

                // make sure the border fits into the canvas
                if (height > canvas.ActualHeight) height = canvas.ActualHeight - Padding.Top - Padding.Bottom;

                y = (canvas.ActualHeight / 2) - (height / 2) - Padding.Top;
            }

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
            double width = innerBorder.ActualWidth;
            double height = innerBorder.ActualHeight;

            //if (HorizontalContentAlignment == HorizontalAlignment.Left)
            //    leave it where it is because we are aligned from the left anyway
            if (HorizontalContentAlignment == HorizontalAlignment.Right)
                x = canvas.ActualWidth - width - Padding.Left - Padding.Right;
            else if (HorizontalContentAlignment == HorizontalAlignment.Center)
                x = (canvas.ActualWidth / 2) - (width / 2) - Padding.Left;
            else if (HorizontalContentAlignment == HorizontalAlignment.Stretch)
            {
                // ResizableDefaultWidth is ignored if stretch, ResizableMaxWidth is not
                width = ResizableMaxWidth;

                // make sure the border fits into the canvas
                if (width + Padding.Left + Padding.Right > canvas.ActualWidth)
                    width = canvas.ActualWidth - Padding.Left - Padding.Right;

                x = (canvas.ActualWidth / 2) - (width / 2) - Padding.Left;
            }

            // ensure 0 at a minimum
            if (width >= canvas.ActualWidth - Padding.Left - Padding.Right)
                x = 0;

            //if (VerticalContentAlignment == VerticalAlignment.Top)
            //    leave it where it is because we are aligned from the top anyway
            if (VerticalContentAlignment == VerticalAlignment.Bottom)
                y = canvas.ActualHeight - height - Padding.Top - Padding.Bottom;
            else if (VerticalContentAlignment == VerticalAlignment.Center)
                y = (canvas.ActualHeight / 2) - (height / 2) - Padding.Top;
            else if (VerticalContentAlignment == VerticalAlignment.Stretch)
            {
                // ResizableDefaultHeight is ignored if stretch, ResizableMaxHeight is not
                height = ResizableMaxHeight;

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

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            SetValue(ResultProperty, MessageBoxResult.Cancel);
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
                if (xAdjust <= ResizableMinWidth) xAdjust = ResizableMinWidth;
                if (xAdjust >= ResizableMaxWidth) xAdjust = ResizableMaxWidth;

                if (yAdjust <= ResizableMinHeight) yAdjust = ResizableMinHeight;
                if (yAdjust >= ResizableMaxHeight) yAdjust = ResizableMaxHeight;

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
