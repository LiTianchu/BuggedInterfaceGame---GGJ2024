using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using DG.Tweening; // Add this for DOTween

public class Change : MonoBehaviour
{
    [Header("Authentication Settings(Optional)")]
    [SerializeField] string username;
    [SerializeField] string password;
    [SerializeField] TMP_InputField usernameField; 
    [SerializeField] TMP_InputField passwordField;

    [Header("GameObjects to Activate/Deactivate upon changing displayed panel")]
    [SerializeField] GameObject deactivated;
    [SerializeField] GameObject activated;
    [SerializeField] bool hasTransition = true;

    [Header("Transition Settings")]
    [SerializeField] float transitionDuration = 0.5f;
    [SerializeField] TransitionType transitionType = TransitionType.Fade;
    [SerializeField] Ease easeType = Ease.OutQuad;

    public enum TransitionType
    {
        Fade,
        Scale,
        Slide,
        FadeAndScale
    }

    public void Replace()
    {
        bool authenticated = true;

        // Check authentication if fields are provided
        if (usernameField != null && passwordField != null)
        {
            authenticated = (usernameField.text == username && passwordField.text == password);
        }

        if (authenticated)
        {
            if (hasTransition)
            {
                StartCoroutine(ReplaceWithTransition());
            }
            else
            {
                // Immediate replacement without transition
                if (deactivated != null) deactivated.SetActive(false);
                if (activated != null) activated.SetActive(true);
            }
        }
    }

    private IEnumerator ReplaceWithTransition()
    {
        // Phase 1: Transition out the current panel
        if (deactivated != null)
        {
            yield return StartCoroutine(TransitionOut(deactivated));
        }

        // Phase 2: Switch panels
        if (deactivated != null) deactivated.SetActive(false);
        if (activated != null) activated.SetActive(true);

        // Phase 3: Transition in the new panel
        if (activated != null)
        {
            yield return StartCoroutine(TransitionIn(activated));
        }
    }

    private IEnumerator TransitionOut(GameObject panel)
    {
        CanvasGroup canvasGroup = GetOrAddCanvasGroup(panel);
        
        switch (transitionType)
        {
            case TransitionType.Fade:
                yield return canvasGroup.DOFade(0f, transitionDuration).SetEase(easeType).WaitForCompletion();
                break;
                
            case TransitionType.Scale:
                yield return panel.transform.DOScale(0f, transitionDuration).SetEase(easeType).WaitForCompletion();
                break;
                
            case TransitionType.Slide:
                Vector3 originalPos = panel.transform.localPosition;
                yield return panel.transform.DOLocalMoveX(originalPos.x - Screen.width, transitionDuration).SetEase(easeType).WaitForCompletion();
                break;
                
            case TransitionType.FadeAndScale:
                var sequence = DOTween.Sequence();
                sequence.Append(canvasGroup.DOFade(0f, transitionDuration).SetEase(easeType));
                sequence.Join(panel.transform.DOScale(0.8f, transitionDuration).SetEase(easeType));
                yield return sequence.WaitForCompletion();
                break;
        }
    }

    private IEnumerator TransitionIn(GameObject panel)
    {
        CanvasGroup canvasGroup = GetOrAddCanvasGroup(panel);
        
        // Set initial states
        switch (transitionType)
        {
            case TransitionType.Fade:
                canvasGroup.alpha = 0f;
                yield return canvasGroup.DOFade(1f, transitionDuration).SetEase(easeType).WaitForCompletion();
                break;
                
            case TransitionType.Scale:
                panel.transform.localScale = Vector3.zero;
                yield return panel.transform.DOScale(1f, transitionDuration).SetEase(easeType).WaitForCompletion();
                break;
                
            case TransitionType.Slide:
                Vector3 targetPos = panel.transform.localPosition;
                panel.transform.localPosition = new Vector3(targetPos.x + Screen.width, targetPos.y, targetPos.z);
                yield return panel.transform.DOLocalMove(targetPos, transitionDuration).SetEase(easeType).WaitForCompletion();
                break;
                
            case TransitionType.FadeAndScale:
                canvasGroup.alpha = 0f;
                panel.transform.localScale = Vector3.one * 0.8f;
                
                var sequence = DOTween.Sequence();
                sequence.Append(canvasGroup.DOFade(1f, transitionDuration).SetEase(easeType));
                sequence.Join(panel.transform.DOScale(1f, transitionDuration).SetEase(easeType));
                yield return sequence.WaitForCompletion();
                break;
        }
    }

    private CanvasGroup GetOrAddCanvasGroup(GameObject panel)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = panel.AddComponent<CanvasGroup>();
        }
        return canvasGroup;
    }

    // Alternative method without DOTween (using Unity's built-in Animation)
    private IEnumerator FadeTransitionLegacy(GameObject fadeOut, GameObject fadeIn)
    {
        if (fadeOut != null)
        {
            CanvasGroup outGroup = GetOrAddCanvasGroup(fadeOut);
            float elapsed = 0f;
            
            while (elapsed < transitionDuration)
            {
                elapsed += Time.deltaTime;
                outGroup.alpha = Mathf.Lerp(1f, 0f, elapsed / transitionDuration);
                yield return null;
            }
            
            fadeOut.SetActive(false);
        }

        if (fadeIn != null)
        {
            fadeIn.SetActive(true);
            CanvasGroup inGroup = GetOrAddCanvasGroup(fadeIn);
            inGroup.alpha = 0f;
            float elapsed = 0f;
            
            while (elapsed < transitionDuration)
            {
                elapsed += Time.deltaTime;
                inGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / transitionDuration);
                yield return null;
            }
        }
    }
}
