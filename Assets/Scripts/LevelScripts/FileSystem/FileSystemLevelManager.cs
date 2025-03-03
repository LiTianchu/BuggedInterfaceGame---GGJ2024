using System.Collections;
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

    private float _timeElapsed ;
    private int _zergCount;

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


    // Start is called before the first frame update
    void Start()
    {
        _timeElapsed = zergSpawnRate;
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
    }


    public void SpawnZerg()
    {
        // get a random point outside the grid
        Vector2 spawnPoint = GetRandomPointOutsideBox(gridSystem.GridLowerLeft, gridSystem.GridUpperRight, 5.0f);
        Zerg zerg = Instantiate(zergPrefab, spawnPoint, Quaternion.identity);
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
