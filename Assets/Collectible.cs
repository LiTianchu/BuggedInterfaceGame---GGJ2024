using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Collectible : MonoBehaviour,IPointerDownHandler
{
    [SerializeField] private int coinAmount;
    [SerializeField] private bool destroyAfterCollect = true;

    public void OnPointerDown(PointerEventData eventData)
    {
        InventoryManager.Instance.AddCoin(coinAmount);

        if (destroyAfterCollect)
        {
            Destroy(gameObject);
        }

    }
}
