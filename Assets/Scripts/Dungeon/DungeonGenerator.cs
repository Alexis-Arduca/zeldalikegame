using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DungeonGenerator : MonoBehaviour
{
    [Tooltip("All prefabs template")]
    public List<GameObject> roomPrefabs;
    public List<Item> dungeonItems;
    public List<Enemy> dungeonEnemys;
    public List<Enemy> dungeonBoss;

    public int roomCount = 10;
    public Vector2 roomSize = new Vector2(20f, 11f);

    private HashSet<Vector2> occupiedPositions = new HashSet<Vector2>();
    private List<Vector2> roomPositions = new List<Vector2>();
    private HashSet<(Vector2, Vector2)> connections = new HashSet<(Vector2, Vector2)>();

    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        Vector2 currentPos = Vector2.zero;
        roomPositions.Add(currentPos);
        occupiedPositions.Add(currentPos);

        Vector2[] directions = new Vector2[] {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right
        };

        while (roomPositions.Count < roomCount)
        {
            Vector2 basePos = roomPositions[Random.Range(0, roomPositions.Count)];

            foreach (Vector2 dir in Shuffle(directions))
            {
                Vector2 newPos = basePos + dir;
                if (!occupiedPositions.Contains(newPos))
                {
                    roomPositions.Add(newPos);
                    occupiedPositions.Add(newPos);
                    break;
                }
            }
        }

        CreateConnections();

        int fightRoomMin = Mathf.FloorToInt(roomCount * 0.01f);
        int fightRoomMax = Mathf.FloorToInt(roomCount * 0.30f);
        int fightRoomCount = Random.Range(fightRoomMin, fightRoomMax + 1);

        var shuffledPositions = new List<Vector2>(roomPositions);
        ShuffleList(shuffledPositions);

        Vector2 startRoom = roomPositions[0];
        Vector2 bossRoom = shuffledPositions.First(pos => pos != startRoom);
        List<Vector2> fightRooms = shuffledPositions
            .Where(pos => pos != startRoom && pos != bossRoom)
            .Take(fightRoomCount).ToList();

        foreach (Vector2 gridPos in roomPositions)
        {
            bool connectTop    = connections.Contains((gridPos, gridPos + Vector2.up));
            bool connectBottom = connections.Contains((gridPos, gridPos + Vector2.down));
            bool connectLeft   = connections.Contains((gridPos, gridPos + Vector2.left));
            bool connectRight  = connections.Contains((gridPos, gridPos + Vector2.right));

            var candidates = roomPrefabs.Where(prefab =>
            {
                var tpl = prefab.GetComponent<RoomTemplate>();
                return tpl.doorTop == connectTop &&
                       tpl.doorBottom == connectBottom &&
                       tpl.doorLeft == connectLeft &&
                       tpl.doorRight == connectRight;
            }).ToList();

            GameObject toInstantiate = (candidates.Count > 0) ?
                candidates[Random.Range(0, candidates.Count)] :
                roomPrefabs[0];

            Vector2 worldPos = Vector2.Scale(gridPos, roomSize);
            GameObject roomObj = Instantiate(toInstantiate, worldPos, Quaternion.identity, transform);

            var template = roomObj.GetComponent<RoomTemplate>();
            var dungeonRoom = roomObj.GetComponent<DungeonRoom>();

            if (gridPos == startRoom)
            {
                template.roomType = RoomTemplate.RoomType.Start;
            }
            else if (gridPos == bossRoom)
            {
                template.roomType = RoomTemplate.RoomType.Boss;
                if (dungeonRoom != null && dungeonBoss.Count > 0)
                {
                    dungeonRoom.assignedBoss = dungeonBoss[Random.Range(0, dungeonBoss.Count)];
                }
            }
            else if (fightRooms.Contains(gridPos))
            {
                template.roomType = RoomTemplate.RoomType.Fight;
                if (dungeonRoom != null && dungeonEnemys.Count > 0)
                {
                    int enemyCount = Random.Range(2, 6);
                    dungeonRoom.assignedEnemies = new List<Enemy>();
                    for (int i = 0; i < enemyCount; i++)
                    {
                        var enemy = dungeonEnemys[Random.Range(0, dungeonEnemys.Count)];
                        dungeonRoom.assignedEnemies.Add(enemy);
                    }
                }
            }
        }
    }

    void CreateConnections()
    {
        var remaining = new HashSet<Vector2>(roomPositions);
        var connected = new HashSet<Vector2>();

        Vector2 start = roomPositions[Random.Range(0, roomPositions.Count)];
        connected.Add(start);
        remaining.Remove(start);

        while (remaining.Count > 0)
        {
            foreach (var current in connected.ToList())
            {
                foreach (var dir in new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right })
                {
                    Vector2 neighbor = current + dir;
                    if (remaining.Contains(neighbor))
                    {
                        connections.Add((current, neighbor));
                        connections.Add((neighbor, current));
                        connected.Add(neighbor);
                        remaining.Remove(neighbor);
                        break;
                    }
                }
            }
        }

        foreach (var pos in roomPositions)
        {
            foreach (var dir in new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right })
            {
                Vector2 neighbor = pos + dir;
                if (roomPositions.Contains(neighbor) &&
                    !connections.Contains((pos, neighbor)) &&
                    Random.value < 0.2f)
                {
                    connections.Add((pos, neighbor));
                    connections.Add((neighbor, pos));
                }
            }
        }
    }

    Vector2[] Shuffle(Vector2[] array)
    {
        Vector2[] copy = (Vector2[])array.Clone();
        for (int i = 0; i < copy.Length; i++)
        {
            int rand = Random.Range(i, copy.Length);
            (copy[i], copy[rand]) = (copy[rand], copy[i]);
        }
        return copy;
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }
}
