using UnityEngine;
using UnityEngine.UI;

public class PlayerDarkness : MonoBehaviour
{
    public GameObject player;
    public Material lightMaterial;
    public Camera mainCamera;

    void Update()
    {
        Vector2 viewportPos = mainCamera.WorldToViewportPoint(player.transform.position);
        lightMaterial.SetVector("_PlayerPos", viewportPos);
    }
}
