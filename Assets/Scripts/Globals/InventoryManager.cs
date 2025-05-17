using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : GlobalSingleton<InventoryManager>
{
    private int _keyCount;
    private int _coinCount;

    public int KeyCount { get { return _keyCount; } }
    public int CoinCount { get { return _coinCount; } }

    public event Action<int> OnKeyCountChanged;
    public event Action<int> OnCoinCountChanged;

    public void AddKey()
    {
        _keyCount++;
        OnKeyCountChanged?.Invoke(_keyCount);
    }

    public void AddCoin()
    {
        _coinCount++;
        OnCoinCountChanged?.Invoke(_coinCount);
    }

    /// <summary>
    /// <returns>True if coins were successfully used, false otherwise.</returns>
    /// </summary>
    /// <param name="amount">The amount of coins to use.</param>
    public bool UseCoin(int amount)
    {
        if (_coinCount >= amount)
        {
            _coinCount -= amount;
            OnCoinCountChanged?.Invoke(_coinCount);
            return true;
        }
        else
        {
            Debug.Log("Not enough coins");
            return false;
        }
    }
}
