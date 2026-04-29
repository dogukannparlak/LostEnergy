using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// AnimationPlayer bileşeni için özel Inspector.
/// "Temel Controller Kur" butonu ile Animator Controller otomatik yapılandırılır.
/// Ardından AnimationClip'leri mavi alana sürükle-bırak yap.
/// </summary>
[CustomEditor(typeof(AnimationPlayer))]
public class AnimationPlayerEditor : Editor
{
    private static readonly Color DropZoneNormal    = new Color(0.25f, 0.45f, 0.75f, 0.25f);
    private static readonly Color DropZoneHover     = new Color(0.25f, 0.75f, 0.45f, 0.40f);
    private static readonly Color RowEven           = new Color(1f, 1f, 1f, 0.03f);
    private static readonly Color RowOdd            = new Color(0f, 0f, 0f, 0.06f);
    private static readonly Color RemoveButtonColor = new Color(1f, 0.35f, 0.35f);
    private static readonly Color PlayButtonColor   = new Color(0.35f, 0.85f, 0.45f);

    private SerializedProperty _animationsProp;
    private SerializedProperty _autoPlayProp;
    private SerializedProperty _crossFadeProp;

    private void OnEnable()
    {
        _animationsProp = serializedObject.FindProperty("animations");
        _autoPlayProp   = serializedObject.FindProperty("autoPlayIndex");
        _crossFadeProp  = serializedObject.FindProperty("crossFadeDuration");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        DrawPlayerHeader();
        EditorGUILayout.Space(6);
        DrawSettings();
        EditorGUILayout.Space(6);
        DrawAnimationList();
        EditorGUILayout.Space(4);
        DrawDropZone();

        serializedObject.ApplyModifiedProperties();
    }

    // ── GUI Sections ─────────────────────────────────────────────────────

