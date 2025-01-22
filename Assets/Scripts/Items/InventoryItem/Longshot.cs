using UnityEngine;

[CreateAssetMenu(fileName = "NewLongshot", menuName = "Inventory/NewLongshot")]
public class Longshot : Item
{
    public GameObject longshotPrefab;
    private static GameObject currentLongshotInstance;

    public Longshot() : base("Longshot", null)
    {
    }

    public override void Use()
    {
        if (currentLongshotInstance != null)
        {
            Debug.Log("Can't use longshot currently");
            return;
        }

        Debug.Log("Use Longshot!");
        ShootLongshot();
    }

    private void ShootLongshot()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            currentLongshotInstance = Instantiate(longshotPrefab, player.transform.position, Quaternion.identity);
            LongshotProjectile longshotScript = currentLongshotInstance.GetComponent<LongshotProjectile>();

            if (longshotScript != null)
            {
                longshotScript.Initialize(player);
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

                if (playerMovement != null)
                {
                    if (playerMovement.GetLastDirection().x < 0)
                    {
                        longshotScript.transform.localScale = new Vector3(-0.4f, 0.4f, 0.4f);
                    }
                    else if (playerMovement.GetLastDirection().x > 0)
                    {
                        longshotScript.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    }

                    if (playerMovement.GetLastDirection().y > 0)
                    {
                        longshotScript.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                        longshotScript.transform.rotation = Quaternion.Euler(0, 0, 90);
                        longshotScript.direction = Vector2.right;
                    }
                    else if (playerMovement.GetLastDirection().y < 0)
                    {
                        longshotScript.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                        longshotScript.transform.rotation = Quaternion.Euler(0, 0, -90);
                        longshotScript.direction = Vector2.right;
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Player object not found!");
        }
    }

    public static void ResetLongshotInstance()
    {
        currentLongshotInstance = null;
    }
}
