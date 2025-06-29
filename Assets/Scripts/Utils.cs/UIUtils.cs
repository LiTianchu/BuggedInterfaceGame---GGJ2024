using UnityEngine;

public class UIUtils
{ 
    
     public static CanvasGroup GetOrAddCanvasGroup(RectTransform panel)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = panel.gameObject.AddComponent<CanvasGroup>();
        }
        return canvasGroup;
    }
}