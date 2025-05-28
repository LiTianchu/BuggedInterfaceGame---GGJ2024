using UnityEngine;

public class FadingText : Fade
{
    [SerializeField] private float showTime = 5.0f;

    private float _timeElapsed = 0.0f;
    private bool _hasFaded = false;
    private void OnEnable()
    {
        ResetFadingText();
    }
    
    public void ResetFadingText()
    {
        ResetOpacity();
        _timeElapsed = 0.0f;
        _hasFaded = false;
    }

    private void Update()
    {
        _timeElapsed += Time.deltaTime;
        if (_timeElapsed >= showTime && !_hasFaded)
        {
            _hasFaded = true;
            StartFade();
        }
    }
    
    
    

}