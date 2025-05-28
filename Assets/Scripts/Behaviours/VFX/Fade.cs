using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Fade : MonoBehaviour
{
    [SerializeField]
    private float fadeDuration = 2.0f;
    [SerializeField]
    private FadeMode fadeMode = FadeMode.In;
    [SerializeField]
    private bool fadeOnStart = true;
    private Vector2 _originalPos;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    // Start is called before the first frame update
    void Start()
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
    public void StartFade(){
        if(fadeMode == FadeMode.In)
        {
           UIManager.Instance.UIFadeIn(_canvasGroup, fadeDuration, _originalPos, _originalPos);
        }else{
            UIManager.Instance.UIFadeOut(_canvasGroup, fadeDuration, _originalPos, _originalPos);
        }
    }
    

    public void SetFadeTime(float time)
    {
        fadeDuration = time;
    }

    protected void ResetOpacity()
    {
        if(_canvasGroup == null)
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }
        
        if (fadeMode == FadeMode.In)
        {
            _canvasGroup.alpha = 0;
        }
        else
        {
            _canvasGroup.alpha = 1;
        }
    }

    private enum FadeMode
    {
        In,
        Out
    }


}
