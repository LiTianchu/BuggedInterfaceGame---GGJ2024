using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boolet : MonoBehaviour
{
    protected RectTransform rectTransform;
    protected Stickman stickman;
    public bool harmful = true;

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
            null,
            out Vector2 localCursorPosition
        );

        return rectTransform.rect.Contains(localCursorPosition);
    }

    protected virtual void GameOver()
    {
        // Reload the scene or handle game over logic
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IsHarmful(bool harm)
    {
        harmful = harm;
    }
}
