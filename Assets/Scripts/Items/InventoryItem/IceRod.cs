using UnityEngine;

[CreateAssetMenu(fileName = "NewIceRod", menuName = "Inventory/IceRod")]
public class IceRod : Item
{
    public GameObject iceballPrefab;
    private Transform spawnPoint;

    public IceRod() : base("IceRod", null)
    {
    }

    public override void Use()
    {
        base.Use();

        spawnPoint = GameObject.FindGameObjectWithTag("Player").transform;

        if (spawnPoint != null && iceballPrefab != null)
        {
            PlayerMovement playerMovement = spawnPoint.GetComponent<PlayerMovement>();
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
