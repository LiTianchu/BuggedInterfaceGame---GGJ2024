using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SteamStoreManager : MonoBehaviour
{
    [Header("Store List Puzzle")]
    [SerializeField] private BrokenObject not;
    [SerializeField] private List<StoreItem> hiddenStoreItems;

    [Header("Store A QR Puzzle")]
    [SerializeField] private CrumblingPieces storeAPuzzle;
    [SerializeField] private StorePuzzleWinScreen storeAPuzzleWinScreen;

    [Header("Store B Birthday Puzzle")]
    [SerializeField] private RectTransform storeBPuzzle;
    [SerializeField] private TMP_Text storeBPuzzleNumText1;
    [SerializeField] private TMP_Text storeBPuzzleNumText2;
    [SerializeField] private TMP_Text storeBPuzzleNumText3;
    [SerializeField] private TMP_Text storeBPuzzleNumText4;
    [SerializeField] private FadeAfterShowTime wrongAgeText;
    [SerializeField] private Button continueButtonB;
    [SerializeField] private string correctBirthday = "2000";
    [SerializeField] private StorePuzzleWinScreen storeBPuzzleWinScreen;

    [Header("Store C TnC Puzzle")]
    [SerializeField] private RectTransform storeCPuzzle;
    [SerializeField] private List<Toggle> storeCToggleButtons;
    [SerializeField] private List<bool> correctAnswersC;
    [SerializeField] private Button continueButtonC;
    [SerializeField] private StorePuzzleWinScreen storeCPuzzleWinScreen;


    [Header("Store D Select All Images Puzzle")]
    [SerializeField] private RectTransform storeDPuzzle;
    [SerializeField] private List<Toggle> storeDToggleButtons;
    [SerializeField] private List<bool> correctAnswersD;
    [SerializeField] private Button continueButtonD;
    [SerializeField] private StorePuzzleWinScreen storeDPuzzleWinScreen;

    // Start is called before the first frame update
    void Start()
    {
        storeAPuzzle.OnPuzzlePieceRight += HandleStoreAPuzzleWin;
        not.OnBroken += ShowAllStoreItems;
        continueButtonB.onClick.AddListener(CheckBirthday);
        continueButtonC.onClick.AddListener(CheckTnC);
        continueButtonD.onClick.AddListener(CheckSelectAllImages);
    }



    void OnDestroy()
    {
        storeAPuzzle.OnPuzzlePieceRight -= HandleStoreAPuzzleWin;
        not.OnBroken -= ShowAllStoreItems;
    }

    private void ShowAllStoreItems()
    {
        foreach (StoreItem item in hiddenStoreItems)
        {
            item.gameObject.SetActive(true);
        }
    }

    private void HandleStoreAPuzzleWin()
    {
        storeAPuzzleWinScreen.gameObject.SetActive(true);
        storeAPuzzle.gameObject.SetActive(false);
        storeAPuzzle.OnPuzzlePieceRight -= HandleStoreAPuzzleWin;
    }
    private void HandleStoreBPuzzleWin()
    {
        storeBPuzzleWinScreen.gameObject.SetActive(true);
        storeBPuzzle.gameObject.SetActive(false);
    }

    private void HandleStoreCPuzzleWin()
    {
        storeCPuzzleWinScreen.gameObject.SetActive(true);
        storeCPuzzle.gameObject.SetActive(false);
    }

    private void HandleStoreDPuzzleWin()
    {
        storeDPuzzleWinScreen.gameObject.SetActive(true);
        storeDPuzzle.gameObject.SetActive(false);
    }


    private void CheckBirthday()
    {
        string inputBirthday = storeBPuzzleNumText1.text + storeBPuzzleNumText2.text + storeBPuzzleNumText3.text + storeBPuzzleNumText4.text;
        if (inputBirthday.Equals(correctBirthday))
        {
            Debug.Log("Correct birthday entered: " + inputBirthday);
            wrongAgeText.gameObject.SetActive(false);
            HandleStoreBPuzzleWin();
        }
        else
        {
            // Incorrect birthday, show error message
            Debug.Log("Incorrect birthday entered: " + inputBirthday);
            wrongAgeText.gameObject.SetActive(true);
            wrongAgeText.Reshow();
        }
    }
    private void CheckTnC()
    {
        bool allCorrect = true;
        for (int i = 0; i < storeCToggleButtons.Count; i++)
        {
            if (storeCToggleButtons[i].isOn != correctAnswersC[i])
            {
                allCorrect = false;
                break;
            }
        }

        if (allCorrect)
        {
            HandleStoreCPuzzleWin();
        }
        else
        {
            Debug.Log("Some TnC answers are incorrect.");
        }
    }

    private void CheckSelectAllImages()
    {
        bool allCorrect = true;
        for (int i = 0; i < storeDToggleButtons.Count; i++)
        {
            if (storeDToggleButtons[i].isOn != correctAnswersD[i])
            {
                allCorrect = false;
                break;
            }
        }

        if (allCorrect)
        {
            HandleStoreDPuzzleWin();
        }
        else
        {
            Debug.Log("Some image selections are incorrect.");
        }
    }




}
