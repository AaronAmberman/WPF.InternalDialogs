using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace WPF.InternalDialogs
{
    // https://github.com/BenjaminGale/ModalContentPresenter/blob/master/ModalContentPresenter/ModalContentPresenter.cs

    /// <summary>Provides a base class for all other Internal Dialog types.</summary>
    public class InternalDialog : ContentControl
    {
        #region Fields

        private KeyboardNavigationMode cachedTabNavigationMode;
        private KeyboardNavigationMode cachedDirectionalNavigationMode;
        private IInputElement? cachedFocusedElement;
        private DispatcherFrame? frame;

        #endregion

        #region Properties

        /// <summary>Gets or sets whether or not the dialog will block upon opening (similar to ShowDialog). Default is false.</summary>
        public bool BlockUntilReturned
        {
            get { return (bool)GetValue(BlockUntilReturnedProperty); }
            set { SetValue(BlockUntilReturnedProperty, value); }
        }

        public static readonly DependencyProperty BlockUntilReturnedProperty =
            DependencyProperty.Register("BlockUntilReturned", typeof(bool), typeof(InternalDialog), new PropertyMetadata(false));

        /// <summary>Gets or sets the behavior to take when closing the dialog in regards to setting focus to content underneath.</summary>
        public InternalDialogCloseFocusBehavior CloseFocusBehavior
        {
            get { return (InternalDialogCloseFocusBehavior)GetValue(CloseFocusBehaviorProperty); }
            set { SetValue(CloseFocusBehaviorProperty, value); }
        }

        public static readonly DependencyProperty CloseFocusBehaviorProperty =
            DependencyProperty.Register("CloseFocusBehavior", typeof(InternalDialogCloseFocusBehavior), typeof(InternalDialog), 
                new PropertyMetadata(InternalDialogCloseFocusBehavior.FocusPreviousFocusedIInputElement));

        /// <summary>Gets or sets whether or not the dialog will close on escape key up.</summary>
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
        /// Gets or sets the UIElement used to pass focus back to upon close (Visibility.Collapsed). Not the 
        /// IInputElement from Keyboard.FocusedElement. Generally a root Grid, Panel or Border. This UIElement 
        /// is not used for positioning. UI placement is up to the front-end designer using the instance of 
        /// InternalDialog.
        /// </summary>
        /// <remarks>
        /// See VisibilityChanged for usage.
        /// </remarks>
        public UIElement FocusParent
        {
            get { return (UIElement)GetValue(FocusParentProperty); }
            set { SetValue(FocusParentProperty, value); }
        }

        public static readonly DependencyProperty FocusParentProperty =
            DependencyProperty.Register("FocusParent", typeof(UIElement), typeof(InternalDialog), new PropertyMetadata(null));

        /// <summary>gets or sets the result for the internal dialog.</summary>
        public InternalDialogResult Result
        {
            get { return (InternalDialogResult)GetValue(ResultProperty); }
            set { SetValue(ResultProperty, value); }
        }

        public static readonly DependencyProperty ResultProperty =
            DependencyProperty.Register("Result", typeof(InternalDialogResult), typeof(InternalDialog), new PropertyMetadata(InternalDialogResult.None));

        #endregion

        #region Constructors

        static InternalDialog()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(InternalDialog), new FrameworkPropertyMetadata(typeof(InternalDialog)));
            VisibilityProperty.OverrideMetadata(typeof(InternalDialog), new FrameworkPropertyMetadata(Visibility.Collapsed, VisibilityChanged));            
        }

        #endregion

        #region Methods

        private static void VisibilityChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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

                instance.KeyUp += instance.InternalDialog_KeyUp;

                instance.cachedTabNavigationMode = KeyboardNavigation.GetTabNavigation(instance.FocusParent);
                instance.cachedDirectionalNavigationMode = KeyboardNavigation.GetDirectionalNavigation(instance.FocusParent);

                instance.cachedFocusedElement = Keyboard.FocusedElement;

                KeyboardNavigation.SetTabNavigation(instance.FocusParent, KeyboardNavigationMode.None);
                KeyboardNavigation.SetDirectionalNavigation(instance.FocusParent, KeyboardNavigationMode.None);

                instance.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));

                if (instance.BlockUntilReturned)
                {
                    instance.frame = new DispatcherFrame();

                    Dispatcher.PushFrame(instance.frame);
                }
            }
            else // Collapsed
            {
                if (instance.BlockUntilReturned)
                {
                    if (instance.frame != null)
                    {
                        instance.frame.Continue = false;
                        instance.frame = null;
                    }
                }

                instance.KeyUp -= instance.InternalDialog_KeyUp;

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
            }
        }

        private void InternalDialog_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && CloseOnEscape)
            {
                Result = InternalDialogResult.Cancel;
                Visibility = Visibility.Collapsed;
            }
        }

        #endregion
    }
}
