using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BossZerg : Zerg
{
    [Header("Boss Zerg Teleport Settings")]
    [SerializeField] private float teleportCooldown = 15f;
    [SerializeField] private float teleportDisappearTime = 0.7f;
    [SerializeField] private float teleportWaitTime = 0.2f;
    [SerializeField] private float teleportAppearTime = 0.7f;
    [SerializeField] private Vector2 teleportPosLowerLeft = new Vector2(-7.3f, -1.7f);
    [SerializeField] private Vector2 teleportPosUpperRight = new Vector2(7.5f, 4.5f);
    [SerializeField] private float minTeleportDistance = 3f;
    [SerializeField] private float minDistanceFromCenter = 2f;

    [Header("Zerg Summon Interval Settings")]
    [SerializeField] private float smallZergSpawnInterval = 5f;
    [SerializeField] private float bigZergSpawnInterval = 10f;


    [Header("Spawn Event Settings")]
    [SerializeField] private ClusterSpawnEvent clusterSpawnEventMany;
    [SerializeField] private RingSpawnEvent ringSpawnEventMany;
    [SerializeField] private ClusterSpawnEvent clusterSpawnEventLess;
    [SerializeField] private RingSpawnEvent ringSpawnEventLess;

    private float _timeSinceLastTeleport = 0f;
    private float _timeSinceLastSmallZergSpawn = 0f;
    private float _timeSinceLastbigZergSpawn = 0f;
    private bool _isTeleporting = false;
    private bool _isBossReady = false;
    public bool IsBossReady { get => _isBossReady; set => _isBossReady = value; }
    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
        if (!_isBossReady) { return; }

        // spawn small zergs
        _timeSinceLastSmallZergSpawn += Time.deltaTime;
        if (_timeSinceLastSmallZergSpawn >= smallZergSpawnInterval)
        {
            _timeSinceLastSmallZergSpawn = 0f;
            
            CurrentLevel.CreateSpawnEvent(ringSpawnEventMany,
                    FileSystemLevelManager.Instance.SmallZergPool,
                    transform.position);
            
        }

        // spawn big zergs
        _timeSinceLastbigZergSpawn += Time.deltaTime;
        if (_timeSinceLastbigZergSpawn >= bigZergSpawnInterval)
        {
            _timeSinceLastbigZergSpawn = 0f;
            
            CurrentLevel.CreateSpawnEvent(ringSpawnEventLess,
                    FileSystemLevelManager.Instance.BigZergPool,
                    transform.position);
        }


        // teleport logic
        if (!_isTeleporting && _timeSinceLastTeleport >= teleportCooldown)
        {
            _isTeleporting = true;
            transform.localScale = Vector3.one;
            CanBeTargeted = false;
            DOTween.Sequence().
                Append(transform.DOScale(Vector3.zero, teleportDisappearTime)).
                AppendCallback(() =>
                {
                    Vector2 spawnPos = default;
                    do
                    {
                        spawnPos = VectorUtils.GetRandomPointInBox(
                                                teleportPosLowerLeft,
                                                teleportPosUpperRight);
                    } while (Vector2.Distance(spawnPos, transform.position) < minTeleportDistance ||
                             Vector2.Distance(spawnPos, Vector2.zero) < minDistanceFromCenter);
                    transform.position = spawnPos;
                }).
                AppendInterval(teleportWaitTime).
                Append(transform.DOScale(Vector3.one, teleportAppearTime)).
                AppendCallback(() =>
                {
                    CanBeTargeted = true;
                    _timeSinceLastTeleport = 0f;
                    _isTeleporting = false;
                });
        }
        else
        {
            _timeSinceLastTeleport += Time.deltaTime;
        }
    }


}
