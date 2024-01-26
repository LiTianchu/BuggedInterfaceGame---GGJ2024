using TMPro;
using UnityEngine;


//Attach this to a UI element that you want to show as a toast
//Only one toast object can exist in the scene
[RequireComponent(typeof(CanvasGroup))]
public class Toast : Singleton<Toast>
{
    [SerializeField]
    private float showHeight;
    [SerializeField]
    private float hideHeight;
    [SerializeField]
    private float showDuration;
    [SerializeField]
    private AudioClip warningSound;

    private TMP_Text _text;
    private RectTransform _rectTransform;
    private float _timeToHide;
    private CanvasGroup _canvasGroup;
    private bool _isShowing;
    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _text = GetComponentInChildren<TMP_Text>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _rectTransform.anchoredPosition = new Vector2(0, hideHeight);
    }

    private void Update()
    {
        if (GameManager.Instance.TimePassed >= _timeToHide && _isShowing)
        {
            HideToast();
        }
    }

    public void ShowToast(string content)
    {
        UIManager.Instance.UIFadeIn(_canvasGroup, 0.5f, new Vector2(0, hideHeight), new Vector2(0,showHeight));
        _timeToHide = GameManager.Instance.TimePassed + showDuration;
        _text.text = content;
        _isShowing = true;
        if(warningSound != null){
            AudioManager.Instance.PlaySFX(warningSound);
        }
    }

    public void HideToast()
    {
        UIManager.Instance.UIFadeOut(_canvasGroup, 0.5f, new Vector2(0, showHeight), new Vector2(0, hideHeight));
        _isShowing = false;
    }
}