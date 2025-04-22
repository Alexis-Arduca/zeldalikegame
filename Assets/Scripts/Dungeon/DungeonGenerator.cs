using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DungeonGenerator : MonoBehaviour
{
    [Tooltip("All prefabs template")]
    public List<GameObject> roomPrefabs;

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

            GameObject toInstantiate;
            if (candidates.Count > 0)
            {
                toInstantiate = candidates[Random.Range(0, candidates.Count)];
            }
            else
            {
                toInstantiate = roomPrefabs[0];
            }

            Vector2 worldPos = Vector2.Scale(gridPos, roomSize);
            Instantiate(toInstantiate, worldPos, Quaternion.identity, transform);
        }
    }

    void CreateConnections()
    {
        var remaining = new HashSet<Vector2>(roomPositions);
        var connected = new HashSet<Vector2>();
        var edges = new List<(Vector2 from, Vector2 to)>();

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
}
