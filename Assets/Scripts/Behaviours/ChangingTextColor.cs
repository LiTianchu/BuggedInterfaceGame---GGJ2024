using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangingTextColor : MonoBehaviour
{
     [SerializeField]
    private float colorChangingSpeed = 5f;
    private TMP_Text _text;

    private float h;
    private float s;
    private float v;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<TMP_Text>();
        Color.RGBToHSV(_text.color, out h, out s, out v);
    }

    // Update is called once per frame
    void Update()
    {
        
        h += colorChangingSpeed * Time.deltaTime;
        if (h > 360)
        {
            h = h - 360.0f;
        }
        _text.color = Color.HSVToRGB(h/360, s, v);
    }
}
