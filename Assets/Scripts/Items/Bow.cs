using UnityEngine;

[CreateAssetMenu(fileName = "NewBow", menuName = "Inventory/Bow")]
public class Bow : Item
{
    public GameObject arrowPrefab;

    public Bow() : base("Bow", null)
    {
    }

    public override void Use()
    {
        Debug.Log("Shoot an Arrow with the bow!");
        ShootArrow();
    }

    private void ShootArrow()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            GameObject arrow = Instantiate(arrowPrefab, player.transform.position, Quaternion.identity);
            Arrow arrowScript = arrow.GetComponent<Arrow>();

            if (arrowScript != null)
            {
                PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
                if (playerMovement != null)
                {
                    arrowScript.direction = playerMovement.GetLastDirection();

                    if (arrowScript.direction.x < 0)
                    {
                        arrow.transform.localScale = new Vector3(-0.4f, 0.4f, 0.4f);
                    }
                    else if (arrowScript.direction.x > 0)
                    {
                        arrow.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                    }

                    if (arrowScript.direction.y > 0)
                    {
                        arrow.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                        arrow.transform.rotation = Quaternion.Euler(0, 0, 90);
                        arrowScript.direction = Vector2.right;
                    }
                    else if (arrowScript.direction.y < 0)
                    {
                        arrow.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
                        arrow.transform.rotation = Quaternion.Euler(0, 0, -90);
                        arrowScript.direction = Vector2.right;
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Player object not found!");
        }
    }
}
