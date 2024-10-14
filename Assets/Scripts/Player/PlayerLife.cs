using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    private int maxPossibleHeart;
    private int maxHeart;
    private double currentHeart;
    private int defense;
    private int heartFragment;

    // Start is called before the first frame update
    void Start()
    {
        maxPossibleHeart = 16;
        maxHeart = 3;
        currentHeart = 3;
        heartFragment = 0;
        defense = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHeart <= 0)
        {
            PlayerDeath();
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
        double damage = Math.Max(attack - defense, 0);
        currentHeart -= damage;

        currentHeart = Math.Max(currentHeart, 0);
        
        Debug.Log($"Player took {damage} damage. Current hearts: {currentHeart}");
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
