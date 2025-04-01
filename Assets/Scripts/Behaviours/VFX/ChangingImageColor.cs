using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangingImageColor : MonoBehaviour
{
     [SerializeField]
    private float colorChangingSpeed = 5f;
    private Image _img;

    private float h;
    private float s;
    private float v;
    // Start is called before the first frame update
    void Start()
    {
        _img = GetComponent<Image>();
        Color.RGBToHSV(_img.color, out h, out s, out v);
    }

    // Update is called once per frame
    void Update()
    {
        
        h += colorChangingSpeed * Time.deltaTime;
        if (h > 360)
        {
            h = h - 360.0f;
        }
        _img.color = Color.HSVToRGB(h/360, s, v);
    }
}
