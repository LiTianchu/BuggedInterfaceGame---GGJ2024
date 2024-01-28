using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayer : MonoBehaviour
{
    [SerializeField]
    private string videoURL;
    public void Play() {
        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Application.OpenURL(videoURL);
        }else{
            this.gameObject.SetActive(true);
        }
    }

    private void OnEnable() {
        AudioManager.Instance.PauseBGM();
    }
    private void OnDisable() {
        AudioManager.Instance.ResumeBGM();
    }
}
