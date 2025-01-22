using UnityEngine;

[CreateAssetMenu(fileName = "NewBoomerang", menuName = "Inventory/Boomerang")]
public class Boomerang : Item
{
    public GameObject boomerangPrefab;

    public Boomerang() : base("Boomerang", null)
    {
    }

    public override void Use()
    {
        Debug.Log("Use Boomerang");
        LaunchBoomerang();
    }

    private void LaunchBoomerang()
    {
        if (GameObject.FindObjectOfType<BoomerangPrefab>() != null)
        {
            Debug.Log("A boomerang is already active. Cannot launch another one.");
            return;
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            GameObject boomerang = Instantiate(boomerangPrefab, player.transform.position, Quaternion.identity);
            BoomerangPrefab boomerangScript = boomerang.GetComponent<BoomerangPrefab>();

            if (boomerangScript != null)
            {
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    boomerangScript.direction = playerMovement.GetLastDirection();
                }
            }
        }
        else
        {
            Debug.LogError("Player object not found!");
        }
    }
}
