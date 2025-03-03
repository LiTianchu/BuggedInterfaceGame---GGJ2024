using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class FileSystemLevelManager : Singleton<FileSystemLevelManager>
{
    [TitleGroup("Level Settings")]
    [SerializeField] private GridSystem gridSystem;
    [SerializeField] private List<FileSystemFile> files;
    [TitleGroup("Zerg Settings")]
    [ShowInInspector, PropertyRange(0, 100)]
    [SerializeField] private int maxZergCount = 50;
    [ShowInInspector, PropertyRange(0.1, 10)]
    [SerializeField] private float zergSpawnRate = 1.0f;
    [SerializeField] private Zerg zergPrefab;
    [SerializeField] private Transform zergParent;
    [SerializeField] private LayerMask zergLayer;

    private float _timeElapsed;
    private int _zergCount;
    private Camera _mainCamera;


    public List<FileSystemFile> Files { get => files; set => files = value; }
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
        Vector2 spawnPoint = GetRandomPointOutsideBox(gridSystem.GridLowerLeft, gridSystem.GridUpperRight, 5.0f);
        Zerg zerg = Instantiate(zergPrefab, spawnPoint, Quaternion.identity);
        zerg.transform.SetParent(zergParent);
        _zergCount++;
    }

    public Vector2 GetRandomPointOutsideBox(Vector2 lowerLeft, Vector2 upperRight, float minOffset)
    {
        float randNum = Random.Range(0, 1.0f);
        float randomOffset = Random.Range(0, minOffset);
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


}
