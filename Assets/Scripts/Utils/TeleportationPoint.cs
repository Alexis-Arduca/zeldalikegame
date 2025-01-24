using UnityEngine;

public class TeleportPoint : MonoBehaviour
{
    public string pointName;
    public Vector3 teleportPosition;

    private void Awake()
    {
        teleportPosition = transform.position;
    }
}
