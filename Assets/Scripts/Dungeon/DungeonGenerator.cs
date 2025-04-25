using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DungeonGenerator : MonoBehaviour
{
    [Tooltip("All prefabs template")]
    public List<GameObject> roomPrefabs;
    public List<GameObject> dungeonCollectibles;
    public List<Enemy> dungeonEnemys;
    public List<Enemy> dungeonBoss;

    public int roomCount = 10;
    public Vector2 roomSize = new Vector2(20f, 11f);

    private HashSet<Vector2> occupiedPositions = new HashSet<Vector2>();
    private List<Vector2> roomPositions = new List<Vector2>();
    private HashSet<(Vector2, Vector2)> connections = new HashSet<(Vector2, Vector2)>();

    private List<Vector2> doorKeyRooms = new List<Vector2>();
    private List<Vector2> chestRooms = new List<Vector2>();

    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        Vector2 startPos = Vector2.zero;
        roomPositions.Add(startPos);
        occupiedPositions.Add(startPos);

        Vector2[] dirs = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        while (roomPositions.Count < roomCount)
        {
            var basePos = roomPositions[Random.Range(0, roomPositions.Count)];
            foreach (var dir in Shuffle(dirs))
            {
                var np = basePos + dir;
                if (!occupiedPositions.Contains(np))
                {
                    roomPositions.Add(np);
                    occupiedPositions.Add(np);
                    break;
                }
            }
        }

        CreateConnections();

        foreach (var gridPos in roomPositions)
        {
            bool top = connections.Contains((gridPos, gridPos + Vector2.up));
            bool bot = connections.Contains((gridPos, gridPos + Vector2.down));
            bool left = connections.Contains((gridPos, gridPos + Vector2.left));
            bool right = connections.Contains((gridPos, gridPos + Vector2.right));

            var candidates = roomPrefabs.Where(p =>
            {
                var tpl = p.GetComponent<RoomTemplate>();
                return tpl.doorTop == top &&
                       tpl.doorBottom == bot &&
                       tpl.doorLeft == left &&
                       tpl.doorRight == right;
            }).ToList();

            var prefab = candidates.Count > 0
                ? candidates[Random.Range(0, candidates.Count)]
                : roomPrefabs[0];

            var worldPos = Vector2.Scale(gridPos, roomSize);
            var instance = Instantiate(prefab, worldPos, Quaternion.identity, transform);

            var tplComp = instance.GetComponent<RoomTemplate>();
            if (tplComp.hasDoorKey) doorKeyRooms.Add(gridPos);
            if (tplComp.hasChest)
            {
                chestRooms.Add(gridPos);
                var chests = instance.GetComponentsInChildren<Chest>();
                if (chests.Length == 0)
                {
                    Debug.LogWarning($"Room at {gridPos} marked as hasChest but contains no Chest components");
                }
                else
                {
                    Debug.Log($"Room at {gridPos} has {chests.Length} chest(s): {string.Join(", ", chests.Select(c => c.GetType().Name))}");
                }
            }
        }

        // Check collectibles availables
        if (dungeonCollectibles.Count == 0)
        {
            Debug.LogWarning("No collectibles assigned to dungeonCollectibles list");
        }
        else
        {
            Debug.Log($"Available collectibles: {dungeonCollectibles.Count}");
        }

        // Prepare collectibles (key)
        var keyCollectibles = dungeonCollectibles;
        if (keyCollectibles.Count < doorKeyRooms.Count)
        {
            Debug.LogWarning($"Not enough key collectibles ({keyCollectibles.Count}) for {doorKeyRooms.Count} door key rooms");
        }

        // BFS 
        var queue = new Queue<Vector2>();
        var seen = new HashSet<Vector2> { startPos };
        queue.Enqueue(startPos);

        var reachableOrder = new List<Vector2>();
        while (queue.Count > 0)
        {
            var cur = queue.Dequeue();
            reachableOrder.Add(cur);
            foreach (var dir in dirs)
            {
                var nb = cur + dir;
                if (roomPositions.Contains(nb) && !seen.Contains(nb))
                {
                    seen.Add(nb);
                    queue.Enqueue(nb);
                }
            }
        }

        // 1) Place key if door key
        var filledChests = new HashSet<Vector2>();
        int keyIndex = 0;
        foreach (var doorPos in doorKeyRooms)
        {
            Vector2? chestForThis = null;
            foreach (var room in reachableOrder)
            {
                if (room == doorPos) break;
                if (chestRooms.Contains(room) && !filledChests.Contains(room))
                {
                    chestForThis = room;
                    break;
                }
            }

            if (chestForThis.HasValue && keyIndex < keyCollectibles.Count)
            {
                FillChestAt(chestForThis.Value, keyCollectibles[keyIndex]);
                filledChests.Add(chestForThis.Value);
                keyIndex++;
            }
            else
            {
                Debug.LogWarning($"Could not place key for door at {doorPos}: " +
                    (chestForThis.HasValue ? "No more key collectibles" : "No available chest"));
            }
        }

        // 2) Filling other chests
        Debug.Log($"=====================[ Filling {chestRooms.Count} chests ]");
        foreach (var chestPos in chestRooms)
        {
            if (filledChests.Contains(chestPos))
                continue;

            if (dungeonCollectibles.Count > 0)
            {
                var payload = dungeonCollectibles[Random.Range(0, dungeonCollectibles.Count)];
                FillChestAt(chestPos, payload);
                filledChests.Add(chestPos);
            }
            else
            {
                Debug.LogWarning($"No collectibles available to fill chest at {chestPos}");
            }
        }
    }

    void FillChestAt(Vector2 chestGridPos, GameObject collectiblePrefab = null, Item item = null)
    {
        // Find the room corresponding to the position
        var roomObj = transform.Cast<Transform>()
            .Select(t => t.gameObject)
            .FirstOrDefault(go =>
                Vector2.Scale(go.transform.position, Vector2.one / roomSize) == chestGridPos
            );

        if (roomObj == null)
        {
            Debug.LogWarning($"No room found at grid position {chestGridPos}");
            return;
        }

        // Looking at all chests in a room
        var chests = roomObj.GetComponentsInChildren<Chest>();
        if (chests.Length == 0)
        {
            Debug.LogWarning($"No chests found in room at grid position {chestGridPos}");
            return;
        }

        // One per one chest
        Chest targetChest = null;
        if (collectiblePrefab != null)
        {
            targetChest = chests.OfType<ChestCollectibles>().FirstOrDefault();
            if (targetChest != null)
            {
                targetChest.FillChest(collectiblePrefab);
                Debug.Log($"Filled chest at {chestGridPos} with collectible {collectiblePrefab.name}");
            }
            else
            {
                Debug.LogWarning($"No ChestCollectibles found in room at {chestGridPos} for collectible {collectiblePrefab.name}. Available chest types: {string.Join(", ", chests.Select(c => c.GetType().Name))}");
            }
        }
        else if (item != null)
        {
            targetChest = chests.OfType<ChestItem>().FirstOrDefault();
            if (targetChest != null)
            {
                targetChest.FillChest(item);
                Debug.Log($"Filled chest at {chestGridPos} with item {item.name}");
            }
            else
            {
                Debug.LogWarning($"No ChestItem found in room at {chestGridPos} for item {item.name}. Available chest types: {string.Join(", ", chests.Select(c => c.GetType().Name))}");
            }
        }
        else
        {
            Debug.LogWarning($"No collectible or item provided for chest at {chestGridPos}");
        }
    }

    void CreateConnections()
    {
        var remaining = new HashSet<Vector2>(roomPositions);
        var connected = new HashSet<Vector2>();
        var dirs = new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        var start = roomPositions[0];
        connected.Add(start);
        remaining.Remove(start);

        while (remaining.Count > 0)
        {
            foreach (var cur in connected.ToList())
            {
                foreach (var dir in dirs)
                {
                    var nb = cur + dir;
                    if (remaining.Contains(nb))
                    {
                        connections.Add((cur, nb));
                        connections.Add((nb, cur));
                        connected.Add(nb);
                        remaining.Remove(nb);
                        break;
                    }
                }
            }
        }

        foreach (var pos in roomPositions)
        {
            foreach (var dir in dirs)
            {
                var nb = pos + dir;
                if (roomPositions.Contains(nb) &&
                    !connections.Contains((pos, nb)) &&
                    Random.value < 0.2f)
                {
                    connections.Add((pos, nb));
                    connections.Add((nb, pos));
                }
            }
        }
    }

    Vector2[] Shuffle(Vector2[] arr)
    {
        var copy = (Vector2[])arr.Clone();
        for (int i = 0; i < copy.Length; i++)
        {
            int j = Random.Range(i, copy.Length);
            (copy[i], copy[j]) = (copy[j], copy[i]);
        }
        return copy;
    }
}