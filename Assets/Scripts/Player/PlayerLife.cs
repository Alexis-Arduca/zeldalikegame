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
    private int heartFragment;
    public bool isInvincible = false;
    [SerializeField] private float invincibilityDuration = DEFAULT_INVINCIBILITY_DURATION;

    void Start()
    {
        maxHeart = DEFAULT_MAX_HEART;
        currentHeart = DEFAULT_CURRENT_HEART;
        heartFragment = 0;
        defense = 0;
        inventory = FindObjectOfType<GameManager>()?.GetInventory();

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

    public double GetMaxHeart() => maxHeart;
    public double GetCurrentHeart() => currentHeart;

    public void TakeDamage(double attack)
    {
        if (isInvincible) return;

        double damage = Math.Max(attack - defense, 0);
        currentHeart = Math.Max(currentHeart - damage, 0);
        Debug.Log($"Player took {damage} damage. Current hearts: {currentHeart}");
        StartCoroutine(BecomeTemporarilyInvincible());
    }

    private IEnumerator BecomeTemporarilyInvincible()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
    }

    public void HeartContainerUpdater()
    {
        if (maxHeart < MAX_POSSIBLE_HEART)
        {
            maxHeart += 1;
            currentHeart = maxHeart;
            Debug.Log($"Max hearts increased! Current max hearts: {maxHeart}");
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
                Debug.Log($"Max hearts increased! Current max hearts: {maxHeart}");
            }
            currentHeart = maxHeart;
        }
    }

    private void PlayerDeath()
    {
        Debug.Log("Player is dead!");
    }
}
