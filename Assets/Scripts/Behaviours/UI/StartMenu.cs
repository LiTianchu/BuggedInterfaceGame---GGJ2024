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

    [Required]
    [SerializeField] private bool show = false;


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

        itemGrid.gameObject.SetActive(show);
    }


    public void ToggleMenu()
    {
        show = !show;
        itemGrid.gameObject.SetActive(show);
        if (show)
        {
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            CheckTurretFiles();

        }
        else
        {
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void CheckTurretFiles()
    {

        foreach (StartMenuTurretFile turretFile in _turretFiles)
        {

            if (InventoryManager.Instance.TurretFiles.ContainsKey(turretFile.TurretFile))
            {
                TurretStateEnum state = InventoryManager.Instance.TurretFiles[turretFile.TurretFile];
                if (state == TurretStateEnum.Locked)
                {
                    turretFile.gameObject.SetActive(false);
                }
                else
                {
                    turretFile.gameObject.SetActive(true);
                }
            }
            else
            {
                turretFile.gameObject.SetActive(false);
            }

        }
    }

}
