using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Attach this to a button and assign the need functions to the OnClick() event of the button
[RequireComponent(typeof(Button))]
public class ButtonClickEffect : MonoBehaviour
{
    [SerializeField]
    private AudioClip buttonClickSound;

    public void PlaySound()
    {
        if(buttonClickSound != null){
            AudioManager.Instance.PlaySFX(buttonClickSound);
        }
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }

    public void QuitGame()
    {
        StartCoroutine(GameManager.Instance.QuitGame(0));
    }

    public void RestartScene()
    {
        GameManager.Instance.RestartScene();
    }
    public void BackToMenu()
    {
        GameManager.Instance.BackToMenu();
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(GameManager.Instance.LoadScene(sceneName,0));
    }

    public void PlayVideo(VideoPlayer videoPlayer)
    {
        videoPlayer.Play();
    }



}
