using System;
using System.Diagnostics;
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
    [TemplatePart(Name = "PART_CancelButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_CloseButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_OkButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_TitleThumb", Type = typeof(Thumb))]
    [TemplatePart(Name = "PART_ResizeThumbContainer", Type = typeof(Grid))]
    [TemplatePart(Name = "PART_ResizeThumb", Type = typeof(Thumb))]
    public sealed class InputBoxInternalDialog : InternalDialog
    {
        #region Fields

        private Canvas canvas;
        private Border innerBorder;
        private Button cancelButton;
        private Button closeButton;
        private Button okButton;
        private Thumb titleThumb;
        private Grid resizeThumbContainer;
        private Thumb resizeThumb;

        private Size defaultSize = Size.Empty;
        private bool initialLayoutComplete;
        private bool isSizeAndPositionUserManaged;

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
            DependencyProperty.Register("ButtonAreaBackground", typeof(SolidColorBrush), typeof(InputBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the style to use for the buttons in the input box.</summary>
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
            DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(InputBoxInternalDialog), new PropertyMetadata(null));

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
            DependencyProperty.Register("CloseButtonStyle", typeof(Style), typeof(InputBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the input to display in the text box portion of the input box.</summary>
        public string Input
        {
            get
            {
                VerifyDisposed();

                return (string)GetValue(InputProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(InputProperty, value);
            }
        }

        public static readonly DependencyProperty InputProperty =
            DependencyProperty.Register("Input", typeof(string), typeof(InputBoxInternalDialog), new PropertyMetadata(string.Empty));

        /// <summary>Gets or sets whether or not the input text box accepts return. Default is false.</summary>
        public bool InputBoxAcceptsReturn
        {
            get
            {
                VerifyDisposed();

                return (bool)GetValue(InputBoxAcceptsReturnProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(InputBoxAcceptsReturnProperty, value);
            }
        }

        public static readonly DependencyProperty InputBoxAcceptsReturnProperty =
            DependencyProperty.Register("InputBoxAcceptsReturn", typeof(bool), typeof(InputBoxInternalDialog), new PropertyMetadata(false));

        /// <summary>Gets or sets whether or not the input text box accepts tabs. Default is false.</summary>
        public bool InputBoxAcceptsTab
        {
            get
            {
                VerifyDisposed();

                return (bool)GetValue(InputBoxAcceptsTabProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(InputBoxAcceptsTabProperty, value);
            }
        }

        public static readonly DependencyProperty InputBoxAcceptsTabProperty =
            DependencyProperty.Register("InputBoxAcceptsTab", typeof(bool), typeof(InputBoxInternalDialog), new PropertyMetadata(false));

        /// <summary>Gets or sets the background for the input box part of the input box internal dialog. Not the same as Background.</summary>
        public SolidColorBrush InputBoxBackground
        {
            get
            {
                VerifyDisposed();

                return (SolidColorBrush)GetValue(InputBoxBackgroundProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(InputBoxBackgroundProperty, value);
            }
        }

        public static readonly DependencyProperty InputBoxBackgroundProperty =
            DependencyProperty.Register("InputBoxBackground", typeof(SolidColorBrush), typeof(InputBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the default height of the resizable portion.</summary>
        public double InputBoxDefaultHeight
        {
            get
            {
                VerifyDisposed();

                return (double)GetValue(InputBoxDefaultHeightProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(InputBoxDefaultHeightProperty, value);
            }
        }

        public static readonly DependencyProperty InputBoxDefaultHeightProperty =
            DependencyProperty.Register("InputBoxDefaultHeight", typeof(double), typeof(MovableResizableInternalDialog), new PropertyMetadata(double.NaN));

        /// <summary>Gets or sets the default width of the resizable portion.</summary>
        public double InputBoxDefaultWidth
        {
            get
            {
                VerifyDisposed();

                return (double)GetValue(InputBoxDefaultWidthProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(InputBoxDefaultWidthProperty, value);
            }
        }

        public static readonly DependencyProperty InputBoxDefaultWidthProperty =
            DependencyProperty.Register("InputBoxDefaultWidth", typeof(double), typeof(MovableResizableInternalDialog), new PropertyMetadata(double.NaN));

        /// <summary>Gets or sets the input box maximum height. Default is 600.0.</summary>
        public double InputBoxMaxHeight
        {
            get
            {
                VerifyDisposed();

                return (double)GetValue(InputBoxMaxHeightProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(InputBoxMaxHeightProperty, value);
            }
        }

        public static readonly DependencyProperty InputBoxMaxHeightProperty =
            DependencyProperty.Register("InputBoxMaxHeight", typeof(double), typeof(InputBoxInternalDialog), new PropertyMetadata(600.0));

        /// <summary>Gets or sets the input box maximum width. Default is 800.0</summary>
        public double InputBoxMaxWidth
        {
            get
            {
                VerifyDisposed();

                return (double)GetValue(InputBoxMaxWidthProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(InputBoxMaxWidthProperty, value);
            }
        }

        public static readonly DependencyProperty InputBoxMaxWidthProperty =
            DependencyProperty.Register("InputBoxMaxWidth", typeof(double), typeof(InputBoxInternalDialog), new PropertyMetadata(800.0));

        /// <summary>Gets or sets the input box minimum height. Default is 50.0.</summary>
        public double InputBoxMinHeight
        {
            get
            {
                VerifyDisposed();

                return (double)GetValue(InputBoxMinHeightProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(InputBoxMinHeightProperty, value);
            }
        }

        public static readonly DependencyProperty InputBoxMinHeightProperty =
            DependencyProperty.Register("InputBoxMinHeight", typeof(double), typeof(InputBoxInternalDialog), new PropertyMetadata(50.0));

        /// <summary>Gets or sets the input box minimum width. default is 100.0.</summary>
        public double InputBoxMinWidth
        {
            get
            {
                VerifyDisposed();

                return (double)GetValue(InputBoxMinWidthProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(InputBoxMinWidthProperty, value);
            }
        }

        public static readonly DependencyProperty InputBoxMinWidthProperty =
            DependencyProperty.Register("InputBoxMinWidth", typeof(double), typeof(InputBoxInternalDialog), new PropertyMetadata(100.0));

        /// <summary>Gets or sets the message to display to the user.</summary>
        public string InputBoxMessage
        {
            get
            {
                VerifyDisposed();

                return (string)GetValue(InputBoxMessageProperty);
            }
            set
            {
                VerifyDisposed();

                SetValue(InputBoxMessageProperty, value);
            }
        }

        public static readonly DependencyProperty InputBoxMessageProperty =
            DependencyProperty.Register("InputBoxMessage", typeof(string), typeof(InputBoxInternalDialog), new PropertyMetadata(string.Empty));

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
            DependencyProperty.Register("ResizeGripContent", typeof(object), typeof(InputBoxInternalDialog), new PropertyMetadata(null));

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
            DependencyProperty.Register("ResizeGripCursor", typeof(Cursor), typeof(InputBoxInternalDialog), new PropertyMetadata(Cursors.SizeNWSE));

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
            DependencyProperty.Register("ResizeGripVisibility", typeof(Visibility), typeof(InputBoxInternalDialog), new PropertyMetadata(Visibility.Visible));

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
            DependencyProperty.Register("Title", typeof(string), typeof(InputBoxInternalDialog), new PropertyMetadata(string.Empty));

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
            DependencyProperty.Register("TitleAreaBackground", typeof(SolidColorBrush), typeof(InputBoxInternalDialog), new PropertyMetadata(null));

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
            DependencyProperty.Register("TitleCursor", typeof(Cursor), typeof(InputBoxInternalDialog), new PropertyMetadata(Cursors.SizeAll));

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
            DependencyProperty.Register("TitleHorizontalAlignment", typeof(HorizontalAlignment), typeof(InputBoxInternalDialog),
                new PropertyMetadata(HorizontalAlignment.Left));

        #endregion

        #region Constructors

        static InputBoxInternalDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InputBoxInternalDialog), new FrameworkPropertyMetadata(typeof(InputBoxInternalDialog)));
            VisibilityProperty.OverrideMetadata(typeof(InputBoxInternalDialog), new FrameworkPropertyMetadata(Visibility.Collapsed, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, VisibilityChangedCallback));
        }

        public InputBoxInternalDialog()
        {
            LayoutUpdated += InputBoxInternalDialog_LayoutUpdated;
            SizeChanged += InputBoxInternalDialog_SizeChanged;
        }

        #endregion

        #region Methods

        private void InputBoxInternalDialog_LayoutUpdated(object sender, EventArgs e)
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

        private void InputBoxInternalDialog_SizeChanged(object sender, SizeChangedEventArgs e)
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
            InputBoxInternalDialog instance = d as InputBoxInternalDialog;

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
            double width = double.IsNaN(InputBoxDefaultWidth) ? innerBorder.ActualWidth : InputBoxDefaultWidth;
            double height = double.IsNaN(InputBoxDefaultHeight) ? innerBorder.ActualHeight : InputBoxDefaultHeight;

            defaultSize = new Size(width, height);

            // respect max
            if (HorizontalContentAlignment == HorizontalAlignment.Stretch)
                width = InputBoxMaxWidth;

            // make sure we fit
            if (width > canvas.ActualWidth)
                width = canvas.ActualWidth - Padding.Left - Padding.Right;

            // make sure we aren't smaller than minimum
            if (width < InputBoxMinWidth)
                width = InputBoxMinWidth;

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
                height = InputBoxMaxHeight;

            // make sure we fit
            if (height > canvas.ActualHeight)
                height = canvas.ActualHeight - Padding.Left - Padding.Right;

            // make sure we aren't smaller than minimum
            if (height < InputBoxMinHeight)
                height = InputBoxMinHeight;

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
            if (width < InputBoxMinWidth) width = InputBoxMinWidth;

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
                // InputBoxDefaultWidth is ignored if stretch, InputBoxMaxWidth is not
                width = InputBoxMaxWidth;

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
            if (height < InputBoxMinHeight) height = InputBoxMinHeight;

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
                // InputBoxDefaultHeight is ignored if stretch, InputBoxMaxHeight is not
                height = InputBoxMaxHeight;

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

            if (cancelButton != null)
                cancelButton.Click += CancelButton_Click;

            if (closeButton != null)
                closeButton.Click += CloseButton_Click;

            if (okButton != null)
                okButton.Click += OkButton_Click;

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
                if (xAdjust <= InputBoxMinWidth) xAdjust = InputBoxMinWidth;
                if (xAdjust >= InputBoxMaxWidth) xAdjust = InputBoxMaxWidth;

                if (yAdjust <= InputBoxMinHeight) yAdjust = InputBoxMinHeight;
                if (yAdjust >= InputBoxMaxHeight) yAdjust = InputBoxMaxHeight;

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
            switch (InputBoxMaxHeight)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "InputBoxMaxHeight");
            }

            if (InputBoxMaxHeight < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "InputBoxMaxHeight");
            }

            switch (InputBoxMaxWidth)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "InputBoxMaxWidth");
            }

            if (InputBoxMaxWidth < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "InputBoxMaxWidth");
            }

            switch (InputBoxMinHeight)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "InputBoxMinHeight");
            }

            if (InputBoxMinHeight < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "InputBoxMinHeight");
            }

            switch (InputBoxMinWidth)
            {
                case 0.0:
                case double.PositiveInfinity:
                case double.NegativeInfinity:
                case double.NaN:
                    throw new ArgumentException("Cannot be 0.0, double.PositiveInfinity, double.NegativeInfinity or double.NaN.", "InputBoxMinWidth");
            }

            if (InputBoxMinWidth < 0.0)
            {
                throw new ArgumentException("Cannot be less than 0.0.", "InputBoxMinWidth");
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposedValue)
            {
                if (disposing)
                {
                    LayoutUpdated -= InputBoxInternalDialog_LayoutUpdated;
                    SizeChanged -= InputBoxInternalDialog_SizeChanged;

                    if (cancelButton != null)
                        cancelButton.Click -= CancelButton_Click;

                    if (closeButton != null)
                        closeButton.Click -= CloseButton_Click;

                    if (okButton != null)
                        okButton.Click -= OkButton_Click;

                    if (titleThumb != null)
                        titleThumb.DragDelta -= TitleThumb_DragDelta;

                    if (resizeThumb != null)
                        resizeThumb.DragDelta -= ResizeThumb_DragDelta;
                }

                disposedValue = true;
            }
        }

        #endregion
    }
}
