using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Eraser : Heart
{
    public GameObject[] toDeletePlayer; 
    public GameObject[] toDeleteEnemy; 
    public GameObject[] toDeleteCommon; 

    protected override void StickmanCollides()
    {
        DeletePlayerPowers();
    }

    protected override void ItemClicked()
    {
        if (stickman != null)
        {
            DeleteEnemyPowers();
        }
    }

    
    public void DeletePlayerPowers()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.scene.IsValid() && IsInstanceOfPrefab(obj, toDeletePlayer))
            {
                Destroy(obj);
            }
        }

        if (stickman != null && stickman.IsImmune())
        {
            stickman.DestroyImmunity();
        }
    }

    public void DeleteEnemyPowers()
    {
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.scene.IsValid() && IsInstanceOfPrefab(obj, toDeleteEnemy))
            {
                Destroy(obj);
            }
        }
    }
    
    bool IsInstanceOfPrefab(GameObject instance, GameObject[] prefabs)
    {
        // Checks if the object is an instance of the given prefab
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (instance.scene.IsValid() && instance.name.Contains(prefabs[i].name))
            {
                return true;
            }
        }
        return false;
    }
}