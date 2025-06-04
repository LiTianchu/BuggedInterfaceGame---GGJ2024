using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

public class FileSystemLevelManager : Singleton<FileSystemLevelManager>
{
    [SerializeField] private BulletSpawner bulletSpawner;
    [TitleGroup("Levels")]
    [SerializeField] private FileSystemLevel level1;
    [SerializeField] private FileSystemLevel level2;
    [SerializeField] private FileSystemLevel level3;
    [SerializeField] private FileSystemLevel level4;

    public BulletSpawner BulletSpawner { get => bulletSpawner; }

    private FileSystemLevel _currentLevel;
    public FileSystemLevel CurrentLevel
    {
        get => _currentLevel;
        set
        {
            if (_currentLevel != null)
            {
                _currentLevel.gameObject.SetActive(false);
            }
            _currentLevel = value;
            _currentLevel.gameObject.SetActive(true);
        }
    }

    public List<FileSystemLevel> Levels
    {
        get
        {
            return new List<FileSystemLevel>
            {
                level1,
                level2,
                level3,
                level4
            };
        }
    }

    public event System.Action<FileSystemLevel> OnLevelCleared;
    public event System.Action<FileSystemLevel> OnNewFileSystemLevelEntered;


    public void StartLevel(FileSystemLevel level)
    {
        CurrentLevel = level;
        OnNewFileSystemLevelEntered?.Invoke(CurrentLevel);
    }

    public void Start()
    {
        if (CurrentLevel == null)
        {
            CurrentLevel = level1; // Default to level1 if no level is set
        }

    }
    
    public void PublishCurrentLevelCleared()
    {
        OnLevelCleared?.Invoke(CurrentLevel);
    }
}

public enum ZergTypeEnum
{
    Small,
    Big
}
