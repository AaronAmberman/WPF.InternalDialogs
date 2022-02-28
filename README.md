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
                             Message="This is a simple message box." 
                             Title="Message Box Example 1" MessageBoxImage="Information" MessageBoxButton="OKCancel"
                             FocusParent="{Binding ElementName=rootGrid}" />
```      

See, very easy to use!

### Types
- **InternalDialog**
    - This is the base class for all the other types. This type allows content to be displayed in a light box modally. It just uses a ContentPresenter so there is no special visual management here.
- **MovableResizableInternalDialog**
    - This class allows developers to design and develop their own movable resizable content.
- **InputBoxInternalDialog**
    - This class allows developers to capture basic user input.
- **MessageBoxInternalDialog**
    - This is a message box that is an internal dialog.
- **ProgressInternalDialog**
    - This class displays a simple message with a progress bar.


### Class Overviews
- **InternalDialog**
    - *CloseFocusBehavior* - Gets or sets the behavior to take when closing the dialog in regards to setting focus to content underneath.
    - *CloseOnEscape* - Gets or sets whether or not the dialog will close on escape key up. Default is true.
    - *ContentPadding* - Gets or sets the padding for the content inside the border.
    - *CornerRadius* - Gets or sets the corner radius for the border.
    - ***FocusParent - Gets or sets the UIElement used to borrow focus from / pass focus back to upon open / close (Visibility.Visible / Visibility.Collapsed). Not the IInputElement from Keyboard.FocusedElement. Generally a root Grid, Panel or Border. This UIElement is not used for positioning. UI placement is up to the front-end designer using the instance of InternalDialog.***
    - *IsModal* - Gets or sets whether or not the dialog will block upon opening (similar to ShowDialog vs Show). Default is false.
    - *Result* - Gets or sets the result of the internal dialog.
        - For example the MessageBoxInternalDialog will set this to OK, Cancel, Yes or No accordingly.
- **InputBoxInternalDialog**
    - *ButtonAreaBackground* - Gets or sets the background for the button area.
    - *ButtonStyle* - Gets or sets the style to use for the buttons in the input box.
    - *CloseButtonStyle* - Gets or sets the style to use for the close button at the top right.
    - *Input* - Gets or sets the input to display in the text box portion of the input box.
    - *InputBoxAcceptsReturn* - Gets or sets whether or not the input text box accepts return. Default is false.
    - *InputBoxAcceptsTab* - Gets or sets whether or not the input text box accepts tabs. Default is false.
    - *InputBoxBackground* - Gets or sets the background for the content part of the movable resizable internal dialog. Not the same as Background.
    - *InputBoxMaxHeight* - Gets or sets the movable resizable internal dialog maximum height. Default is 600.0.
    - *InputBoxMaxWidth* - Gets or sets the movable resizable internal dialog maximum width. Default is 800.0.
    - *InputBoxMinHeight* - Gets or sets the movable resizable internal dialog minimum height. Default is 50.0.
    - *InputBoxMinWidth* - Gets or sets the movable resizable internal dialog minimum width. Default is 100.0.
    - *InputBoxMessage* - Gets or sets the message to display to the user.
    - *ResizeGripContent* - Gets or sets the content for the resize grip. Resize Grip is 18x18 and the top left slightly overlays the bottom right of the resizable area. Plan your visuals accordingly. There is also sometihng to know, the opacity for the whole resize grip area is .8 or 80% and on mouse over goes to 1.0 or 100%. This is so we can generically achieve a mouse over look. Plan your visuals accordingly.
    - *ResizeGripCursor* - Gets or sets the cursor for the resize gripper. Default is Cursors.SizeNWSE.
    - *ResizeGripVisibility* - Gets or sets the visibility of the resize grip. Visible = resizing enabled, Collapsed/Hidden = resizing disabled.
    - *TitleBackground* - Gets or sets the background for the title area.
    - *TitleContent* - Gets or sets the title content. The title content has IsHitTestVisible="False" set so the underlying Thumb can work.
    - *TitleCursor* - Gets or sets the cursor for the title area. Default is Cursors.SizeAll.
    - *TitleHorizontalAlignment* - Gets or sets the horizontal alignment of the title.
- **MovableResizableInternalDialog**
    - *AnswerAreaBackground* - Gets or sets the background for the answer area.
        - This would be where the buttons are
    - *AnswerAreaContent* - Gets or sets the answer area content.
    - *CloseButtonStyle* - Gets or sets the style to use for the close button at the top right.
    - *ContentBackground* - Gets or sets the background for the content part of the movable resizable internal dialog. Not the same as Background.
    - *ResizableMaxHeight* - Gets or sets the movable resizable internal dialog maximum height. Default is 600.0.
    - *ResizableMaxWidth* - Gets or sets the movable resizable internal dialog maximum width. Default is 800.0.
    - *ResizableMinHeight* - Gets or sets the movable resizable internal dialog minimum height. Default is 50.0.
    - *ResizableMinWidth* - Gets or sets the movable resizable internal dialog minimum width. Default is 100.0.
    - *ResizeGripContent* - Gets or sets the content for the resize grip. Resize Grip is 18x18 and the top left slightly overlays the bottom right of the resizable area. Plan your visuals accordingly. There is also sometihng to know, the opacity for the whole resize grip area is .8 or 80% and on mouse over goes to 1.0 or 100%. This is so we can generically achieve a mouse over look. Plan your visuals accordingly.
    - *ResizeGripCursor* - Gets or sets the cursor for the resize gripper. Default is Cursors.SizeNWSE.
    - *ResizeGripVisibility* - Gets or sets the visibility of the resize grip. Visible = resizing enabled, Collapsed/Hidden = resizing disabled.
    - *TitleBackground* - Gets or sets the background for the title area.
    - *TitleContent* - Gets or sets the title content. The title content has IsHitTestVisible="False" set so the underlying Thumb can work.
    - *TitleCursor* - Gets or sets the cursor for the title area. Default is Cursors.SizeAll.
    - *TitleHorizontalAlignment* - Gets or sets the horizontal alignment of the title.
- **MessageBoxInternalDialog**
    - *ButtonAreaBackground* - Gets or sets the background for the button area.
    - *ButtonStyle* - Gets or sets the style to use for the buttons in the input box.
    - *CloseButtonStyle* - Gets or sets the style to use for the close button at the top right.
    - *Message* - Gets or sets the message to display in the dialog.
    - *MessageBoxBackground* - Gets or sets the background for the message box part of the message box internal dialog. Not the same as Background.
    - *MessageBoxButton* - Gets or sets the message box buttons shown.
    - *MessageBoxImage* - Gets or sets the image for the message. This is not the same as System.Windows.MessageBoxImage so be careful to use WPF.InternalDialogs.MessageBoxInternalDialogImage.
    - *MessageBoxMaxHeight* - Gets or sets the movable resizable internal dialog maximum height. Default is 600.0.
    - *MessageBoxMaxWidth* - Gets or sets the movable resizable internal dialog maximum width. Default is 800.0.
    - *MessageBoxMinHeight* - Gets or sets the movable resizable internal dialog minimum height. Default is 50.0.
    - *MessageBoxMinWidth* - Gets or sets the movable resizable internal dialog minimum width. Default is 100.0.
    - *ResizeGripContent* - Gets or sets the content for the resize grip. Resize Grip is 18x18 and the top left slightly overlays the bottom right of the resizable area. Plan your visuals accordingly. There is also sometihng to know, the opacity for the whole resize grip area is .8 or 80% and on mouse over goes to 1.0 or 100%. This is so we can generically achieve a mouse over look. Plan your visuals accordingly.
    - *ResizeGripCursor* - Gets or sets the cursor for the resize gripper. Default is Cursors.SizeNWSE.
    - *ResizeGripVisibility* - Gets or sets the visibility of the resize grip. Visible = resizing enabled, Collapsed/Hidden = resizing disabled.
    - *TitleBackground* - Gets or sets the background for the title area.
    - *TitleContent* - Gets or sets the title content. The title content has IsHitTestVisible="False" set so the underlying Thumb can work.
    - *TitleCursor* - Gets or sets the cursor for the title area. Default is Cursors.SizeAll.
    - *TitleHorizontalAlignment* - Gets or sets the horizontal alignment of the title.

#### FocusParent
This property is required for every InternalDialog. It will throw an exception at runtime if this value is null. This property should be some kinf of root Grid, Border or Panel that contains the other focusable children. It should NOT be a singular IInputElement to pass focus back to or steal it from. You can choose various types of "re-focusing" strategies by setting *CloseFocusBehavior*. The default is to focus back to the control that had focus previously but you can choose first, last, next or previous as well.

#### IsModal
This property does what you would think and blocks code execution until returned. If you have a potential for multiple instances to of InternalDialogs to show at the same time that are both modal then just be sure of the effects of entering another event loop when you already have one running. This is doable but might have unintended consequences for you. How IsModal works...

We push a DispatcherFrame onto the Dispatcher so that it enters a new event loop. So just be wary when showing multiple InternalDialogs of its affects on your code.

##### Multiple Instances
Another thing to is that you should strive to have 1 InternalDialog per window that is used to represent the same thing. MessageBoxInternalDialog doesn't need to have 2 instances in the same window. You should just put it as the top most item in your XAML (so it covers all other visual objects). You only need one generic InternalDialog at runtime because its content can be dynamic and it should just be assigned programmatically or it should be bound to. Here is the thing, you can have as many as you want but if you try to pop open 2 that have IsModal on then you might start getting behavior you might not want to occur. Just be aware of how DispatcherFrames affect your code.

#### Customer Gripper Made Easy
We use a ContentPresenter to paint the resize gripper so that you can put whatever kind of content you'd like for the gripper. Just be mindful that it is an 18x18 area that ever so slightly overlays the bottom right of the content with its top left points.

![CustomGrip](https://user-images.githubusercontent.com/23512394/156063732-c20dbf8a-aa29-4545-91ee-dd72ea9374b2.png)

#### Custom Cursors
We thought as a part of styling you might want to change the drag cursor for the title and the resize gripper. *TitleCursor* and *ResizeGripCursor*, respectively, allow you to do that.

#### Custom Button Style
We also thought that you'd want the buttons in the InternDialogs to be restylable without having to retemplate the entire controls...so we added *CloseButtonStyle* and *ButtonStyle* to allow you to do just that.
