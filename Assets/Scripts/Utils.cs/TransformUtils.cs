using UnityEngine;

public class TransformUtils
{
    public static Rect GetRectTransfromWorldCoord(RectTransform rectTransform)
    {
        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        Vector2 min = corners[0];
        Vector2 max = corners[2];
        return new Rect(min, max - min);
    }

    public static Vector2 GetRectTransformWorldCoordCenter(RectTransform rectTransform)
    {
        Rect rect = GetRectTransfromWorldCoord(rectTransform);
        return rect.center;
    }
}