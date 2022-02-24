using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WPF.InternalDialogs
{
    /// <summary>An internal dialog message box that is very similar to the normal message box. This class cannot be inherited.</summary>
    [TemplatePart(Name = "PART_CancelButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_CloseButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_OkButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_NoButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_YesButton", Type = typeof(Button))]
    public sealed class MessageBoxInternalDialog : InternalDialog
    {
        #region Fields

        private Button? cancelButton;
        private Button? closeButton;
        private Button? okButton;
        private Button? noButton;
        private Button? yesButton;

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

        /// <summary>Gets or sets the message box maximum height.</summary>
        public double MessageBoxMaxHeight
        {
            get { return (double)GetValue(MessageBoxMaxHeightProperty); }
            set { SetValue(MessageBoxMaxHeightProperty, value); }
        }

        public static readonly DependencyProperty MessageBoxMaxHeightProperty =
            DependencyProperty.Register("MessageBoxMaxHeight", typeof(double), typeof(MessageBoxInternalDialog), new PropertyMetadata(Double.PositiveInfinity));

        /// <summary>Gets or sets the message box maximum width.</summary>
        public double MessageBoxMaxWidth
        {
            get { return (double)GetValue(MessageBoxMaxWidthProperty); }
            set { SetValue(MessageBoxMaxWidthProperty, value); }
        }

        public static readonly DependencyProperty MessageBoxMaxWidthProperty =
            DependencyProperty.Register("MessageBoxMaxWidth", typeof(double), typeof(MessageBoxInternalDialog), new PropertyMetadata(Double.PositiveInfinity));

        /// <summary>Gets or sets the message box minimum height.</summary>
        public double MessageBoxMinHeight
        {
            get { return (double)GetValue(MessageBoxMinHeightProperty); }
            set { SetValue(MessageBoxMinHeightProperty, value); }
        }

        public static readonly DependencyProperty MessageBoxMinHeightProperty =
            DependencyProperty.Register("MessageBoxMinHeight", typeof(double), typeof(MessageBoxInternalDialog), new PropertyMetadata(0.0));

        /// <summary>Gets or sets the message box minimum width.</summary>
        public double MessageBoxMinWidth
        {
            get { return (double)GetValue(MessageBoxMinWidthProperty); }
            set { SetValue(MessageBoxMinWidthProperty, value); }
        }

        public static readonly DependencyProperty MessageBoxMinWidthProperty =
            DependencyProperty.Register("MessageBoxMinWidth", typeof(double), typeof(MessageBoxInternalDialog), new PropertyMetadata(0.0));

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
        }

        #endregion

        #region Methods

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

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

        #endregion
    }
}
