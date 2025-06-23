using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Heart : MonoBehaviour, IPointerClickHandler
{
    protected Stickman stickman; // Reference to the Stickman
    public float timeToLive = 5.0f; // Time before the heart disappears

    public void Initialize(Stickman stickman)
    {
        this.stickman = stickman;
    }

    void Update()
    {
        timeToLive -= Time.deltaTime;
        if (IsStickmanColliding())
        {
            StickmanCollides();
            Destroy(gameObject);
        }
        if (timeToLive <= 0)
        {
            Destroy(gameObject);
        }
    }

    protected virtual void StickmanCollides()
    {
        stickman.ReduceHealth(-1); // Increase health by 1        
    }

    protected virtual void ItemClicked()
    {
        stickman.ReduceHealth(1); // Reduce health by 1
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (stickman != null)
        {
            ItemClicked(); 
            Destroy(gameObject);
        }
    }

    bool IsStickmanColliding()
    {
        RectTransform stickmanRect = stickman.GetComponent<RectTransform>();
        RectTransform heartRect = GetComponent<RectTransform>();

        Vector3[] stickmanCorners = new Vector3[4];
        Vector3[] heartCorners = new Vector3[4];

        stickmanRect.GetWorldCorners(stickmanCorners);
        heartRect.GetWorldCorners(heartCorners);

        Rect stickmanWorldRect = new Rect(stickmanCorners[0].x, stickmanCorners[0].y, stickmanCorners[2].x - stickmanCorners[0].x, stickmanCorners[2].y - stickmanCorners[0].y);
        Rect heartWorldRect = new Rect(heartCorners[0].x, heartCorners[0].y, heartCorners[2].x - heartCorners[0].x, heartCorners[2].y - heartCorners[0].y);

        return stickmanWorldRect.Overlaps(heartWorldRect);
    }
}