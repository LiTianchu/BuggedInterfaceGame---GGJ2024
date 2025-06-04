using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

public class FileSystemLevelIntro : FileSystemLevel
{

    [TitleGroup("Zerg Settings")]
    [SerializeField] private int maxZergCount = 50;
    [SerializeField] private float smallZergSpawnRate = 1.0f;
    [SerializeField] private float bigZergSpawnRate = 5.0f;

   // [SerializeField] private Transform zergParent;


    public static readonly int ZERGS_TO_DESTROY = 50;
    private float _smallZergSpawnTimeElapsed;
    private float _bigZergSpawnTimeElapsed;

    public override int ZergDestroyedCount
    {
        get => _zergDestroyedCount;
        set
        {
            _zergDestroyedCount = value;
            if (_zergDestroyedCount >= ZERGS_TO_DESTROY)
            {
                _hasWon = true;
                FileSystemLevelManager.Instance.PublishCurrentLevelCleared();
                //OnAllZergDestroyed?.Invoke(); // Notify that all zergs are destroyed
            }
        }
    }
    //public event System.Action OnAllZergDestroyed;
    // Start is called before the first frame update
    public new void Start()
    {
        base.Start();
    }

    public void OnEnable()
    {
        if (!_hasWon)
        {
            _smallZergSpawnTimeElapsed = smallZergSpawnRate;
            _bigZergSpawnTimeElapsed = bigZergSpawnRate;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_hasWon)
        {
            _smallZergSpawnTimeElapsed += Time.deltaTime;
            _bigZergSpawnTimeElapsed += Time.deltaTime;

            // spawn small zergs
            if (_smallZergSpawnTimeElapsed >= smallZergSpawnRate && _zergCount < maxZergCount)
            {
                SpawnZerg();
                _smallZergSpawnTimeElapsed = 0.0f;
            }

            // spawn big zergs
            if (_bigZergSpawnTimeElapsed >= bigZergSpawnRate && _zergCount < maxZergCount)
            {
                SpawnZerg(ZergTypeEnum.Big);
                _bigZergSpawnTimeElapsed = 0.0f;
            }
        }
    }

    public void SpawnZerg(ZergTypeEnum zergType = ZergTypeEnum.Small)
    {
        // get a random point outside the grid
        Vector2 spawnPoint = VectorUtils.GetRandomPointOutsideBox(gridSystem.GridLowerLeft, gridSystem.GridUpperRight, 5f, 10f);
        Zerg zerg = null;
        switch (zergType)
        {
            case ZergTypeEnum.Small:
                zerg = FileSystemLevelManager.Instance.SmallZergPool.Get();
                zerg.Initialize(FileSystemLevelManager.Instance.SmallZergPool);
                break;
            case ZergTypeEnum.Big:
                zerg = FileSystemLevelManager.Instance.BigZergPool.Get();
                zerg.Initialize(FileSystemLevelManager.Instance.BigZergPool);
                break;
        }

        zerg.transform.SetPositionAndRotation(spawnPoint, Quaternion.identity);
        zerg.transform.SetParent(zergContainer);
        _zergCount++;
        zerg.gameObject.name = $"{zergType}-{_zergCount}";
        Debug.Log($"Spawned {zergType} zerg with index {_zergCount}");
    }

 
}
