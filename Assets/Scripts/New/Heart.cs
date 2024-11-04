using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Heart : MonoBehaviour, IPointerClickHandler
{
    private Stickman stickman; // Reference to the Stickman]
    public float timeToLive = 5.0f; // Time before the heart disappears

    void Start()
    {
        stickman = FindObjectOfType<Stickman>();
    }

    void Update()
    {
        timeToLive -= Time.deltaTime;
        if (timeToLive <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (stickman != null)
        {
            stickman.ReduceHealth(1); // Reduce health by 1
            Destroy(gameObject); // Destroy the heart
        }
    }
}