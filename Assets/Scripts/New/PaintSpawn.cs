using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintSpawn : MonoBehaviour
{
    public GameObject stickmanPrefab; // Reference to the Stickman prefab
    public Canvas mainCanvas; // Reference to the main canvas

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // This method is called when the object becomes enabled and active
    void OnEnable()
    {
        SpawnStickman();
    }

    // Method to spawn the Stickman object
    void SpawnStickman()
    {
        if (stickmanPrefab != null && mainCanvas != null)
        {
            GameObject stickman = Instantiate(stickmanPrefab, mainCanvas.transform);
            stickman.transform.localPosition = Vector3.zero; // Adjust position as needed
        }
        else
        {
            Debug.LogError("Stickman prefab or main canvas is not assigned.");
        }
    }
}