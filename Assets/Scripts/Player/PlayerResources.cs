using UnityEngine;

public class PlayerResources : MonoBehaviour
{
    private const int DEFAULT_MAX_ARROW = 30;
    private const int DEFAULT_MAX_BOMB = 10;
    private const int DEFAULT_QUIVER_LEVEL = 1;
    private const int DEFAULT_BOMB_BAG_LEVEL = 1;

    private int currentArrow;
    private int maxArrow;
    private int quiverLevel;
    private int currentBomb;
    private int maxBomb;
    private int bombBagLevel;

    void Start()
    {
        maxArrow = DEFAULT_MAX_ARROW;
        maxBomb = DEFAULT_MAX_BOMB;
        currentArrow = maxArrow;
        currentBomb = maxBomb;
        quiverLevel = DEFAULT_QUIVER_LEVEL;
        bombBagLevel = DEFAULT_BOMB_BAG_LEVEL;

        GameEventsManager.instance.collectibleEvents.onArrowCollected += RefillArrow;
        GameEventsManager.instance.collectibleEvents.onBombCollected += RefillBomb;
    }

    void OnDisable()
    {
        GameEventsManager.instance.collectibleEvents.onArrowCollected -= RefillArrow;
        GameEventsManager.instance.collectibleEvents.onBombCollected -= RefillBomb;
    }

    public bool UseBow()
    {
        if (currentArrow > 0)
        {
            currentArrow -= 1;
            return true;
        }
        return false;
    }

    public bool UseBomb()
    {
        if (currentBomb > 0)
        {
            currentBomb -= 1;
            return true;
        }
        return false;
    }

    public void RefillArrow(int value)
    {
        currentArrow = Mathf.Clamp(currentArrow + value, 0, maxArrow);
    }

    public void RefillBomb(int value)
    {
        currentBomb = Mathf.Clamp(currentBomb + value, 0, maxBomb);
    }

    public void QuiverUpdate()
    {
        quiverLevel += 1;
        if (quiverLevel == 2)
        {
            maxArrow = 50;
            currentArrow = maxArrow;
        }
        else if (quiverLevel == 3)
        {
            maxArrow = 99;
            currentArrow = maxArrow;
        }
    }

    public void BombBagUpdate()
    {
        bombBagLevel += 1;
        if (bombBagLevel == 2)
        {
            maxBomb = 20;
            currentBomb = maxBomb;
        }
        else if (bombBagLevel == 3)
        {
            maxBomb = 30;
            currentBomb = maxBomb;
        }
    }
}
