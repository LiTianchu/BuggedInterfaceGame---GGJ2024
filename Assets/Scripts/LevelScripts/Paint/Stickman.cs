
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Stickman : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject brush;
    public GameObject[] itemPrefabs; // Item prefabs
    public GameObject warningLinePrefab; // Warning line prefab
    public GameObject immunityImagePrefab; // Immunity image prefab
    public GameObject[] cards;
    public Collectible coinDrop;

    [Header("Boss Settings")]
    public float itemSpawnMinInterval = 3f;
    public float itemSpawnMaxInterval = 7f;
    public float itemSpawnMinDistance = 100f; // Minimum distance from the Stickman to spawn items
    public int maxTry = 10;
    public float movementSpeed = 200f; // Speed of the Stickman movement
    public float attackInterval = 5.0f; // Time between attacks
    public float frenzyHpRatioThreshold = 0.3f; // Health ratio threshold for frenzy mode
    public float frenzyMovementSpeedMultiplier = 1.5f; // Frenzy movement speed multiplier
    public float frenzyAttackInterval = 4.0f; // Attack interval during frenzy mode
    public int maxHealth = 5; // Health of the Stickman
    public int recentSpawnMemory = 5; // recent spawn memory for penalty calculation when selecting random items
    public float recentSpawnPenalty = 0.5f; // 0: no penalty, 1: full penalty

    private RectTransform rectTransform;
    private Vector2 minBounds, maxBounds;
    private float halfHeight, halfWidth;
    private Vector2 stickmanSize;
    private Canvas canvas;
    private RectTransform enemyPowerContainer;
    private RectTransform playerPowerContainer;
    private int health;
    private Image stickmanImage;
    private List<int> recentlySpawnedItems = new List<int>();

    private float attackTimer;
    private bool isImmune = false;
    private bool isFrenzy = false;
    private bool isGameOver = false;

    public Canvas MainCanvas
    {
        get { return canvas; }
    }

    public RectTransform EnemyPowerContainer
    {
        get { return enemyPowerContainer; }
    }

    public RectTransform PlayerPowerContainer
    {
        get { return playerPowerContainer; }
    }

    public bool IsGameOver
    {
        get { return isGameOver; }
    }

    public event System.Action OnLevelFailed;
    public event System.Action OnLevelCompleted;


    public void Initialize(Canvas canvas, RectTransform enemyPowerContainer, RectTransform playerPowerContainer)
    {
        this.canvas = canvas;
        this.enemyPowerContainer = enemyPowerContainer;
        this.playerPowerContainer = playerPowerContainer;
    }

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        //Canvas canvas = GetComponentInParent<Canvas>();

        if (canvas != null)
        {
            RectTransform canvasRect = canvas.GetComponent<RectTransform>();
            minBounds = canvasRect.rect.min;
            maxBounds = canvasRect.rect.max;

            // Get the size of the Stickman
            stickmanSize = rectTransform.rect.size;
        }

        health = maxHealth; // Initialize health to max health

        attackTimer = attackInterval;
        stickmanImage = GetComponent<Image>();

        // Start the coroutine to spawn hearts
        StartCoroutine(SpawnItem());
        StartCoroutine(RicochetEffect());
        //SetImmunity(1000f);

        // pre-fill the recently spawned items list
        for (int i = 0; i < recentSpawnMemory; i++)
        {
            recentlySpawnedItems.Add(i % itemPrefabs.Length);
        }

        if (stickmanImage.material != null)
        {
            stickmanImage.material = new Material(stickmanImage.material); // Create a new instance of the material to avoid shared state issues
        }
    }

    void Update()
    {
        if (isGameOver) return; // Skip update if game is over

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

            if (isFrenzy)
            {
                attackTimer = frenzyAttackInterval; // Reset attack timer for frenzy mode
            }
            else
            {
                attackTimer = attackInterval; // Reset attack timer for normal mode
            }
            //attackTimer = attackInterval;
        }

        // Check for cursor collision
        if (IsCursorColliding() && !isImmune)
        {
            Debug.Log("Stickman");
            LevelOver();
        }
    }

    void PerformAttack()
    {
        // Example attacks
        int attackType = Random.Range(1, 4);

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
            case 3:
                // Attack 5: Summon cards
                StartCoroutine(SummonCards());
                break;
        }
    }

    IEnumerator RicochetEffect()
    {
        Vector2 direction = new Vector2(60, 60).normalized;
        float speed = movementSpeed;

        // if (isFrenzy)
        // {
        //     speed *= frenzyMovementSpeedMultiplier; // Increase speed in frenzy mode
        // }

        float ricochetDuration = Mathf.Infinity; // Infinite duration
        float elapsedTime = 0;

        while (elapsedTime < ricochetDuration)
        {
            Vector2 pos = rectTransform.anchoredPosition;
            if (!isGameOver)
            {
                pos += direction * speed * (isFrenzy ? frenzyMovementSpeedMultiplier : 1) * Time.deltaTime;
            }

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
            projectileRect.SetParent(enemyPowerContainer, true); // Set parent to enemyPowerContainer
            Boolet booletComponent = projectile.GetComponent<Boolet>();
            if (booletComponent != null)
            {
                booletComponent.Initialize(canvas); // Initialize the Boolet with the canvas
            }
            // Set initial position and direction
            projectileRect.anchoredPosition = rectTransform.anchoredPosition;
            Vector2 cursorPosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform,
                cursorPosition,
                canvas.worldCamera,
                out Vector2 localCursorPosition
            );

            Vector2 direction = (localCursorPosition - rectTransform.anchoredPosition).normalized;

            // Move the projectile
            StartCoroutine(MoveProjectile(projectileRect, direction));
        }
    }

    IEnumerator MoveProjectile(RectTransform projectileRect, Vector2 direction)
    {
        if (isGameOver) yield break; // Exit if game is over

        float speed = 200f; // Increased speed
        float homingStrength = 0.1f;
        float lifetime = 4.0f;
        float elapsedTime = 0;

        //Canvas canvas = GetComponentInParent<Canvas>();
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

            if (projectileRect == null) yield break; // Exit if projectileRect is destroyed
            // Homing behavior
            Vector2 desiredDirection = (localCursorPosition - projectileRect.anchoredPosition).normalized;
            direction = Vector2.Lerp(direction, desiredDirection, homingStrength);
            if (!isGameOver)
            {
                projectileRect.anchoredPosition += direction * speed * Time.deltaTime;
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (projectileRect == null) yield break; // Exit if projectileRect is destroyed
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
            warningLine.transform.SetParent(enemyPowerContainer, true); // Set parent to enemyPowerContainer
            Boolet booletComponent = warningLine.GetComponent<Boolet>();
            if (booletComponent != null)
            {
                booletComponent.Initialize(canvas); // Initialize the Boolet with the canvas
            }

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
            if (warningLine == null) continue; // Skip if the warning line was destroyed

            warningLine.GetComponent<Boolet>().IsHarmful(true); // Assuming the warning line has an IsHarmful property

            DOTween.Sequence()
                .Append(warningLine.GetComponent<RectTransform>().DOScale(new Vector3(3f, 3f, 1), 0.15f))
                .Append(warningLine.GetComponent<RectTransform>().DOScale(Vector3.one, 0.1f));

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
            if (warningLine == null) continue; // Skip if the warning line was already destroyed

            DOTween.Sequence()
                .Append(warningLine.GetComponent<RectTransform>().DOScale(Vector3.zero, 0.2f))
                .OnComplete(() => Destroy(warningLine)); // Destroy after scaling down
        }
    }

    IEnumerator SummonCards()
    {
        float cardDuration = 3.0f;
        float elapsedTime = 0;

        while (elapsedTime < cardDuration)
        {
            // Get a random card prefab
            GameObject cardPrefab = cards[Random.Range(0, cards.Length)];

            // Calculate a random position within the bounds
            float randomX = Random.Range(minBounds.x + stickmanSize.x / 2, maxBounds.x - stickmanSize.x / 2);
            float notrandomY = maxBounds.y + stickmanSize.y;
            Vector2 randomPosition = new Vector2(randomX, notrandomY);
            // convert random position to screen position
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(randomPosition);

            // Instantiate the card
            GameObject card = Instantiate(cardPrefab, screenPosition, Quaternion.identity, transform.parent);
            RectTransform cardRect = card.GetComponent<RectTransform>();
            cardRect.anchoredPosition = randomPosition;
            card.transform.SetParent(enemyPowerContainer, true); // Set parent to bulletContainer

            Boolet booletComponent = card.GetComponent<Boolet>();
            if (booletComponent != null)
            {
                booletComponent.Initialize(canvas); // Initialize the Boolet with the canvas
            }

            elapsedTime += 0.2f;
            yield return new WaitForSeconds(0.2f);
        }
    }

    bool IsCursorColliding()
    {
        Vector2 cursorPosition = Input.mousePosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            cursorPosition,
            canvas.worldCamera,
            out Vector2 localCursorPosition
        );

        return rectTransform.rect.Contains(localCursorPosition);
    }

    IEnumerator SpawnItem()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(itemSpawnMinInterval, itemSpawnMaxInterval)); // Wait for a random time between 5 and 10 seconds

            if (itemPrefabs != null)
            {
                // Get a random item prefab
                GameObject itemPrefab = GetWeightedRandomItem(); // the more frequent an item spawned recently, the less likely will get selected
                int selectedItemIndex = System.Array.IndexOf(itemPrefabs, itemPrefab);
                Vector2 randomPosition;

                int tries = 0;
                do
                { // repeat when the item is too close to stickman
                    // Calculate a random position within the bounds
                    float randomX = Random.Range(minBounds.x + stickmanSize.x / 2, maxBounds.x - stickmanSize.x / 2);
                    float randomY = Random.Range(minBounds.y + stickmanSize.y / 2, maxBounds.y - stickmanSize.y / 2);
                    randomPosition = new Vector2(randomX, randomY);
                    tries++;
                    // Check if the item is too close to the Stickman
                    if (Vector2.Distance(randomPosition, rectTransform.anchoredPosition) > itemSpawnMinDistance) break; // Adjust the distance threshold as needed
                } while (tries < maxTry);

                // Instantiate the item
                GameObject item = Instantiate(itemPrefab, randomPosition, Quaternion.identity, transform.parent);
                RectTransform itemRect = item.GetComponent<RectTransform>();
                item.transform.SetParent(playerPowerContainer, true); // Set parent to bulletContainer
                itemRect.anchoredPosition = randomPosition;

                // Track recent spawned item
                AddToRecentSpawned(selectedItemIndex); // Add to recent spawned items

                Heart itemComponent = item.GetComponent<Heart>();
                if (itemComponent != null)
                {
                    itemComponent.Initialize(this); // Initialize the Heart with the Stickman reference
                }
            }
        }
    }

    public void ReduceHealth(int amount)
    {
        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth); // Ensure health does not go below 0
        Debug.Log("Stickman health: " + health);
        if (amount > 0)
        {
            //flash color for damage
            DOTween.Sequence()
             .Append(stickmanImage.material.DOColor(Color.red, 0.15f))
             .Append(stickmanImage.material.DOColor(Color.white, 0.15f))
             .Append(stickmanImage.material.DOColor(Color.black, 0.01f));

            //shake
            transform.DOShakePosition(0.1f, 30, 10, 90, false, true);
        }
        else if (amount < 0)
        {
            //flash color for healing
            DOTween.Sequence()
             .Append(stickmanImage.material.DOColor(Color.green, 0.3f))
             .Append(stickmanImage.material.DOColor(Color.black, 0.01f));
        }
        CheckFrenzyMode();

        if (health <= 0)
        {
            Death();
        }

    }

    private void CheckFrenzyMode()
    {
        if ((float)health / maxHealth <= frenzyHpRatioThreshold)
        {
            isFrenzy = true;
            Debug.Log("Stickman is now in frenzy mode!");
            GetComponent<Image>().color = Color.red; // Change color to indicate frenzy mode
        }
        else
        {
            isFrenzy = false;
            Debug.Log("Stickman is no longer in frenzy mode.");
            GetComponent<Image>().color = Color.white; // Reset color
        }
    }

    public void Death()
    {
        //drop coin
        Collectible coin = Instantiate(coinDrop, canvas.transform);
        RectTransform coinRect = coin.GetComponent<RectTransform>();
        coinRect.anchoredPosition = rectTransform.anchoredPosition; // Set the position of the coin to the Stickman's position
        Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
        if (rb != null)
        {

            rb.AddForce(new Vector2(0, 3f)); // Apply force to the coin
        }

        OnLevelCompleted?.Invoke(); // Trigger the level completed event
        gameObject.SetActive(false); // Deactivate the Stickman object
    }

    public void SetImmunity()
    {
        if (isImmune) { DestroyImmunity(); }
        float duration = immunityImagePrefab.GetComponent<FlashBeforeDisappear>().TimeToLive;
        StartCoroutine(ImmunityCoroutine(duration));
    }

    private IEnumerator ImmunityCoroutine(float duration)
    {
        isImmune = true;
        GameObject immunityImage = Instantiate(immunityImagePrefab, canvas.transform); // Use canvas instead of transform.parent
        RectTransform immunityImageRect = immunityImage.GetComponent<RectTransform>();

        // Get the correct camera reference
        Camera uiCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        float elapsedTime = 0;
        while (elapsedTime < duration)
        {
            Vector2 cursorPosition = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.GetComponent<RectTransform>(), // Use canvas rect instead of parent
                cursorPosition,
                uiCamera, // Use proper camera reference
                out Vector2 localCursorPosition
            );

            if (immunityImageRect == null) yield break; // Exit if immunityImageRect is destroyed halfway

            immunityImageRect.anchoredPosition = localCursorPosition;
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        if (isImmune) DestroyImmunity();
    }

    public void DestroyImmunity()
    {
        GameObject immunityImage = GameObject.Find("BarrierImage(Clone)");
        Destroy(immunityImage);
        isImmune = false;
    }

    private GameObject GetWeightedRandomItem()
    {
        float[] weights = new float[itemPrefabs.Length];
        for (int i = 0; i < itemPrefabs.Length; i++)
        {
            weights[i] = 1.0f;

            int recentCount = CountRecentSpawns(i);

            if (recentCount > 0)
            {
                weights[i] -= Mathf.Pow(recentSpawnPenalty, recentCount);
            }
        }
        return SelectWeightedRandom(weights);
    }

    private int CountRecentSpawns(int itemIndex)
    {
        int count = 0;
        foreach (int recentIndex in recentlySpawnedItems)
        {
            if (recentIndex == itemIndex)
            {
                count++;
            }
        }
        return count;
    }

    private GameObject SelectWeightedRandom(float[] weights)
    {

        float totalWeight = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            totalWeight += weights[i];

        }

        string stat = "";

        for (int i = 0; i < recentlySpawnedItems.Count; i++)
        {
            stat += $"{itemPrefabs[recentlySpawnedItems[i]].name} ";
        }

        stat += "\nWeights: \n";

        for (int i = 0; i < weights.Length; i++)
        {
            stat += $"{itemPrefabs[i].name}: {weights[i] / totalWeight}% \n";
        }

        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;
        for (int i = 0; i < weights.Length; i++)
        {
            currentWeight += weights[i];
            if (randomValue <= currentWeight)
            {
                stat += $"Selected item: {itemPrefabs[i].name}";
                Debug.Log(stat);
                return itemPrefabs[i];
            }
        }

        stat += "Selected item: " + itemPrefabs[0].name + " (default)";
        Debug.Log(stat);
        return itemPrefabs[0];
    }

    private void AddToRecentSpawned(int itemIndex)
    {
        recentlySpawnedItems.Add(itemIndex);
        if (recentlySpawnedItems.Count > recentSpawnMemory)
        {
            recentlySpawnedItems.RemoveAt(0); // Remove the oldest item if we exceed the memory limit
        }
    }

    public void LevelOver()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isGameOver = true; // Set game over state
        OnLevelFailed?.Invoke(); // Trigger the level failed event
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