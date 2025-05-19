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

    private RectTransform _rectTransform;
    private StartMenu _startMenu;
    private Color _originalMenuColor;
    private Vector3 _originalAnchorPos;
    private Camera _mainCamera;

    public new void Awake()
    {
        base.Awake();
        turretFileName.text = turretFilePrefab.ToString();
        _rectTransform = GetComponent<RectTransform>();
        _mainCamera = Camera.main;
    }

    public void Initialize(StartMenu startMenu)
    {
        _startMenu = startMenu;
        _originalMenuColor = _startMenu.ItemGrid.GetComponent<Image>().color;
    }

    protected override void HandleBeginDrag()
    {
        _originalAnchorPos = _rectTransform.anchoredPosition;
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
        _rectTransform.anchoredPosition = _originalAnchorPos;
    }

    protected override void HandleDrag()
    {
        Vector2 pointerPos = Input.mousePosition;
        Vector2 worldSpacePos = _mainCamera.ScreenToWorldPoint(pointerPos);
    }
}
