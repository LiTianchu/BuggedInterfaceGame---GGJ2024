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
        UIManager.Instance.ShowUI(nextLevel.gameObject);
        UIManager.Instance.HideUI(gameObject);
        //xtLevel.gameObject.SetActive(true);
        //gameObject.SetActive(false);
    }

    public void Help()
    {
        UIManager.Instance.ShowUI(helpPanel);
        //helpPanel.SetActive(true);
    }

    public void CloseHelp()
    {
        UIManager.Instance.HideUI(helpPanel);
        //helpPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
