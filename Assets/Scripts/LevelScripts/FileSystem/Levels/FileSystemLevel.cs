using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.WSA;

public class FileSystemLevel : MonoBehaviour
{
    [TitleGroup("Level Settings")]
    [SerializeField] protected GridSystem gridSystem;
    [SerializeField] protected FileSystemFile criticalFile;
    [SerializeField] private LayerMask zergLayer;
    [SerializeField] private FileSystemLevel previousLevel;

    [TitleGroup("Containers")]
    [SerializeField] protected Transform fileContainer;
    [SerializeField] protected Transform zergContainer;
    [SerializeField] protected Canvas mainCanvas;

    protected List<FileSystemFile> _files;
    protected int _zergCount;
    protected Camera _mainCamera;
    protected bool _hasWon = false;

    public List<FileSystemFile> Files
    {
        get
        {
            if (_files == null)
            {
                _files = new List<FileSystemFile>();
                foreach (FileSystemFile file in fileContainer.GetComponentsInChildren<FileSystemFile>())
                {
                    _files.Add(file);
                    if (file is FolderFile folderFile)
                    {
                        folderFile.CurrentLevel = this;
                    }
                }
            }
            return _files;
        }
    }
    public GridSystem GridSystem { get => gridSystem; }
    public FileSystemFile CriticalFile { get => criticalFile; }
    public Transform FileContainer { get => fileContainer; }
    public Transform ZergContainer { get => zergContainer; }
    public Canvas MainCanvas { get => mainCanvas; }
    public bool HasWon { get => _hasWon; }


    public event System.Action OnFileSystemLayoutChanged;
    public event System.Action OnLevelWon;

    public int ZergCount { get => _zergCount; set => _zergCount = value; }
    protected int _zergDestroyedCount = 0;
    public virtual int ZergDestroyedCount
    {
        get => _zergDestroyedCount;
        set
        {
            _zergDestroyedCount = value;

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

    // Start is called before the first frame update
    public void Start()
    {
        _mainCamera = Camera.main;

        if (_files == null)
        {
            _files = new List<FileSystemFile>();
            foreach (FileSystemFile file in fileContainer.GetComponentsInChildren<FileSystemFile>())
            {
                _files.Add(file);
                if (file is FolderFile folderFile)
                {
                    folderFile.CurrentLevel = this;
                }
            }
        }

        foreach (FileSystemFile file in _files)
        {
            file.GetComponent<DraggableWorldSpace>().OnDropped += () =>
            {
                OnFileSystemLayoutChanged?.Invoke(); // propagate the event when file changed position
            };
        }

        criticalFile.OnFileDestroyed += HandleCriticalFileDestroyed;
    }

    // Update is called once per frame
    void Update()
    {
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


    public void AddFile(FileSystemFile file)
    {
        _files.Add(file);
        file.gameObject.SetActive(true);
        file.transform.SetParent(fileContainer);
    }

    public bool RemoveFile(FileSystemFile file)
    {
        return _files.Remove(file);
    }

    private void HandleCriticalFileDestroyed()
    {
        FileSystemLevelManager.Instance.CurrentLevel = previousLevel;
    }

    public void PublishWin()
    {
        if (_hasWon) return; // prevent multiple wins

        _hasWon = true;
        OnLevelWon?.Invoke();
        FileSystemLevelManager.Instance.PublishCurrentLevelCleared();
        Debug.Log($"Level {gameObject.name} won!");
    }


    public virtual void CreateSpawnEvent(AbstractSpawnEvent spawnEvent, ObjectPool<Zerg> zergPool, Vector3 position = default) { }
}
