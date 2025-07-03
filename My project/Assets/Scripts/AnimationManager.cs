using System.Collections;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [Tooltip(
        "1. Place this script on the object that you want to animate.   " +
        "2. Go to the _Animator_ panel and make a transition from an empty state to the animation and vise versa.  " +
        "3. Select _Parameters_ in the top left corner and add a boolean.   " +
        "4. Name the boolean _Button_.   " +
        "5. Select the arrow going from the empty state to the animation, add a condition and set the condition for a true bool.   " +
        "6. Do the same for transition from the animation to the empty state, but make the bool false.   " +
        "7. Make the empty state the default.   " +
        "8. Drag this script to the _On Click_ field on the button you want to do the animation." +
        "9. Select _OnButtonClick_ method.   " +
        "10. If you need help, contact Ilian.")]
    public string Instructions = "Hover your mouse over this variable.";

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OnButtonClickEnable()
    {
        animator.SetBool("Button", true);
    }

    public void OnButtonClickDisable()
    {
        animator.SetBool("Button", false);
    }
}
