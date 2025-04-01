using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Camera Movement Settings")]
    [SerializeField] private float speed = 2f; // Speed of upward movement

    void Update()
    {
        // Move the camera upward every frame
        transform.position += Vector3.up * speed * Time.deltaTime;
    }
}