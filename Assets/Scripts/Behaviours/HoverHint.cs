using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HoverHint : Singleton<HoverHint>
{
    [SerializeField]
    private TMP_Text hintContent;
    [SerializeField]
    private float maxWidth;
    [SerializeField]
    private AudioClip hoverSound;

    private float width;
    private float height;

    private RectTransform _rectTransform;

    public float Width { get => width; set => width = value; }
    public float Height { get => height; set => height = value; }

    private void Start()
    {
        gameObject.SetActive(false);
        _rectTransform = GetComponent<RectTransform>();
        Debug.Log(Screen.width.ToString() + " " + Screen.height.ToString());
    }

    public void ShowHint(string content, Vector2 screenPos, bool autoSize)
    {

        float width;
        float height;
        hintContent.ForceMeshUpdate();
        if (autoSize)
        {
            width = hintContent.preferredWidth < maxWidth ? hintContent.preferredWidth : maxWidth;
            height = hintContent.preferredHeight;
        }
        else
        {
            width = Width;
            height = Height;
        }

        _rectTransform.sizeDelta = new Vector2(width, height);
        Debug.Log(width.ToString() + " " + height.ToString());

        //prevent it from going out of screen
        if (screenPos.x + width > Screen.width)
        {
            screenPos.x = Screen.width - width;
        }else if(screenPos.x - width < 0)
        {
            screenPos.x = width;
        }

        if(screenPos.y + height > Screen.height)
        {
            screenPos.y = Screen.height - height;
        }else if(screenPos.y - height < 0)
        {
            screenPos.y = height;
        }

        //set active and show content
        if (!gameObject.activeSelf)
        {
            Activate();
        }
        transform.position = screenPos;
        hintContent.text = content;

    }

    public void Activate()
    {
        gameObject.SetActive(true);
        AudioManager.Instance.PlaySFX(hoverSound);
    }

    public void HideHint()
    {
        gameObject.SetActive(false);
    }
}
