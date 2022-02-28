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

```xml
<id:MessageBoxInternalDialog x:Name="mbiDialog" Grid.Row="0" Grid.RowSpan="4"
                             Message="This is a simple message box!!!" 
                             Title="Message Box Example 1" MessageBoxImage="Information" MessageBoxButton="OK"
                             FocusParent="{Binding ElementName=rootGrid}" />
```      

See, very easy to use!

### Types
- InternalDialog
    - This is the base class for all the other types. This type allows content to be displayed in a light box modally. It just uses a ContentPresenter so there is no special visual management here.
- MovableResizableInternalDialog
    - This class allows developers to design and develop their own movable resizable content.
- InputBoxInternalDialog
    - This class allows developers to capture basic user input.
- MessageBoxInternalDialog
    - This is a message box that is an internal dialog.
- ProgressInternalDialog
    - This class display a simple message with a progress bar.
