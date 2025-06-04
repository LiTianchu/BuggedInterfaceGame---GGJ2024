using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FolderFile : FileSystemFile
{
    [SerializeField] private FileSystemLevel levelToLoad;
    private FileSystemLevel _currentLevel;
    public FileSystemLevel CurrentLevel
    {
        get => _currentLevel;
        set => _currentLevel = value;
    }

    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        FileSystemLevelManager.Instance.OnLevelCleared += UnlockFolder;
    }

    private void UnlockFolder(FileSystemLevel level)
    {
        if (level!=CurrentLevel)
        {
            return;
        }


        if (Locked)
        {
            FileSystemLevelManager.Instance.OnLevelCleared -= UnlockFolder;

            Locked = false;
            Debug.Log("Folder unlocked");
        }
    }

    private void OnMouseDown()
    {
        if (Locked)
        {
            Debug.Log("Folder is locked");
            return;
        }
        FileSystemLevelManager.Instance.StartLevel(levelToLoad);
    }
}
