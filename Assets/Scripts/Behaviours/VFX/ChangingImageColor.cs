using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangingImageColor : MonoBehaviour
{
    [SerializeField]
    private float colorChangingSpeed = 5f;
    
    private Image _img;
    private SpriteRenderer _spriteRenderer;

    private float h;
    private float s;
    private float v;
    // Start is called before the first frame update
    void Start()
    {
        _img = GetComponent<Image>();
        if (_img == null)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();

        }

        if (_img != null)
        {
            Color.RGBToHSV(_img.color, out h, out s, out v);
        }
        else if (_spriteRenderer != null)
        {
            Color.RGBToHSV(_spriteRenderer.color, out h, out s, out v);
        }
    }

    // Update is called once per frame
    void Update()
    {

        h += colorChangingSpeed * Time.deltaTime;
        if (h > 360)
        {
            h = h - 360.0f;
        }
        if (_img != null)
        {
            _img.color = Color.HSVToRGB(h / 360, s, v);
        }
        else if (_spriteRenderer != null)
        {
            _spriteRenderer.color = Color.HSVToRGB(h / 360, s, v);
        }
        
    }
}
