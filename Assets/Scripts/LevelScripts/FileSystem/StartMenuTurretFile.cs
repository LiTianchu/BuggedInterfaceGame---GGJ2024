using System;
using System.Collections;
using DG.Tweening;
using PixelCrushers.DialogueSystem;
using Sirenix.OdinInspector;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuTurretFile : Draggable
{
    [Header("Linked Turret File")]
    [Required]
    [SerializeField] private TurretFile turretFilePrefab;
    [Required]
    [SerializeField] private TMP_Text turretFileName;
    [SerializeField] private float deployCooldown = 5f;


    [Header("Other Settings")]
    [SerializeField] private LayerMask fileSystemGridLayer;
    [SerializeField] private Color usedColor = new Color(1, 1, 1, 0.0f);
    [SerializeField] private Image deployCDProgressImage;

    private StartMenu _startMenu;
    private Color _originalMenuColor;
    private Vector3 _originalAnchorPos = new Vector3(999, 999, 999);
    private Grid _gridBelow;
    private Image _image;
    private bool _isDeployed = false;
    private TurretFile _deployedTurretFile;
    private float _deployCooldownTimer = 0f;
    private Material _deployCooldownMaterialInstance;
    public TurretFile TurretFile { get { return turretFilePrefab; } }

    public new void Awake()
    {
        base.Awake();
        turretFileName.text = turretFilePrefab.ToString();
        stayInView = false; // deactivate stay in view to use custom logic

        TryGetDependentComponents();
    }

    public void OnFileSystemEntered()
    {
        FileSystemLevelManager.Instance.OnNewFileSystemLevelEntered += HandleNewFileSystemLevelEntered;
    }

    public void OnFileSystemClosed()
    {
        FileSystemLevelManager.Instance.OnNewFileSystemLevelEntered -= HandleNewFileSystemLevelEntered;
    }

    public void Initialize(StartMenu startMenu)
    {
        _startMenu = startMenu;
        _originalMenuColor = _startMenu.ItemGrid.GetComponent<Image>().color;
    }

    public void TryGetDependentComponents()
    {
        if (_rectTransform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        if (_image == null)
        {
            _image = GetComponent<Image>();
        }
        
        if (_deployCooldownMaterialInstance == null)
        {
            // create material instance
            _deployCooldownMaterialInstance = new Material(deployCDProgressImage.material);
            deployCDProgressImage.material = _deployCooldownMaterialInstance;
            _deployCooldownMaterialInstance.SetFloat("_Progress", 0.0f);
            deployCDProgressImage.gameObject.SetActive(false);
        }
    }

    public void DeployTurret()
    {
        if (_isDeployed)
        {
            Debug.Log($"Turret file {turretFilePrefab} already deployed");
            return;
        }

        if (_gridBelow != null)
        {
            _deployedTurretFile = Instantiate(turretFilePrefab, _gridBelow.transform.position, Quaternion.identity);
            _deployedTurretFile.gameObject.SetActive(true);
            DraggableWorldSpace draggableWorldSpace = _deployedTurretFile.GetComponent<DraggableWorldSpace>();
            draggableWorldSpace.DropObject();
            _deployedTurretFile.GetComponent<FileSystemFile>().OnFileDestroyed += HandleTurretFileDestoryed;
            SetDeployed();

            FileSystemLevelManager.Instance.CurrentLevel.AddFile(_deployedTurretFile.GetComponent<FileSystemFile>());
        }
    }

    private void HandleTurretFileDestoryed()
    {
        _deployedTurretFile.GetComponent<FileSystemFile>().OnFileDestroyed -= HandleTurretFileDestoryed;
        SetAsNotDeployed(true);
    }

    protected override void HandleBeginDrag()
    {
        if (_originalAnchorPos == new Vector3(999, 999, 999))
        {
            _originalAnchorPos = _rectTransform.anchoredPosition;
        }

        _startMenu.CanvasGroup.blocksRaycasts = false;
        _startMenu.CanvasGroup.interactable = false;
        _startMenu.ItemGrid.GetComponent<Image>().color = new Color(_originalMenuColor.r,
                                                                    _originalMenuColor.g,
                                                                    _originalMenuColor.b,
                                                                    0.2f);
    }

    protected override void HandleEndDrag()
    {
        _startMenu.CanvasGroup.blocksRaycasts = true;
        _startMenu.CanvasGroup.interactable = true;
        _startMenu.ItemGrid.GetComponent<Image>().color = _originalMenuColor;
        if (_gridBelow != null) // drop the turret file and mark as used
        {
            _gridBelow.NormalColor();


            DeployTurret();
        }

        _rectTransform.anchoredPosition = _originalAnchorPos;
    }

    protected override void HandleDrag()
    {
        Vector2 worldSpacePos = TransformUtils.GetRectTransformWorldCoordCenter(_rectTransform);

        // raycast to check if the pointer is over the grid
        RaycastHit2D hit = Physics2D.Raycast(worldSpacePos, Vector2.zero, Mathf.Infinity, fileSystemGridLayer);
        if (hit.collider != null)
        {
            Grid grid = hit.collider.GetComponent<Grid>();
            if (grid != null)
            {
                if (_gridBelow != null) // reset the previous grid color
                {
                    _gridBelow.NormalColor();
                }

                if (grid.CurrentDraggable != null && grid.CurrentDraggable != this) // if the grid is already occupied, do nothing
                {
                    grid.NormalColor();
                    _gridBelow = null;
                    return;
                }

                Debug.Log($"Dragging over {grid}");
                grid.Highlight(new Color(0, 1, 0, 0.2f));
                _gridBelow = grid;
            }
        }
        else
        {
            if (_gridBelow != null)
            {
                _gridBelow.NormalColor();
                _gridBelow = null;
            }
        }
    }

    private void HandleNewFileSystemLevelEntered(FileSystemLevel level)
    {
        SetAsNotDeployed(false);
    }

    protected override void HandleOnPointerDown()
    {
        base.HandleOnPointerDown();
        if (DialogueLua.GetVariable("DragStartMenuIconDialog").asBool == false)
        {
            DialogueLua.SetVariable("DragStartMenuIconDialog", true);
            DialogueManager.StopAllConversations(); // replace
            DialogueManager.StartConversation("Clicked On Game Icon In Start Menu");
        }
    }

    private void SetDeployed()
    {
        _isDeployed = true;
        IsDraggable = false;
        _image.color = usedColor;
    }

    private void SetAsNotDeployed(bool withCooldown = false)
    {
        TryGetDependentComponents();

        if (_originalAnchorPos == new Vector3(999, 999, 999))
        {
            _originalAnchorPos = _rectTransform.anchoredPosition;
        }

        _image.color = Color.white;
        _isDeployed = false;
        IsDraggable = true;
        _gridBelow = null;
        _rectTransform.anchoredPosition = _originalAnchorPos;
        _deployedTurretFile = null;

        if (withCooldown)
        {
            //_deployCooldownTimer = deployCooldown;
            StartDeployCooldownCountdown();
        }
        else
        {
            deployCDProgressImage.gameObject.SetActive(false);
            _deployCooldownMaterialInstance.SetFloat("_Progress", 0.0f);
        }
    }

    private void StartDeployCooldownCountdown()
    {
        deployCDProgressImage.gameObject.SetActive(true);
        IsDraggable = false;
        _image.color = usedColor;
        _deployCooldownMaterialInstance.SetFloat("_Progress", 1.0f);

        DOTween.Sequence()
            .Append(_deployCooldownMaterialInstance.DOFloat(0f, "_Progress", deployCooldown).SetEase(Ease.Linear))
            .OnComplete(() =>
            {
                IsDraggable = true;
                _image.color = Color.white;
                deployCDProgressImage.gameObject.SetActive(false);
            });
    }
}
