using UnityEngine;

[CreateAssetMenu(fileName = "NewFireRod", menuName = "Inventory/FireRod")]
public class FireRod : Item
{
    public GameObject fireballPrefab;
    private Transform spawnPoint;

    public FireRod() : base("FireRod", null)
    {
    }

    public override void Use()
    {
        base.Use();

        spawnPoint = GameObject.FindGameObjectWithTag("Player").transform;

        if (spawnPoint != null && fireballPrefab != null)
        {
            PlayerMovement playerMovement = spawnPoint.GetComponent<PlayerMovement>();
            Vector2 direction = playerMovement.GetLastDirection();

            Vector2 spawnPosition = (Vector2)spawnPoint.position + direction.normalized * 1f;

            Instantiate(fireballPrefab, spawnPosition, spawnPoint.rotation);
        }
        else
        {
            Debug.LogError("Fireball prefab or spawn point is not assigned.");
        }
    }
}
