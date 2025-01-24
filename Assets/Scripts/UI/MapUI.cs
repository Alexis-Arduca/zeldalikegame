using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapUI : MonoBehaviour
{
    public GameObject buttonPrefab;
    public Transform buttonParent;

    public void PopulateMenu(TeleportPoint[] teleportPoints)
    {
        Debug.Log($"Populating menu with {teleportPoints.Length} teleport points.");

        foreach (Transform child in buttonParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var point in teleportPoints)
        {
            TeleportPoint tempPoint = point;
            GameObject newButton = Instantiate(buttonPrefab, buttonParent);

            TMPro.TextMeshProUGUI buttonText = newButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            buttonText.text = tempPoint.pointName;
    
            Button buttonComponent = newButton.GetComponent<Button>();

            buttonComponent.onClick.RemoveAllListeners();
            buttonComponent.onClick.AddListener(() =>
            {
                Debug.Log($"Button clicked for {tempPoint.pointName}");
                TeleportToPoint(tempPoint);
            });
        }
    }

    public void TeleportToPoint(TeleportPoint point)
    {
        Debug.Log($"Teleporting to {point.pointName} at {point.teleportPosition}");
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            player.transform.position = point.teleportPosition;
        }

        gameObject.SetActive(false);
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
    }
}
