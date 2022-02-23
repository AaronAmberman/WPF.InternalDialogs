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

        /// <summary>Gets or sets the image source for the message box icon. Overrides what MessageBoxImage would set.</summary>
        public string ImageSource
        {
            get { return (string)GetValue(ImageSourceProperty); }
            set 
            { 
                SetValue(ImageSourceProperty, value);

                if (!isImageSourceInternallySet && !string.IsNullOrEmpty(value))
                {
                    // if the image source is set (if not empty) then we'll set image to None,
                    // if we didn't set it internally because of the MessageBoxImage property
                    MessageBoxImage = MessageBoxImage.None;
                }                
            }
        }

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(string), typeof(MessageBoxInternalDialog), new PropertyMetadata(0));

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

                // if no image then we have no work
                if (value == MessageBoxImage.None) return;

                isImageSourceInternallySet = true;

                switch (value)
                {
                    case MessageBoxImage.Error:
                        ImageSource = "pack://application:,,,/Error.png";
                        break;
                    case MessageBoxImage.Information:
                        ImageSource = "pack://application:,,,/Information.png";
                        break;
                    case MessageBoxImage.Question:
                        ImageSource = "pack://application:,,,/Question.png";
                        break;
                    case MessageBoxImage.Warning:
                        ImageSource = "pack://application:,,,/Warning.png";
                        break;
                }

                isImageSourceInternallySet = false;
            }
        }

        public static readonly DependencyProperty MessageBoxImageProperty =
            DependencyProperty.Register("MessageBoxImage", typeof(MessageBoxImage), typeof(MessageBoxInternalDialog), new PropertyMetadata(MessageBoxImage.None));

        /// <summary>Gets or sets the message to display in the dialog.</summary>
        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(MessageBoxInternalDialog), new PropertyMetadata(string.Empty));

        #endregion

        #region Constructors

        static MessageBoxInternalDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(MessageBoxInternalDialog), new FrameworkPropertyMetadata(typeof(MessageBoxInternalDialog)));
            VisibilityProperty.OverrideMetadata(typeof(InternalDialog), new FrameworkPropertyMetadata(Visibility.Collapsed, VisibilityChanged));
        }

        #endregion

        #region Methods

        new protected static void VisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            MessageBoxInternalDialog? instance = d as MessageBoxInternalDialog;

            if (instance == null) return;

            // call our base
            InternalDialog.VisibilityChanged(d, e);

            // do our work
            Visibility visibility = (Visibility)e.NewValue;

            // we will have Visible and Collapsed states, no Hidden
            if (visibility == Visibility.Hidden)
            {
                instance.Visibility = Visibility.Collapsed;

                // kick out and let us setting the new value make the below logic run,
                // we'll leave our callback (this if will not be hit next callback)
                return;
            }

            if (visibility == Visibility.Visible)
            {
                switch (instance.MessageBoxButton)
                {
                    case MessageBoxButton.OK:
                        if (instance.cancelButton != null)
                            instance.cancelButton.Visibility = Visibility.Collapsed;

                        if (instance.okButton != null)
                            instance.okButton.Visibility = Visibility.Visible;

                        if (instance.noButton != null)
                            instance.noButton.Visibility = Visibility.Collapsed;

                        if (instance.yesButton != null)
                            instance.yesButton.Visibility = Visibility.Collapsed;
                        break;
                    case MessageBoxButton.OKCancel:
                        if (instance.cancelButton != null)
                            instance.cancelButton.Visibility = Visibility.Visible;

                        if (instance.okButton != null)
                            instance.okButton.Visibility = Visibility.Visible;

                        if (instance.noButton != null)
                            instance.noButton.Visibility = Visibility.Collapsed;

                        if (instance.yesButton != null)
                            instance.yesButton.Visibility = Visibility.Collapsed;
                        break;
                    case MessageBoxButton.YesNo:
                        if (instance.cancelButton != null)
                            instance.cancelButton.Visibility = Visibility.Collapsed;

                        if (instance.okButton != null)
                            instance.okButton.Visibility = Visibility.Collapsed;

                        if (instance.noButton != null)
                            instance.noButton.Visibility = Visibility.Visible;

                        if (instance.yesButton != null)
                            instance.yesButton.Visibility = Visibility.Visible;
                        break;
                    case MessageBoxButton.YesNoCancel:
                        if (instance.cancelButton != null)
                            instance.cancelButton.Visibility = Visibility.Visible;

                        if (instance.okButton != null)
                            instance.okButton.Visibility = Visibility.Collapsed;

                        if (instance.noButton != null)
                            instance.noButton.Visibility = Visibility.Visible;

                        if (instance.yesButton != null)
                            instance.yesButton.Visibility = Visibility.Visible;
                        break;
                }
            }
            //else // Collapsed
            //{
                
            //}
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            cancelButton = GetTemplateChild("PART_CancelButton") as Button;
            okButton = GetTemplateChild("PART_OkButton") as Button;
            noButton = GetTemplateChild("PART_NoButton") as Button;
            yesButton = GetTemplateChild("PART_YesButton") as Button;
        }

        #endregion
    }
}
