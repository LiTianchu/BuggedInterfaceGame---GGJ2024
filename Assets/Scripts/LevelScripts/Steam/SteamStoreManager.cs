using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamStoreManager : MonoBehaviour
{
    [Header("Store A Puzzle")]
    [SerializeField] private CrumblingPieces storeAPuzzle;
    [SerializeField] private StorePuzzleWinScreen storeAPuzzleWinScreen;
    // Start is called before the first frame update
    void Start()
    {
        storeAPuzzle.OnPuzzlePieceRight += HandleStoreAPuzzleWin;
    }
    
    void OnDestroy()
    {
        storeAPuzzle.OnPuzzlePieceRight -= HandleStoreAPuzzleWin;
    }

    private void HandleStoreAPuzzleWin()
    {
        storeAPuzzleWinScreen.gameObject.SetActive(true);
        storeAPuzzle.gameObject.SetActive(false);
        storeAPuzzle.OnPuzzlePieceRight -= HandleStoreAPuzzleWin;
    }

 
}
