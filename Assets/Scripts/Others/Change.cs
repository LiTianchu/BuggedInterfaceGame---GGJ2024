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
        // Ensure both panels are active for transitions
        if (deactivated != null) deactivated.SetActive(true);
        if (activated != null) 
        {
            activated.SetActive(true);
            // Set the incoming panel to invisible state before transition
            SetInitialInvisibleState(activated);
        }

        // Phase 1: Transition out the current panel
        if (deactivated != null)
        {
            yield return TransitionOut(deactivated);
        }

        // Phase 2: Transition in the new panel (both are active now)
        if (activated != null)
        {
            yield return TransitionIn(activated);
        }

        // Phase 3: Clean up - deactivate the old panel
        if (deactivated != null) deactivated.SetActive(false);
    }

    private void SetInitialInvisibleState(GameObject panel)
    {
        CanvasGroup canvasGroup = GetOrAddCanvasGroup(panel);
        
        switch (transitionType)
        {
            case TransitionType.Fade:
                canvasGroup.alpha = 0f;
                break;
                
            case TransitionType.Scale:
                panel.transform.localScale = Vector3.zero;
                break;
                
            case TransitionType.Slide:
                // Move panel off-screen to the right
                Vector3 currentPos = panel.transform.localPosition;
                panel.transform.localPosition = new Vector3(currentPos.x + Screen.width, currentPos.y, currentPos.z);
                break;
                
            case TransitionType.FadeAndScale:
                canvasGroup.alpha = 0f;
                panel.transform.localScale = Vector3.one * 0.8f;
                break;
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
        
        // The initial state is already set by SetInitialInvisibleState()
        // Now animate to visible state
        switch (transitionType)
        {
            case TransitionType.Fade:
                yield return canvasGroup.DOFade(1f, transitionDuration).SetEase(easeType).WaitForCompletion();
                break;
                
            case TransitionType.Scale:
                yield return panel.transform.DOScale(1f, transitionDuration).SetEase(easeType).WaitForCompletion();
                break;
                
            case TransitionType.Slide:
                // Get the target position (where it should end up)
                Vector3 targetPos = GetOriginalPosition(panel);
                yield return panel.transform.DOLocalMove(targetPos, transitionDuration).SetEase(easeType).WaitForCompletion();
                break;
                
            case TransitionType.FadeAndScale:
                var sequence = DOTween.Sequence();
                sequence.Append(canvasGroup.DOFade(1f, transitionDuration).SetEase(easeType));
                sequence.Join(panel.transform.DOScale(1f, transitionDuration).SetEase(easeType));
                yield return sequence.WaitForCompletion();
                break;
        }
    }

    // Store original positions for slide transitions
    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();

    private Vector3 GetOriginalPosition(GameObject panel)
    {
        if (!originalPositions.ContainsKey(panel))
        {
            // Store the original position when first accessed
            originalPositions[panel] = panel.transform.localPosition;
        }
        return originalPositions[panel];
    }

    // Call this in Start() or Awake() to store original positions
    private void Start()
    {
        if (activated != null)
        {
            originalPositions[activated] = activated.transform.localPosition;
        }
        if (deactivated != null)
        {
            originalPositions[deactivated] = deactivated.transform.localPosition;
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
