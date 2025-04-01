using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnClickSound : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private AudioClip clip;

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.PlaySFX(clip);
    }
}


