using UnityEngine;
using UnityEngine.UI;

public class LightFollowFixed : MonoBehaviour
{
    public RectTransform lightImage;
    public GameObject player;
    public Canvas canvas;
    public Camera mainCamera;

    void Update()
    {
        Vector2 viewportPos = mainCamera.WorldToViewportPoint(player.transform.position);
        Vector2 canvasSize = canvas.GetComponent<RectTransform>().sizeDelta;

        Vector2 localPos = new Vector2(
            (viewportPos.x - 0.5f) * canvasSize.x,
            (viewportPos.y - 0.5f) * canvasSize.y
        );

        lightImage.anchoredPosition = localPos;

        lightImage.GetComponent<Image>().material.SetVector("_PlayerPos", viewportPos);
    }
}
