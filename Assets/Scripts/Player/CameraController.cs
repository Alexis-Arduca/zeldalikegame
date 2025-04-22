using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    public Transform player;
    public float followSpeed = 5f;

    private bool followPlayer = true;
    private Vector3 targetPosition;
    private float targetSize;

    private Camera cam;

    void Awake()
    {
        if (Instance == null) Instance = this;
        cam = GetComponent<Camera>();
        targetSize = cam.orthographicSize;
    }

    void LateUpdate()
    {
        if (followPlayer)
        {
            targetPosition = player.position;
        }

        Vector3 smoothPos = Vector3.Lerp(transform.position, new Vector3(targetPosition.x, targetPosition.y, transform.position.z), followSpeed * Time.deltaTime);
        transform.position = smoothPos;

        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, followSpeed * Time.deltaTime);
    }

    public void SetFixedCamera(Vector3 position, float size)
    {
        followPlayer = false;
        targetPosition = position;
        targetSize = size;
    }

    public void SetFollowPlayer(bool follow)
    {
        followPlayer = follow;
        targetSize = 5f;
    }
}
