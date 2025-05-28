using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrokenButton : BrokenObject
{
    [SerializeField]
    private MonoBehaviour lockedComponent;

    protected override void Initialize()
    {
        
        //lockedComponent.enabled = false;
    }

    protected override void HandleBroken()
    {
        lockedComponent.enabled = true;
    }
}
