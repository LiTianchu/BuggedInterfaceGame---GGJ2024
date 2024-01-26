using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attach this to a UI element that you want to resize horizontally based on the children
//Need to manually set the left and right padding, item size and item spacing
public class HorizontalResize : MonoBehaviour
{
    [SerializeField]
    private float leftPadding;
    [SerializeField]
    private float rightPadding;
    [SerializeField]
    private float itemSize;
    [SerializeField]
    private float itemSpacing;
    // Start is called before the first frame update
    void Start()
    {
        Resize();
    }

    public void Resize()
    {
        RectTransform rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(leftPadding + rightPadding + ChildCountActive() * (itemSize + itemSpacing) - itemSpacing, rect.sizeDelta.y);
    }

    public int ChildCountActive()
    {
        int k = 0;
        foreach (Transform c in transform)
        {
            if (c.gameObject.activeSelf)
                k++;
        }
        return k;
    }
}
