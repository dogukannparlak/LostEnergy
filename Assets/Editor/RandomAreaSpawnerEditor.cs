using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// Custom Inspector for RandomAreaSpawner.
/// Adds "Spawn in Scene" and "Clear Spawned Objects" buttons so you can
/// populate the scene in Edit Mode without pressing Play.
/// All operations are Undo/Redo compatible (Ctrl+Z works).
/// </summary>
[CustomEditor(typeof(RandomAreaSpawner))]
public class RandomAreaSpawnerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw all the original Inspector fields as usual.
        DrawDefaultInspector();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Edit Mode Tools", EditorStyles.boldLabel);

        // Spawn button
        GUI.backgroundColor = new Color(0.4f, 0.9f, 0.5f);
        if (GUILayout.Button("Spawn in Scene", GUILayout.Height(32)))
            SpawnInEditMode();

        // Clear button
        GUI.backgroundColor = new Color(1f, 0.5f, 0.4f);
        if (GUILayout.Button("Clear Spawned Objects", GUILayout.Height(26)))
            ClearSpawned();

        GUI.backgroundColor = Color.white;
    }

    // -------------------------------------------------------------------------
    // Spawn logic (Edit Mode)
    // -------------------------------------------------------------------------

    private void SpawnInEditMode()
    {
        RandomAreaSpawner spawner = (RandomAreaSpawner)target;

        // Read serialized fields via SerializedProperty (works even with private fields).
        serializedObject.Update();

        SerializedProperty prefabsProp     = serializedObject.FindProperty("prefabs");
        SerializedProperty spawnCountProp  = serializedObject.FindProperty("spawnCount");
        SerializedProperty spawnYProp      = serializedObject.FindProperty("spawnY");
        SerializedProperty minDistProp     = serializedObject.FindProperty("minDistance");
        SerializedProperty maxAttProp      = serializedObject.FindProperty("maxAttemptsPerObject");
        SerializedProperty centerProp      = serializedObject.FindProperty("areaCenter");
        SerializedProperty sizeProp        = serializedObject.FindProperty("areaSize");
        SerializedProperty parentProp      = serializedObject.FindProperty("spawnParent");

        // Gather non-null prefabs from the array.
        var prefabs = new List<GameObject>();
        for (int i = 0; i < prefabsProp.arraySize; i++)
        {
            var go = prefabsProp.GetArrayElementAtIndex(i).objectReferenceValue as GameObject;
            if (go != null) prefabs.Add(go);
        }

        if (prefabs.Count == 0)
        {
            Debug.LogWarning("[RandomAreaSpawner] No prefabs assigned — nothing to spawn.");
            return;
        }

        int      spawnCount  = spawnCountProp.intValue;
        float    spawnY      = spawnYProp.floatValue;
        float    minDist     = minDistProp.floatValue;
        int      maxAtt      = maxAttProp.intValue;
        Vector3  areaCenter  = centerProp.vector3Value;
        Vector2  areaSize    = sizeProp.vector2Value;
        Transform parent     = parentProp.objectReferenceValue as Transform;

        float halfX  = areaSize.x * 0.5f;
        float halfZ  = areaSize.y * 0.5f;
        float sqrMin = minDist * minDist;

        var placed  = new List<Vector2>();
        int spawned = 0;

        // Group all operations into one Undo step so a single Ctrl+Z undoes everything.
        Undo.SetCurrentGroupName("Spawn Objects in Scene");
        int undoGroup = Undo.GetCurrentGroup();

        for (int i = 0; i < spawnCount; i++)
        {
            bool success = false;

            for (int attempt = 0; attempt < maxAtt; attempt++)
            {
                float rx = Random.Range(areaCenter.x - halfX, areaCenter.x + halfX);
                float rz = Random.Range(areaCenter.z - halfZ, areaCenter.z + halfZ);
                Vector2 candidate = new Vector2(rx, rz);

                // Distance check against already-placed objects.
                bool tooClose = false;
                foreach (Vector2 p in placed)
                {
                    if ((candidate - p).sqrMagnitude < sqrMin)
                    {
                        tooClose = true;
                        break;
                    }
                }
                if (tooClose) continue;

                // PrefabUtility keeps the prefab connection intact in the scene.
                GameObject prefab   = prefabs[Random.Range(0, prefabs.Count)];
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab, parent);
                instance.transform.SetPositionAndRotation(
                    new Vector3(rx, spawnY, rz), Quaternion.identity);

                // Register with Undo so Ctrl+Z destroys the object.
                Undo.RegisterCreatedObjectUndo(instance, "Spawn Object");

                spawner.editorSpawned.Add(instance);
                placed.Add(candidate);
                spawned++;
                success = true;
                break;
            }

            if (!success)
            {
                Debug.LogWarning(
                    $"[RandomAreaSpawner] Could not place object {i + 1} after {maxAtt} attempts. " +
                    "Try increasing Area Size or reducing Min Distance / Spawn Count.");
            }
        }

        // Merge individual Undo records into one group.
        Undo.CollapseUndoOperations(undoGroup);

        EditorUtility.SetDirty(spawner);
        EditorSceneManager.MarkSceneDirty(spawner.gameObject.scene);

        Debug.Log($"[RandomAreaSpawner] Spawned {spawned}/{spawnCount} objects in Scene view.");
    }

    // -------------------------------------------------------------------------
    // Clear logic (Edit Mode)
    // -------------------------------------------------------------------------

    private void ClearSpawned()
    {
        RandomAreaSpawner spawner = (RandomAreaSpawner)target;

        if (spawner.editorSpawned == null || spawner.editorSpawned.Count == 0)
        {
            Debug.Log("[RandomAreaSpawner] No tracked objects to clear.");
            return;
        }

        Undo.SetCurrentGroupName("Clear Spawned Objects");
        int undoGroup = Undo.GetCurrentGroup();

        foreach (GameObject obj in spawner.editorSpawned)
        {
            if (obj != null)
                Undo.DestroyObjectImmediate(obj);
        }

        spawner.editorSpawned.Clear();
        Undo.CollapseUndoOperations(undoGroup);

        EditorUtility.SetDirty(spawner);
        EditorSceneManager.MarkSceneDirty(spawner.gameObject.scene);

        Debug.Log("[RandomAreaSpawner] Cleared all editor-spawned objects.");
    }
}
