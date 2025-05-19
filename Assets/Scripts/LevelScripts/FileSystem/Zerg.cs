using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Pool;

public class Zerg : MonoBehaviour
{
    [SerializeField] private SpriteRenderer zergSpriteRenderer;
    [ShowInInspector, PropertyRange(0, 100)]
    [SerializeField] private int zergMaxHp = 5;
    [ShowInInspector, PropertyRange(0, 100)]
    [SerializeField] private int zergDamage = 1;
    [ShowInInspector, PropertyRange(0, 100)]
    [SerializeField] private float zergMoveSpeed = 5.0f;
    [ShowInInspector, PropertyRange(0.01, 3)]
    [SerializeField] private float zergAttackRange = 0.5f;
    [ShowInInspector, PropertyRange(0, 10)]
    [SerializeField] private float zergAttackRate = 1.0f;

    private int _zergHp;
    private FileSystemFile _targetFile;
    private float _timeElapsed = 0.0f;
    private ObjectPool<Zerg> _pool;

    void Start()
    {
        _timeElapsed = zergAttackRate;
        zergSpriteRenderer.sortingOrder = 1;
        FileSystemLevelManager.Instance.OnFileSystemLayoutChanged += ResetTarget;
    }

    // Update is called once per frame
    void Update()
    {
        _timeElapsed += Time.deltaTime;
        if (_targetFile == null || !_targetFile.gameObject.activeSelf)
        {
            FindTarget();
        }
        else
        {
            ChaseAndAttack();
        }
    }

    public void Initialize(ObjectPool<Zerg> pool)
    {
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
        foreach (FileSystemFile file in FileSystemLevelManager.Instance.ActiveFiles)
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
        if (Vector2.Distance(this.transform.position, _targetFile.transform.position) < zergAttackRange) // attack
        {
            if (_timeElapsed >= zergAttackRate)
            {
                _timeElapsed = 0.0f;
                _targetFile.TakeDamage(zergDamage);
            }
        }
        else // move towards the target
        {
            Vector2 direction = _targetFile.transform.position - transform.position;
            direction.Normalize();
            transform.position += Time.deltaTime * zergMoveSpeed * (Vector3)direction;
        }

    }

    public void TakeDamage(int damage)
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
                _pool.Release(this);
                Debug.Log("Zerg is dead");
            });
        }
    }

    public bool IsAlive()
    {
        return _zergHp > 0;
    }


}
