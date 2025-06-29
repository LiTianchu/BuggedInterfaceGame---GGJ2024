using DG.Tweening;
using UnityEngine;

//A singleton manager that is shared between scenes to control the general UI behavious of the project
public class UIManager : GlobalSingleton<UIManager>
{
    public void ToggleUI(GameObject component,bool forceNoTransition = false)
    {
        if (forceNoTransition)
        {
            component.SetActive(!component.activeSelf);

        }
        else
        {
            UITransition transitionObject = component.GetComponent<UITransition>();
            if (transitionObject != null)
            {
                if (component.activeSelf)
                {
                    transitionObject.TransitionOut();
                }
                else
                {
                    transitionObject.TransitionIn();
                }
            }
            else
            {
                component.SetActive(!component.activeSelf);
            }
        }

    }

    public void ShowUI(GameObject component, bool forceNoTransition = false)
    {
        if (component == null)
        {
            Debug.Log("UI to show is null");
            return;
        }

        if (forceNoTransition)
        {
            component.SetActive(true);
        }
        else
        {
            UITransition transitionObject = component.GetComponent<UITransition>();
            if (transitionObject != null)
            {
                transitionObject.TransitionIn();
            }
            else
            {
                component.SetActive(true);
            }
        }
        
    }

    public void HideUI(GameObject component, bool forceNoTransition = false)
    {
        if (component == null)
        {
            Debug.Log("UI to hide is null");
            return;
        }
        
        if (forceNoTransition)
        {
            component.SetActive(false);
        }
        else
        {
            UITransition transitionObject = component.GetComponent<UITransition>();
            if (transitionObject != null)
            {
                transitionObject.TransitionOut();
            }
            else
            {
                component.SetActive(false);
            }
        }
    }

    public void BringToFront(GameObject component){
        component.transform.SetAsLastSibling();
    }

     public void UIFadeIn(CanvasGroup uiCanvasGroup, float fadeTime, Vector3 originAnchorPos, Vector3 targetAnchorPos)
    {
        RectTransform uiRect = uiCanvasGroup.gameObject.GetComponent<RectTransform>();
        uiCanvasGroup.alpha = 0f; //starting opacity
        uiRect.anchoredPosition = originAnchorPos; //starting position stays
        uiRect.DOAnchorPos(targetAnchorPos, fadeTime, false).SetEase(Ease.OutFlash); //flash in animation
        uiCanvasGroup.DOFade(1, fadeTime); //fade out animtion
    }

    public void UIFadeOut(CanvasGroup uiCanvasGroup, float fadeTime, Vector3 originAnchorPos, Vector3 targetAnchorPos)
    {
        RectTransform uiRect = uiCanvasGroup.gameObject.GetComponent<RectTransform>();
        uiCanvasGroup.alpha = 1f; //starting opacity
        uiRect.anchoredPosition = originAnchorPos;
        uiRect.DOAnchorPos(targetAnchorPos, fadeTime, false).SetEase(Ease.OutFlash); //flash in animation
        uiCanvasGroup.DOFade(0, fadeTime); //fade out animtion
    }

    public void UISlide(CanvasGroup uiCanvasGroup, float slideTime, Vector3 originAnchorPos, Vector3 targetAnchorPos)
    {
        RectTransform uiRect = uiCanvasGroup.gameObject.GetComponent<RectTransform>();
        uiRect.anchoredPosition = originAnchorPos;
        uiRect.DOAnchorPos(targetAnchorPos, slideTime, false).SetEase(Ease.OutFlash); //flash in animation
    }
}
