using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class FileSystemFile : MonoBehaviour
{
    [ShowInInspector, PropertyRange(0,100)]
    [SerializeField] private int fileHp = 10;
    [SerializeField] private Slider hpBar;

    // Start is called before the first frame update
    void Start()
    {
        hpBar.maxValue = fileHp;
        hpBar.value = fileHp;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int damage){
        fileHp -= damage;
        hpBar.value = fileHp;
        if(fileHp <= 0){
           this.gameObject.SetActive(false);
        }
    }
}
