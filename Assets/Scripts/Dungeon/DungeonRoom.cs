using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class DungeonRoom : MonoBehaviour
{
    private Vector3 fixedCameraPosition;
    private float fixedOrthographicSize;

    private void Start()
    {
        fixedCameraPosition = GetComponent<Renderer>()?.bounds.center ?? transform.position;

        Bounds bounds = GetComponent<Renderer>()?.bounds ?? new Bounds(transform.position, Vector3.zero);
        float roomHeight = bounds.size.y;
        float roomWidth = bounds.size.x;

        float aspectRatio = (float)Screen.width / Screen.height;
        float sizeBasedOnHeight = roomHeight / 2f;
        float sizeBasedOnWidth = roomWidth / (2f * aspectRatio);
        fixedOrthographicSize = Mathf.Max(sizeBasedOnHeight, sizeBasedOnWidth);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraController.Instance.SetFixedCamera(fixedCameraPosition, fixedOrthographicSize);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraController.Instance.SetFollowPlayer(true);
        }
    }
}
