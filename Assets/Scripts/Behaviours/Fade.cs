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
        if(fadeMode == FadeMode.In)
        {
            GetComponent<CanvasGroup>().alpha = 0;
        }
        else
        {
            GetComponent<CanvasGroup>().alpha = 1;
        }
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _originalPos = _rectTransform.anchoredPosition;
        

        if(!fadeOnStart)
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

    private enum FadeMode
    {
        In,
        Out
    }


}
