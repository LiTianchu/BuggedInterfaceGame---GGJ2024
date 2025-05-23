using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class StorePuzzleWinScreen : MonoBehaviour
{

    [Required]
    [SerializeField] private TMP_Text successText;
    [Required]
    [SerializeField] private Image image;

    private int _cost;
    private TurretFile _turretFile;


    void OnEnable()
    {
        Purchase();
    }

    public void Initialize(int cost, TurretFile turretFile)
    {
        _cost = cost;
        _turretFile = turretFile;
    }


    private void Purchase()
    {
        // Implement purchase logic here
        bool enoughCoin = InventoryManager.Instance.UseCoin(_cost);

        if(enoughCoin == false)
        {
            Debug.Log("Not enough coins");
            successText.text = "Not enough coins";
            return;
        }

        
        Debug.Log($"Purchased {_turretFile}");
        InventoryManager.Instance.UpdateTurretFile(_turretFile, TurretStateEnum.Unlocked);
        Debug.Log($"Turret {_turretFile} unlocked");
        successText.text = $"You have downloaded {_turretFile}!";


    }
}
