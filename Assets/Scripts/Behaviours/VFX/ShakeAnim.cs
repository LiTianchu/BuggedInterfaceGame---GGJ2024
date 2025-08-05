using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeAnim : MonoBehaviour
{
    [SerializeField]
    private float shakeRate = 0.1f;
    [SerializeField]
    private float shakeAmount = 1f;
    [SerializeField]
    private bool isShaking = false;
    [SerializeField]
    private AudioClip shakeSound;

    private float _nextShakeTime;   
    private Vector3 _originalPos;
    private float _additionalShakeAmount = 0f;
    public float ShakeRate { get => shakeRate; set => shakeRate = value; }
    public float ShakeAmount { get => shakeAmount; set => shakeAmount = value; }
    public float AdditionalShakeAmount { get => _additionalShakeAmount; set => _additionalShakeAmount = value; } //controlled by other scripts

    // Update is called once per frame
    private void Start()
    {
        _originalPos = transform.localPosition;
        _nextShakeTime = shakeRate;
    }
    void Update()
    {
        if (isShaking && GameManager.Instance.TimePassed >= _nextShakeTime)
        {
            float finalShakeAmount = shakeAmount + _additionalShakeAmount;
            _nextShakeTime = GameManager.Instance.TimePassed + shakeRate;
            transform.localPosition = _originalPos + new Vector3(Random.Range(-finalShakeAmount, finalShakeAmount), Random.Range(-finalShakeAmount, finalShakeAmount));
        }
    }

    public void PlayOneShot(float duration)
    {
        transform.localPosition = _originalPos;
        StopShake();
        StartShake();
        Invoke(nameof(StopShake), duration);
    }

    public void StartShake()
    {
        isShaking = true;
        if(shakeSound != null){
            AudioManager.Instance.PlaySFX(shakeSound);
        }
    }

    public void StopShake()
    {
        isShaking = false;
    }
}
