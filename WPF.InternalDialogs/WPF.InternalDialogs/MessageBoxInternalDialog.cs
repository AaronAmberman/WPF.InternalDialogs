using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF.InternalDialogs
{
    [TemplatePart(Name = "PART_CancelButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_OkButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_NoButton", Type = typeof(Button))]
    [TemplatePart(Name = "PART_YesButton", Type = typeof(Button))]
    public class MessageBoxInternalDialog : InternalDialog
    {
        #region Fields

        private bool isImageSourceInternallySet = false;
        private Button? cancelButton;
        private Button? okButton;
        private Button? noButton;
        private Button? yesButton;

        #endregion

        #region Properties

        /// <summary>gets or sets the style to use for the buttons in the message box.</summary>
        public Style ButtonStyle
        {
            get { return (Style)GetValue(ButtonStyleProperty); }
            set { SetValue(ButtonStyleProperty, value); }
        }

        public static readonly DependencyProperty ButtonStyleProperty =
            DependencyProperty.Register("ButtonStyle", typeof(Style), typeof(MessageBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the content for the image section of the message box internal dialog.</summary>
        public object ImageContent
        {
            get { return (object)GetValue(ImageContentProperty); }
            set { SetValue(ImageContentProperty, value); }
        }

        public static readonly DependencyProperty ImageContentProperty =
            DependencyProperty.Register("ImageContent", typeof(object), typeof(MessageBoxInternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets the image source for the message box icon. Overrides what MessageBoxImage would set.</summary>
        public Uri ImageSource
        {
            get { return (Uri)GetValue(ImageSourceProperty); }
            set 
            { 
                SetValue(ImageSourceProperty, value);

                //if (!isImageSourceInternallySet && !string.IsNullOrEmpty(value.AbsoluteUri))
                //{
                //    // if the image source is set (if not empty) then we'll set image to None,
                //    // if we didn't set it internally because of the MessageBoxImage property
                //    MessageBoxImage = MessageBoxImage.None;
                //}                
            }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(Uri), typeof(MessageBoxInternalDialog), 
                new PropertyMetadata(new Uri("pack://application:,,,/Error.png", UriKind.Absolute)));

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

        /// <summary>
        /// Gets or sets the image to use for the icon. If ImageSource is set then that source is used and this is ignored. However, 
        /// it is up to the developer to change the image for different states...if so desired. If ImageSource is set then this is 
        /// set to MessageBoxImage.None. Default is None.
        /// </summary>
        public MessageBoxImage MessageBoxImage
        {
            get { return (MessageBoxImage)GetValue(MessageBoxImageProperty); }
            set 
            { 
                SetValue(MessageBoxImageProperty, value);

                //switch (value)
                //{
                //    case MessageBoxImage.None:
                //        if (!isImageSourceInternallySet) // if this was set by the user then set image source to none
                //        {
                //            ImageSource = new Uri("");
                //        }
                //        break;
                //    case MessageBoxImage.Error:
                //        ImageSource = new Uri("pack://application:,,,/Error.png", UriKind.Absolute);
                //        break;
                //    case MessageBoxImage.Information:
                //        ImageSource = new Uri("pack://application:,,,/Information.png", UriKind.Absolute);
                //        break;
                //    case MessageBoxImage.Question:
                //        ImageSource = new Uri("pack://application:,,,/Question.png", UriKind.Absolute);
                //        break;
                //    case MessageBoxImage.Warning:
                //        ImageSource = new Uri("pack://application:,,,/Warning.png", UriKind.Absolute);
                //        break;
                //}
            }
        }

        public static readonly DependencyProperty MessageBoxImageProperty =
            DependencyProperty.Register("MessageBoxImage", typeof(MessageBoxImage), typeof(MessageBoxInternalDialog), new PropertyMetadata(MessageBoxImage.Question));

        /// <summary>Gets or sets the message to display in the dialog.</summary>
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(MessageBoxInternalDialog), new PropertyMetadata(string.Empty));

        /// <summary>Gets or sets the title to the message box.</summary>
        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(MessageBoxInternalDialog), new PropertyMetadata(string.Empty));

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
            //VisibilityProperty.OverrideMetadata(typeof(MessageBoxInternalDialog), new FrameworkPropertyMetadata(Visibility.Collapsed, VisibilityChangedCallback));
        }

        #endregion

        #region Methods

        //new protected static void VisibilityChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    MessageBoxInternalDialog? instance = d as MessageBoxInternalDialog;

        //    if (instance == null) return;

        //    // call our base
        //    InternalDialog.VisibilityChangedCallback(d, e);

        //    // do our work
        //    Visibility visibility = (Visibility)e.NewValue;

        //    // we will have Visible and Collapsed states, no Hidden
        //    if (visibility == Visibility.Hidden)
        //    {
        //        instance.Visibility = Visibility.Collapsed;

        //        // kick out and let us setting the new value make the below logic run,
        //        // we'll leave our callback (this if will not be hit next callback)
        //        return;
        //    }

        //    //if (visibility == Visibility.Visible)
        //    //{
                
        //    //}
        //    //else // Collapsed
        //    //{

        //    //}
        //}

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            cancelButton = GetTemplateChild("PART_CancelButton") as Button;
            okButton = GetTemplateChild("PART_OkButton") as Button;
            noButton = GetTemplateChild("PART_NoButton") as Button;
            yesButton = GetTemplateChild("PART_YesButton") as Button;

            if (cancelButton != null)
                cancelButton.Click += CancelButton_Click;

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
