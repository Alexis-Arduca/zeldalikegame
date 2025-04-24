using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(BoxCollider2D))]
public class DungeonRoom : MonoBehaviour
{
    private Vector3 fixedCameraPosition;
    private float fixedOrthographicSize;
    private RoomTemplate roomTemplate;
    private List<GameObject> doors;

    public Enemy assignedBoss;
    public List<Enemy> assignedEnemies;

    private void Awake()
    {
        roomTemplate = GetComponent<RoomTemplate>();
        if (roomTemplate == null)
        {
            Debug.LogWarning("RoomTemplate not found on " + gameObject.name);
            return;
        }

        fixedCameraPosition = GetComponent<Renderer>()?.bounds.center ?? transform.position;

        Bounds bounds = GetComponent<Renderer>()?.bounds ?? new Bounds(transform.position, Vector3.zero);
        float roomHeight = bounds.size.y;
        float roomWidth = bounds.size.x;

        float aspectRatio = (float)Screen.width / Screen.height;
        float sizeBasedOnHeight = roomHeight / 2f;
        float sizeBasedOnWidth = roomWidth / (2f * aspectRatio);
        fixedOrthographicSize = Mathf.Max(sizeBasedOnHeight, sizeBasedOnWidth);

        HandleRoomTypeOnStart();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        CameraController.Instance.SetFixedCamera(fixedCameraPosition, fixedOrthographicSize);

        switch (roomTemplate.roomType)
        {
            case RoomTemplate.RoomType.Start:
                break;
            case RoomTemplate.RoomType.Boss:
                EnableDoors();
                break;
            case RoomTemplate.RoomType.Fight:
                EnableDoors();
                break;
            case RoomTemplate.RoomType.Treasure:
                break;
            case RoomTemplate.RoomType.Corridor:
                break;
            case RoomTemplate.RoomType.Normal:
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraController.Instance.SetFollowPlayer(true);
        }
    }

    private void HandleRoomTypeOnStart()
    {
        Bounds bounds = GetComponent<Renderer>()?.bounds ?? new Bounds(transform.position, Vector3.zero);

        GetDoors();

        switch (roomTemplate.roomType)
        {
            case RoomTemplate.RoomType.Boss:
                SpawnBoss(bounds);
                break;

            case RoomTemplate.RoomType.Treasure:
                break;

            case RoomTemplate.RoomType.Fight:
                SpawnEnemies(bounds);
                break;

            case RoomTemplate.RoomType.Corridor:
                break;
        }
    }

/// <summary>
/// Door System
/// </summary>
    private void GetDoors()
    {
        doors = new List<GameObject>();

        foreach (Transform child in transform)
        {
            if (child.CompareTag("Door"))
            {
                doors.Add(child.gameObject);
            }
        }

        DisableDoors();
    }

    private void EnableDoors()
    {
        foreach (GameObject door in doors)
        {
            if (door != null)
            {
                door.SetActive(true);
            }
        }
    }

    private void DisableDoors()
    {
        foreach (GameObject door in doors)
        {
            if (door != null)
            {
                door.SetActive(false);
            }
        }
    }

/// <summary>
/// Enemy Spawn
/// </summary>
/// <param name="bounds"></param>
    private void SpawnBoss(Bounds bounds)
    {
        if (assignedBoss != null)
        {
            Vector3 spawnPos = bounds.center + new Vector3(0, bounds.size.y / 4f, 0);
            var bossObject = Instantiate(assignedBoss.gameObject, spawnPos, Quaternion.identity);
            var boss = bossObject.GetComponent<Enemy>();
            if (boss != null)
            {
                boss.OnDeath += CheckAllEnemiesAndBossDead;
            }
        }
    }

    private void SpawnEnemies(Bounds bounds)
    {
        if (assignedEnemies == null || assignedEnemies.Count == 0)
            return;

        foreach (var enemy in assignedEnemies)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(bounds.min.x + 1f, bounds.max.x - 1f),
                Random.Range(bounds.min.y + 1f, bounds.max.y - 1f),
                0f
            );
            var enemyObject = Instantiate(enemy.gameObject, randomPos, Quaternion.identity);
            var enemyScript = enemyObject.GetComponent<Enemy>();
            if (enemyScript != null)
            {
                enemyScript.OnDeath += CheckAllEnemiesAndBossDead;
            }
        }
    }

    private void CheckAllEnemiesAndBossDead()
    {
        bool allEnemiesDead = true;

        foreach (var enemy in assignedEnemies)
        {
            if (enemy != null && enemy.gameObject != null)
            {
                allEnemiesDead = false;
                break;
            }
        }

        if (assignedBoss != null && assignedBoss.gameObject != null)
        {
            allEnemiesDead = false;
        }

        if (allEnemiesDead)
        {
            EndRoom();
        }
    }

    private void EndRoom()
    {
        switch (roomTemplate.roomType)
        {
            case RoomTemplate.RoomType.Boss:
                DisableDoors();
                break;

            case RoomTemplate.RoomType.Fight:
                DisableDoors();
                break;
        }
    }
}
