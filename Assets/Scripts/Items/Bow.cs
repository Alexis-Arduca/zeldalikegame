using UnityEngine;

public class Bow : Item
{
    public Bow(Sprite sprite) : base("Bow", sprite) { } // Passer le sprite au constructeur de Item

    public override void Use()
    {
        Debug.Log("Shoot an Arrow with the bow!");
    }
}
