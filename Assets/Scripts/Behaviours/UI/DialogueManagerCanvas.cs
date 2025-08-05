using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas))]
public class DialogueManagerCanvas : MonoBehaviour
{
    private Canvas _canvas;

    void Start()
    {
        _canvas = GetComponent<Canvas>();

        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;

        FindCamera();

    }

    void Update()
    {

        // Ensure the canvas camera is set correctly
        if (_canvas.worldCamera == null)
        {
            FindCamera();
        }

    }

    void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scene loaded: {scene.name}");

        FindCamera();
    }

    private void FindCamera()
    {
        Camera mainCamera = Camera.main;

        if (mainCamera != null)
        {
            _canvas.worldCamera = mainCamera;
            Debug.Log($"Canvas camera set to: {mainCamera.name}");
        }
        else
        {
            Debug.LogWarning("No main camera found in scene!");
            // Fallback: find any camera
            Camera anyCamera = FindObjectOfType<Camera>();
            if (anyCamera != null)
            {
                _canvas.worldCamera = anyCamera;
                Debug.Log($"Canvas camera set to fallback: {anyCamera.name}");
            }
        }
    }
}
