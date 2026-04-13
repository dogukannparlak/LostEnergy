#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public static class LevelBuilder
{
    const float FLOOR_Y      = -0.1f;
    const float WALL_H       = 4f;
    const float TRUNK_HALF_H = 1.2f;
    const float LEAVES_Y     = 3.2f;

    [MenuItem("Tools/Level Builder/Build Forest Level 1")]
    public static void Build()
    {
        var old = GameObject.Find("Level_01_Forest");
        if (old != null)
        {
            if (!EditorUtility.DisplayDialog("Level Builder",
                "Level_01_Forest zaten var. Yeniden olusturulsun mu?", "Evet", "Hayir"))
                return;
            Undo.DestroyObjectImmediate(old);
        }

        EnsureFolder("Assets/Materials");
        EnsureFolder("Assets/Materials/Level01");
        const string dir = "Assets/Materials/Level01";

        var matFloor  = GetMat(dir, "M_ForestFloor",  C(64,  105, 40));
        var matWall   = GetMat(dir, "M_ForestWall",   C(28,   66, 22));
        var matTrunk  = GetMat(dir, "M_TreeTrunk",    C(90,   57, 25));
        var matLeaves = GetMat(dir, "M_TreeLeaves",   C(32,   90, 20));
        var matHazard = GetMat(dir, "M_HazardFloor",  C(130,  28, 20));

        var root = new GameObject("Level_01_Forest");
        Undo.RegisterCreatedObjectUndo(root, "Build Forest Level 1");

        // S1 - Giris Koridoru (Z: 0-20, W: 6)
        var s1 = Seg(root, "S1_GirisKoridoru");
        Floor(s1,  V3(0, FLOOR_Y, 10),  V3(6,  0.2f, 20), matFloor);
        TreeRow(s1, -3.8f,  0, 20, 2.8f, matTrunk, matLeaves);
        TreeRow(s1,  3.8f,  0, 20, 2.8f, matTrunk, matLeaves);
        Wall(s1, "BackWall", V3(0, 2f, -1), V3(9, WALL_H, 1), matWall);

        // S2 - Acik Alan (Z: 20-44, W: 20)
        var s2 = Seg(root, "S2_AcikAlan");
        Floor(s2, V3(0, FLOOR_Y, 32), V3(20, 0.2f, 24), matFloor);
        TreeRow(s2, -11f, 20, 44, 3.5f, matTrunk, matLeaves);
        TreeRow(s2,  11f, 20, 44, 3.5f, matTrunk, matLeaves);
        Tree(s2, V3(-6, 0, 24), matTrunk, matLeaves);
        Tree(s2, V3( 7, 0, 28), matTrunk, matLeaves);
        Tree(s2, V3(-4, 0, 36), matTrunk, matLeaves);
        Tree(s2, V3( 6, 0, 40), matTrunk, matLeaves);
        Wall(s2, "Funnel_L", V3(-7f, 2f, 44.5f), V3(8, WALL_H, 1), matWall);
        Wall(s2, "Funnel_R", V3( 7f, 2f, 44.5f), V3(8, WALL_H, 1), matWall);

        // S3 - Orta Koridor (Z: 44-62, W: 5)
        var s3 = Seg(root, "S3_OrtaKoridor");
        Floor(s3, V3(0, FLOOR_Y, 53), V3(5, 0.2f, 18), matFloor);
        TreeRow(s3, -3.2f, 44, 62, 2.5f, matTrunk, matLeaves);
        TreeRow(s3,  3.2f, 44, 62, 2.5f, matTrunk, matLeaves);

        // S4 - Hazard Alani (Z: 62-86, W: 22)
        var s4 = Seg(root, "S4_HazardAlani");
        Floor(s4, V3(0, FLOOR_Y, 74), V3(22, 0.2f, 24), matHazard);
        TreeRow(s4, -12f, 62, 86, 3.5f, matTrunk, matLeaves);
        TreeRow(s4,  12f, 62, 86, 3.5f, matTrunk, matLeaves);
        Tree(s4, V3(-8, 0, 68), matTrunk, matLeaves);
        Tree(s4, V3( 9, 0, 80), matTrunk, matLeaves);
        Wall(s4, "Funnel2_L", V3(-7f, 2f, 86.5f), V3(10, WALL_H, 1), matWall);
        Wall(s4, "Funnel2_R", V3( 7f, 2f, 86.5f), V3(10, WALL_H, 1), matWall);

        // S5 - Son Koridor (Z: 86-104, W: 5)
        var s5 = Seg(root, "S5_SonKoridor");
        Floor(s5, V3(0, FLOOR_Y, 95), V3(5, 0.2f, 18), matFloor);
        TreeRow(s5, -3.2f, 86, 104, 2.5f, matTrunk, matLeaves);
        TreeRow(s5,  3.2f, 86, 104, 2.5f, matTrunk, matLeaves);

        // S6 - Cikis Alani (Z: 104-118, W: 10)
        var s6 = Seg(root, "S6_CikisAlani");
        Floor(s6, V3(0, FLOOR_Y, 111), V3(10, 0.2f, 14), matFloor);
        TreeRow(s6, -6f, 104, 118, 3f, matTrunk, matLeaves);
        TreeRow(s6,  6f, 104, 118, 3f, matTrunk, matLeaves);
        Wall(s6, "Wall_Son", V3(0, 2f, 119), V3(14, WALL_H, 1), matWall);

        AssetDatabase.SaveAssets();

        Debug.Log(
            "<color=lime>[LevelBuilder] Forest Level 1 hazir!</color>\n\n" +
            "--- Simdi bunlari yap ---\n" +
            "Player        -> Position (0, 1, 5)\n" +
            "RespawnPoint  -> Position (0, 1, 5)\n" +
            "ExitDoor      -> Position (0, 0, 112)\n" +
            "HazardZone    -> Position (0, 0, 74)  Radius = 7\n\n" +
            "Kristal Pozisyonlari:\n" +
            "  (0,1,8)   (-1,1,12)  (1,1,17)\n" +
            "  (-3,1,25) (4,1,32)   (2,1,40)\n" +
            "  (0,1,50)  (-1,1,57)\n" +
            "  (0,1,68)  (3,1,78)  <- hazard icinde!\n\n" +
            "Eski duz 'Floor' objesini gizle (Inspector checkbox)."
        );

        Selection.activeGameObject = root;
        SceneView.lastActiveSceneView?.FrameSelected();
    }

    // ── Helpers ──────────────────────────────────────────────────

    static GameObject Seg(GameObject parent, string name)
    {
        var g = new GameObject(name);
        g.transform.SetParent(parent.transform);
        return g;
    }

    static void Floor(GameObject parent, Vector3 pos, Vector3 scale, Material mat)
    {
        var g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = "Floor";
        g.transform.SetParent(parent.transform, false);
        g.transform.position   = pos;
        g.transform.localScale = scale;
        g.GetComponent<Renderer>().sharedMaterial = mat;
    }

    static void Wall(GameObject parent, string name, Vector3 pos, Vector3 scale, Material mat)
    {
        var g = GameObject.CreatePrimitive(PrimitiveType.Cube);
        g.name = name;
        g.transform.SetParent(parent.transform, false);
        g.transform.position   = pos;
        g.transform.localScale = scale;
        g.GetComponent<Renderer>().sharedMaterial = mat;
    }

    static void TreeRow(GameObject parent, float x, float zFrom, float zTo,
                        float spacing, Material trunk, Material leaves)
    {
        for (float z = zFrom + spacing * 0.5f; z < zTo; z += spacing)
            Tree(parent, V3(x, 0, z), trunk, leaves);
    }

    static void Tree(GameObject parent, Vector3 pos, Material trunkMat, Material leavesMat)
    {
        var grp = new GameObject("Tree");
        grp.transform.SetParent(parent.transform, false);

        var t = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        t.name = "Trunk";
        t.transform.SetParent(grp.transform, false);
        t.transform.position   = pos + V3(0, TRUNK_HALF_H, 0);
        t.transform.localScale = V3(0.4f, TRUNK_HALF_H, 0.4f);
        t.GetComponent<Renderer>().sharedMaterial = trunkMat;

        var l = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        l.name = "Leaves";
        l.transform.SetParent(grp.transform, false);
        l.transform.position   = pos + V3(0, LEAVES_Y, 0);
        l.transform.localScale = V3(2.4f, 3f, 2.4f);
        l.GetComponent<Renderer>().sharedMaterial = leavesMat;
        Object.DestroyImmediate(l.GetComponent<Collider>());
    }

    static Material GetMat(string folder, string name, Color color)
    {
        string path = $"{folder}/{name}.mat";
        var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        if (mat != null) { mat.color = color; return mat; }
        var sh = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
        mat = new Material(sh) { name = name, color = color };
        AssetDatabase.CreateAsset(mat, path);
        return mat;
    }

    static void EnsureFolder(string path)
    {
        if (AssetDatabase.IsValidFolder(path)) return;
        int i = path.LastIndexOf('/');
        AssetDatabase.CreateFolder(path[..i], path[(i + 1)..]);
    }

    static Color   C(byte r, byte g, byte b)      => new Color32(r, g, b, 255);
    static Vector3 V3(float x, float y, float z)   => new Vector3(x, y, z);
}
#endif
