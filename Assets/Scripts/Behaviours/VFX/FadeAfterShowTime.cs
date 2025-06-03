using UnityEngine;

public class FadeAfterShowTime: Fade
{
    [SerializeField] private float showTime = 5.0f;

    public float ShowTime
    {
        get => showTime;
        set => showTime = value;
    }
    
    private float _timeElapsed = 0.0f;
    private bool _hasFaded = false;
    private void OnEnable()
    {
        Reshow();
    }
    
    public void Reshow()
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