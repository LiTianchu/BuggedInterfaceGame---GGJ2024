using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class Zerg : MonoBehaviour
{
    [ShowInInspector, PropertyRange(0, 100)]
    [SerializeField] private int zergHp = 5;
    [ShowInInspector, PropertyRange(0, 100)]
    [SerializeField] private int zergDamage = 1;
    [ShowInInspector, PropertyRange(0, 100)]
    [SerializeField] private float zergMoveSpeed = 5.0f;
    [ShowInInspector, PropertyRange(0.01, 3)]
    [SerializeField] private float zergAttackRange = 0.5f;
    [ShowInInspector, PropertyRange(0, 10)]
    [SerializeField] private float zergAttackRate = 1.0f;

    private FileSystemFile _targetFile;
    private float _timeElapsed = 0.0f;

    void Start()
    {
        _timeElapsed = zergAttackRate;
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

    public void FindTarget()
    {
        FileSystemFile closestFile = null;
        float closestDistance = float.MaxValue;
        foreach (FileSystemFile file in FileSystemLevelManager.Instance.ActiveFiles)
        {
            float dist = Vector2.Distance(this.transform.position, file.transform.position);
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
            Vector2 direction = _targetFile.transform.position - this.transform.position;
            direction.Normalize();
            this.transform.position += (Vector3)direction * zergMoveSpeed * Time.deltaTime;
        }

    }

    private void OnMouseDown() {
        Debug.Log("Zerg Clicked");
    }
}
