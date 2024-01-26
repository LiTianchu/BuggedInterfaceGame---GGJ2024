using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
