using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

public class FileSystemLevelManager : Singleton<FileSystemLevelManager>
{
    [SerializeField] private BulletSpawner bulletSpawner;
    [TitleGroup("UI")]
    [SerializeField] private Breadcrumb breadcrumb;
    [TitleGroup("Levels")]
    [SerializeField] private FileSystemLevel entryLevel;
    [TitleGroup("Zergs")]
    [SerializeField] private Zerg smallZergPrefab;
    [SerializeField] private Zerg bigZergPrefab;
    [SerializeField] private BossZerg bossZergPrefab;
    [SerializeField] private float levelTransitionDuration = 0.5f;

    public BulletSpawner BulletSpawner { get => bulletSpawner; }

    private FileSystemLevel _currentLevel;
    public FileSystemLevel CurrentLevel
    {
        get => _currentLevel;
        set
        {
            if (_currentLevel != null) // unload the new level
            {
                _currentLevel.gameObject.SetActive(false);
            }

            // load new level
            _currentLevel = value;
            _currentLevel.gameObject.SetActive(true);
        }
    }

    public Breadcrumb Breadcrumb { get => breadcrumb; }


    public event Action<FileSystemLevel> OnLevelCleared;
    public event Action<FileSystemLevel> OnNewFileSystemLevelEntered;


    public void StartLevel(FileSystemLevel level)
    {
        Breadcrumb.AddBreadcrumb(level.gameObject.name, level);
        StartCoroutine(StartLevelCoroutine(level));
    }

    private IEnumerator StartLevelCoroutine(FileSystemLevel level)
    {
        // Wait for the current level to finish its animations
        foreach (FileSystemFile file in CurrentLevel.Files)
        {
            file.transform.DORotate(new Vector3(0, 0, 180), levelTransitionDuration).SetEase(Ease.InElastic);
            file.transform.DOScale(0, levelTransitionDuration).SetEase(Ease.InElastic);
        }

        yield return new WaitForSeconds(levelTransitionDuration);

        CurrentLevel = level;


        foreach (FileSystemFile file in CurrentLevel.Files)
        {
            file.transform.rotation = Quaternion.Euler(0, 0, 180);
            file.transform.localScale = Vector3.zero;

            file.transform.DORotate(new Vector3(0, 0, 0), levelTransitionDuration).SetEase(Ease.InOutElastic);
            file.transform.DOScale(1, levelTransitionDuration).SetEase(Ease.InOutElastic);
        }
        yield return new WaitForSeconds(levelTransitionDuration);
        OnNewFileSystemLevelEntered?.Invoke(CurrentLevel);
    }

    public void Start()
    {
        if (CurrentLevel == null)
        {
            CurrentLevel = entryLevel; // Default to level1 if no level is set
            Breadcrumb.AddBreadcrumb(CurrentLevel.gameObject.name, CurrentLevel);
        }
        CreateZergPools();

    }

    public void PublishCurrentLevelCleared()
    {
        OnLevelCleared?.Invoke(CurrentLevel);
    }

    // object pools
    private ObjectPool<Zerg> _smallZergPool;
    private ObjectPool<Zerg> _bigZergPool;
    public ObjectPool<Zerg> SmallZergPool { get => _smallZergPool; }
    public ObjectPool<Zerg> BigZergPool { get => _bigZergPool; }

    private void CreateZergPools()
    {
        _smallZergPool = new ObjectPool<Zerg>(
            CreateSmallZerg,
            OnTakeZergFromPool,
            OnReturnZergToPool,
            OnDestroyZerg,
            false,
            10, // initial size
            5000 // max size
        );

        _bigZergPool = new ObjectPool<Zerg>(
            CreateBigZerg,
            OnTakeZergFromPool,
            OnReturnZergToPool,
            OnDestroyZerg,
            false,
            10, // initial size
            5000 // max size
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
        zerg.CurrentLevel = CurrentLevel;
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

    public BossZerg GetBossZergInstance()
    {
        BossZerg bossZerg = Instantiate(bossZergPrefab);
        bossZerg.transform.SetParent(CurrentLevel.ZergContainer);
        bossZerg.gameObject.name = "BossZerg";
        bossZerg.CurrentLevel = CurrentLevel;
        return bossZerg;
    }
}

public enum ZergTypeEnum
{
    Small,
    Big
}
