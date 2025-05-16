using UnityEngine;

public class AdjustMinLayoutHeight : MonoBehaviour
{
    private int framesLeft = 3;

    public void Activate()
    {
        enabled = true;
        framesLeft = 3;
    }
    private void Update()
    {
        if (transform.childCount > 0)
        {
            var childRT = transform.GetChild(0).GetComponent<RectTransform>();
            var layoutElement = GetComponent<UnityEngine.UI.LayoutElement>();
            if (childRT != null && layoutElement != null)
            {
                layoutElement.minHeight = childRT.rect.height;
                framesLeft--;
                if (framesLeft <= 0) enabled = false;
            }
        }
    }
}
