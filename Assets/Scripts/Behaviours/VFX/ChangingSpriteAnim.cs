using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingSpriteAnim : MonoBehaviour
{
    [SerializeField]
    private Sprite[] sprites;
    [SerializeField]
    private float timeBetweenSprites = 0.5f;

    private SpriteRenderer _spriteRenderer;
    

    private float _nextSpriteTime = 0f;
    private int _currentSpriteIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _nextSpriteTime = timeBetweenSprites;
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.Instance.TimePassed >= _nextSpriteTime)
        {
            _nextSpriteTime = GameManager.Instance.TimePassed + timeBetweenSprites;
            _currentSpriteIndex++;
            if (_currentSpriteIndex >= sprites.Length)
            {
                _currentSpriteIndex = 0;
            }
            _spriteRenderer.sprite = sprites[_currentSpriteIndex];
        }
    }
}
