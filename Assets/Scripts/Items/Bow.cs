using UnityEngine;

public class Bow : Item
{
    public GameObject arrowPrefab;

    public Bow(Sprite sprite, GameObject arrowPrefab) : base("Bow", sprite)
    {
        this.arrowPrefab = arrowPrefab;
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
            GameObject arrow = GameObject.Instantiate(arrowPrefab, player.transform.position, Quaternion.identity);

            Arrow arrowScript = arrow.GetComponent<Arrow>();
            if (arrowScript != null)
            {
                arrowScript.direction = Vector2.right;
            }
        }
        else
        {
            Debug.LogError("Player object not found!");
        }
    }
}
