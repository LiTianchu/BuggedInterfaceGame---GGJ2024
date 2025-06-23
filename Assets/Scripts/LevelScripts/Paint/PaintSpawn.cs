using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintSpawn : MonoBehaviour
{
    public GameObject stickmanPrefab; // Reference to the Stickman prefab
    public Canvas mainCanvas; // Reference to the main canvas

    private Stickman _spawnedStickman; // Reference to the spawned Stickman

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

    void OnDisable()
    {
        if (_spawnedStickman != null)
        {
            Destroy(_spawnedStickman.gameObject);
            _spawnedStickman = null;
        }
    }

    // Method to spawn the Stickman object
    void SpawnStickman()
    {
        if (_spawnedStickman == null && stickmanPrefab != null && mainCanvas != null)
        {
            GameObject stickman = Instantiate(stickmanPrefab, transform);
            stickman.transform.localPosition = Vector3.zero; // Adjust position as needed
            _spawnedStickman = stickman.GetComponent<Stickman>();
        }
    }
}
