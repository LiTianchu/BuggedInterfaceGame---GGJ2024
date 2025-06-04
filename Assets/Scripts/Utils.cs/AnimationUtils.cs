using DG.Tweening;
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


    public static void AnimateZergSpawn(Zerg zerg, float spawnAnimationDuration)
    {
        // Start with scale 0 and fade 0
        Vector3 originalScale = zerg.transform.localScale;
        zerg.transform.localScale = Vector3.zero;
        zerg.StallSeconds(spawnAnimationDuration);
        SpriteRenderer spriteRenderer = zerg.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            Color originalColor = spriteRenderer.color;
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);

            // Animate scale with ease out back effect
            zerg.transform.DOScale(originalScale, spawnAnimationDuration)
                .SetEase(Ease.OutBack);

            // Animate fade in
            spriteRenderer.DOColor(new Color(originalColor.r, originalColor.g, originalColor.b, 1), spawnAnimationDuration)
                .SetEase(Ease.OutSine);
        }
        else
        {
            throw new System.Exception("Zerg prefab does not have a SpriteRenderer component. Please add one to the Zerg prefab.");
        }
    }
}