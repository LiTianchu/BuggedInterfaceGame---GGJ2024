using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHubManager : Singleton<LevelHubManager>
{
    [SerializeField] private List<GameObject> levels;


    public void ShowLevelHubScreen()
    {
        foreach (GameObject level in levels)
        {

            if (level.CompareTag("LevelHubScreen"))
            {
                if (!level.activeSelf)
                {
                    level.SetActive(true);
                }
            }
            else
            {
                level.SetActive(false);
            }
        }
    }

    public void ShowFileSystemScreen()
    {
        foreach (GameObject level in levels)
        {
            if (level.CompareTag("FileSystemScreen"))
            {
                if (!level.activeSelf)
                {
                    level.SetActive(true);
                }
            }
            else
            {
                level.SetActive(false);
            }
        }
    }


}
