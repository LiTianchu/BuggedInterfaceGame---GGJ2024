using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class FileSystemLevel : MonoBehaviour
{
    [TitleGroup("Level Settings")]
    [SerializeField] protected GridSystem gridSystem;
    [SerializeField] protected KeyFile keyFile;
    [SerializeField] private LayerMask zergLayer;

    [TitleGroup("Containers")]
    [SerializeField] protected Transform fileContainer;
    [SerializeField] protected Transform zergContainer;

    protected List<FileSystemFile> _files;
    protected int _zergCount;
    protected Camera _mainCamera;
    protected bool _hasWon = false;

    public List<FileSystemFile> Files { get => _files; }


    public event System.Action OnFileSystemLayoutChanged;
    
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

    // Start is called before the first frame update
    public void Start()
    {
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

        keyFile.OnFileDestroyed += HandleKeyFileDestroyed;
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

    private void HandleKeyFileDestroyed()
    {
        // handle key file destroyed
        // for example, show game over screen or restart level
        Debug.Log("Key file destroyed!");
    }
}
