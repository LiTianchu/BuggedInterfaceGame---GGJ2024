using DG.Tweening;
using UnityEngine;

//A singleton manager that is shared between scenes to control the general UI behavious of the project
public class UIManager : GlobalSingleton<UIManager>
{
    public void ToggleUI(GameObject component)
    {
        component.SetActive(!component.activeSelf);

    }

    public void ShowUI(GameObject component)
    {
        if (component == null)
        {
            Debug.Log("UI to show is null");
            return;
        }
        component.SetActive(true);
    }

    public void HideUI(GameObject component)
    {
        if (component == null)
        {
            Debug.Log("UI to hide is null");
            return;
        }
        component.SetActive(false);
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
