using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//This allows the current object to act like a tab that controls the showing and hiding of another UI object
public class Tab : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private GameObject window;
    [SerializeField]
    private bool windowOpenedByDefault = true;
    [SerializeField]
    private Color windowOpenedColor;
    [SerializeField] 
    private Color windowClosedColor;

    private Image _img;

    private void Start()
    {
        _img = GetComponent<Image>();
        if (windowOpenedByDefault) {
            UIManager.Instance.ShowUI(window);
            _img.color = windowOpenedColor;
        }
        else
        {
            UIManager.Instance.HideUI(window);
            _img.color = windowClosedColor;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_img == null)
        {
            _img = GetComponent<Image>();   
        }
        UIManager.Instance.ToggleUI(window);
        if (window.activeSelf)
        {
            _img.color = windowOpenedColor;
        }else
        {
            _img.color = windowClosedColor;
        }
    }

    
}