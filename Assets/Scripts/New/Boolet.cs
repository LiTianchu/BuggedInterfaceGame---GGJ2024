using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boolet : MonoBehaviour
{
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Check for cursor collision
        if (IsCursorColliding())
        {
            GameOver();
        }
    }

    bool IsCursorColliding()
    {
        Vector2 cursorPosition = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            cursorPosition,
            null,
            out Vector2 localCursorPosition
        );

        return rectTransform.rect.Contains(localCursorPosition);
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        // Reload the scene or handle game over logic
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
