using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DungeonGenerator : MonoBehaviour
{
    [Tooltip("All prefabs template")]
    public List<GameObject> roomPrefabs;
    public List<GameObject> dungeonCollectibles;
    public GameObject dungeonKey;
    public GameObject dungeonBossKey;
    public List<Enemy> dungeonEnemys;
    public List<Enemy> dungeonBoss;

    public int roomCount = 10;
    private Vector2 roomSize = new Vector2(20f, 11f);

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

    /// <summary>
    /// Procedural Dugeon Generation function
    /// </summary>
    void GenerateDungeon()
    {
        Vector2 startPos = Vector2.zero;

        // Select a random room with a 'Start' RoomType
        var startRooms = roomPrefabs.Where(p => p.GetComponent<RoomTemplate>().roomType == RoomTemplate.RoomType.Start).ToList();
        var startRoomPrefab = startRooms[Random.Range(0, startRooms.Count)];
        var startRoomTemplate = startRoomPrefab.GetComponent<RoomTemplate>();
        if (startRoomTemplate.doorBottom)
        {
            Debug.LogError("Selected Start room cannot have doorBottom");
            return;
        }

        // Place 'Start' room (the only one)
        roomPositions.Add(startPos);
        occupiedPositions.Add(startPos);
        var startInstance = Instantiate(startRoomPrefab, Vector2.Scale(startPos, roomSize), Quaternion.identity, transform);

        // Save start room property
        if (startRoomTemplate.hasDoorKey) doorKeyRooms.Add(startPos);
        if (startRoomTemplate.hasChest)
        {
            chestRooms.Add(startPos);
            var chests = startInstance.GetComponentsInChildren<Chest>();
            if (chests.Length == 0)
            {
                Debug.LogWarning($"Start room at {startPos} marked as hasChest but contains no Chest components");
            }
            else
            {
                Debug.Log($"Start room at {startPos} has {chests.Length} chest(s): {string.Join(", ", chests.Select(c => c.GetType().Name))}");
            }
        }

        // Generate other room
        Vector2[] dirs = { Vector2.up, Vector2.left, Vector2.right };
        while (roomPositions.Count < roomCount)
        {
            var basePos = roomPositions[Random.Range(0, roomPositions.Count)];
            foreach (var dir in Shuffle(dirs))
            {
                var np = basePos + dir;
                if (np == startPos + Vector2.down)
                    continue;
                if (!occupiedPositions.Contains(np))
                {
                    roomPositions.Add(np);
                    occupiedPositions.Add(np);
                    break;
                }
            }
        }

        CreateConnections();

        // Instantiate other room
        foreach (var gridPos in roomPositions)
        {
            if (gridPos == startPos) continue;

            bool top = connections.Contains((gridPos, gridPos + Vector2.up));
            bool bot = connections.Contains((gridPos, gridPos + Vector2.down));
            bool left = connections.Contains((gridPos, gridPos + Vector2.left));
            bool right = connections.Contains((gridPos, gridPos + Vector2.right));

            var candidates = roomPrefabs.Where(p =>
            {
                var tpl = p.GetComponent<RoomTemplate>();
                return tpl.roomType != RoomTemplate.RoomType.Start &&
                       tpl.doorTop == top &&
                       tpl.doorBottom == bot &&
                       tpl.doorLeft == left &&
                       tpl.doorRight == right;
            }).ToList();

            var prefab = candidates.Count > 0
                ? candidates[Random.Range(0, candidates.Count)]
                : roomPrefabs.First(p => p.GetComponent<RoomTemplate>().roomType != RoomTemplate.RoomType.Start);

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

        // Be sure if there is at least the same amount of chest and the same amount of door key
        if (chestRooms.Count < doorKeyRooms.Count + 1)
        {
            Debug.LogError($"Not enough chests ({chestRooms.Count}) for {doorKeyRooms.Count} door keys and 1 boss key");
            return;
        }

        // BFS (Breadth-First Search)
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

        // 1) Fill basic key for each door key present in the dungeon
        var filledChests = new HashSet<Vector2>();
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

            if (chestForThis.HasValue)
            {
                FillChestAt(chestForThis.Value, dungeonKey);
                filledChests.Add(chestForThis.Value);
                Debug.Log($"Placed door key in chest at {chestForThis.Value} for door at {doorPos}");
            }
            else
            {
                Debug.LogWarning($"No available chest to place key for door at {doorPos}");
            }
        }

        // 2) Fill a chest with a unique boss key (one per dungeon)
        Vector2? bossKeyChest = null;
        foreach (var room in reachableOrder)
        {
            if (chestRooms.Contains(room) && !filledChests.Contains(room))
            {
                bossKeyChest = room;
                break;
            }
        }

        if (bossKeyChest.HasValue)
        {
            FillChestAt(bossKeyChest.Value, dungeonBossKey);
            filledChests.Add(bossKeyChest.Value);
            Debug.Log($"Placed boss key in chest at {bossKeyChest.Value}");
        }
        else
        {
            Debug.LogWarning("No available chest to place boss key");
        }

        // 3) Fill Chest with collectibles
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

    /// <summary>
    /// Fill all chest based on some parameters
    /// </summary>
    /// <param name="chestGridPos"></param>
    /// <param name="collectiblePrefab"></param>
    /// <param name="item"></param>
    void FillChestAt(Vector2 chestGridPos, GameObject collectiblePrefab = null, Item item = null)
    {
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

        var chests = roomObj.GetComponentsInChildren<Chest>();
        if (chests.Length == 0)
        {
            Debug.LogWarning($"No chests found in room at grid position {chestGridPos}");
            return;
        }

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

    /// <summary>
    /// Create connections with each rooms
    /// </summary>
    void CreateConnections()
    {
        var remaining = new HashSet<Vector2>(roomPositions);
        var connected = new HashSet<Vector2>();
        var dirs = new Vector2[] { Vector2.up, Vector2.left, Vector2.right };
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
            foreach (var dir in new Vector2[] { Vector2.up, Vector2.down, Vector2.left, Vector2.right })
            {
                var nb = pos + dir;
                if (pos == Vector2.zero && dir == Vector2.down)
                    continue;
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
