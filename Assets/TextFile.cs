using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TextFile : FileSystemFile
{
    [TextArea]
    [SerializeField] private string textContent;
    [SerializeField] private UITransition textFileView;
    [SerializeField] private TMP_Text textFileText;


    public void OpenTextFile()
    {
        textFileView.TransitionIn();
        textFileText.text = textContent;
    }

    private void OnMouseDown()
    {
        if (Locked)
        {
            Debug.Log("Text file is locked");
            return;
        }
        OpenTextFile();
    }
}
