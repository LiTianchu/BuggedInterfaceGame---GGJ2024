using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinAmountView : MonoBehaviour
{
    [SerializeField] private TMP_Text coinAmountText;

    void OnEnable()
    {
        InventoryManager.Instance.OnCoinCountChanged += UpdateCoinAmount;
        UpdateCoinAmount(InventoryManager.Instance.CoinCount);
    }

    void OnDisable()
    {
        InventoryManager.Instance.OnCoinCountChanged -= UpdateCoinAmount;
    }

    private void UpdateCoinAmount(int amount)
    {
        coinAmountText.text = amount.ToString();
    }
}
