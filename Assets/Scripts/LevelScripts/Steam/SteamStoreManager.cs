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
    [SerializeField] private FadingText wrongAgeText;
    [SerializeField] private Button continueButton;
    [SerializeField] private string correctBirthday = "2000";
    [SerializeField] private StorePuzzleWinScreen storeBPuzzleWinScreen;

    // Start is called before the first frame update
    void Start()
    {
        storeAPuzzle.OnPuzzlePieceRight += HandleStoreAPuzzleWin;
        not.OnBroken += ShowAllStoreItems;
        continueButton.onClick.AddListener(CheckBirthday);
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
            wrongAgeText.ResetFadingText();
        }
    }
    
    private void HandleStoreBPuzzleWin()
    {
        storeBPuzzleWinScreen.gameObject.SetActive(true);
        storeBPuzzle.gameObject.SetActive(false);
    }

 
}
