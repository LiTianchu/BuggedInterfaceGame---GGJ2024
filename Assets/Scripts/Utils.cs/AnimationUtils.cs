using System.Collections;
using DG.Tweening;
using UnityEngine;

public class AnimationUtils
{
    public enum TransitionType
    {
        Fade,
        Scale,
        Slide,
        FadeAndScale,
        NoTransition,
    }

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

    #region UI Fade Transition Methods

    public static IEnumerator UIFadeTransition(RectTransform deactivated, RectTransform activated, float duration, Ease easeType = Ease.OutQuad)
    {
        if (deactivated != null) deactivated.gameObject.SetActive(true);
        if (activated != null)
        {
            activated.gameObject.SetActive(true);
            UISetFadeInvisible(activated);
        }

        if (deactivated != null)
        {
            yield return UIFadeOut(deactivated, duration, easeType);
        }

        if (activated != null)
        {
            yield return UIFadeIn(activated, duration, easeType);
        }

        if (deactivated != null) deactivated.gameObject.SetActive(false);
    }

    public static void UISetFadeInvisible(RectTransform panel)
    {
        CanvasGroup canvasGroup = UIUtils.GetOrAddCanvasGroup(panel);
        canvasGroup.alpha = 0f;
    }

    public static IEnumerator UIFadeOut(RectTransform panel, float duration, Ease easeType = Ease.OutQuad)
    {
        yield return UIFade(panel, 1f, 0f, duration, easeType);
    }

    public static IEnumerator UIFadeIn(RectTransform panel, float duration, Ease easeType = Ease.OutQuad)
    {
        yield return UIFade(panel, 0f, 1f, duration, easeType);
    }

    public static IEnumerator UIFade(RectTransform panel, float initialAlpha, float endAlpha, float duration, Ease easeType = Ease.OutQuad)
    {
        CanvasGroup canvasGroup = UIUtils.GetOrAddCanvasGroup(panel);
        canvasGroup.alpha = initialAlpha;
        yield return canvasGroup.DOFade(endAlpha, duration).SetEase(easeType).WaitForCompletion();
    }

    public static IEnumerator UIFadeTo(RectTransform panel, float targetAlpha, float duration, Ease easeType = Ease.OutQuad)
    {
        CanvasGroup canvasGroup = UIUtils.GetOrAddCanvasGroup(panel);
        yield return canvasGroup.DOFade(targetAlpha, duration).SetEase(easeType).WaitForCompletion();
    }
    #endregion

    #region UI Scale Transition Methods

    public static IEnumerator UIScaleTransition(RectTransform deactivated, RectTransform activated, float duration, Ease easeType = Ease.OutBack)
    {
        if (deactivated != null) deactivated.gameObject.SetActive(true);
        if (activated != null)
        {
            activated.gameObject.SetActive(true);
            UISetScaleInvisible(activated);
        }

        if (deactivated != null)
        {
            yield return UIScaleOut(deactivated, duration, easeType);
        }

        if (activated != null)
        {
            yield return UIScaleIn(activated, duration, easeType);
        }

        if (deactivated != null) deactivated.gameObject.SetActive(false);
    }

    public static void UISetScaleInvisible(RectTransform panel)
    {
        panel.localScale = Vector3.zero;
    }

    public static IEnumerator UIScaleOut(RectTransform panel, float duration, Ease easeType = Ease.InBack)
    {
        yield return UIScale(panel, Vector3.one, Vector3.zero, duration, easeType);
    }

    public static IEnumerator UIScaleIn(RectTransform panel, float duration, Ease easeType = Ease.OutBack)
    {
        yield return UIScale(panel, Vector3.zero, Vector3.one, duration, easeType);
    }

    public static IEnumerator UIScale(RectTransform panel, Vector3 initialScale, Vector3 targetScale, float duration, Ease easeType = Ease.OutBack)
    {
        panel.localScale = initialScale;
        yield return panel.DOScale(targetScale, duration).SetEase(easeType).WaitForCompletion();
    }

    public static IEnumerator UIScaleTo(RectTransform panel, Vector3 targetScale, float duration, Ease easeType = Ease.OutBack)
    {
        yield return panel.DOScale(targetScale, duration).SetEase(easeType).WaitForCompletion();
    }

