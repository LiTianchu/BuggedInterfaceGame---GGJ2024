using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//attach this to a UI element that you want to show a hover hint
public class HoverHintOrigin : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [TextArea(5, 10)]
    [SerializeField]
    private string hintContent;
    [SerializeField]
    private RectTransform hintSpawnAnchor;
    [SerializeField]
    private float height;
    [SerializeField]
    private float width;

    
    public void SetHintContent(string content)
    {
        hintContent = content;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HoverHint.Instance.Width = width;
        HoverHint.Instance.Height = height;
        if (hintSpawnAnchor == null)
        {
            HoverHint.Instance.ShowHint(hintContent, this.transform.position,false);
        }
        else
        {
            HoverHint.Instance.ShowHint(hintContent, hintSpawnAnchor.transform.position, false);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideHint();
    }

    public void HideHint()
    {
        HoverHint.Instance.HideHint();
    }
}
