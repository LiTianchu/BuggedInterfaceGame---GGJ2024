using System.Collections.Generic;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using Unity.VisualScripting;
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

    [Header("Stage A Zerg Summon Interval Settings")]
    [SerializeField] private float smallZergSpawnIntervalStageA = 2f;
    [SerializeField] private float bigZergSpawnIntervalStageA = 15f;

    [Header("Stage B Zerg Summon Interval Settings")]
    [SerializeField] private float smallZergSpawnIntervalStageB = 1.5f;
    [SerializeField] private float bigZergSpawnIntervalStageB = 12.5f;
    [SerializeField] private float stageBHPThresholdFraction = 0.5f;
    [SerializeField] private float laserInterval = 20f;

    [Header("Spawn Event Settings")]
    [SerializeField] private ClusterSpawnEvent clusterSpawnEventMany;
    [SerializeField] private RingSpawnEvent ringSpawnEventMany;
    [SerializeField] private ClusterSpawnEvent clusterSpawnEventLess;
    [SerializeField] private RingSpawnEvent ringSpawnEventLess;

    [Header("Drop Settings")]
    [SerializeField] private Collectible coinPrefab;
    [SerializeField] private int coinCount = 10;


    private float _timeSinceLastTeleport = 0f;
    private float _timeSinceLastSmallZergSpawn = 0f;
    private float _timeSinceLastbigZergSpawn = 0f;
    private float _timeSinceLastLaser = 0f;
    private bool _isTeleporting = false;
    private bool _isBossReady = false;
    private Dictionary<string,bool> _bossHPAnnouncements = new Dictionary<string, bool>
    {
        { "75", false },
        { "50", false },
        { "25", false },
        { "10", false }
    };



    public bool IsBossReady { get => _isBossReady; set => _isBossReady = value; }
    // Start is called before the first frame update
    protected new void Start()
    {
        base.Start();
        _timeSinceLastSmallZergSpawn = smallZergSpawnIntervalStageA;
        _timeSinceLastLaser = laserInterval;
    }

    // Update is called once per frame
    protected new void Update()
    {
        base.Update();
        if (!_isBossReady) { return; }

        bool isInStageB = IsInStageB();
        float smallZergSpawnInterval = isInStageB ?
        smallZergSpawnIntervalStageA :
        smallZergSpawnIntervalStageB;

        float bigZergSpawnInterval = isInStageB ?
        bigZergSpawnIntervalStageB :
        bigZergSpawnIntervalStageB;

        // spawn small zergs
        _timeSinceLastSmallZergSpawn += Time.deltaTime;
        if (_timeSinceLastSmallZergSpawn >= smallZergSpawnInterval)
        {
            _timeSinceLastSmallZergSpawn = 0f;
            if (Random.Range(0f, 1f) < 0.5f)
            {
                CurrentLevel.CreateSpawnEvent(clusterSpawnEventMany,
                    FileSystemLevelManager.Instance.SmallZergPool,
                    transform.position);
            }
            else
            {
                CurrentLevel.CreateSpawnEvent(ringSpawnEventMany,
                    FileSystemLevelManager.Instance.SmallZergPool,
                    transform.position);
            }
        }

        // spawn big zergs
        _timeSinceLastbigZergSpawn += Time.deltaTime;
        if (_timeSinceLastbigZergSpawn >= bigZergSpawnInterval)
        {
            _timeSinceLastbigZergSpawn = 0f;

            if (Random.Range(0f, 1f) < 0.5f)
            {
                CurrentLevel.CreateSpawnEvent(clusterSpawnEventLess,
                    FileSystemLevelManager.Instance.BigZergPool,
                    transform.position);
            }
            else
            {
                CurrentLevel.CreateSpawnEvent(ringSpawnEventLess,
                        FileSystemLevelManager.Instance.BigZergPool,
                        transform.position);
            }
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

        if (isInStageB)
        {
            // laser attack logic
            _timeSinceLastLaser += Time.deltaTime;
            if (_timeSinceLastLaser >= laserInterval)
            {
                _timeSinceLastLaser = 0f;
                SpawnLaserAttack();
            }
        }

    }

    protected override void OnDeath()
    {
        Camera camera = Camera.main;
        // Drop coins on death
        Vector2 direction = new Vector2(1, 0).normalized; // top right direction
        Vector2 screenSpaceAnchorPosition = camera.WorldToScreenPoint(transform.position);

        // convert world position to canvas coordinates
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            MainCanvas.GetComponent<RectTransform>(),
            screenSpaceAnchorPosition,
            MainCanvas.worldCamera,
            out Vector2 canvasPosition
        );

        for (int i = 0; i < coinCount; i++)
        {
            float rotationDegree = 180f * ((float)i / coinCount);
            Quaternion rotation = Quaternion.Euler(0, 0, rotationDegree);

            Collectible coin = Instantiate(coinPrefab, MainCanvas.transform);
            coin.GetComponent<RectTransform>().anchoredPosition = canvasPosition;
            Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(rotation * direction * 5f, ForceMode2D.Impulse);
            }
        }

        DialogueManager.StopAllConversations(); // Stop any ongoing conversations
        DialogueManager.StartConversation("File System After Beat Boss");
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        float hpRatio = (float)ZergHp / ZergMaxHp;

        // dialogue
        DialogueLua.SetVariable("BossZergHpRatio", hpRatio);

        if (hpRatio <= 0.75 && hpRatio > 0.5)
        {
            if (!_bossHPAnnouncements["75"])
            {
                _bossHPAnnouncements["75"] = true;
                DialogueManager.StopAllConversations(); // replace
                DialogueManager.StartConversation("Zerg Boss HP");
            }
        }
        else if (hpRatio <= 0.5 && hpRatio > 0.25)
        {
            if (!_bossHPAnnouncements["50"])
            {
                _bossHPAnnouncements["50"] = true;
                DialogueManager.StopAllConversations(); // replace
                DialogueManager.StartConversation("Zerg Boss HP");
            }
        }
        else if (hpRatio <= 0.25 && hpRatio > 0.1f)
        {
            if (!_bossHPAnnouncements["25"])
            {
                _bossHPAnnouncements["25"] = true;
                DialogueManager.StopAllConversations(); // replace
                DialogueManager.StartConversation("Zerg Boss HP");
            }
        }
        else if (hpRatio <= 0.1f)
    {
            if (!_bossHPAnnouncements["10"])
            {
                _bossHPAnnouncements["10"] = true;
                DialogueManager.StopAllConversations(); // replace
                DialogueManager.StartConversation("Zerg Boss HP");
            }
        }
        
    }

    public bool IsInStageB()
    {
        return (float)ZergHp / ZergMaxHp > stageBHPThresholdFraction;
    }

    public void SpawnLaserAttack()
    {
        if (!IsInStageB()) { return; }

        Debug.Log("Boss Zerg is attacking with laser!");
    }
}
