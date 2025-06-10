using UnityEngine;

public class PlayerMagic : MonoBehaviour
{
    private const int DEFAULT_CURRENT_MAGIC = 20;
    private const int DEFAULT_MAX_MAGIC = 100;
    private const int DEFAULT_MAGIC_LEVEL = 1;

    private int currentMagic;
    private int maxMagic;
    private int magicLevel;

    void Start()
    {
        currentMagic = DEFAULT_CURRENT_MAGIC;
        maxMagic = DEFAULT_MAX_MAGIC;
        magicLevel = DEFAULT_MAGIC_LEVEL;

        GameEventsManager.instance.magicEvents.onMagicCollected += RefillMagic;
        GameEventsManager.instance.magicEvents.onMagicUsed += ConsumeMagic;
    }

    void OnDisable()
    {
        GameEventsManager.instance.magicEvents.onMagicCollected -= RefillMagic;
        GameEventsManager.instance.magicEvents.onMagicUsed -= ConsumeMagic;
    }

    public void RefillMagic(int value)
    {
        currentMagic = Mathf.Clamp(currentMagic + value, 0, maxMagic);
    }

    public bool CanUse(int cost) => cost <= currentMagic;

    public void ConsumeMagic(int cost)
    {
        currentMagic = Mathf.Clamp(currentMagic - cost, 0, maxMagic);
    }

    public int GetCurrentMagic() => currentMagic;
    public int GetCurrentLevel() => magicLevel;

    public void UpgradeMagicLevel()
    {
        magicLevel += 1;
        maxMagic = 100 + (magicLevel - 1) * 50;
    }
}
