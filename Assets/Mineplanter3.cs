using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mineplanter3 : Mineplanter1
{
    public override void Start() {
        MINES = new()
        {
            new() {1,1,1,0},
            new() {0,2,1,1},
            new() {0,3,1,1},
            new() {1,2,0,1},
        };
        base.Start();
    }

    public override void NextLevel() {
        SceneManager.LoadScene("MineplanterLevel6");
    }
}