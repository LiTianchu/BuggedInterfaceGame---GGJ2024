using UnityEngine;

public class AnimationUtils
{
    public static bool AnimatorIsPlaying(Animator animator)
    {
        if (animator == null)
        {
            Debug.LogWarning("Animator is null. Cannot check if animation is playing.");
            return false;
        }
        
        return animator.GetCurrentAnimatorStateInfo(0).length >
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }
}