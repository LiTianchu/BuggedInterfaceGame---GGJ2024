using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FileSystemFile : MonoBehaviour
{
    [ShowInInspector, PropertyRange(0, 1000)]
    [SerializeField] private int fileHp = 10;
    [SerializeField] private Slider hpBar;
    [SerializeField] private bool locked = false;
    [SerializeField] private bool destroyWhenDied = false;
    private DraggableWorldSpace _draggableWorldSpace;
    private int _currFileHp = 10;
    public int FileHp { get => fileHp; }
    public int CurrFileHp { get => _currFileHp; }
    public bool Locked
    {
        get => locked; set
        {
            locked = value;
        }
    }

    public event System.Action OnFileDestroyed;
    // Start is called before the first frame update
    protected void Start()
    {
        _currFileHp = fileHp;
        if (hpBar != null)
        {
            hpBar.maxValue = _currFileHp;
            hpBar.value = _currFileHp;
        }
        _draggableWorldSpace = GetComponent<DraggableWorldSpace>();
    }

    public void TakeDamage(int damage)
    {
        _currFileHp -= damage;
        if (hpBar != null)
        {
            hpBar.value = _currFileHp;
        }

        if (_currFileHp <= 0)
        {
            OnFileDestroyed?.Invoke();
            if (_draggableWorldSpace != null)
            {
                _draggableWorldSpace.UnbindDropArea();
            }

            if (destroyWhenDied)
            {
                FileSystemLevelManager.Instance.CurrentLevel.RemoveFile(this);
                Destroy(gameObject);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnMouseDown()
    {
        if (locked) { return; }
        // Override this method to handle file click events
        Debug.Log("File clicked: " + gameObject.name);
    }

    public void ResetFile()
    {
        _currFileHp = fileHp;
        if (hpBar != null)
        {
            hpBar.maxValue = _currFileHp;
            hpBar.value = _currFileHp;
        }
        gameObject.SetActive(true);
    }

}
