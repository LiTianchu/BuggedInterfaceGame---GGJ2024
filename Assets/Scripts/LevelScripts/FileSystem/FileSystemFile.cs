using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class FileSystemFile : MonoBehaviour
{
    [ShowInInspector, PropertyRange(0, 1000)]
    [SerializeField] private int fileHp = 10;
    [SerializeField] private Slider hpBar;
    [SerializeField] private bool locked = false;

    public int FileHp { get => fileHp; }
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
        hpBar.maxValue = fileHp;
        hpBar.value = fileHp;
    }

    public void TakeDamage(int damage)
    {
        fileHp -= damage;
        hpBar.value = fileHp;
        if (fileHp <= 0)
        {
            gameObject.SetActive(false);
            OnFileDestroyed?.Invoke();
        }
    }

    private void OnMouseDown()
    {
        if (locked) { return; }
        // Override this method to handle file click events
        Debug.Log("File clicked: " + gameObject.name);
    }

}
