using System;
using System.Collections;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

public class Zerg : MonoBehaviour
{
    [SerializeField] private SpriteRenderer zergSpriteRenderer;
    [ShowInInspector, PropertyRange(0, 1000)]
    [SerializeField] private int zergMaxHp = 5;
    [ShowInInspector, PropertyRange(0, 100)]
    [SerializeField] private int zergDamage = 1;
    [ShowInInspector, PropertyRange(0, 100)]
    [SerializeField] private float zergMoveSpeed = 5.0f;
    [ShowInInspector, PropertyRange(0.01, 3)]
    [SerializeField] private float zergAttackRange = 0.5f;
    [ShowInInspector, PropertyRange(0, 10)]
    [SerializeField] private float zergAttackRate = 1.0f;
    [SerializeField] private float refreshTargetInterval = 0.5f;

    private int _zergHp;
    private FileSystemFile _targetFile;
    private float _timeSinceLastAttack = 0.0f;
    private float _timeSinceLastTargetRefresh = 0.0f;
    private ObjectPool<Zerg> _pool;
    private bool _canBeTargeted = true;
    private FileSystemLevel _currentLevel;
     private Canvas _mainCanvas;

    public int ZergHp { get => _zergHp; }
    public int ZergDamage { get => zergDamage; }
    public float ZergMoveSpeed { get => zergMoveSpeed; }
    public float ZergAttackRange { get => zergAttackRange; }
    public float ZergAttackRate { get => zergAttackRate; }
    public int ZergMaxHp { get => zergMaxHp; }
    public bool CanBeTargeted { get => _canBeTargeted; set => _canBeTargeted = value; }
    public FileSystemLevel CurrentLevel { get => _currentLevel; set => _currentLevel = value; }
    public Canvas MainCanvas { get => _mainCanvas;}
    public event Action OnZergDestroyed;

    protected void Start()
    {
        _timeSinceLastAttack = zergAttackRate;
        _timeSinceLastTargetRefresh = refreshTargetInterval;
        zergSpriteRenderer.sortingOrder = 1;
        FileSystemLevelManager.Instance.CurrentLevel.OnFileSystemLayoutChanged += ResetTarget;

        // create material instance to avoid shared state issues
        if (zergSpriteRenderer.material != null)
        {
            zergSpriteRenderer.material = new Material(zergSpriteRenderer.material);
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        _timeSinceLastAttack += Time.deltaTime;
        _timeSinceLastTargetRefresh += Time.deltaTime;
        if (_targetFile == null || !_targetFile.gameObject.activeSelf || _timeSinceLastTargetRefresh >= refreshTargetInterval)
        {
            _timeSinceLastTargetRefresh = 0.0f;
            FindTarget();
        }
        else
        {
            ChaseAndAttack();
        }
    }

    public void Initialize(Canvas mainCanvas, ObjectPool<Zerg> pool = null)
    {
        _mainCanvas = mainCanvas;
        _pool = pool;
        _zergHp = zergMaxHp;
        zergSpriteRenderer.color = Color.white;
    }

    public void ResetTarget()
    {
        _targetFile = null;
    }

    public void FindTarget()
    {
        FileSystemFile closestFile = null;
        float closestDistance = float.MaxValue;
        foreach (FileSystemFile file in FileSystemLevelManager.Instance.CurrentLevel.ActiveFiles)
        {
            float dist = Vector2.Distance(transform.position, file.transform.position);
            if (dist < closestDistance)
            {
                closestDistance = dist;
                closestFile = file;
            }
        }

        _targetFile = closestFile;
    }

    public void ChaseAndAttack()
    {
        if (_targetFile == null || !_targetFile.gameObject.activeSelf)
        {
            Debug.Log("Target file is null or inactive");
            return;
        }

        // check if can attack the target
        if (zergDamage > 0 && Vector2.Distance(this.transform.position, _targetFile.transform.position) < zergAttackRange) // attack
        {
            if (_timeSinceLastAttack >= zergAttackRate)
            {
                _timeSinceLastAttack = 0.0f;
                _targetFile.TakeDamage(zergDamage);
            }
        }
        else if (zergMoveSpeed > 0) // move towards the target if can move
        {
            Vector2 direction = _targetFile.transform.position - transform.position;
            direction.Normalize();
            transform.position += Time.deltaTime * zergMoveSpeed * (Vector3)direction;
        }
    }

    public virtual void TakeDamage(int damage)
    {
        if (_zergHp <= 0)
        {
            return;
        }

        _zergHp -= damage;

        //flash color
        DOTween.Sequence()
         .Append(zergSpriteRenderer.material.DOColor(Color.red, 0.05f))
         .Append(zergSpriteRenderer.material.DOColor(Color.black, 0.05f))
         .Append(zergSpriteRenderer.material.DOColor(Color.white, 0.01f));

        //shake
        transform.DOShakePosition(0.1f, 0.1f, 10, 90, false, true);


        if (_zergHp <= 0)
        {
            //play death animation
            zergSpriteRenderer.DOFade(0.0f, 0.3f).OnComplete(() =>
            {
                FileSystemLevelManager.Instance.CurrentLevel.ZergDestroyedCount++;
                OnZergDestroyed?.Invoke();
                ReleaseZerg();
                OnDeath();
                // Debug.Log($"Zerg {gameObject.name} destroyed, {FileSystemLevelManager.Instance.CurrentLevel.ZergDestroyedCount} destroyed in total.");
            });
        }
    }

    public bool IsAlive()
    {
        return _zergHp > 0;
    }

    public void StallSeconds(float seconds)
    {
        // This method can be used to stall the zerg for a certain amount of time
        // For example, if you want to pause the zerg's movement or actions
        StartCoroutine(StallCoroutine(seconds));
    }

    private IEnumerator StallCoroutine(float seconds)
    {
        float originalSpeed = zergMoveSpeed;
        zergMoveSpeed = 0; // stop moving
        yield return new WaitForSeconds(seconds);
        zergMoveSpeed = originalSpeed; // resume moving

    }

    public void ReleaseZerg()
    {
        if (_pool != null)
        {
            _pool.Release(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDeath()
    {

    }
}
