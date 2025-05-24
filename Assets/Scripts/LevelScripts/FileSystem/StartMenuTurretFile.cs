using System.Collections;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuTurretFile : Draggable
{
    [Header("Linked Turret File")]
    [Required]
    [SerializeField] private TurretFile turretFilePrefab;
    [Required]
    [SerializeField] private TMP_Text turretFileName;

    [Header("Other Settings")]
    [SerializeField] private LayerMask fileSystemGridLayer;
    [SerializeField] private Color usedColor = new Color(1, 1, 1, 0.0f);

    private StartMenu _startMenu;
    private Color _originalMenuColor;
    private Vector3 _originalAnchorPos = new Vector3(999, 999, 999);
    private Grid _gridBelow;
    private Image _image;
    private bool _isDeployed = false;

    public TurretFile TurretFile { get { return turretFilePrefab; } }

    public new void Awake()
    {
        base.Awake();
        turretFileName.text = turretFilePrefab.ToString();
        _rectTransform = GetComponent<RectTransform>();
        stayInView = false; // deactivate stay in view to use custom logic
        _image = GetComponent<Image>();

    }

   

    public void Initialize(StartMenu startMenu)
    {
        _startMenu = startMenu;
        _originalMenuColor = _startMenu.ItemGrid.GetComponent<Image>().color;
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
            TurretFile turretFile = Instantiate(turretFilePrefab, _gridBelow.transform.position, Quaternion.identity);
            turretFile.gameObject.SetActive(true);
            DraggableWorldSpace draggableWorldSpace = turretFile.GetComponent<DraggableWorldSpace>();
            draggableWorldSpace.DropObject();
            _isDeployed = true;
            IsDraggable = false;

            FileSystemLevelManager.Instance.AddFile(turretFile.GetComponent<FileSystemFile>());
        }
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
            _image.color = usedColor;

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
}
