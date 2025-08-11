using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ResponseNotification : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Color colorWhenAlert = Color.white;
    [SerializeField] private float colorTransitInTime = 0.2f;
    [SerializeField] private float colorStayTime = 2f;
    [SerializeField] private float colorTransitOutTime = 1f;

    private Color _originalColor;
    private Sequence _sequence;
    public void Start()
    {
        _originalColor = image.color;
    }

    public void ShowAlert()
    {
        if(_sequence != null && _sequence.IsActive())
        {
            _sequence.Kill();
            _sequence = null;
        }

        image.color = _originalColor;

        _sequence = DOTween.Sequence()
            .Append(image.DOColor(colorWhenAlert, colorTransitInTime).SetEase(Ease.OutQuad))
            .Append(image.DOColor(_originalColor, colorTransitOutTime).SetEase(Ease.InQuad).SetDelay(colorStayTime)).OnComplete(()=>
            { 
                _sequence = null;
            });
    }


}
