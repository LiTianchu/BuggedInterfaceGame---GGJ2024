using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Breadcrumb : MonoBehaviour
{
    [SerializeField] private Button breadcrumbItemPrefab;
    [SerializeField] private Transform breadcrumbContainer;
    [SerializeField] private RectTransform breadcrumbOverlay;

    //private Dictionary<Transform, FileSystemLevel> _breadcrumbLinks = new Dictionary<Transform, FileSystemLevel>();
    private List<Tuple<Button, FileSystemLevel>> _breadcrumbButtons = new List<Tuple<Button, FileSystemLevel>>();
    private Color _originalSealColor;

    public void Start()
    {
        _originalSealColor = breadcrumbOverlay.GetComponent<Image>().color;
        FileSystemLevelManager.Instance.OnLevelTransit += HandleLevelTransit;
        FileSystemLevelManager.Instance.OnLevelCleared += HandleLevelCleared;
    }

    public void HandleLevelCleared(FileSystemLevel clearedLevel)
    {

        if (breadcrumbOverlay.gameObject.activeSelf)
        {
            OffSeal();
        }
    }

    public void HandleLevelTransit(FileSystemLevel oldLevel, FileSystemLevel newLevel)
    {
        if (!breadcrumbOverlay.gameObject.activeSelf && !newLevel.HasWon)
        {
            ShowSeal();
        }
        else if (newLevel.HasWon)
        {
            OffSeal();
        }
    }
    public void OffSeal()
    {
        // off the seal
        breadcrumbOverlay.GetComponent<Image>().color = _originalSealColor;
        breadcrumbOverlay.GetComponent<Image>().DOColor(new Color(_originalSealColor.r,
                                                                    _originalSealColor.g,
                                                                    _originalSealColor.b,
                                                                    0.0f), 0.2f)
                                                .SetEase(Ease.OutQuad)
                                                .OnComplete(() =>
                                                {
                                                    breadcrumbOverlay.gameObject.SetActive(false);
                                                });

    }

    public void ShowSeal()
    {
        // show the seal
        breadcrumbOverlay.gameObject.SetActive(true);
        breadcrumbOverlay.GetComponent<Image>().color = new Color(_originalSealColor.r,
                                                                    _originalSealColor.g,
                                                                    _originalSealColor.b,
                                                                    0.0f);
        breadcrumbOverlay.GetComponent<Image>().DOColor(_originalSealColor, 0.2f).SetEase(Ease.OutQuad);
    }




    public void AddBreadcrumb(string folderName, FileSystemLevel redirectionLevel)
    {
        Button newItem = Instantiate(breadcrumbItemPrefab, breadcrumbContainer);
        newItem.GetComponentInChildren<TMP_Text>().text = folderName + " > ";
        newItem.gameObject.SetActive(true);
        _breadcrumbButtons.Add(new Tuple<Button, FileSystemLevel>(newItem, redirectionLevel));

        newItem.onClick.AddListener(() =>
        {
            if (FileSystemLevelManager.Instance.CurrentLevel != redirectionLevel
                    && FileSystemLevelManager.Instance.CurrentLevel.HasWon) // prevent navigation when in battle
            {
                TransitToPreviousLevel(redirectionLevel);
            }
        });

    }

    public void TransitToPreviousLevel(FileSystemLevel level)
    {
        Button btn = null;

        foreach (Tuple<Button, FileSystemLevel> item in _breadcrumbButtons)
        {
            if (item.Item2 == level)
            {
                btn = item.Item1;
                break;
            }
        }

        RemoveButtonsAfter(btn);
        FileSystemLevelManager.Instance.StartLevel(level);
    }

    public void RemoveButtonsAfter(Button button)
    {
        if (_breadcrumbButtons.Any(item => item.Item1 == button))
        {
            List<Tuple<Button, FileSystemLevel>> itemsToRemove = new();
            int indexOfButton = _breadcrumbButtons.IndexOf(_breadcrumbButtons.First(item => item.Item1 == button));
            for (int i = indexOfButton; i < _breadcrumbButtons.Count; i++)
            {
                itemsToRemove.Add(_breadcrumbButtons[i]);
            }

            foreach (Tuple<Button, FileSystemLevel> item in itemsToRemove)
            {
                _breadcrumbButtons.Remove(item);
                Destroy(item.Item1.gameObject);
            }

        }
    }
}
