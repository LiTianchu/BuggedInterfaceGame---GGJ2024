using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class FadeIn : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.UIFadeIn(GetComponent<CanvasGroup>(), 2f, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
    }


}
