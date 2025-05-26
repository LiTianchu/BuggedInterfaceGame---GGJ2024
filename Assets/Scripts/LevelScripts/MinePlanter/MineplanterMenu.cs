using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MineplanterMenu : MonoBehaviour
{
    [SerializeField] private RectTransform nextLevel;
    public GameObject helpPanel;

    public void PlayGame()
    {
        nextLevel.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void Help()
    {
        helpPanel.SetActive(true);
    }

    public void CloseHelp()
    {
        helpPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
