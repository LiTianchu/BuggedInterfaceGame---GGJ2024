using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stickman : MonoBehaviour
{
    public GameObject brush;

    private RectTransform rectTransform;
    private Vector2 minBounds, maxBounds;
    private Vector2 stickmanSize;

    public float attackInterval = 5.0f; // Time between attacks
    private float attackTimer;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        Canvas canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            minBounds = canvasRect.rect.min;
            maxBounds = canvasRect.rect.max;

            // Get the size of the Stickman
            stickmanSize = rectTransform.rect.size;
        }

        attackTimer = attackInterval;
    }

    void Update()
    {
        Vector2 pos = rectTransform.anchoredPosition;

        // Adjust boundaries to account for the Stickman's size
        float halfWidth = stickmanSize.x / 2;
        float halfHeight = stickmanSize.y / 2;

        pos.x = Mathf.Clamp(pos.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
        pos.y = Mathf.Clamp(pos.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);

        rectTransform.anchoredPosition = pos;

        // Handle boss attacks
        attackTimer -= Time.deltaTime;
        if (attackTimer <= 0)
        {
            PerformAttack();
            attackTimer = attackInterval;
        }

        // Check for cursor collision
        if (IsCursorColliding())
        {
            GameOver();
        }
    }

    void PerformAttack()
    {
        // Example attacks
        int attackType = Random.Range(0, 2);
        switch (attackType)
        {
            case 0:
                // Attack 2: Ricochet randomly around the screen
                StartCoroutine(RicochetEffect());
                break;
            case 1:
                // Attack 3: Throw pencils and brushes like a machine gun
                StartCoroutine(MachineGunEffect());
                break;
        }
    }

    IEnumerator RicochetEffect()
    {
        Vector2 direction = Random.insideUnitCircle.normalized;
        float speed = 800f;
        float ricochetDuration = 4.0f;
        float elapsedTime = 0;

        while (elapsedTime < ricochetDuration)
        {
            Vector2 pos = rectTransform.anchoredPosition;
            pos += direction * speed * Time.deltaTime;

            // Check for collision with boundaries and bounce
            if (pos.x < minBounds.x + stickmanSize.x / 2 || pos.x > maxBounds.x - stickmanSize.x / 2)
            {
                direction.x = -direction.x;
            }
            if (pos.y < minBounds.y + stickmanSize.y / 2 || pos.y > maxBounds.y - stickmanSize.y / 2)
            {
                direction.y = -direction.y;
            }

            rectTransform.anchoredPosition = pos;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    IEnumerator MachineGunEffect()
    {
        float fireDuration = 3.0f;
        float elapsedTime = 0;

        while (elapsedTime < fireDuration)
        {
            FireProjectile();
            yield return new WaitForSeconds(0.1f);
            elapsedTime += 0.1f;
        }
    }

    void FireProjectile()
    {
        if (brush != null)
        {
            // Create a projectile (brush)
            GameObject projectile = Instantiate(brush, rectTransform.position, Quaternion.identity, transform.parent);
            RectTransform projectileRect = projectile.GetComponent<RectTransform>();

            // Set initial position and direction
            projectileRect.anchoredPosition = rectTransform.anchoredPosition;
            Vector2 cursorPosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                cursorPosition,
                null,
                out Vector2 localCursorPosition
            );
            Vector2 direction = (localCursorPosition - rectTransform.anchoredPosition).normalized;

            // Move the projectile
            StartCoroutine(MoveProjectile(projectileRect, direction));
        }
    }

    IEnumerator MoveProjectile(RectTransform projectileRect, Vector2 direction)
    {
        float speed = 600f; // Increased speed
        float homingStrength = 0.1f;
        float lifetime = 4.0f;
        float elapsedTime = 0;

        while (elapsedTime < lifetime)
        {
            Vector2 cursorPosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                cursorPosition,
                null,
                out Vector2 localCursorPosition
            );

            // Homing behavior
            Vector2 desiredDirection = (localCursorPosition - projectileRect.anchoredPosition).normalized;
            direction = Vector2.Lerp(direction, desiredDirection, homingStrength);

            projectileRect.anchoredPosition += direction * speed * Time.deltaTime;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(projectileRect.gameObject);
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