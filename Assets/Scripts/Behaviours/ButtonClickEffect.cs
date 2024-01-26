using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonClickEffect : MonoBehaviour
{
    [SerializeField]
    private AudioClip buttonClickSound;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => AudioManager.Instance.PlaySFX(buttonClickSound));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
