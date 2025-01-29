using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    private int currentArrow;
    private int maxArrow;
    private int quiverLevel;
    private int currentBomb;
    private int maxBomb;
    private int bombBagLevel;

    // Start is called before the first frame update
    void Start()
    {
        maxArrow = 30;
        maxBomb = 10;

        currentArrow = maxArrow;
        currentBomb = maxBomb;

        quiverLevel = 1;
        bombBagLevel = 1;
    }

    public bool UseBow()
    {
        if (currentArrow > 0) {
            currentArrow -= 1;
            return true;
        }
        return false;
    }

    public bool UseBomb()
    {
        if (currentBomb > 0) {
            currentBomb -= 1;
            return true;
        }
        return false;
    }

    public void RefillArrow(int value)
    {
        currentArrow = (currentArrow + value > maxArrow) ? maxArrow : currentArrow + value;
    }

    public void RefillBomb(int value)
    {
        currentBomb = (currentBomb + value > maxBomb) ? maxBomb : currentBomb + value;
    }

    private void QuiverUpdate()
    {
        quiverLevel += 1;

        if (quiverLevel == 2) {
            maxArrow = 50;
            currentArrow = maxArrow;
        } else if (quiverLevel == 3) {
            maxArrow = 99;
            currentArrow = maxArrow;
        }
    }

    private void BombBagUpdate()
    {
        bombBagLevel += 1;

        if (bombBagLevel == 2) {
            maxBomb = 20;
            currentBomb = maxBomb;
        } else if (bombBagLevel == 3) {
            maxBomb = 30;
            currentBomb = maxBomb;
        }
    }
}
