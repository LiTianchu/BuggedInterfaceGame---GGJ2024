using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFile : FileSystemFile
{
    protected new void Start()
    {
        base.Start();
        FileSystemLevelManager.Instance.OnAllZergDestroyed += UnlockKey;
    }

    private void UnlockKey()
    {
        if (Locked)
        {
            FileSystemLevelManager.Instance.OnAllZergDestroyed -= UnlockKey;

            Locked = false;
            Debug.Log("Key unlocked");
        }
    }

    private void OnMouseDown()
    {
        if (Locked)
        {
            Debug.Log("Key is locked");
            return;
        }
        InventoryManager.Instance.AddKey(1);
        Destroy(gameObject);
    }
}
