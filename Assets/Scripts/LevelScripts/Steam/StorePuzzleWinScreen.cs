using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class StorePuzzleWinScreen : MonoBehaviour
{

    [Required]
    [SerializeField] private TMP_Text successText;
    [Required]
    [SerializeField] private Image image;

    private int _cost;
    private bool _isKey = false;
    private TurretFile _turretFile;

    public bool IsKey
    {
        get => _isKey;
        set => _isKey = value;
    }


    void OnEnable()
    {
        Purchase();
    }

    public void Initialize(int cost, bool isKey)
    {
        _isKey = isKey;
        _cost = cost;
    }

    public void Initialize(int cost, GameObject award)
    {
        if (award.TryGetComponent(out TurretFile turretFile))
        {
            Initialize(cost, turretFile);
        }
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

        if (enoughCoin == false)
        {
            Debug.Log("Not enough coins");
            successText.text = "Not enough coins";
            return;
        }

        if (_isKey)
        {
            Debug.Log($"Purchased key for {_cost} coins");
            InventoryManager.Instance.UnlockKeyFile();
        }
        else
        {
            Debug.Log($"Purchased {_turretFile}");
            InventoryManager.Instance.UpdateTurretFile(_turretFile, TurretStateEnum.Unlocked);
            Debug.Log($"Turret {_turretFile} unlocked");
            successText.text = $"You have downloaded {_turretFile}!";

            DialogueLua.SetVariable("TotalTurretUnlocked", DialogueLua.GetVariable("TotalTurretUnlocked").asInt + 1);
            if(DialogueLua.GetVariable("TotalTurretUnlocked").asInt == 1)
            {
                DialogueManager.StopAllConversations(); // replace
                DialogueManager.StartConversation("First Obtained Game");
            }
        }


    }
}
