namespace WPF.InternalDialogs
{
    public enum InternalDialogCloseFocusBehavior
    {
        FocusPreviousFocusedIInputElement = 0,
        FocusNextFocusableIInputElement = 1,
        FocusStepBakcwardsToFocusableIInputElement = 2,
        FocusFirstIInputElement = 3,
        FocusLastIInputElement = 4,
    }

    public enum InternalDialogResult
    {
        None = 0,
        Cancel = 1,
        Yes = 2,
        No = 3,
        Ok = 4
    }
}
