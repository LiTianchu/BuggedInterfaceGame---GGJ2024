using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stickman : MonoBehaviour
{
    public GameObject brush;
    public GameObject[] itemPrefabs; // Item prefabs
    public GameObject warningLinePrefab; // Warning line prefab
    public GameObject immunityImagePrefab; // Immunity image prefab

    private RectTransform rectTransform;
    private Vector2 minBounds, maxBounds;
    private float halfHeight, halfWidth;
    private Vector2 stickmanSize;

    public int health = 10;
    public float attackInterval = 5.0f; // Time between attacks
    private float attackTimer;
    private bool isImmune = false;

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

        // Start the coroutine to spawn hearts
        StartCoroutine(SpawnHeart());
        StartCoroutine(RicochetEffect());
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
        if (IsCursorColliding() && !isImmune)
        {
            Debug.Log("Stickman");
            GameOver();
        }
    }

    void PerformAttack()
    {
        // Example attacks
        int attackType = Random.Range(1, 3);
        
        switch (attackType)
        {
            case 1:
                // Attack 3: Throw pencils and brushes like a machine gun
                StartCoroutine(MachineGunEffect());
                break;
            case 2:
                // Attack 4: Vergil Judgment Cut
                StartCoroutine(JudgmentCutEffect());
                break;
        }
    }

    IEnumerator RicochetEffect()
    {
        Vector2 direction = new Vector2(60, 60).normalized;
        float speed = 500f;
        float ricochetDuration = Mathf.Infinity; // Infinite duration
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
        float speed = 200f; // Increased speed
        float homingStrength = 0.1f;
        float lifetime = 4.0f;
        float elapsedTime = 0;

        Canvas canvas = GetComponentInParent<Canvas>();
        Camera camera = null;
        if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            camera = canvas.worldCamera;
        }

        while (elapsedTime < lifetime)
        {
            Vector2 cursorPosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(),
                cursorPosition,
                camera,
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

    IEnumerator JudgmentCutEffect()
    {
        int numberOfCuts = 11;
        float warningDuration = 1f;
        float harmfulDuration = 1f;

        List<GameObject> warningLines = new List<GameObject>();

        // Spawn warning lines
        for (int i = 0; i < numberOfCuts; i++)
        {
            float randomX = Random.Range(minBounds.x + stickmanSize.x / 2, maxBounds.x - stickmanSize.x / 2);
            Vector2 startPosition = new Vector2(randomX, minBounds.y);
            Vector2 endPosition = new Vector2(randomX, maxBounds.y);

            GameObject warningLine = Instantiate(warningLinePrefab, startPosition, Quaternion.identity, transform.parent);
            RectTransform warningLineRect = warningLine.GetComponent<RectTransform>();
            warningLineRect.anchoredPosition = startPosition;

            // Apply random rotation
            float randomAngle = Random.Range(0f, 360f);
            warningLineRect.rotation = Quaternion.Euler(0, 0, randomAngle);

            warningLines.Add(warningLine);
        }

        // Wait for warning duration
        yield return new WaitForSeconds(warningDuration);

        // Make lines harmful
        foreach (GameObject warningLine in warningLines)
        {
            warningLine.GetComponent<Boolet>().IsHarmful(true); // Assuming the warning line has an IsHarmful property
            warningLine.GetComponent<Image>().color = Color.red; // Assuming the warning line has an Image component
        }

        // Wait for harmful duration
        float elapsedTime = 0;
        while (elapsedTime < harmfulDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Destroy lines
        foreach (GameObject warningLine in warningLines)
        {
            Destroy(warningLine);
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

    IEnumerator SpawnHeart()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(5.0f, 10.0f)); // Wait for a random time between 5 and 10 seconds

            if (itemPrefabs != null)
            {
                // Get a random item prefab
                GameObject heartPrefab = itemPrefabs[Random.Range(0, itemPrefabs.Length)]; 

                // Calculate a random position within the bounds
                float randomX = Random.Range(minBounds.x + stickmanSize.x / 2, maxBounds.x - stickmanSize.x / 2);
                float randomY = Random.Range(minBounds.y + stickmanSize.y / 2, maxBounds.y - stickmanSize.y / 2);
                Vector2 randomPosition = new Vector2(randomX, randomY);

                // Instantiate the heart
                GameObject heart = Instantiate(heartPrefab, randomPosition, Quaternion.identity, transform.parent);
                RectTransform heartRect = heart.GetComponent<RectTransform>();
                heartRect.anchoredPosition = randomPosition;
            }
        }
    }

    public void ReduceHealth(int amount)
    {
        if (!isImmune)
        {
            health -= amount;
            if (health <= 0)
            {
                GameOver();
            }
        }
    }

    public void SetImmunity(float duration)
    {
        StartCoroutine(ImmunityCoroutine(duration));
    }

    private IEnumerator ImmunityCoroutine(float duration)
    {
        isImmune = true;
        GameObject immunityImage = Instantiate(immunityImagePrefab, transform.parent);
        RectTransform immunityImageRect = immunityImage.GetComponent<RectTransform>();

        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            Vector2 cursorPosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                immunityImageRect.parent as RectTransform,
                cursorPosition,
                null,
                out Vector2 localCursorPosition
            );
            immunityImageRect.anchoredPosition = localCursorPosition;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (isImmune) DestroyImmunity();
    }

    public void DestroyImmunity() {
        GameObject immunityImage = GameObject.Find("BarrierImage(Clone)");
        Destroy(immunityImage);
        isImmune = false;
    }

    void GameOver()
    {
        Debug.Log("Game Over!");
        // Reload the scene or handle game over logic
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool IsImmune()
    {
        return isImmune;
    }

    public Vector2 GetMinBounds()
    {
        return minBounds;
    }

    public Vector2 GetMaxBounds()
    {
        return maxBounds;
    }
}