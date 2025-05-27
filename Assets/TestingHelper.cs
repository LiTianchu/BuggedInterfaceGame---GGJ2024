using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TestingHelper : MonoBehaviour
{
    [Header("Turret Testing")]
    [SerializeField] private List<TurretFile> turretFile;
    [SerializeField] private bool unlockAllTurrets = false;


    private void Start()
    {
        if (unlockAllTurrets)
        {
            foreach (TurretFile file in turretFile)
            {
                InventoryManager.Instance.UpdateTurretFile(file, TurretStateEnum.Unlocked);
            }
        }
    }
}
