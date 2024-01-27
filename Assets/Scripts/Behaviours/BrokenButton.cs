using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrokenButton : BrokenObject
{
    private Button _button;

    protected override void Initialize()
    {
        _button = GetComponent<Button>();
        _button.enabled = false;
    }

    protected override void HandleBroken()
    {
        _button.enabled = true;
    }
}
