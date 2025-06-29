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
    private StartMenuKeyFile _keyFile;


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
                continue;
            }

            StartMenuKeyFile keyFile = child.GetComponent<StartMenuKeyFile>();
            if (keyFile != null)
            {
                keyFile.Initialize(this);
                _keyFile = keyFile;
                continue;
            }

        }
        if (show)
        {
            UIManager.Instance.ShowUI(itemGrid.gameObject, true);
        }
        else
        {
            UIManager.Instance.HideUI(itemGrid.gameObject, true);
        }
   
    }


    public void ToggleMenu()
    {
        show = !show;

        if (show)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
            UIManager.Instance.ShowUI(itemGrid.gameObject);

            CheckTurretFiles();

        }
        else
        {

            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            UIManager.Instance.HideUI(itemGrid.gameObject);
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

        if (InventoryManager.Instance.KeyFileUnlocked)
        {
            _keyFile.gameObject.SetActive(true);
        }
        else
        {
            _keyFile.gameObject.SetActive(false);
        }
    }

}
