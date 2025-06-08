using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VillageEnter : MonoBehaviour
{
    [Header("ConnectedNames")]
    public string areaConnectedName;
    public string villageName;

    [Header("UI Elements")]
    public GameObject villageWindow;

    private bool inVillage = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inVillage = !inVillage;

            if (inVillage == true)
            {
                villageWindow.GetComponentInChildren<TMP_Text>().text = villageName;
            }
            else
            {
                villageWindow.GetComponentInChildren<TMP_Text>().text = areaConnectedName;
            }

            villageWindow.GetComponent<VillageWindowUI>().StartAnimation();
        }
    }
}