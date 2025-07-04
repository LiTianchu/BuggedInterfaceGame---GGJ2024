using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MineplanterLevel6: MineplanterLevel3
{
    public GameObject refImage;
    protected int progress = 0;
    protected List<List<int>>[] LETTERS =
    {
        // new List<List<int>>()
        // {
        //     new List<int>(){0,0,0},
        //     new List<int>(){4,0,4},
        //     new List<int>(){2,0,2},
        // },
        new List<List<int>>()
        {
            new List<int>(){0,5,0},
            new List<int>(){0,0,0},
            new List<int>(){0,5,0},
        },
        // new List<List<int>>()
        // {
        //     new List<int>(){0,0,0},
        //     new List<int>(){0,0,0},
        //     new List<int>(){0,5,0},
        // },
        // new List<List<int>>()
        // {
        //     new List<int>(){0,5,0},
        //     new List<int>(){0,0,0},
        //     new List<int>(){0,5,0},
        // },
        // new List<List<int>>()
        // {
        //     new List<int>(){0,4,0},
        //     new List<int>(){0,0,3},
        //     new List<int>(){0,4,0},
        // },
        // new List<List<int>>()
        // {
        //     new List<int>(){0,0,0},
        //     new List<int>(){0,0,0},
        //     new List<int>(){0,0,0},
        // },
    };

    public override void Start() {
        MINES = LETTERS[progress];
        base.Start();
    }

    public override void ValidateInput(List<List<int>> input)
    {
        if (CheckIfInputMatchesMines(input))
        {
            if (progress < LETTERS.Length - 1)
            {
                // Colour the refImage[progress] text green
                refImage.transform.GetChild(progress).GetComponent<TextMeshProUGUI>().color = Color.green;
                progress++;
                MINES = LETTERS[progress];
                Debug.Log("Progress: " + progress);
            }
            else
            {
                UIManager.Instance.ShowUI(levelClearPrompt.gameObject);
                //levelClearPrompt.SetActive(true);
                //StartCoroutine(GoBackLevel());
                
            }
            Debug.Log("Input matches the MINES list!");
            // Add logic for when the input matches the MINES list
        }
        else
        {
            Debug.Log("Input does not match the MINES list.");
            // Add logic for when the input does not match the MINES list
        }
    }

    // public IEnumerator GoBackLevel()
    // {
    //     yield return new WaitForSeconds(5);
    //     SceneManager.LoadScene("Level 2");
    // }
}
