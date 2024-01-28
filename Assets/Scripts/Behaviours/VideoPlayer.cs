using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayer : MonoBehaviour
{
    private void OnEnable() {
        AudioManager.Instance.PauseBGM();
    }
    private void OnDisable() {
        AudioManager.Instance.ResumeBGM();
    }
}
