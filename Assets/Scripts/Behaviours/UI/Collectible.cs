using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Collectible : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] private AutoRotate spriteRotator;
    [SerializeField] private LayerMask boundaryLayerMask;
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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if((1<<collision.gameObject.layer & boundaryLayerMask) != 0)
        {
            if (spriteRotator != null)
            {
                spriteRotator.SetRotationEnabled(true);
            }
        }
    }
}
