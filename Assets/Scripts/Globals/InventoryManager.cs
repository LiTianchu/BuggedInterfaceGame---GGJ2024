using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : GlobalSingleton<InventoryManager>
{
    private int _keyCount;
    private int _coinCount;
    private Dictionary<TurretFile, TurretStateEnum> _turretFiles = new();

    public int KeyCount { get { return _keyCount; } }
    public int CoinCount { get { return _coinCount; } }
    public Dictionary<TurretFile, TurretStateEnum> TurretFiles { get { return _turretFiles; } }

    public event Action<int> OnKeyCountChanged;
    public event Action<int> OnCoinCountChanged;
    public event Action<Dictionary<TurretFile, TurretStateEnum>> OnTurretInventoryChanged;

   

    public void AddKey(int amount = 1)
    {
        _keyCount += amount;
        OnKeyCountChanged?.Invoke(_keyCount);
        Debug.Log("Added key");
        Debug.Log($"Keys: {_keyCount}");
    }

    public void AddCoin(int amount = 1)
    {
        _coinCount += amount;
        OnCoinCountChanged?.Invoke(_coinCount);
        Debug.Log("Added coin");    
        Debug.Log($"Coins: {_coinCount}");
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
            Debug.Log($"Used {amount} coins");
            Debug.Log($"Coins: {_coinCount}");
            return true;
        }
        else
        {
            Debug.Log("Not enough coins");
            return false;
        }
    }

    public void UpdateTurretFile(TurretFile turretFile, TurretStateEnum turretState = TurretStateEnum.Locked)
    {
        if (_turretFiles.ContainsKey(turretFile))
        {
            if (_turretFiles[turretFile] == turretState)
            {
                Debug.Log($"Turret {turretFile} already in {turretState} state");
                return;
            }
            _turretFiles[turretFile] = turretState;
        }
        else
        {
            _turretFiles.Add(turretFile, turretState);
        }
        OnTurretInventoryChanged?.Invoke(_turretFiles);
    }
}

public enum TurretStateEnum
{
    Locked,
    Unlocked,
    Using
}
