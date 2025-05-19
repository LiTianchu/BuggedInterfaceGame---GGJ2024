using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [Required]
    [SerializeField] private GridLayoutGroup itemGrid;

    [Required]
    [SerializeField] private CanvasGroup canvasGroup;


    List<StartMenuTurretFile> _turretFiles = new List<StartMenuTurretFile>();


    public List<StartMenuTurretFile> TurretFiles { get { return _turretFiles; } }
    public CanvasGroup CanvasGroup { get { return canvasGroup; } }
    public GridLayoutGroup ItemGrid { get { return itemGrid; } }
    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in itemGrid.transform)
        {
            StartMenuTurretFile turretFile = child.GetComponent<StartMenuTurretFile>();
            if (turretFile != null)
            {
                _turretFiles.Add(turretFile);
                turretFile.Initialize(this);
            }
        }
    }


}
