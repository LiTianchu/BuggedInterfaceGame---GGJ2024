using System.Collections;
using UnityEngine;

public class FadeAfterShowTime : Fade
{
    [SerializeField] private float showTime = 5.0f;
    [SerializeField] private bool fadeInWhenAppearing = true;
    public float ShowTime
    {
        get => showTime;
        set => showTime = value;
    }

    private float _timeElapsed = 0.0f;
    private bool _hasStartedFadeOut = false;
    private bool _isFadingIn = false;

    private IEnumerator _waitForFadeInCoroutine;

    private void OnEnable()
    {
        Reshow();
    }


    public void Reshow()
    {
        // Reset elapsed time and fade state
        _timeElapsed = 0.0f;
        _hasStartedFadeOut = false;
        _isFadingIn = false;

        if (_waitForFadeInCoroutine != null && _waitForFadeInCoroutine.MoveNext())
        {
            StopCoroutine(_waitForFadeInCoroutine);
        }

        if (fadeInWhenAppearing)
        {
            // Set initial opacity to 0 and fade in
            SetOpacity(0);
            FadeIn(new Vector2(0,-5f));
            _isFadingIn = true;
            _waitForFadeInCoroutine = WaitForFadeIn();
            StartCoroutine(_waitForFadeInCoroutine);
        }
        else
        {
            // Just reset to full opacity immediately
            ResetOpacity();
        }
    }

    private IEnumerator WaitForFadeIn()
    {
        yield return new WaitForSeconds(FadeDuration);
        _isFadingIn = false;
    }

    private void Update()
    {
        if (!_isFadingIn)
        {
            _timeElapsed += Time.deltaTime;
        }

        if (_timeElapsed >= showTime && !_hasStartedFadeOut)
        {
            _hasStartedFadeOut = true;
            FadeOut(new Vector2(0,10f));
        }
    }
}