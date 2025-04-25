using UnityEngine;

public class ChestCollectibles : Chest
{
    public override void FillChest(GameObject collectiblePrefab)
    {
        if (collectiblePrefab == null)
        {
            Debug.LogWarning($"Attempted to fill chest {name} with null collectible");
            return;
        }

        if (!collectiblePrefab.GetComponent<SpriteRenderer>())
        {
            Debug.LogWarning($"Collectible {collectiblePrefab.name} for chest {name} has no SpriteRenderer");
            return;
        }

        collectibleInChest = collectiblePrefab;
        Debug.Log($"Chest {name} filled with collectible {collectiblePrefab.name}");
    }
}