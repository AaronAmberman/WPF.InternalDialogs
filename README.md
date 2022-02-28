# WPF.InternalDialogs
Easy to use LightBox-esque dialog "boxes" for WPF.

Inspired by https://github.com/BenjaminGale/ModalContentPresenter/blob/master/ModalContentPresenter/ModalContentPresenter.cs! Just built a whole message box suite on top of it.

## Why
1. Very easy to use!
2. Works like normal message boxes do...just no separate window.
    - We use the Visibility property. There is no Show or ShowDialog.
3. Can be shown modally or not.
    - Set the IsModal property.
4. Can be completely themed without having to retemplate the controls.
    - Can be retemplated if desired.
5. Proper look-less control template design so can be used in EDA or MVVM development.
    - Dependency properties make it so they can be assigned or bound to.
6. Can be dragged by user.
    - Bounds checking so the user if the user drags the dialog off screen then it snaps back into the interface a little bit. Enough to grab and move again.
7. Can be resized by user.
8. The MessageBoxInternalDialog has 23 different icons...not just 4.
9. The MovableResizableInternalDialog makes it very easy to make your own movable resizable internal dialog if you want completely custom user interfaces.
10. The resize gripper content can be whatever you want it to be as it is just a ContentPresenter. :) 

![DraggableMessageBox](https://user-images.githubusercontent.com/23512394/156051059-286e3e62-69a4-4089-8d26-3aea36da3c3c.gif)
