using System.Collections.Generic;
using UnityEngine;

public class CrystalSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    [Tooltip("Pool of crystal prefabs to choose from. Each spawn picks one at random.")]
    [SerializeField] private GameObject[] prefabs;

    [Header("Spawn Settings")]
    [Tooltip("Maximum number of crystals that should exist under the parent at any time.")]
    [SerializeField] private int maxCount = 5;

    [Tooltip("Fixed Y position for every spawned crystal.")]
    [SerializeField] private float spawnY = 0f;

    [Tooltip("Minimum distance between any two spawned crystals.")]
    [SerializeField] private float minDistance = 1.5f;

    [Tooltip("How many random positions to try before giving up on a single crystal.")]
    [SerializeField] private int maxAttemptsPerObject = 30;

    [Header("Spawn Area")]
    [Tooltip("World-space center of the rectangular spawn area.")]
    [SerializeField] private Vector3 areaCenter = Vector3.zero;

    [Tooltip("Width (X) and depth (Z) of the spawn area.")]
    [SerializeField] private Vector2 areaSize = new Vector2(10f, 10f);

    [Header("Hierarchy")]
    [Tooltip("Parent for spawned crystals. Leave empty to spawn under this GameObject.")]
    [SerializeField] private Transform spawnParent;

    private readonly List<Vector2> _placedPositions = new List<Vector2>();

    // -------------------------------------------------------------------------

    private void Start()
    {
        SpawnMissing();
    }

    /// <summary>
    /// Counts existing children and spawns only as many as needed to reach maxCount.
    /// Safe to call repeatedly (e.g. on player respawn) — will never exceed maxCount.
    /// </summary>
    public void SpawnMissing()
    {
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogWarning($"[CrystalSpawner] '{name}': No prefabs assigned.");
            return;
        }

        Transform parent = spawnParent != null ? spawnParent : transform;
        int existing     = parent.childCount;
        int needed       = maxCount - existing;

        if (needed <= 0)
        {
            Debug.Log($"[CrystalSpawner] '{name}': Already at max ({existing}/{maxCount}). Skipping spawn.");
            return;
        }

        // Seed distance-check list with already existing children.
        _placedPositions.Clear();
        for (int i = 0; i < parent.childCount; i++)
        {
            Vector3 p = parent.GetChild(i).position;
            _placedPositions.Add(new Vector2(p.x, p.z));
        }

        int spawned = 0;
        for (int i = 0; i < needed; i++)
        {
            if (TryPlaceObject(parent))
                spawned++;
            else
                Debug.LogWarning(
                    $"[CrystalSpawner] '{name}': Could not find a valid position for crystal {i + 1} " +
                    $"after {maxAttemptsPerObject} attempts.");
        }

        Debug.Log($"[CrystalSpawner] '{name}': Spawned {spawned}/{needed} — total {existing + spawned}/{maxCount}.");
    }

    private bool TryPlaceObject(Transform parent)
    {
        float halfX = areaSize.x * 0.5f;
        float halfZ = areaSize.y * 0.5f;

        for (int attempt = 0; attempt < maxAttemptsPerObject; attempt++)
        {
            float rx = Random.Range(areaCenter.x - halfX, areaCenter.x + halfX);
            float rz = Random.Range(areaCenter.z - halfZ, areaCenter.z + halfZ);
            Vector2 candidate = new Vector2(rx, rz);

            if (IsTooClose(candidate))
                continue;

            Instantiate(prefabs[Random.Range(0, prefabs.Length)],
                        new Vector3(rx, spawnY, rz),
                        Quaternion.identity,
                        parent);
            _placedPositions.Add(candidate);
            return true;
        }

        return false;
    }

    private bool IsTooClose(Vector2 candidate)
    {
        float sqrMin = minDistance * minDistance;
        foreach (Vector2 placed in _placedPositions)
            if ((candidate - placed).sqrMagnitude < sqrMin)
                return true;
        return false;
    }

    private void OnDrawGizmos()
    {
        Vector3 size   = new Vector3(areaSize.x, 0.02f, areaSize.y);
        Vector3 center = new Vector3(areaCenter.x, spawnY, areaCenter.z);

        Gizmos.color = new Color(0.6f, 0.2f, 1f, 0.35f);
        Gizmos.DrawCube(center, size);

        Gizmos.color = new Color(0.6f, 0.2f, 1f, 1f);
        Gizmos.DrawWireCube(center, size);
    }
}
