using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewOcarina", menuName = "Inventory/Ocarina")]
public class Ocarina : Item
{
    public List<Song> songs;

    public Ocarina() : base("Ocarina", null)
    {
    }

    public override void Use()
    {
        base.Use();
        Debug.Log("Ocarina activated!");

        OcarinaManager ocarinaManager = GameObject.FindObjectOfType<OcarinaManager>();

        if (ocarinaManager != null)
        {
            ocarinaManager.equippedOcarina = this;
        }
        else
        {
            Debug.LogError("No OcarinaManager found in the scene!");
        }
    }
}
