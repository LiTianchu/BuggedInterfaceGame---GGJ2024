using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayBGM(clip);
    }

}
