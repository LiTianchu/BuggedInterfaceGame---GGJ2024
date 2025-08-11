using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Breadcrumb : MonoBehaviour
{
    [SerializeField] private Button breadcrumbItemPrefab;
    [SerializeField] private Transform breadcrumbContainer;

    //private Dictionary<Transform, FileSystemLevel> _breadcrumbLinks = new Dictionary<Transform, FileSystemLevel>();
    private List<Button> _breadcrumbButtons = new List<Button>();
    public void AddBreadcrumb(string folderName, FileSystemLevel redirectionLevel)
    {
        Button newItem = Instantiate(breadcrumbItemPrefab, breadcrumbContainer);
        newItem.GetComponentInChildren<TMP_Text>().text = folderName+" > ";
        newItem.gameObject.SetActive(true);
        _breadcrumbButtons.Add(newItem);

        newItem.onClick.AddListener(() =>
        {
            if (FileSystemLevelManager.Instance.CurrentLevel != redirectionLevel
                    && FileSystemLevelManager.Instance.CurrentLevel.HasWon) // prevent navigation when in battle
            {
                RemoveButtonsAfter(newItem);
                FileSystemLevelManager.Instance.StartLevel(redirectionLevel);
            }
        });

    }

    public void RemoveButtonsAfter(Button button)
    {
        if (_breadcrumbButtons.Contains(button))
        {
            List<Button> buttonsToRemove = new();
            int indexOfButton = _breadcrumbButtons.IndexOf(button);
            for (int i = indexOfButton; i < _breadcrumbButtons.Count; i++)
            {
                buttonsToRemove.Add(_breadcrumbButtons[i]);
            }

            foreach (Button btn in buttonsToRemove)
            {
                _breadcrumbButtons.Remove(btn);
                Destroy(btn.gameObject);
            }

        }
    }
}
