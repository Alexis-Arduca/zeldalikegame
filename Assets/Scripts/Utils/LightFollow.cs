using UnityEngine;
using UnityEngine.UI;

public class LightFollow : MonoBehaviour
{
    public RectTransform lightImage;
    public Camera mainCamera;
    public GameObject player;

    void Update()
    {
        Vector3 playerPos = player.transform.position;
        
        Vector2 screenPos = mainCamera.WorldToScreenPoint(playerPos);

        lightImage.position = screenPos;

        lightImage.GetComponent<Image>().material.SetVector("_PlayerPos", screenPos / new Vector2(Screen.width, Screen.height));
    }
}
