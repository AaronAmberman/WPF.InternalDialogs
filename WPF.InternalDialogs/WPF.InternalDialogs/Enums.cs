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

    public enum MessageBoxInternalDialogImage
    {
        None = 0,
        Alert = 1,
        Blocked = 2,
        CriticalError = 3,
        Downgrade = 4,
        Help = 5,
        Hidden = 6,
        Information = 7,
        Installed = 8,
        Invalid = 9,
        No = 10,
        Offline = 11,
        OK = 12,
        Pause = 13,
        Ready = 14,
        Required = 15,
        Run = 16,
        SecurityCritical = 17,
        SecurityOK = 18,
        SecurityWarning = 19,
        Stop = 20,
        Suppressed = 21,
        Update = 22,
        Warning = 23
    }
}
