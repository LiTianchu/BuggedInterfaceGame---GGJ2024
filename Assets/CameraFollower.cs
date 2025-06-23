using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [Header("Camera Clamp Settings")]
    [SerializeField] private Vector2 minPosition;
    [SerializeField] private Vector2 maxPosition;

    [Header("Camera Follow Settings")]
    [SerializeField] private Transform target;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        // Get the target position
        Vector3 desiredPosition = target.localPosition;
        desiredPosition.z = transform.localPosition.z; // Keep the camera's z position

        // Clamp the position within the specified bounds
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minPosition.x, maxPosition.x);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minPosition.y, maxPosition.y);

      
        transform.localPosition = desiredPosition;
    }
}
