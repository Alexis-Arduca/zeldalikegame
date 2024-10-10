using UnityEngine;

public class Longshot : Item
{
    public GameObject longshotPrefab;
    private GameObject longshotInstance;

    public Longshot(Sprite sprite, GameObject longshotPrefab) : base("Longshot", sprite)
    {
        this.longshotPrefab = longshotPrefab;
    }

    public override void Use()
    {
        Debug.Log("Use Longshot!");

        ShootLongshot();
    }

    private void ShootLongshot()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            longshotInstance = GameObject.Instantiate(longshotPrefab, player.transform.position, Quaternion.identity);

            LongshotProjectile longshotScript = longshotInstance.GetComponent<LongshotProjectile>();
            if (longshotScript != null)
            {
                longshotScript.Initialize(player);
            }
        }
        else
        {
            Debug.LogError("Player object not found!");
        }
    }
}