    #endregion

    #region UI Slide Transition Methods

    public static IEnumerator UISlideTransition(RectTransform deactivated, RectTransform activated, float duration, Vector2 inStartPos, Vector2 stayPos, Vector2 outTargetPos, Ease easeType = Ease.OutQuad)
    {
        if (deactivated != null) deactivated.gameObject.SetActive(true);
        if (activated != null)
        {
            activated.gameObject.SetActive(true);
            UISetSlideInvisible(activated, inStartPos);
        }

        if (deactivated != null)
        {

            yield return UISlideTo(deactivated, outTargetPos, duration, easeType);
        }

        if (activated != null)
        {
            yield return UISlideTo(activated, stayPos, duration, easeType);
        }

        if (deactivated != null) deactivated.gameObject.SetActive(false);
    }

    public static void UISetSlideInvisible(RectTransform panel, Vector2 hidePos)
    {
        // Set panel to start position (off-screen)
        panel.anchoredPosition = hidePos;
    }
    public static IEnumerator UISlide(RectTransform panel, Vector2 slideFromPos, Vector2 slideToPosition, float duration, Ease easeType = Ease.InQuad)
    {
        panel.anchoredPosition = slideFromPos;
        yield return panel.DOAnchorPos(slideToPosition, duration).SetEase(easeType).WaitForCompletion();
    }

    public static IEnumerator UISlideTo(RectTransform panel, Vector2 slideToPos, float duration, Ease easeType = Ease.OutQuad)
    {
        yield return panel.DOAnchorPos(slideToPos, duration).SetEase(easeType).WaitForCompletion();
    }

    #endregion

    #region UI Fade and Scale Methods

    public static IEnumerator UIFadeAndScaleTransition(RectTransform deactivated, RectTransform activated, float duration, Ease easeType = Ease.OutQuad)
    {
        if (deactivated != null) deactivated.gameObject.SetActive(true);
        if (activated != null)
        {
            activated.gameObject.SetActive(true);
            UISetFadeAndScaleInvisible(activated);
        }

        if (deactivated != null)
        {
            yield return UIFadeAndScaleOut(deactivated, duration, easeType);
        }

        if (activated != null)
        {
            yield return UIFadeAndScaleIn(activated, duration, easeType);
        }

        if (deactivated != null) deactivated.gameObject.SetActive(false);
    }

    public static void UISetFadeAndScaleInvisible(RectTransform panel)
    {
        CanvasGroup canvasGroup = UIUtils.GetOrAddCanvasGroup(panel);
        canvasGroup.alpha = 0f;
        panel.localScale = Vector3.one * 0.8f;
    }

    public static IEnumerator UIFadeAndScaleOut(RectTransform panel, float duration, Ease easeType = Ease.InQuad)
    {
        yield return UIFadeAndScale(panel, 1f, 0f, Vector3.one, Vector3.zero, duration, easeType);
    }

    public static IEnumerator UIFadeAndScaleIn(RectTransform panel, float duration, Ease easeType = Ease.OutQuad)
    {
        yield return UIFadeAndScale(panel, 0f, 1f, Vector3.zero, Vector3.one, duration, easeType);
    }

    public static IEnumerator UIFadeAndScale(RectTransform panel, float initialAlpha, float endAlpha, Vector3 initialScale, Vector3 targetScale, float duration, Ease easeType = Ease.OutQuad)
    {
        CanvasGroup canvasGroup = UIUtils.GetOrAddCanvasGroup(panel);
        canvasGroup.alpha = initialAlpha;
        panel.localScale = initialScale;

        yield return UIFadeAndScaleTo(panel, endAlpha, targetScale, duration, easeType);
    }

    public static IEnumerator UIFadeAndScaleTo(RectTransform panel, float endAlpha, Vector3 targetScale, float duration, Ease easeType = Ease.OutQuad)
    {
        CanvasGroup canvasGroup = UIUtils.GetOrAddCanvasGroup(panel);
        var sequence = DOTween.Sequence();
        sequence.Append(canvasGroup.DOFade(endAlpha, duration).SetEase(easeType));
        sequence.Join(panel.DOScale(targetScale, duration).SetEase(easeType));
        yield return sequence.WaitForCompletion();
    }

    #endregion


}