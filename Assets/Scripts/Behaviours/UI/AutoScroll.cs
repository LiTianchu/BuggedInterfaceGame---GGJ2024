using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Attach this to a scroll view that you want to auto scroll
[RequireComponent(typeof(ScrollRect))]
public class AutoScroll : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed; //positive is scroll down, negative is scroll up
    [SerializeField]
    private ScrollDirection scrollDirection;
    
    private RectTransform _content;
    // Start is called before the first frame update
    void Start()
    {
        _content = GetComponent<ScrollRect>().content.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(scrollDirection == ScrollDirection.Horizontal)
        {
            _content.anchoredPosition += new Vector2(scrollSpeed * Time.deltaTime, 0);
        }else if(scrollDirection == ScrollDirection.Vertical)
        {
            _content.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
        }
    }

    private enum ScrollDirection
    {
        Horizontal,
        Vertical
    }
}
