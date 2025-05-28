using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

public class FileSystemLevelManager : Singleton<FileSystemLevelManager>
{
    [TitleGroup("Level Settings")]
    [SerializeField] private GridSystem gridSystem;
    [SerializeField] private KeyFile keyFile;


    [TitleGroup("Zerg Settings")]
    [SerializeField] private int maxZergCount = 50;
    [SerializeField] private float smallZergSpawnRate = 1.0f;
    [SerializeField] private float bigZergSpawnRate = 5.0f;
    [SerializeField] private Zerg smallZergPrefab;
    [SerializeField] private Zerg bigZergPrefab;
    [SerializeField] private Transform zergParent;
    [SerializeField] private LayerMask zergLayer;
    [SerializeField] private BulletSpawner bulletSpawner;

    [TitleGroup("Containers")]
    [SerializeField] private Transform fileContainer;
    [SerializeField] private Transform zergContainer;


    public static readonly int ZERGS_TO_DESTROY = 50;


    private List<FileSystemFile> _files;
    private float _smallZergSpawnTimeElapsed;
    private float _bigZergSpawnTimeElapsed;
    private int _zergCount;
    private Camera _mainCamera;
    private bool _hasWon = false;
    private int _zergDestroyedCount = 0;



    public List<FileSystemFile> Files { get => _files; }
    public BulletSpawner BulletSpawner { get => bulletSpawner; }
    public int ZergCount { get => _zergCount; }
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

    public List<FileSystemFile> ActiveFiles
    {
        get
        {
            List<FileSystemFile> activeFiles = new List<FileSystemFile>();
            foreach (FileSystemFile file in Files)
            {
                if (file.gameObject.activeSelf)
                {
                    activeFiles.Add(file);
                }
            }
            return activeFiles;
        }
    }

    public event System.Action OnAllZergDestroyed;
    public event System.Action OnFileSystemLayoutChanged;


    // Start is called before the first frame update
    void Start()
    {
        _smallZergSpawnTimeElapsed = smallZergSpawnRate;
        _bigZergSpawnTimeElapsed = bigZergSpawnRate;
        _mainCamera = Camera.main;

        _files = new List<FileSystemFile>();
        foreach (FileSystemFile file in fileContainer.GetComponentsInChildren<FileSystemFile>())
        {
            _files.Add(file);
        }

        foreach (FileSystemFile file in _files)
        {
            file.GetComponent<DraggableWorldSpace>().OnDropped += () =>
            {
                OnFileSystemLayoutChanged?.Invoke(); // propagate the event when file changed position
            };
        }

        CreateZergPools();
        keyFile.OnFileDestroyed += HandleKeyFileDestroyed;
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

        if (Input.GetMouseButtonDown(0)) // left mouse button
        {
            Vector2 mousePosition = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero, Mathf.Infinity, zergLayer);

            if (hit.collider != null)
            {
                //Debug.Log("Clicked on: " + hit.collider.gameObject.name);
                hit.collider.gameObject.GetComponent<Zerg>().TakeDamage(1);
            }
        }
    }


    public void SpawnZerg(ZergTypeEnum zergType = ZergTypeEnum.Small)
    {
        // get a random point outside the grid
        Vector2 spawnPoint = GetRandomPointOutsideBox(gridSystem.GridLowerLeft, gridSystem.GridUpperRight, 5f, 10f);
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

    public Vector2 GetRandomPointOutsideBox(Vector2 lowerLeft, Vector2 upperRight, float minOffset, float maxOffset)
    {
        float randNum = Random.Range(0, 1.0f);
        float randomOffset = Random.Range(minOffset, maxOffset);
        float x;
        float y;
        if (randNum < 0.25f)
        { // top
            x = Random.Range(lowerLeft.x, upperRight.x);
            y = upperRight.y + randomOffset;
        }
        else if (randNum < 0.5f)
        { // bottom
            x = Random.Range(lowerLeft.x, upperRight.x);
            y = lowerLeft.y - randomOffset;
        }
        else if (randNum < 0.75f)
        { // left
            y = Random.Range(lowerLeft.y, upperRight.y);
            x = lowerLeft.x - randomOffset;
        }
        else
        { // right
            y = Random.Range(lowerLeft.y, upperRight.y);
            x = upperRight.x + randomOffset;
        }

        return new Vector2(x, y);
    }

    public void AddFile(FileSystemFile file)
    {
        _files.Add(file);
        file.gameObject.SetActive(true);
        file.transform.SetParent(fileContainer);
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

    private void HandleKeyFileDestroyed()
    {
        // handle key file destroyed
        // for example, show game over screen or restart level
        Debug.Log("Key file destroyed!");
    }
}

public enum ZergTypeEnum
{
    Small,
    Big
}
