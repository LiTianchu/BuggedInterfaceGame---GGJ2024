using System.Collections;
using System.Collections.Generic;
using System.Data;
using PixelCrushers.DialogueSystem;
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
        if (coinAmount > 0)
        {
            DialogueLua.SetVariable("TotalCoinCollected", DialogueLua.GetVariable("TotalCoinCollected").asInt + coinAmount);
            Debug.Log($"Collected {coinAmount} coins. Total Collected Till Now: {DialogueLua.GetVariable("TotalCoinCollected").asInt}");

            if(DialogueLua.GetVariable("TotalCoinCollected").asInt == 1)
            {
                DialogueManager.StopAllConversations(); // replace
                DialogueManager.StartConversation("First Picked Up Coin");
            }

            InventoryManager.Instance.AddCoin(coinAmount);
        }

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
