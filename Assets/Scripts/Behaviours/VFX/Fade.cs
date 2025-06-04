using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Fade : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration = 2.0f;
    [SerializeField]
    private FadeModeEnum fadeMode = FadeModeEnum.In;
    [SerializeField]
    private bool fadeOnStart = true;
    [SerializeField]
    private bool destroyAfterFadeOut = false;
    private Vector2 _originalPos;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;

    public float FadeDuration
    {
        get => fadeDuration;
        set => fadeDuration = value;
    }

    public FadeModeEnum FadeMode
    {
        get => fadeMode;
        set => fadeMode = value;
    }

    public bool FadeOnStart
    {
        get => fadeOnStart;
        set => fadeOnStart = value;
    }

    public bool DestroyAfterFadeOut
    {
        get => destroyAfterFadeOut;
        set => destroyAfterFadeOut = value;
    }

    public event System.Action OnFadeOutStarted;
    public event System.Action OnFadeInStarted;
    public event System.Action OnFadeOutCompleted;
    public event System.Action OnFadeInCompleted;


    void Awake()
    {

        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _originalPos = _rectTransform.anchoredPosition;

        ResetOpacity();

        if (!fadeOnStart)
        {
            return;
        }
        StartFade();

    }
    public void StartFade()
    {
        if (fadeMode == FadeModeEnum.In)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }
    }

    public void FadeIn(Vector2 startPosOffset = default)
    {
        StartCoroutine(FadeInCoroutine(startPosOffset));
    }

    private IEnumerator FadeInCoroutine(Vector2 startPosOffset = default)
    {
        if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        _originalPos = _rectTransform.anchoredPosition;
        UIManager.Instance.UIFadeIn(_canvasGroup, fadeDuration, _originalPos + startPosOffset, _originalPos);
        OnFadeInStarted?.Invoke();
        yield return new WaitForSeconds(fadeDuration);
        OnFadeInCompleted?.Invoke();
    }

    public void FadeOut(Vector2 targetPosOffset = default)
    {
        StartCoroutine(FadeOutCoroutine(targetPosOffset));
    }

    private IEnumerator FadeOutCoroutine(Vector2 targetPosOffset = default)
    {
         if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        _originalPos = _rectTransform.anchoredPosition;
        UIManager.Instance.UIFadeOut(_canvasGroup, fadeDuration, _originalPos, _originalPos + targetPosOffset);
        OnFadeOutStarted?.Invoke();
        yield return new WaitForSeconds(fadeDuration);
        OnFadeOutCompleted?.Invoke();
        if (destroyAfterFadeOut)
        {
            Destroy(gameObject);
        }
    }

    public void SetFadeTime(float time)
    {
        fadeDuration = time;
    }

    protected void ResetOpacity()
    {
        if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        if (fadeMode == FadeModeEnum.In)
        {
            _canvasGroup.alpha = 0;
        }
        else
        {
            _canvasGroup.alpha = 1;
        }
    }

    protected void SetOpacity(float opacity)
    {
        if (_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        _canvasGroup.alpha = opacity;
    }

    public enum FadeModeEnum
    {
        In,
        Out
    }


}
