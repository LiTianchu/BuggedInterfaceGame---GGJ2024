using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField]
    private string sceneName = null;
    public void LoadScene()
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
