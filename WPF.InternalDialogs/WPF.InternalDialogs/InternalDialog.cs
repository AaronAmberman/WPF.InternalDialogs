using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace WPF.InternalDialogs
{
    // https://github.com/BenjaminGale/ModalContentPresenter/blob/master/ModalContentPresenter/ModalContentPresenter.cs

    /// <summary>Provides a base class for all other Internal Dialog types. However, it is still usable by itself, just add your own content.</summary>
    public class InternalDialog : ContentControl
    {
        #region Fields

        private KeyboardNavigationMode cachedTabNavigationMode;
        private KeyboardNavigationMode cachedDirectionalNavigationMode;
        private IInputElement? cachedFocusedElement;
        private DispatcherFrame? frame;

        #endregion

        #region Properties
        
        /// <summary>Gets or sets the behavior to take when closing the dialog in regards to setting focus to content underneath.</summary>
        public InternalDialogCloseFocusBehavior CloseFocusBehavior
        {
            get { return (InternalDialogCloseFocusBehavior)GetValue(CloseFocusBehaviorProperty); }
            set { SetValue(CloseFocusBehaviorProperty, value); }
        }

        public static readonly DependencyProperty CloseFocusBehaviorProperty =
            DependencyProperty.Register("CloseFocusBehavior", typeof(InternalDialogCloseFocusBehavior), typeof(InternalDialog), 
                new PropertyMetadata(InternalDialogCloseFocusBehavior.FocusPreviousFocusedIInputElement));

        /// <summary>Gets or sets whether or not the dialog will close on escape key up. Default is true.</summary>
        public bool CloseOnEscape
        {
            get { return (bool)GetValue(CloseOnEscapeProperty); }
            set { SetValue(CloseOnEscapeProperty, value); }
        }

        public static readonly DependencyProperty CloseOnEscapeProperty =
            DependencyProperty.Register("CloseOnEscape", typeof(bool), typeof(InternalDialog), new PropertyMetadata(true));

        /// <summary>Gets or sets the padding for the content inside the border.</summary>
        public Thickness ContentPadding
        {
            get { return (Thickness)GetValue(ContentPaddingProperty); }
            set { SetValue(ContentPaddingProperty, value); }
        }

        public static readonly DependencyProperty ContentPaddingProperty =
            DependencyProperty.Register("ContentPadding", typeof(Thickness), typeof(InternalDialog), new PropertyMetadata(new Thickness(0)));

        /// <summary>Gets or sets the corner radius for the border.</summary>
        public CornerRadius CornerRadius
        {
            get { return (CornerRadius)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public static readonly DependencyProperty CornerRadiusProperty =
            DependencyProperty.Register("CornerRadius", typeof(CornerRadius), typeof(InternalDialog), new PropertyMetadata(new CornerRadius(0)));

        /// <summary>
        /// Gets or sets the UIElement used to borrow focus from / pass focus back to upon open / close (Visibility.Visible / 
        /// Visibility.Collapsed). Not the IInputElement from Keyboard.FocusedElement. Generally a root Grid, Panel or Border. 
        /// This UIElement is not used for positioning. UI placement is up to the front-end designer using the instance of 
        /// InternalDialog.
        /// </summary>
        /// <remarks>
        /// See VisibilityChanged for usage (it's all about the focus).
        /// </remarks>
        public UIElement FocusParent
        {
            get { return (UIElement)GetValue(FocusParentProperty); }
            set { SetValue(FocusParentProperty, value); }
        }

        public static readonly DependencyProperty FocusParentProperty =
            DependencyProperty.Register("FocusParent", typeof(UIElement), typeof(InternalDialog), new PropertyMetadata(null));

        /// <summary>Gets or sets whether or not the dialog will block upon opening (similar to ShowDialog). Default is false.</summary>
        public bool IsModal
        {
            get { return (bool)GetValue(IsModalProperty); }
            set { SetValue(IsModalProperty, value); }
        }

        public static readonly DependencyProperty IsModalProperty =
            DependencyProperty.Register("IsModal", typeof(bool), typeof(InternalDialog), new PropertyMetadata(false));

        /// <summary>gets or sets the result for the internal dialog.</summary>
        public MessageBoxResult Result
        {
            get { return (MessageBoxResult)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }

        public static readonly DependencyProperty ResultProperty =
            DependencyProperty.Register("Result", typeof(MessageBoxResult), typeof(InternalDialog), new PropertyMetadata(MessageBoxResult.None));

        #endregion

        #region Events

        public static readonly RoutedEvent PreviewVisibilityChangedEvent = EventManager.RegisterRoutedEvent(
            "PreviewVisibilityChanged", RoutingStrategy.Tunnel, typeof(RoutedEventHandler), typeof(InternalDialog));

        /// <summary>Occurs when the visibility changed.</summary>
        public event RoutedEventHandler PreviewVisibilityChanged
        {
            add { AddHandler(PreviewVisibilityChangedEvent, value); }
            remove { RemoveHandler(PreviewVisibilityChangedEvent, value); }
        }

        public static readonly RoutedEvent VisibilityChangedEvent = EventManager.RegisterRoutedEvent(
            "VisibilityChanged", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(InternalDialog));

        /// <summary>Occurs when the visibility changed.</summary>
        public event RoutedEventHandler VisibilityChanged
        {
            add { AddHandler(VisibilityChangedEvent, value); }
            remove { RemoveHandler(VisibilityChangedEvent, value); }
        }

        #endregion

        #region Constructors

        static InternalDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InternalDialog), new FrameworkPropertyMetadata(typeof(InternalDialog)));
            VisibilityProperty.OverrideMetadata(typeof(InternalDialog), new FrameworkPropertyMetadata(Visibility.Collapsed, VisibilityChangedCallback));            
        }

        #endregion

        #region Methods

        protected static void VisibilityChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InternalDialog? instance = d as InternalDialog;
            
            if (instance == null) return;

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
                if (instance.FocusParent == null)
                    throw new InvalidOperationException("FocusParent null");

                // raise preview event (tunneling)
                RoutedEventArgs args = new RoutedEventArgs(PreviewVisibilityChangedEvent);

                instance.RaiseEvent(args);

                // set key up for escape on close
                instance.KeyUp += instance.InternalDialog_KeyUp;

                // handle focus management
                instance.cachedTabNavigationMode = KeyboardNavigation.GetTabNavigation(instance.FocusParent);
                instance.cachedDirectionalNavigationMode = KeyboardNavigation.GetDirectionalNavigation(instance.FocusParent);

                instance.cachedFocusedElement = Keyboard.FocusedElement;

                KeyboardNavigation.SetTabNavigation(instance.FocusParent, KeyboardNavigationMode.None);
                KeyboardNavigation.SetDirectionalNavigation(instance.FocusParent, KeyboardNavigationMode.None);

                instance.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));

                // raise non preview event (bubbling)
                if (!args.Handled)
                {
                    args = new RoutedEventArgs(VisibilityChangedEvent);

                    instance.RaiseEvent(args);
                }

                // if modal...block
                if (instance.IsModal)
                {
                    instance.frame = new DispatcherFrame();

                    Dispatcher.PushFrame(instance.frame);
                }
            }
            else // Collapsed
            {
                // if modal...unblock
                if (instance.IsModal)
                {
                    if (instance.frame != null)
                    {
                        instance.frame.Continue = false;
                        instance.frame = null;
                    }
                }

                // raise preview event (tunneling)
                RoutedEventArgs args = new RoutedEventArgs(PreviewVisibilityChangedEvent);

                instance.RaiseEvent(args);

                // remove escape key up handler
                instance.KeyUp -= instance.InternalDialog_KeyUp;

                // reset focus to what it was before we were shown
                KeyboardNavigation.SetTabNavigation(instance.FocusParent, instance.cachedTabNavigationMode);
                KeyboardNavigation.SetDirectionalNavigation(instance.FocusParent, instance.cachedDirectionalNavigationMode);

                switch (instance.CloseFocusBehavior)
                {
                    case InternalDialogCloseFocusBehavior.FocusFirstIInputElement:
                        instance.FocusParent.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
                        break;
                    case InternalDialogCloseFocusBehavior.FocusLastIInputElement:
                        instance.FocusParent.MoveFocus(new TraversalRequest(FocusNavigationDirection.Last));
                        break;
                    case InternalDialogCloseFocusBehavior.FocusNextFocusableIInputElement:
                        instance.FocusParent.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                        break;
                    case InternalDialogCloseFocusBehavior.FocusStepBakcwardsToFocusableIInputElement:
                        instance.FocusParent.MoveFocus(new TraversalRequest(FocusNavigationDirection.Previous));
                        break;
                    case InternalDialogCloseFocusBehavior.FocusPreviousFocusedIInputElement:
                        Keyboard.Focus(instance.cachedFocusedElement);
                        break;
                }

                // raise non preview event (bubbling)
                if (!args.Handled)
                {
                    args = new RoutedEventArgs(VisibilityChangedEvent);

                    instance.RaiseEvent(args);
                }
            }
        }

        private void InternalDialog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && CloseOnEscape)
            {
                Result = MessageBoxResult.Cancel;
                Visibility = Visibility.Collapsed;
            }
        }

        #endregion
    }
}
