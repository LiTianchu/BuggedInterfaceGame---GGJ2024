using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoftKeyboard : Singleton<SoftKeyboard>
{
    [SerializeField] private bool normalMode = false;
    [SerializeField] private RectTransform keyboardFrame;
    [Header("Keyboard Row Groups")]
    [SerializeField] private HorizontalLayoutGroup row1;
    [SerializeField] private HorizontalLayoutGroup row2;
    [SerializeField] private HorizontalLayoutGroup row3;
    [SerializeField] private Button backspace;

    private static readonly string[] row1Keys = new string[] { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P" };
    private static readonly string[] row2Keys = new string[] { "A", "S", "D", "F", "G", "H", "J", "K", "L" };
    private static readonly string[] row3Keys = new string[] { "Z", "X", "C", "V", "B", "N", "M" };


    private static readonly string[] row1ActualOutput = new string[] { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j" };
    private static readonly string[] row2ActualOutput = new string[] { "k", "l", "m", "n", "o", "p", "q", "r", "s" };
    private static readonly string[] row3ActualOutput = new string[] { "t", "u", "v", "w", "x", "y", "z" };


    private int _row1Count;
    private int _row2Count;
    private int _row3Count;

    private SoftKeyboardInput _targetedInputField;

    private string hiddenInput = "";
    public string printScreen = "prtsc";
    public GameObject flashPanel;
    public Color yellow = new Color(1, 1, 0, 1);

    // Start is called before the first frame update
    public void Start()
    {
        _row1Count = row1Keys.Length;
        _row2Count = row2Keys.Length;
        _row3Count = row3Keys.Length;
        SetupKeyboard();
        HideKeyboard(true);
    }

    private void SetupKeyboard()
    {
        for (int i = 0; i < _row1Count; i++)
        {
            Button key = row1.transform.GetChild(i).GetComponent<Button>();
            key.GetComponentInChildren<TMP_Text>().text = row1Keys[i];

            int index = i; // pass by value

            key.onClick.AddListener(() =>
            {
                if (normalMode)
                {
                    OnKeyClick(row1Keys[index]);
                }
                else
                {
                    OnKeyClick(row1ActualOutput[index]);
                }
            }
            );
        }
        for (int i = 0; i < _row2Count; i++)
        {
            Button key = row2.transform.GetChild(i).GetComponent<Button>();
            key.GetComponentInChildren<TMP_Text>().text = row2Keys[i];

            int index = i; // pass by value

            key.onClick.AddListener(() =>
            {
                if (normalMode)
                {
                    OnKeyClick(row2Keys[index]);
                }
                else
                {
                    OnKeyClick(row2ActualOutput[index]);
                }
            });
        }
        for (int i = 0; i < _row3Count; i++)
        {
            Button key = row3.transform.GetChild(i).GetComponent<Button>();
            key.GetComponentInChildren<TMP_Text>().text = row3Keys[i];

            int index = i; // pass by value

            key.onClick.AddListener(() => {
                if (normalMode)
                {
                    OnKeyClick(row3Keys[index]);
                }
                else
                {
                    OnKeyClick(row3ActualOutput[index]);
                }
            });
        }

        backspace.onClick.AddListener(() => OnBackspaceClick());
    }

    private void OnBackspaceClick()
    {
        _targetedInputField.RemoveLastCharacter();
        if (hiddenInput.Length > 0)
        {
            hiddenInput = hiddenInput.Substring(0, hiddenInput.Length - 1);
        }
        else
        {
            Debug.Log("No characters to remove from hidden input.");
        }
        
    }

    private void OnKeyClick(string v)
    {
        Debug.Log(v);
        _targetedInputField.AppendCharacter(v.ToLower());

        hiddenInput += v;
        if (hiddenInput.ToLower() == printScreen.ToLower())
        {
            Debug.Log("Print Screen");
            StartCoroutine(FlashEffect());
        }
    }

    public void ShowKeyboard(SoftKeyboardInput inputField)
    {
        _targetedInputField = inputField;
        UIManager.Instance.ShowUI(keyboardFrame.gameObject);
        //keyboardFrame.gameObject.SetActive(true);
        hiddenInput = "";
    }

    public void HideKeyboard(bool noTransition = false)
    {
        _targetedInputField = null;
        UIManager.Instance.HideUI(keyboardFrame.gameObject,noTransition);
        //keyboardFrame.gameObject.SetActive(false);
        hiddenInput = "";
    }

    private IEnumerator FlashEffect()
    {
        CanvasGroup canvasGroup = flashPanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = flashPanel.AddComponent<CanvasGroup>();
        }

        // Flash in
        canvasGroup.alpha = 0.7f;
        yield return new WaitForSeconds(0.1f);

        // Flash out
        canvasGroup.alpha = 0;

        // Capture screenshot
        yield return new WaitForEndOfFrame();
        Texture2D screenshot = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        screenshot.Apply();

        // Check for yellow rectangle
        if (ContainsYellowRectangle(screenshot))
        {
            Debug.Log("Key!");
            SceneManager.LoadScene("Paint Boss");
        }
        else
        {
            Debug.Log("No Key!");
        }
    }

    private bool ContainsYellowRectangle(Texture2D screenshot)
    {
        int minWidth = 150; // Minimum width of the yellow rectangle
        int minHeight = 10; // Minimum height of the yellow rectangle

        for (int y = 0; y < screenshot.height - minHeight; y++)
        {
            for (int x = 0; x < screenshot.width - minWidth; x++)
            {
                bool isYellowRectangle = true;
                for (int j = 0; j < minHeight; j++)
                {
                    for (int i = 0; i < minWidth; i++)
                    {
                        if (screenshot.GetPixel(x + i, y + j) != yellow)
                        {
                            isYellowRectangle = false;
                            break;
                        }
                    }
                    if (!isYellowRectangle)
                    {
                        break;
                    }
                }
                if (isYellowRectangle)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
