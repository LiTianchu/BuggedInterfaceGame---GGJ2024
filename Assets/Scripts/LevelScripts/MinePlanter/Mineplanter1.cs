using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mineplanter1 : Mineplanter
{
    public override void Start() {
         MINES = new()
        {
            new() {false,true,true},
            new() {true,false,false},
            new() {false,false,false},
        };
        base.Start();
    }
    
    public override void NextPuzzle()
    {
        MINES = new List<List<bool>>()
        {
            new() {true,false,true},
            new() {false,true,false},
            new() {false,true,false},
            new() {true,false,true},
        };

        foreach (Transform child in puzzleGrid.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in referenceGrid.transform)
        {
            Destroy(child.gameObject);
        }
        level += 1;
        mines.Clear();
        currentMineNums.Clear();
        neighborMineCounts.Clear();
        levelClearPrompt.SetActive(false);
        SetupLevel();
    }

    public override void NextLevel() {
        nextLevel.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
