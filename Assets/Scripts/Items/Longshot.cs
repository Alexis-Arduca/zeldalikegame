using UnityEngine;

public class Longshot : Item
{
    public Longshot(Sprite sprite) : base("Longshot", sprite) { }

    public override void Use()
    {
        Debug.Log("Use Longshot !");
    }
}
