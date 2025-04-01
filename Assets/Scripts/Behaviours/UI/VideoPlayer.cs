using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayer : MonoBehaviour
{
    [SerializeField]
    private string filename;
    [SerializeField]
    private UnityEngine.Video.VideoPlayer videoPlayer;
    public void Play() {
        // if(Application.platform == RuntimePlatform.WebGLPlayer)
        // {
        //     Application.OpenURL(videoURL);
        // }else{
        //     this.gameObject.SetActive(true);
        // }
        videoPlayer.url= System.IO.Path.Combine (Application.streamingAssetsPath,filename+".mp4"); 
        this.gameObject.SetActive(true);
    }

    public void SetPlayState(bool state) {
        if(state) {
            Play();
        }else{
            this.gameObject.SetActive(false);
        }
    }

    private void OnEnable() {
        AudioManager.Instance.PauseBGM();
    }
    private void OnDisable() {
        AudioManager.Instance.ResumeBGM();
    }
}
