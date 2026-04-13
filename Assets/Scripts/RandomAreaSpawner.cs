using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Spawns a configurable number of randomly selected prefabs inside a defined
/// rectangular area at game start. Only X and Z are randomized; Y is fixed.
/// Overlap is prevented via a minimum-distance check. An iteration cap avoids
/// infinite loops when the area is too crowded.
/// </summary>
public class RandomAreaSpawner : MonoBehaviour
{
    // -------------------------------------------------------------------------
    // Inspector fields
    // -------------------------------------------------------------------------

    [Header("Prefabs")]
    [Tooltip("Pool of prefabs to choose from. Each spawn picks one at random.")]
    [SerializeField] private GameObject[] prefabs;

    [Header("Spawn Settings")]
    [Tooltip("Total number of objects to spawn at Start.")]
    [SerializeField] private int spawnCount = 20;

    [Tooltip("Fixed Y position for every spawned object.")]
    [SerializeField] private float spawnY = 0f;

    [Tooltip("Minimum distance between any two spawned objects.")]
    [SerializeField] private float minDistance = 1.5f;

    [Tooltip("How many random positions to try before giving up on a single object.")]
    [SerializeField] private int maxAttemptsPerObject = 30;

    [Header("Spawn Area")]
    [Tooltip("World-space center of the rectangular spawn area.")]
    [SerializeField] private Vector3 areaCenter = Vector3.zero;

    [Tooltip("Width (X) and depth (Z) of the spawn area.")]
    [SerializeField] private Vector2 areaSize = new Vector2(10f, 10f);

    [Header("Hierarchy")]
    [Tooltip("Optional parent Transform for spawned objects. Leave empty to spawn at root.")]
    [SerializeField] private Transform spawnParent;

    // -------------------------------------------------------------------------
    // Private state
    // -------------------------------------------------------------------------

    // Stores the XZ positions of successfully placed objects for distance checks.
    private readonly List<Vector2> _placedPositions = new List<Vector2>();

    // Tracks objects instantiated from the Editor tool so they can be cleared.
    // Not used at runtime.
    [HideInInspector] public List<GameObject> editorSpawned = new List<GameObject>();

    // -------------------------------------------------------------------------
    // Unity lifecycle
    // -------------------------------------------------------------------------

    private void Start()
    {
        SpawnAll();
    }

    // -------------------------------------------------------------------------
    // Core logic
    // -------------------------------------------------------------------------

    /// <summary>
    /// Attempts to place <see cref="spawnCount"/> objects inside the area,
    /// skipping any candidate position that would violate <see cref="minDistance"/>.
    /// </summary>
    private void SpawnAll()
    {
        // Guard: nothing to spawn if the prefab array is empty or null.
        if (prefabs == null || prefabs.Length == 0)
        {
            Debug.LogWarning($"[RandomAreaSpawner] No prefabs assigned on '{name}'.");
            return;
        }

        _placedPositions.Clear();

        int spawned = 0;

        for (int i = 0; i < spawnCount; i++)
        {
            bool placed = TryPlaceObject();

            if (placed)
            {
                spawned++;
            }
            else
            {
                Debug.LogWarning(
                    $"[RandomAreaSpawner] Could not find a valid position for object {i + 1} " +
                    $"after {maxAttemptsPerObject} attempts. " +
                    "Consider enlarging the area or reducing minDistance / spawnCount.");
            }
        }

        Debug.Log($"[RandomAreaSpawner] Spawned {spawned}/{spawnCount} objects.");
    }

    /// <summary>
    /// Tries up to <see cref="maxAttemptsPerObject"/> random positions for a
    /// single object. Returns <c>true</c> when a valid position is found and
    /// the object is instantiated.
    /// </summary>
    private bool TryPlaceObject()
    {
        float halfX = areaSize.x * 0.5f;
        float halfZ = areaSize.y * 0.5f;

        for (int attempt = 0; attempt < maxAttemptsPerObject; attempt++)
        {
            // Pick a random XZ position within the rectangular area.
            float randomX = Random.Range(areaCenter.x - halfX, areaCenter.x + halfX);
            float randomZ = Random.Range(areaCenter.z - halfZ, areaCenter.z + halfZ);
            Vector2 candidate = new Vector2(randomX, randomZ);

            // Reject the candidate if it is too close to any already-placed object.
            if (IsTooClose(candidate))
                continue;

            // Position is valid — instantiate a random prefab from the pool.
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
            Vector3 spawnPosition = new Vector3(randomX, spawnY, randomZ);
            Transform parent = spawnParent != null ? spawnParent : transform;
            Instantiate(prefab, spawnPosition, Quaternion.identity, parent);

            _placedPositions.Add(candidate);
            return true;
        }

        return false;
    }

    /// <summary>
    /// Returns <c>true</c> if <paramref name="candidate"/> is within
    /// <see cref="minDistance"/> of any previously placed object.
    /// </summary>
    private bool IsTooClose(Vector2 candidate)
    {
        float sqrMin = minDistance * minDistance;

        foreach (Vector2 placed in _placedPositions)
        {
            if ((candidate - placed).sqrMagnitude < sqrMin)
                return true;
        }

        return false;
    }

    // -------------------------------------------------------------------------
    // Editor visualization
    // -------------------------------------------------------------------------

    /// <summary>
    /// Draws a wire cube representing the spawn area in the Scene view.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0f, 1f, 0.4f, 0.4f);

        // Filled semi-transparent rect (flat on XZ).
        Vector3 size   = new Vector3(areaSize.x, 0.02f, areaSize.y);
        Vector3 center = new Vector3(areaCenter.x, spawnY, areaCenter.z);
        Gizmos.DrawCube(center, size);

        // Solid-color outline.
        Gizmos.color = new Color(0f, 1f, 0.4f, 1f);
        Gizmos.DrawWireCube(center, size);
    }
}
