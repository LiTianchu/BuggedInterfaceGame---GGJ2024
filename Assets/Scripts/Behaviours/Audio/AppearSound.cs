using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearSound : MonoBehaviour
{
    [SerializeField] private AudioClip clip;

    private void OnEnable()
    {
        AudioManager.Instance.PlaySFX(clip);
    }

    
}
