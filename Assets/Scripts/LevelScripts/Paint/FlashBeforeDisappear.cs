using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashBeforeDisappear : MonoBehaviour
{
    public float timeToLive = 5.0f; // Time before the heart disappears
    public float flashThereshold = 0.3f; // Threshold for flashing
    public float baseFlashSpeed = 2.0f; // Speed of the flashing effect
    public float urgencyMultiplier = 8.0f; // Multiplier for urgency effect
    public float minAlpha = 0.3f; // Minimum alpha value for flashing
    public float maxAlpha = 1.0f; // Maximum alpha value for flashing
    private float _remainingTime;

    public float TimeToLive { get => timeToLive; set => timeToLive = value; }
    public float FlashThereshold { get => flashThereshold; set => flashThereshold = value; }

    void OnEnable()
    {
        _remainingTime = timeToLive;
    }

    // Update is called once per frame
    void Update()
    {
        _remainingTime -= Time.deltaTime;

        if (_remainingTime / timeToLive < flashThereshold)
        {
            if (_remainingTime / timeToLive < flashThereshold)
            {
                // Improved flashing with configurable parameters
                float normalizedTimeLeft = _remainingTime / timeToLive; // 0 to flashThreshold
                float urgencyFactor = 1f - (normalizedTimeLeft / flashThereshold); // 0 to 1 (more urgent over time)

                // Base flash speed + urgency multiplier
                float flashSpeed = baseFlashSpeed + (urgencyFactor * urgencyMultiplier); // Speed from 2 to 10

                float flashAlpha = Mathf.Lerp(minAlpha, maxAlpha,
                    (Mathf.Sin(Time.time * flashSpeed) + 1f) * 0.5f);

                SetAlpha(flashAlpha);
            }
        }

           if (_remainingTime <= 0)
        {
            Destroy(gameObject);
        }
    }

       private void SetAlpha(float flashAlpha)
    {
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        canvasGroup.alpha = flashAlpha;

    }
}
