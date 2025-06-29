using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MineplanterLevel4 : MineplanterLevel3
{
    public override void Start()
    {
        MINES = new()
        {
            new() {3,0,0,1},
            new() {3,0,1,0},
            new() {1,1,0,0},
            new() {0,0,0,0},
        };
        base.Start();
    }

    public override void NextLevel()
    {
        // nextLevel.gameObject.SetActive(true);
        // gameObject.SetActive(false);
        UIManager.Instance.ShowUI(nextLevel.gameObject);
        UIManager.Instance.HideUI(gameObject,true);
    }
}
