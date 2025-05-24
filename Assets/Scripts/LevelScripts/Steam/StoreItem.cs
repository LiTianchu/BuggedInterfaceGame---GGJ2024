using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StoreItem : MonoBehaviour
{
    [Header("Store Item Data")]
    [SerializeField] private string gameName = "Game Name";
    [SerializeField] private int gamePrice = 1;
    [SerializeField] private TurretFile awardedGameFile;
    [SerializeField] private StorePuzzleWinScreen storePuzzleWinScreen;


    [Header("Store Item UI Elements")]
    [Required]
    [SerializeField] private Image gameIcon;
    [Required]
    [SerializeField] private TMP_Text gameNameLabel;
    [Required]
    [SerializeField] private TMP_Text gamePriceLabel;
    [Required]
    [SerializeField] private Button purchaseButton;



    // Start is called before the first frame update
    private void Start()
    {
        // Set the game name and price labels
        gameNameLabel.text = gameName;
        gamePriceLabel.text = gamePrice.ToString();

        storePuzzleWinScreen.Initialize(gamePrice, awardedGameFile);

        InventoryManager.Instance.OnCoinCountChanged += CheckIfCanPurchase;
    }

    private void OnEnable()
    {
        CheckIfCanPurchase(InventoryManager.Instance.CoinCount);
    }

    private void OnDestroy()
    {
        InventoryManager.Instance.OnCoinCountChanged -= CheckIfCanPurchase;
    }

    private void CheckIfCanPurchase(int coinCount)
    {
        purchaseButton.interactable = coinCount >= gamePrice;
    }

}
