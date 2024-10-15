using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NewBottle ", menuName = "Inventory/Bottle")]
public class Bottle : Item
{
    private bool redPotion;
    private bool bluePotion;
    private bool fairy;
    private PlayerLife playerLife;

    public Bottle() : base("Bottle", null)
    {
    }

    private void OnEnable()
    {
        redPotion = false;
        bluePotion = false;
        fairy = false;
    }

    public override void Use()
    {
        playerLife = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerLife>();

        if (redPotion) {
            UseBottle(playerLife, 5);
            redPotion = false;
        } else if (bluePotion) {
            UseBottle(playerLife, playerLife.GetMaxHeart());
            bluePotion = false;
        } else if (fairy) {
            UseBottle(playerLife, 8);
            fairy = false;
        } else {
            // Pouvoir utiliser la bouteille pour attraper autre chose
            Debug.Log("Bottle is Empty");
        }
    }

    private void UseBottle(PlayerLife playerLife, double healValue)
    {
        double heal = playerLife.GetCurrentHeart() + healValue;

        heal = heal > playerLife.GetMaxHeart() ? playerLife.GetMaxHeart() : heal;

        playerLife.UpdateCurrentHeart(heal);
    }

    public void SetRedPotion()
    {
        redPotion = true;
    }

    public void SetBluePotion()
    {
        bluePotion = true;
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

    public bool GetFairy()
    {
        return fairy;
    }

    public bool IsEmpty()
    {
        if (redPotion == false && bluePotion == false && fairy == false) {
            return true;
        }
        return false;
    }
}
