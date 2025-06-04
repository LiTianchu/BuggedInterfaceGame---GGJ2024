using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

public class FileSystemLevelIntro : FileSystemLevel
{

    [TitleGroup("Zerg Settings")]
    [SerializeField] private int maxZergCount = 50;
    [SerializeField] private float smallZergSpawnRate = 1.0f;
    [SerializeField] private float bigZergSpawnRate = 5.0f;
    [SerializeField] private Zerg smallZergPrefab;
    [SerializeField] private Zerg bigZergPrefab;
    [SerializeField] private Transform zergParent;


    public static readonly int ZERGS_TO_DESTROY = 50;
    private float _smallZergSpawnTimeElapsed;
    private float _bigZergSpawnTimeElapsed;
    private int _zergDestroyedCount = 0;
    public int ZergDestroyedCount
    {
        get => _zergDestroyedCount;
        set
        {
            _zergDestroyedCount = value;
            if (_zergDestroyedCount >= ZERGS_TO_DESTROY)
            {
                _hasWon = true;
                OnAllZergDestroyed?.Invoke(); // Notify that all zergs are destroyed
            }
        }
    }
    public event System.Action OnAllZergDestroyed;
    // Start is called before the first frame update
    public new void Start()
    {
        _smallZergSpawnTimeElapsed = smallZergSpawnRate;
        _bigZergSpawnTimeElapsed = bigZergSpawnRate;
        CreateZergPools();
        base.Start();
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
                zerg = _smallZergPool.Get();
                zerg.Initialize(_smallZergPool);
                break;
            case ZergTypeEnum.Big:
                zerg = _bigZergPool.Get();
                zerg.Initialize(_bigZergPool);
                break;
        }

        zerg.transform.SetPositionAndRotation(spawnPoint, Quaternion.identity);
        zerg.transform.SetParent(zergParent);
        _zergCount++;
        zerg.gameObject.name = $"{zergType}-{_zergCount}";
        Debug.Log($"Spawned {zergType} zerg with index {_zergCount}");
    }
    
    // object pools
    private ObjectPool<Zerg> _smallZergPool;
    private ObjectPool<Zerg> _bigZergPool;

    private void CreateZergPools()
    {
        _smallZergPool = new ObjectPool<Zerg>(
            CreateSmallZerg,
            OnTakeZergFromPool,
            OnReturnZergToPool,
            OnDestroyZerg,
            false,
            10, // initial size
            1000 // max size
        );

        _bigZergPool = new ObjectPool<Zerg>(
            CreateBigZerg,
            OnTakeZergFromPool,
            OnReturnZergToPool,
            OnDestroyZerg,
            false,
            10, // initial size
            1000 // max size
        );
    }

    private Zerg CreateSmallZerg()
    {
        Zerg zerg = Instantiate(smallZergPrefab);
        return zerg;
    }

    private Zerg CreateBigZerg()
    {
        Zerg zerg = Instantiate(bigZergPrefab);
        return zerg;
    }

    private void OnTakeZergFromPool(Zerg zerg)
    {
        zerg.gameObject.SetActive(true);
    }

    private void OnReturnZergToPool(Zerg zerg)
    {
        zerg.gameObject.SetActive(false);
    }

    private void OnDestroyZerg(Zerg zerg)
    {
        Destroy(zerg.gameObject);
    }
}
