using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//This allows the current object to act like a tab that controls the showing and hiding of another UI object
public class Tab : MonoBehaviour, IPointerDownHandler
{
    [SerializeField]
    private List<GameObject> windowList;
    [SerializeField]
    private bool windowOpenedByDefault = true;
    [SerializeField]
    private Color windowOpenedColor;
    [SerializeField] 
    private Color windowClosedColor;
    [SerializeField]
    private bool isToggle = false;

    private Image _img;

    private void Start()
    {
        _img = GetComponent<Image>();
        if (windowOpenedByDefault) {
            ShowAllWindows(true);
        }
        else
        {
            HideAllWindows(true);
        }
    }
    private void Update()
    {
        if (windowList[0].activeSelf)
        {
            _img.color = windowOpenedColor;
        }else
        {
            _img.color = windowClosedColor;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (_img == null)
        {
            _img = GetComponent<Image>();   
        }
        if(isToggle){
            ToggleAllWindows(false);
        }else
        {
            ShowAllWindows(false);
        }
        if (windowList[0].activeSelf)
        {
            _img.color = windowOpenedColor;
        }else
        {
            _img.color = windowClosedColor;
        }
    }

    private void ShowAllWindows(bool noTransition = false){
        foreach (GameObject window in windowList)
        {
            if(!window.activeSelf){
                UIManager.Instance.ShowUI(window, noTransition);
            }
            UIManager.Instance.BringToFront(window);
        }
    }
    private void HideAllWindows(bool noTransition = false){
        foreach (GameObject window in windowList)
        {
            UIManager.Instance.HideUI(window,noTransition);
        }
    }
    private void ToggleAllWindows(bool noTransition = false){
        foreach (GameObject window in windowList)
        {
            UIManager.Instance.ToggleUI(window,noTransition);
            UIManager.Instance.BringToFront(window);
        }
    }

    
}
