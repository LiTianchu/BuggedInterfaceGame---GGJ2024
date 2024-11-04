using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boolet : MonoBehaviour
{
    private RectTransform rectTransform;
    public bool harmful = true;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        // Check for cursor collision
        if (IsCursorColliding() && harmful)
        {
            Debug.Log("Bullet");            
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
        // Reload the scene or handle game over logic
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IsHarmful(bool harm)
    {
        harmful = harm;
    }
}
