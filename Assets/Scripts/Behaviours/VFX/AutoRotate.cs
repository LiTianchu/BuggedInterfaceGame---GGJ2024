using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] private bool rotatingEnabled = true;
    [SerializeField] private float xSpeed = 0f;
    [SerializeField] private float ySpeed = 0f;
    [SerializeField] private float zSpeed = 10f;

    private Transform _transform;
    private RectTransform _rectTransform;
    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        if (_transform == null)
        {
            _rectTransform = GetComponent<RectTransform>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!rotatingEnabled)
        {
            return;
        }

        if (_transform != null)
        {
            _transform.Rotate(new Vector3(xSpeed, ySpeed, zSpeed) * Time.deltaTime);
        }
        else if (_rectTransform != null)
        {
            _rectTransform.Rotate(new Vector3(xSpeed, ySpeed, zSpeed) * Time.deltaTime);
        }
        else
        {
            Debug.LogError("AutoRotate requires a Transform or RectTransform component.");
        }
    }

    public void ToggleRotation()
    {
        rotatingEnabled = !rotatingEnabled;
    }

    public void SetRotationSpeed(float x, float y, float z)
    {
        xSpeed = x;
        ySpeed = y;
        zSpeed = z;
    }

    public void SetRotationEnabled(bool enabled)
    {
        rotatingEnabled = enabled;
    }
    
}
