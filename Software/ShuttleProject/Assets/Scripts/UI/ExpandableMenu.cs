using UnityEngine;

public class ExpandableMenu : MonoBehaviour
{
    /// <summary>
    ///     The bool variable of the animator
    /// </summary>
    private static readonly int Open = Animator.StringToHash("Open");
    
    /// <summary>
    ///     Indicates if a DropDown-Menu is open
    /// </summary>
    private bool open = false;
    
    /// <summary>
    ///     The Animator that is responsible for the DropDown-Menus
    /// </summary>
    public Animator animator;

    /// <summary>
    ///     Changes from open to closed and the other way round
    /// </summary>
    public void ChangeState()
    {
        open = !open;
        if (open)
        {
            animator.SetBool(Open, true);
        }
        else
        {
            animator.SetBool(Open, false);
        }
    }
}
