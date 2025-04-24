using UnityEngine;

public class RoomTemplate : MonoBehaviour
{
    public bool doorTop;
    public bool doorBottom;
    public bool doorLeft;
    public bool doorRight;

    public enum RoomType { Normal, Start, Boss, Fight, Treasure, Corridor }
    public RoomType roomType = RoomType.Normal;
}
