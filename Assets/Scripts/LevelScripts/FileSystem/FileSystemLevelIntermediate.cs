using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;

public class FileSystemLevelIntermediate : FileSystemLevel
{
    [SerializeField] private ClusterSpawnEvent smallZergClusterSpawnEvent;
    [SerializeField] private RingSpawnEvent smallZergRingSpawnEvent;
    [SerializeField] private float zergRingSpawnInterval = 20f;

    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void CreateSpawnEvent(AbstractSpawnEvent spawnEvent, Vector3 position = default)
    {
            AbstractSpawnEvent se = Instantiate(spawnEvent, transform);
            se.Initialize(this,
                        FileSystemLevelManager.Instance.SmallZergPool,
                        zergContainer);
            se.Spawn();
            se.transform.position = position;
    }
}
