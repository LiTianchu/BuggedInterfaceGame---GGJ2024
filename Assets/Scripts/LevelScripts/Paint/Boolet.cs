using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boolet : MonoBehaviour
{
    protected RectTransform rectTransform;
    protected Stickman stickman;
    public bool harmful = true;
    private Canvas _mainCanvas;
    public void Initialize(Canvas mainCanvas) {
        _mainCanvas = mainCanvas;
    }
    protected virtual void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        stickman = FindObjectOfType<Stickman>();
    }

    protected virtual void Update()
    {
        // Check for cursor collision
        if (IsCursorColliding() && harmful && !stickman.IsImmune())
        {
            Debug.Log("Bullet");            
            GameOver();
        }
    }

    protected virtual bool IsCursorColliding()
    {
        Vector2 cursorPosition = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            cursorPosition,
            _mainCanvas.worldCamera, // Use the camera from the main canvas
            out Vector2 localCursorPosition
        );

        return rectTransform.rect.Contains(localCursorPosition);
    }

    protected virtual void GameOver()
    {
        // Reload the scene or handle game over logic
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        stickman.LevelOver(); // Call the LevelOver method on the Stickman
    }

    public void IsHarmful(bool harm)
    {
        harmful = harm;
    }
}
