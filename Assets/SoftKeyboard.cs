using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SoftKeyboard : MonoBehaviour
{
    [Header("Keyboard Row Groups")]
    [SerializeField] private HorizontalLayoutGroup row1;
    [SerializeField] private HorizontalLayoutGroup row2;
    [SerializeField] private HorizontalLayoutGroup row3;

    private static readonly string[] row1Keys = new string[] { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P" };
    private static readonly string[] row2Keys = new string[] { "A", "S", "D", "F", "G", "H", "J", "K", "L" };
    private static readonly string[] row3Keys = new string[] { "Z", "X", "C", "V", "B", "N", "M" };

    private static readonly string[] row1ActualOutput = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
    private static readonly string[] row2ActualOutput = new string[] { "k", "l", "m", "n", "o", "p", "q", "r", "s" };
    private static readonly string[] row3ActualOutput = new string[] { "t", "u", "v", "w", "x", "y", "z" };

    private int row1Count;
    private int row2Count;
    private int row3Count;

    // Start is called before the first frame update
    public void Start()
    {
        row1Count = row1Keys.Length;
        row2Count = row2Keys.Length;
        row3Count = row3Keys.Length;
        SetupKeyboard();
    }

    private void SetupKeyboard()
    {
        for (int i = 0; i < row1Count; i++)
        {
            Button key = row1.transform.GetChild(i).GetComponent<Button>();
            key.GetComponentInChildren<TMP_Text>().text = row1Keys[i];
            
            int index = i; // pass by value

            key.onClick.AddListener(() => OnKeyClick(row1ActualOutput[index]));
        }
        for (int i = 0; i < row2Count; i++)
        {
            Button key = row2.transform.GetChild(i).GetComponent<Button>();
            key.GetComponentInChildren<TMP_Text>().text = row2Keys[i];

            int index = i; // pass by value

            key.onClick.AddListener(() => OnKeyClick(row2ActualOutput[index]));
        }
        for (int i = 0; i < row3Count; i++)
        {
            Button key = row3.transform.GetChild(i).GetComponent<Button>();
            key.GetComponentInChildren<TMP_Text>().text = row3Keys[i];

            int index = i; // pass by value

            key.onClick.AddListener(() => OnKeyClick(row3ActualOutput[index]));
        }
    }

    private void OnKeyClick(string v)
    {
        Debug.Log(v);
    }
}