    private void DrawPlayerHeader()
    {
        var style = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 13,
            alignment = TextAnchor.MiddleCenter
        };
        EditorGUILayout.LabelField("🎬 Animation Player", style, GUILayout.Height(22));
        DrawSeparator();
    }

    private void DrawSettings()
    {
        EditorGUILayout.LabelField("Ayarlar", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(_autoPlayProp,
            new GUIContent("Başlangıç Animasyonu", "Oyun başlayınca otomatik oynatılacak indeks (-1 = kapalı)"));
        EditorGUILayout.PropertyField(_crossFadeProp,
            new GUIContent("CrossFade Süresi (sn)", "Animasyonlar arası geçiş süresi"));
        EditorGUI.indentLevel--;
    }

    private void DrawAnimationList()
    {
        EditorGUILayout.LabelField($"Animasyonlar  ({_animationsProp.arraySize})", EditorStyles.boldLabel);

        if (_animationsProp.arraySize == 0)
        {
            EditorGUILayout.HelpBox("Henüz animasyon yok. Aşağıdaki alana sürükle-bırak yap.", MessageType.Info);
            return;
        }

        int toRemove = -1;

        for (int i = 0; i < _animationsProp.arraySize; i++)
        {
            var entry     = _animationsProp.GetArrayElementAtIndex(i);
            var labelProp = entry.FindPropertyRelative("label");
            var clipProp  = entry.FindPropertyRelative("clip");
            var speedProp = entry.FindPropertyRelative("speed");
            var loopProp  = entry.FindPropertyRelative("loop");

            var rowRect = EditorGUILayout.BeginVertical();
            EditorGUI.DrawRect(rowRect, i % 2 == 0 ? RowEven : RowOdd);
            EditorGUILayout.BeginHorizontal();

            // İndeks
            GUI.color = new Color(0.7f, 0.9f, 1f);
            EditorGUILayout.LabelField($"[{i}]", GUILayout.Width(28));
            GUI.color = Color.white;

            // Clip
            EditorGUILayout.PropertyField(clipProp, GUIContent.none, GUILayout.Width(130));

            // Etiket
            labelProp.stringValue = EditorGUILayout.TextField(labelProp.stringValue, GUILayout.MinWidth(60));

            // Hız
            EditorGUILayout.LabelField("Hız", GUILayout.Width(24));
            speedProp.floatValue = EditorGUILayout.Slider(speedProp.floatValue, 0f, 3f, GUILayout.Width(80));

            // Döngü
            EditorGUILayout.LabelField("Döngü", GUILayout.Width(38));
            loopProp.boolValue = EditorGUILayout.Toggle(loopProp.boolValue, GUILayout.Width(18));

            // ▶ Oynat (sadece Play modunda)
            GUI.enabled = Application.isPlaying;
            GUI.backgroundColor = PlayButtonColor;
            if (GUILayout.Button("▶", GUILayout.Width(24)))
                ((AnimationPlayer)target).PlayByIndex(i);
            GUI.backgroundColor = Color.white;
            GUI.enabled = true;

            // ✕ Sil
            GUI.backgroundColor = RemoveButtonColor;
            if (GUILayout.Button("✕", GUILayout.Width(24)))
                toRemove = i;
            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }

        if (toRemove >= 0)
            _animationsProp.DeleteArrayElementAtIndex(toRemove);
    }

    private void DrawDropZone()
    {
        DrawSeparator();

        var dropRect = GUILayoutUtility.GetRect(0, 50, GUILayout.ExpandWidth(true));
        dropRect = EditorGUI.IndentedRect(dropRect);

        bool isHovering = dropRect.Contains(Event.current.mousePosition);
        EditorGUI.DrawRect(dropRect, isHovering ? DropZoneHover : DropZoneNormal);

        Handles.color = isHovering ? new Color(0.3f, 1f, 0.5f) : new Color(0.4f, 0.6f, 1f);
        Handles.DrawSolidRectangleWithOutline(dropRect, Color.clear, Handles.color);

        var labelStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = 11,
            normal = { textColor = Color.white }
        };
        GUI.Label(dropRect, "＋  AnimationClip'leri buraya sürükle-bırak yap", labelStyle);

        HandleDragAndDrop(dropRect);

        EditorGUILayout.Space(2);
        if (GUILayout.Button("+ Boş Satır Ekle"))
            AddEntry(null);
    }

    // ── Drag & Drop ──────────────────────────────────────────────────────

    private void HandleDragAndDrop(Rect dropRect)
    {
        var evt = Event.current;
        switch (evt.type)
        {
            case EventType.DragUpdated:
            case EventType.DragPerform:
                if (!dropRect.Contains(evt.mousePosition)) return;
                DragAndDrop.visualMode = HasAnimationClips()
                    ? DragAndDropVisualMode.Copy
                    : DragAndDropVisualMode.Rejected;
                if (evt.type == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    AcceptDroppedClips();
                }
                evt.Use();
                break;

            case EventType.Repaint:
                if (dropRect.Contains(evt.mousePosition)) Repaint();
                break;
        }
    }

    private bool HasAnimationClips()
    {
        foreach (var obj in DragAndDrop.objectReferences)
            if (obj is AnimationClip) return true;

        if (DragAndDrop.paths != null)
            foreach (var path in DragAndDrop.paths)
                foreach (var a in AssetDatabase.LoadAllAssetsAtPath(path))
                    if (a is AnimationClip) return true;

        return false;
    }

    private void AcceptDroppedClips()
    {
        serializedObject.Update();
        var clips = new List<AnimationClip>();

        foreach (var obj in DragAndDrop.objectReferences)
            if (obj is AnimationClip ac) clips.Add(ac);

        if (clips.Count == 0 && DragAndDrop.paths != null)
            foreach (var path in DragAndDrop.paths)
                foreach (var a in AssetDatabase.LoadAllAssetsAtPath(path))
                    if (a is AnimationClip ac2 && !ac2.name.StartsWith("__preview__"))
                        clips.Add(ac2);

        foreach (var clip in clips) AddEntry(clip);
        serializedObject.ApplyModifiedProperties();
    }

    private void AddEntry(AnimationClip clip)
    {
        int idx = _animationsProp.arraySize;
        _animationsProp.InsertArrayElementAtIndex(idx);
        var elem      = _animationsProp.GetArrayElementAtIndex(idx);
        elem.FindPropertyRelative("clip").objectReferenceValue = clip;
        elem.FindPropertyRelative("label").stringValue         = clip != null ? clip.name : "Yeni Animasyon";
        elem.FindPropertyRelative("speed").floatValue          = 1f;
        elem.FindPropertyRelative("loop").boolValue            = true;
    }

    // ── Utilities ────────────────────────────────────────────────────────

    private static void DrawSeparator()
    {
        EditorGUILayout.Space(2);
        EditorGUI.DrawRect(EditorGUILayout.GetControlRect(false, 1), new Color(0.5f, 0.5f, 0.5f, 0.4f));
        EditorGUILayout.Space(2);
    }
}
