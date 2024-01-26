using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeIn : MonoBehaviour
{
    private Vector2 _originalPos;
    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _originalPos = _rectTransform.anchoredPosition;
        UIManager.Instance.UIFadeIn(GetComponent<CanvasGroup>(), 2f, _originalPos, _originalPos);
    }


}
