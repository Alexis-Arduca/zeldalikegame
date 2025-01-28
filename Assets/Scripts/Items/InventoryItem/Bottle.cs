using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewBottle ", menuName = "Inventory/Bottle")]
public class Bottle : Item
{
    private bool redPotion;
    private bool bluePotion;
    private bool greenPotion;
    private bool fairy;
    private PlayerLife playerLife;

    public Bottle() : base("Bottle", null)
    {
    }

    private void OnEnable()
    {
        redPotion = false;
        bluePotion = false;
        greenPotion = false;
        fairy = false;
    }

    public override void Use()
    {
        playerLife = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLife>();

        if (redPotion) {
            HealBottle(playerLife, 5);
            redPotion = false;
        } else if (bluePotion) {
            HealBottle(playerLife, playerLife.GetMaxHeart());
            bluePotion = false;
        } else if (greenPotion) {
            RefillBottle(80);
        } else if (fairy) {
            HealBottle(playerLife, 8);
            fairy = false;
        } else {
            // Pouvoir utiliser la bouteille pour attraper autre chose
            Debug.Log("Bottle is Empty");
        }
    }

    private void HealBottle(PlayerLife playerLife, double healValue)
    {
        double heal = playerLife.GetCurrentHeart() + healValue;

        heal = heal > playerLife.GetMaxHeart() ? playerLife.GetMaxHeart() : heal;

        playerLife.UpdateCurrentHeart(heal);
    }

    private void RefillBottle(int value)
    {
        GameEventsManager.instance.magicEvents.OnMagicCollected(value);
    }


    public void SetRedPotion()
    {
        redPotion = true;
    }

    public void SetBluePotion()
    {
        bluePotion = true;
    }

    public void SetGreenPotion()
    {
        greenPotion = true;
    }

    public void SetFairy()
    {
        fairy = true;
    }

    public bool GetRedPotion()
    {
        return redPotion;
    }

    public bool GetBluePotion()
    {
        return bluePotion;
    }

    public bool GetGreenPotion()
    {
        return greenPotion;
    }

    public bool GetFairy()
    {
        return fairy;
    }

    public bool IsEmpty()
    {
        if (redPotion == false && bluePotion == false && greenPotion == false && fairy == false) {
            return true;
        }
        return false;
    }
}
