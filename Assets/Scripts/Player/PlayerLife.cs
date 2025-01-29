using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    private Inventory inventory;
    private GameManager gameManager;
    private int maxPossibleHeart;
    private int maxHeart;
    private double currentHeart;
    private int defense;
    private int heartFragment;

    public bool isInvincible = false;
    public float invincibilityDuration = 1f;

    // Start is called before the first frame update
    void Start()
    {
        maxPossibleHeart = 16;
        maxHeart = 3;
        currentHeart = 3;
        heartFragment = 0;
        defense = 0;
    
        gameManager = FindObjectOfType<GameManager>();
        inventory = gameManager.GetInventory();
    }

    // Update is called once per frame
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
        for (int i = 0; i < inventory.items.Count; i++)
        {
            Item item = inventory.items[i];
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
        if (currentHeart + heal < maxHeart) {
            currentHeart += heal;
        } else {
            currentHeart = maxHeart;
        }
    }

    public double GetMaxHeart()
    {
        return maxHeart;
    }

    public double GetCurrentHeart()
    {
        return currentHeart;
    }

    public void TakeDamage(double attack)
    {
        if (isInvincible) return;

        double damage = Math.Max(attack - defense, 0);
        currentHeart -= damage;
        currentHeart = Math.Max(currentHeart, 0);

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
        if (maxHeart < maxPossibleHeart) {
            maxHeart += 1;
            currentHeart = maxHeart;

            Debug.Log($"Max hearts increased! Current max hearts: {maxHeart}");
        }
    }

    public void HeartFragmentUpdater()
    {
        if (maxHeart < maxPossibleHeart) {
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
