using UnityEngine;

[CreateAssetMenu(fileName = "NewBomb", menuName = "Inventory/Bomb")]
public class Bomb : Item
{
    public GameObject bomb;

    public Bomb() : base("Bomb", null)
    {
    }

    public override void Use()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        Debug.Log("Use Bomb");

        GameObject bombInstance = Instantiate(bomb, player.transform.position, Quaternion.identity);
        BombBehaviour bombScript = bombInstance.AddComponent<BombBehaviour>();
        
        bombScript.explosionDelay = 2.0f;
        bombScript.explosionRadius = 2.0f;
        bombScript.destructibleLayer = LayerMask.GetMask("Destructible");
    }
}
