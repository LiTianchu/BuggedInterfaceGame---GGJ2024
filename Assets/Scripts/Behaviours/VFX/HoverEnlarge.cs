using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

//Attach this to a UI element that you want to enlarge when hovered over
public class HoverEnlarge : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private float multipier;
    [SerializeField]
    private AudioClip hoverSound;


    private Vector3 _originalScale;

    private void Start()
    {
        _originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //enlarge this 
        this.transform.localScale = _originalScale * multipier;
        if(hoverSound != null){
            AudioManager.Instance.PlaySFX(hoverSound);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        this.transform.localScale = _originalScale;
    }
}
