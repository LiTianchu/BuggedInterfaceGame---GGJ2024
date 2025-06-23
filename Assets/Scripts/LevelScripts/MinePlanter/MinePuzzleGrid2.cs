using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinePuzzleGrid2 : MinePuzzleGrid1
{
    protected override int MIN_NUM => 0;
    protected override int MAX_NUM => 8;

    public new void Start()
    {
        _numText = GetComponentInChildren<TMP_Text>();
        UpdateNumView();
    }
}
