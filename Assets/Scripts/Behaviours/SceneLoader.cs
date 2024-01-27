using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    // just provide the correct scene name and add to button
    [SerializeField] private string sceneName;

    public void NextScene()
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
