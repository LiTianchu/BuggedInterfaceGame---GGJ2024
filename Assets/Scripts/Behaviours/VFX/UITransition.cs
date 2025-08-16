using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using static AnimationUtils;

public class UITransition : MonoBehaviour
{
    [SerializeField] float transitionInDuration = 0.3f;
    [SerializeField] float transitionOutDuration = 0.3f;
    [SerializeField] TransitionType transitionType = TransitionType.Fade;
    [SerializeField] Ease easeType = Ease.OutQuad;

    [Header("Scale Transition Settings")]
    [SerializeField] Vector2 scalePivot = new Vector2(0.5f, 0.5f);

    [Header("Slide Transition Settings")]
    [SerializeField] Vector2 slideInFromPos = new Vector2(-10, 0);
    [SerializeField] Vector2 stayPos = new Vector2(0, 0);
    [SerializeField] Vector2 slideOutToPos = new Vector2(10, 0);
    [Header("Other Settings")]
    [SerializeField] private bool autoEnableBeforeTransitionIn = true;
    [SerializeField] private bool autoDisableAfterTransitionOut = true;
    [SerializeField] private bool setTransparentAfterTransitionOut = false;
    private SlideState _slideState = SlideState.None;
    private TransitionState _transitionState = TransitionState.BeforeIn;
    private RectTransform _rectTransform;

    public float TransitionInDuration => transitionInDuration;
    public float TransitionOutDuration => transitionOutDuration;
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        if (_rectTransform == null)
        {
            Debug.LogError("UITransition requires a RectTransform component.");
            return;
        }

        if (transitionType == TransitionType.Scale || transitionType == TransitionType.FadeAndScale)
        {
            _rectTransform.pivot = scalePivot;
        }
    }

    public void ToggleTransition()
    {
        if (_transitionState == TransitionState.BeforeIn || _transitionState == TransitionState.AfterOut)
        {
            TransitionIn();
        }
        else if (_transitionState == TransitionState.Stay)
        {
            TransitionOut();
        }
    }

    public void TransitionIn()
    {
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
            if (_rectTransform == null)
            {
                Debug.LogError("UITransition requires a _rectTransform component.");

            }
        }
        if (autoEnableBeforeTransitionIn)
        {
            _rectTransform.gameObject.SetActive(true);
            if (setTransparentAfterTransitionOut)
            {
                if (_rectTransform.TryGetComponent<CanvasGroup>(out var canvasGroup))
                {
                    canvasGroup.alpha = 1;
                }
                else
                {
                    Debug.LogWarning("CanvasGroup component not found on the GameObject. Cannot set transparency.");
                }
            }
        }
        StartCoroutine(TransitionInRoutine());
    }

    public void TransitionOut()
    {
        if(_transitionState != TransitionState.Stay)
        {
            Debug.Log("Cannot transition out when not in the Stay state.");
            return;
        }

        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
            if (_rectTransform == null)
            {
                Debug.LogError("UITransition requires a _rectTransform component.");

            }
        }
        StartCoroutine(TransitionOutRoutine());
    }

    public IEnumerator TransitionInRoutine()
    {

        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
            if (_rectTransform == null)
            {
                Debug.LogError("UITransition requires a _rectTransform component.");
                yield break;
            }
        }

        if (autoEnableBeforeTransitionIn)
        {
            _rectTransform.gameObject.SetActive(true);
        }
        _transitionState = TransitionState.BeforeIn;


        switch (transitionType)
        {
            case TransitionType.Fade:
                yield return UIFadeIn(_rectTransform,
                                        transitionInDuration,
                                        easeType);
                break;

            case TransitionType.Scale:
                yield return UIScaleIn(_rectTransform,
                                    transitionInDuration,
                                    easeType);
                break;

            case TransitionType.Slide:
                _slideState = SlideState.In;
                yield return UISlide(_rectTransform,
                                    slideInFromPos,
                                    stayPos,
                                    transitionInDuration,
                                    easeType);
                _slideState = SlideState.Stay;
                break;

            case TransitionType.FadeAndScale:
                yield return UIFadeAndScaleIn(_rectTransform,
                                                transitionInDuration,
                                                easeType);
                break;

            case TransitionType.NoTransition:
                // No transition
                break;

            default:
                Debug.LogWarning("Unknown transition type: " + transitionType);
                break;
        }
        _transitionState = TransitionState.Stay;
    }

    public IEnumerator TransitionOutRoutine()
    {
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
            if (_rectTransform == null)
            {
                Debug.LogError("UITransition requires a _rectTransform component.");
                yield break;
            }
        }
        if (_rectTransform == null)
        {
            Debug.LogError("UITransition requires a _rectTransform component.");
            yield break;
        }
        _transitionState = TransitionState.Stay;

        switch (transitionType)
        {
            case TransitionType.Fade:
                yield return UIFadeOut(_rectTransform,
                                        transitionOutDuration,
                                        easeType);
                break;

            case TransitionType.Scale:
                yield return UIScaleOut(_rectTransform,
                                        transitionOutDuration,
                                        easeType);
                break;

            case TransitionType.Slide:
                _slideState = SlideState.Stay;
                yield return UISlide(_rectTransform,
                                    stayPos,
                                    slideOutToPos,
                                    transitionOutDuration,
                                    easeType);
                _slideState = SlideState.Out;
                break;

            case TransitionType.FadeAndScale:
                yield return UIFadeAndScaleOut(_rectTransform,
                                                transitionOutDuration,
                                                easeType);
                break;

            case TransitionType.NoTransition:
                // No transition
                break;

            default:
                Debug.LogWarning("Unknown transition type: " + transitionType);
                break;
        }

        if (setTransparentAfterTransitionOut)
        {
            if (_rectTransform.TryGetComponent<CanvasGroup>(out var canvasGroup))
            {
                canvasGroup.alpha = 0;
            }
            else
            {
                Debug.LogWarning("CanvasGroup component not found on the GameObject. Cannot set transparency.");
            }
        }

        if (autoDisableAfterTransitionOut)
        {
            _rectTransform.gameObject.SetActive(false);
        }


        _transitionState = TransitionState.AfterOut;
    }

    public enum SlideState
    {
        None,
        In,
        Out,
        Stay,
    }

    public enum TransitionState
    {
        BeforeIn,
        Stay,
        AfterOut
    }
}
