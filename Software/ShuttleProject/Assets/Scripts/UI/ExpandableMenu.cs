using UnityEngine;

public class ExpandableMenu : MonoBehaviour
{
    /// <summary>
    ///     The bool variable of the animator
    /// </summary>
    private static readonly int Open = Animator.StringToHash("Open");
    
    /// <summary>
    ///     
    /// </summary>
    private static bool open = false;
    
    /// <summary>
    /// 
    /// </summary>
    public Animator animator;

    /// <summary>
    /// 
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
