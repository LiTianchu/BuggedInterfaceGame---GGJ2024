using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using PixelCrushers.DialogueSystem;
using UnityEngine;

public class TestingHelper : MonoBehaviour
{
    [Header("Turret Testing")]
    [SerializeField] private List<TurretFile> turretFile;
    [SerializeField] private bool unlockAllTurrets = false;
    [SerializeField] private bool unlockKeyFile = false;


    private void Start()
    {
        if (unlockAllTurrets)
        {
            foreach (TurretFile file in turretFile)
            {
                InventoryManager.Instance.UpdateTurretFile(file, TurretStateEnum.Unlocked);
            }
        }

        if(unlockKeyFile)
        {
            InventoryManager.Instance.UnlockKeyFile();
        }

        //StartCoroutine(TestAlertCoroutine());
    }

    public IEnumerator TestAlertCoroutine()
    {
        yield return new WaitForSeconds(1f);
        DialogueManager.ShowAlert("This is an alert message for testing purposes.");
        yield return new WaitForSeconds(1f);
        DialogueManager.ShowAlert("This is another alert message for testing purposes.");
    }
}
