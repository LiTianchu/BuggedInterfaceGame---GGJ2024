using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyFile : FileSystemFile
{
    
    protected new void Start()
    {
        base.Start();
        //FileSystemLevelManager.Instance.OnLevelCleared += UnlockKey;
    }

    private void UnlockKey(FileSystemLevel level)
    {
        //if (level is not FileSystemLevelLast)
        //{
        //   return;
        //}
        
        if (Locked)
        {
            FileSystemLevelManager.Instance.OnLevelCleared -= UnlockKey;

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
        //InventoryManager.Instance.AddKey(1);
        Destroy(gameObject);
    }
}
