using UnityEngine;
using UnityEditor;
using System.IO;

public static class DialogueDataExporter
{
    [MenuItem("Assets/Dialogue/Export to JSON")]
    static void ExportToJson()
    {
        DialogueData data = Selection.activeObject as DialogueData;
        if (data == null) { Debug.LogWarning("Please select a DialogueData asset."); return; }

        string path = EditorUtility.SaveFilePanel("Export Dialogue JSON", "", data.name + ".json", "json");
        if (string.IsNullOrEmpty(path)) return;

        string json = JsonUtility.ToJson(new DialogueDataWrapper(data), true);
        File.WriteAllText(path, json, System.Text.Encoding.UTF8);
        Debug.Log($"Exported: {path}");
    }

    [MenuItem("Assets/Dialogue/Import from JSON")]
    static void ImportFromJson()
    {
        DialogueData data = Selection.activeObject as DialogueData;
        if (data == null) { Debug.LogWarning("Please select the DialogueData asset you want to overwrite."); return; }

        string path = EditorUtility.OpenFilePanel("Import Dialogue JSON", "", "json");
        if (string.IsNullOrEmpty(path)) return;

        string json = File.ReadAllText(path, System.Text.Encoding.UTF8);
        DialogueDataWrapper wrapper = JsonUtility.FromJson<DialogueDataWrapper>(json);
        if (wrapper == null) { Debug.LogError("Failed to read JSON."); return; }

        // Keep the import operation undoable in the editor.
        Undo.RecordObject(data, "Import Dialogue JSON");
        data.lines = wrapper.lines;
        // Mark the asset dirty so Unity saves the imported data.
        EditorUtility.SetDirty(data);
        AssetDatabase.SaveAssets();
        Debug.Log($"Imported: {path}");
    }

    [MenuItem("Assets/Dialogue/Export to JSON", true)]
    static bool ExportValidate() => Selection.activeObject is DialogueData;

    [MenuItem("Assets/Dialogue/Import from JSON", true)]
    static bool ImportValidate() => Selection.activeObject is DialogueData;
}

[System.Serializable]
class DialogueDataWrapper
{
    public DialogueLine[] lines;

    // Wrap the array so JsonUtility can serialize it cleanly.
    public DialogueDataWrapper(DialogueData d) { lines = d.lines; }
}
