using UnityEngine;
using System.Collections;
using System;

public class PlayerLife : MonoBehaviour
{
    private const int DEFAULT_MAX_HEART = 3;
    private const int MAX_POSSIBLE_HEART = 16;
    private const double DEFAULT_CURRENT_HEART = 2;
    private const float DEFAULT_INVINCIBILITY_DURATION = 1f;

    private Inventory inventory;
    private int maxHeart;
    private double currentHeart;
    private int defense;
    public int heartFragment;

    public bool isInvincible = false;

    [SerializeField] private float invincibilityDuration = DEFAULT_INVINCIBILITY_DURATION;
    [SerializeField] private float flickerSpeed = 0.1f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    void Start()
    {
        maxHeart = DEFAULT_MAX_HEART;
        currentHeart = DEFAULT_CURRENT_HEART;
        heartFragment = 0;
        defense = 0;
        inventory = FindFirstObjectByType<GameManager>()?.GetInventory();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();

        if (inventory == null)
        {
            Debug.LogError("Inventory not found!");
        }

        GameEventsManager.instance.collectibleEvents.onRecoveryHeartCollected += HealFromCollectible;
    }

    void OnDisable()
    {
        GameEventsManager.instance.collectibleEvents.onRecoveryHeartCollected -= HealFromCollectible;
    }

    public void LoadPlayerLife(int loadMax, double loadCurrent, int loadFragment)
    {
        maxHeart = loadMax;
        currentHeart = loadCurrent;
        heartFragment = loadFragment;
    }

    void Update()
    {
        if (currentHeart <= 0)
        {
            Bottle bottleWithFairy = GetBottleWithFairy();
            currentHeart = 0;

            if (bottleWithFairy != null)
            {
                bottleWithFairy.Use();
            }
            else
            {
                PlayerDeath();
            }
        }
    }

    private Bottle GetBottleWithFairy()
    {
        foreach (Item item in inventory.items)
        {
            if (item is Bottle bottle && bottle.GetFairy())
            {
                return bottle;
            }
        }
        return null;
    }

    public void UpdateCurrentHeart(double newHeart)
    {
        currentHeart = newHeart;
    }

    public void HealFromCollectible(double heal)
    {
        currentHeart = Math.Min(currentHeart + heal, maxHeart);
    }

    public int GetMaxHeart() => maxHeart;
    public double GetCurrentHeart() => currentHeart;

    public void TakeDamage(double attack)
    {
        if (isInvincible) return;
        if (GetComponent<PlayerController>().isShieldActive) return;

        isInvincible = true;
        double damage = Math.Max(attack - defense, 0);
        currentHeart = Math.Max(currentHeart - damage, 0);

        StartCoroutine(BecomeTemporarilyInvincible());
    }

    private IEnumerator BecomeTemporarilyInvincible()
    {
        float timer = 0f;

        while (timer < invincibilityDuration)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }

            yield return new WaitForSeconds(flickerSpeed);
            timer += flickerSpeed;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        isInvincible = false;
    }

    public void HeartContainerUpdater()
    {
        if (maxHeart < MAX_POSSIBLE_HEART)
        {
            maxHeart += 1;
            currentHeart = maxHeart;
        }
    }

    public void HeartFragmentUpdater()
    {
        if (maxHeart < MAX_POSSIBLE_HEART)
        {
            heartFragment += 1;
            if (heartFragment >= 4)
            {
                maxHeart += 1;
                heartFragment = 0;
            }
            currentHeart = maxHeart;
        }
    }

    private void PlayerDeath()
    {
        //
    }
}
