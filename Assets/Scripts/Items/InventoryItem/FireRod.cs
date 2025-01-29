using UnityEngine;

[CreateAssetMenu(fileName = "NewFireRod", menuName = "Inventory/FireRod")]
public class FireRod : Item
{
    public GameObject fireballPrefab;
    private int magicCost = 20;

    public FireRod() : base("FireRod", null)
    {
    }

    public override void Use()
    {
        base.Use();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerMagic playerMagic = player.GetComponent<PlayerMagic>();

        if (!playerMagic.CanUse(magicCost))
        {
            Debug.Log("Not enough magic to use FireRod.");
            return;
        }

        Transform spawnPoint = player.transform;
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        if (playerMovement == null)
        {
            Debug.LogError("PlayerMovement component not found!");
            return;
        }

        Vector2 direction = playerMovement.GetLastDirection();
        Vector2 spawnPosition = (Vector2)spawnPoint.position + direction.normalized * 1f;

        Instantiate(fireballPrefab, spawnPosition, spawnPoint.rotation);
        playerMagic.ConsumeMagic(magicCost);
    }
}
