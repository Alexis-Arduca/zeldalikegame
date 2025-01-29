using UnityEngine;

[CreateAssetMenu(fileName = "NewIceRod", menuName = "Inventory/IceRod")]
public class IceRod : Item
{
    public GameObject iceballPrefab;
    private Transform spawnPoint;
    private int magicCost = 15;

    public IceRod() : base("IceRod", null)
    {
    }

    public override void Use()
    {
        base.Use();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerMagic playerMagic = player.GetComponent<PlayerMagic>();

        if (!playerMagic.CanUse(magicCost))
        {
            Debug.Log("Not enough magic to use IceRod.");
            return;
        }

        Transform spawnPoint = player.transform;
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

        if (spawnPoint != null && iceballPrefab != null)
        {
            Vector2 direction = playerMovement.GetLastDirection();

            Vector2 spawnPosition = (Vector2)spawnPoint.position + direction.normalized * 1f;

            Instantiate(iceballPrefab, spawnPosition, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("Fireball prefab or spawn point is not assigned.");
        }
    }
}
