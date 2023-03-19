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
7. Can be resized by user.
8. The MessageBoxInternalDialog has 23 different icons...not just 4.
9. The MovableResizableInternalDialog makes it very easy to make your own movable resizable internal dialog if you want completely custom user interfaces.
10. The resize gripper content can be whatever you want it to be as it is just a ContentPresenter. :) 

![InternalDialogs](https://user-images.githubusercontent.com/23512394/226147107-b689cd26-3500-46bb-b58f-79c0bcd99317.gif)

```xml
<id:MessageBoxInternalDialog x:Name="mbiDialog" Grid.Row="0" Grid.RowSpan="4"
                             Message="This is a simple message box." 
                             Title="Message Box Example 1" MessageBoxImage="Information" MessageBoxButton="OKCancel"
                             FocusParent="{Binding ElementName=rootGrid}" />
```      

See, very easy to use!

## Wiki
Please see the [wiki](https://github.com/AaronAmberman/WPF.InternalDialogs/wiki) for type break down.

## Additional Insight
#### FocusParent
This property is required for every InternalDialog. It will throw an exception at runtime if this value is null. This property should be some kind of root Grid, Border or Panel that contains the other focusable children. It should NOT be a singular IInputElement to pass focus back to or steal it from. You can choose various types of "re-focusing" strategies by setting *CloseFocusBehavior*. The default is to focus back to the control that had focus previously but you can choose first, last, next or step backwards as well.

#### IsModal
This property does what you would think and blocks code execution until returned. If you have a potential for multiple instances of InternalDialogs to show at the same time that are both modal then just be sure of the effects of entering another event loop when you already have one running. This is doable but might have unintended consequences for you. How IsModal works...

We push a DispatcherFrame onto the Dispatcher so that it enters a new event loop. So just be wary when showing multiple InternalDialogs of its effects on your code.

##### Multiple Instances
Another thing to is that you should strive to have 1 InternalDialog per window that is used to represent the same thing. MessageBoxInternalDialog doesn't need to have 2 instances in the same window. You should just put it as the top most item in your XAML (so it covers all other visual objects). You only need one generic InternalDialog at runtime because its content can be dynamic and it should just be assigned programmatically or it should be bound to. Here is the thing, you can have as many as you want but if you try to pop open 2 that have IsModal on then you might start getting behavior you might not want to occur. Just be aware of how DispatcherFrames affect your code.

#### Visibility
***Visibility of internal dialogs binds two way by default.***

#### Customer Gripper Made Easy
We use a ContentPresenter to paint the resize gripper so that you can put whatever kind of content you'd like for the gripper. Just be mindful that it is an 18x18 area that ever so slightly overlays the bottom right of the content with its top left points.

![CustomGrip](https://user-images.githubusercontent.com/23512394/156063732-c20dbf8a-aa29-4545-91ee-dd72ea9374b2.png)

#### Custom Cursors
We thought as a part of styling you might want to change the drag cursor for the title and the resize gripper. *TitleCursor* and *ResizeGripCursor*, respectively, allow you to do that.

#### Custom Button Styles
We also thought that you'd want the buttons in the InternDialogs to be restylable without having to retemplate the entire control...so we added *CloseButtonStyle* and *ButtonStyle* to allow you to do just that. Please be aware that when you change the *CloseButtonStyle* you will have to specify your own content. This is not true for *ButtonStyle*. The bottom buttons will always have the content they have. If you'd like custom buttons then please consider using MovableResizableInternalDialog that allows for custom *AnswerAreaContent*.

#### ProgressInternalDialog
We gave you a *ProgressBarStyle* property so custom styling can be applied to the progress bar as well. The thing to note about the ProgressInternalDialog is that even though you can set the style for progress bar the value for it (if not IsIndeterminate) is still managed by the PorgressInternalDialog. Use the *ProgressValue* property to manage this.

#### Close On Escape Key Push?
Yup, we got that!

#### Dispose
Internal Dialogs implement IDisposable so be sure to clean up properly by calling Dispose().

#### Hope you enjoy. Happy coding!
![MBID-Example1](https://user-images.githubusercontent.com/23512394/156067308-d8b9651f-8248-497d-9235-ffd9cad4cf39.png)
![MBID-Example2](https://user-images.githubusercontent.com/23512394/156067331-b7633be1-b45c-4754-9255-e94cb17b3d3f.png)
