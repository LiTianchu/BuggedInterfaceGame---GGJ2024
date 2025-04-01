using UnityEngine;

public interface IDraggableArea
{
    public abstract Vector2[] GetBoundPoints();
    public abstract void HighlightDropArea(DropArea dropArea);
}
