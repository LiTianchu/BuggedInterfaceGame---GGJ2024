using System;
using UnityEngine;

public class BiosEntranceFile : FileSystemFile
{
    protected new void Start()
    {
        base.Start();
    }

    public void UnlockBiosEntrance()
    {
        if (Locked)
        {
            Locked = false;
            Debug.Log("BIOS Entrance unlocked");
        }
    }

    private void OnMouseDown()
    {
        if (Locked)
        {
            Debug.Log("Key is locked");
            return;
        }

        //InventoryManager.Instance.AddKey(3);
        LevelHubManager.Instance.StartLoadingBios();
        Destroy(gameObject);
    }

    public void Highlight()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 0, 1); // Yellow highlight
    }
    
    public void NormalColor()
    {
        GetComponent<SpriteRenderer>().color = Color.white; // Reset to normal color
    }
}