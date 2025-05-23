using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // just provide the correct scene name and add to button
    [SerializeField] private string sceneName;
    [SerializeField] private Fade fadeOverlay;
    [SerializeField] private float fadeDuration = 2.0f;

    public void NextScene()
    {
        string code = SaveSystem.LoadPlayer();
        if (string.IsNullOrEmpty(code))
        {
            code = GameManager.Instance.GenerateCode();
            SaveSystem.SavePlayer(code);
        }
        Debug.Log("Code: " + code);

        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        fadeOverlay.SetFadeTime(fadeDuration);
        fadeOverlay.StartFade();

        yield return new WaitForSeconds(fadeDuration);
        SceneManager.LoadSceneAsync(sceneName);
    }
}
