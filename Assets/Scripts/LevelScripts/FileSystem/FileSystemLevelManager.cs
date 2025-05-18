using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

public class FileSystemLevelManager : Singleton<FileSystemLevelManager>
{
    [TitleGroup("Level Settings")]
    [SerializeField] private GridSystem gridSystem;
    [SerializeField] private List<FileSystemFile> files;

    [TitleGroup("Zerg Settings")]
    [SerializeField] private int maxZergCount = 50;
    [SerializeField] private float zergSpawnRate = 1.0f;
    [SerializeField] private Zerg zergPrefab;
    [SerializeField] private Transform zergParent;
    [SerializeField] private LayerMask zergLayer;
    [SerializeField] private BulletSpawner bulletSpawner;



    private float _timeElapsed;
    private int _zergCount;
    private Camera _mainCamera;




    public List<FileSystemFile> Files { get => files; }
    public BulletSpawner BulletSpawner { get => bulletSpawner; }
    public int ZergCount { get => _zergCount; }
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

    public event System.Action OnFileSystemLayoutChanged;


    // Start is called before the first frame update
    void Start()
    {
        _timeElapsed = zergSpawnRate;
        _mainCamera = Camera.main;

        foreach (FileSystemFile file in files)
        {
            file.GetComponent<DraggableWorldSpace>().OnDropped += () =>
            {
                OnFileSystemLayoutChanged?.Invoke(); // propagate the event when file changed position
            };
        }

        CreateZergPools();
    }

    // Update is called once per frame
    void Update()
    {
        _timeElapsed += Time.deltaTime;
        if (_timeElapsed >= zergSpawnRate && _zergCount < maxZergCount)
        {
            SpawnZerg();
            _timeElapsed = 0.0f;
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


    public void SpawnZerg()
    {
        // get a random point outside the grid
        Vector2 spawnPoint = GetRandomPointOutsideBox(gridSystem.GridLowerLeft, gridSystem.GridUpperRight, 5f,10f);
        Zerg zerg = _smallZergPool.Get();
        zerg.transform.position = spawnPoint;
        zerg.transform.rotation = Quaternion.identity;
        zerg.transform.SetParent(zergParent);
        zerg.Initialize(_smallZergPool);
        _zergCount++;
    }

    public Vector2 GetRandomPointOutsideBox(Vector2 lowerLeft, Vector2 upperRight, float minOffset,float maxOffset)
    {
        float randNum = Random.Range(0, 1.0f);
        float randomOffset = Random.Range( minOffset,maxOffset);
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


    // object pools
    private ObjectPool<Zerg> _smallZergPool;
    private ObjectPool<Zerg> _bigZergPool;

    private void CreateZergPools()
    {
        _smallZergPool = new ObjectPool<Zerg>(
            CreateSmallZerg,
            OnTakeSmallZergFromPool,
            OnReturnSmallZergToPool,
            OnDestroySmallZerg,
            false,
            10, // initial size
            1000 // max size
        );
    }

    private Zerg CreateSmallZerg()
    {
        Zerg zerg = Instantiate(zergPrefab);
        return zerg;
    }

    private void OnTakeSmallZergFromPool(Zerg zerg)
    {
        zerg.gameObject.SetActive(true);
    }

    private void OnReturnSmallZergToPool(Zerg zerg)
    {
        zerg.gameObject.SetActive(false);
    }

    private void OnDestroySmallZerg(Zerg zerg)
    {
        Destroy(zerg.gameObject);
    }
}
