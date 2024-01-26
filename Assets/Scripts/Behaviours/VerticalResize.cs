using UnityEngine;

//Attach this to a UI element that you want to resize in vertical direction
//Need to manually set the top and bottom padding, item size and item spacing
public class VerticalResize : MonoBehaviour
{
    [SerializeField]
    private float topPadding;
    [SerializeField] 
    private float bottomPadding;
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
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, topPadding + bottomPadding + ChildCountActive() * (itemSize + itemSpacing) - itemSpacing); 
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
