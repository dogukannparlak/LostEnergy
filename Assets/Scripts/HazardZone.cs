using UnityEngine;

[ExecuteAlways]
public class HazardZone : MonoBehaviour
{
    [Header("Hazard")]
    public float extraDrainPerSecond = 5f;

    [Header("Zone Size")]
    public float radius = 3f;
    public float centerY = 1f;

    [Header("Visuals")]
    public Color zoneColor = new Color(1f, 0.15f, 0.15f, 0.30f);

    [Header("Audio")]
    [Tooltip("Loops while the player is inside the zone.")]
    public AudioClip hazardMusic;
    [Range(0f, 1f)] public float hazardMusicVolume = 0.6f;
    [Tooltip("Optional SFX or Music mixer group.")]
    public UnityEngine.Audio.AudioMixerGroup hazardMixerGroup;

    private AudioSource _hazardAudio;
    private OxygenSystem _cachedOxy;
    private bool         _inside;
    private bool         _suppressedSceneMusic;

    void Reset()
    {
        SyncCollider();
        BuildDome();
    }

    void Awake()
    {
        SyncCollider();
        CleanupDomes();
        var d = GetDome() ?? BuildDome();
        SyncDomeTransform(d);

        CleanupExtraAudioSources();
        _hazardAudio = GetComponent<AudioSource>();
        if (_hazardAudio == null) _hazardAudio = gameObject.AddComponent<AudioSource>();
        _hazardAudio.spatialBlend = 0f;
        _hazardAudio.loop         = true;
        _hazardAudio.playOnAwake  = false;
        _hazardAudio.volume       = hazardMusicVolume * LostEnergy.SettingsManager.GetEffectiveSfxVolume01();
        if (hazardMixerGroup != null) _hazardAudio.outputAudioMixerGroup = hazardMixerGroup;
    }

    void OnEnable()
    {
        LostEnergy.SettingsManager.AudioSettingsChanged += ApplySettingsVolume;
        ApplySettingsVolume();
    }

    void OnDisable()
    {
        LostEnergy.SettingsManager.AudioSettingsChanged -= ApplySettingsVolume;

        if (_suppressedSceneMusic && MusicManager.Instance != null)
        {
            MusicManager.Instance.ReleaseMusicSuppression();
            _suppressedSceneMusic = false;
        }
    }

    void OnValidate()
    {
        SyncCollider();
        var d = GetDome();
        if (d != null) SyncDomeTransform(d);
    }

    void SyncCollider()
    {
        var box = GetComponent<BoxCollider>();
        if (box != null) DestroyImmediate(box);

        var sc = GetComponent<SphereCollider>();
        if (sc == null) sc = gameObject.AddComponent<SphereCollider>();

        sc.isTrigger = true;
        Vector3 ls = transform.lossyScale;
        sc.radius = radius / Mathf.Max(0.001f, ls.x);
        sc.center = new Vector3(0f, centerY / Mathf.Max(0.001f, ls.y), 0f);
    }

    void SyncDomeTransform(Transform dome)
    {
        Vector3 ls = transform.lossyScale;
        dome.localPosition = new Vector3(0f, centerY / Mathf.Max(0.001f, ls.y), 0f);
        dome.localScale    = new Vector3(
            radius * 2f / Mathf.Max(0.001f, ls.x),
            radius * 2f / Mathf.Max(0.001f, ls.y),
            radius * 2f / Mathf.Max(0.001f, ls.z));
        var mr = dome.GetComponent<MeshRenderer>();
        if (mr != null && mr.sharedMaterial != null)
            mr.sharedMaterial.color = zoneColor;
    }

    Transform GetDome() => transform.Find("HazardDomeVisual");

    Transform BuildDome()
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = "HazardDomeVisual";
        go.transform.SetParent(transform, worldPositionStays: false);
        DestroyImmediate(go.GetComponent<SphereCollider>());
        var mr = go.GetComponent<MeshRenderer>();
        mr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        mr.receiveShadows    = false;
        mr.sharedMaterial    = BuildTransparentMat(zoneColor);
        return go.transform;
    }

    void CleanupDomes()
    {
        bool kept = false;
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var child = transform.GetChild(i);
            if (child.name != "HazardDomeVisual") continue;
            if (!kept) { kept = true; continue; }
            DestroyImmediate(child.gameObject);
        }
    }

    void CleanupExtraAudioSources()
    {
        var audioSources = GetComponents<AudioSource>();
        for (int i = audioSources.Length - 1; i >= 1; i--)
        {
            if (Application.isPlaying) Destroy(audioSources[i]);
            else DestroyImmediate(audioSources[i]);
        }
    }

    static Material BuildTransparentMat(Color col)
    {
        Shader sh = Shader.Find("Universal Render Pipeline/Unlit")
                 ?? Shader.Find("Unlit/Color");
        var mat = new Material(sh);

        // Configure the material as transparent.
        mat.SetFloat("_Surface", 1f);          // 0 = Opaque, 1 = Transparent
        mat.SetFloat("_Blend",   0f);          // 0 = Alpha
        mat.SetFloat("_AlphaClip", 0f);
        mat.SetInt("_SrcBlend",  (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend",  (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_SrcBlendAlpha", (int)UnityEngine.Rendering.BlendMode.One);
        mat.SetInt("_DstBlendAlpha", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite",    0);
        mat.SetInt("_Cull",      0);           // Render both sides.

        mat.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");

        mat.SetOverrideTag("RenderType", "Transparent");
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

        mat.color = col;
        return mat;
    }

    void OnTriggerEnter(Collider other)
    {
        if (_inside) return;
        if (!IsPlayer(other, out OxygenSystem oxy)) return;

        _inside    = true;
        _cachedOxy = oxy;
        _cachedOxy.AddHazardDrain(extraDrainPerSecond);

        if (!_suppressedSceneMusic && MusicManager.Instance != null)
        {
            MusicManager.Instance.SuppressMusic();
            _suppressedSceneMusic = true;
        }

        if (hazardMusic != null && _hazardAudio != null)
        {
            _hazardAudio.clip   = hazardMusic;
            _hazardAudio.volume = hazardMusicVolume * LostEnergy.SettingsManager.GetEffectiveSfxVolume01();
            _hazardAudio.Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!_inside) return;
        if (!IsPlayer(other, out _)) return;

        _inside = false;
        _cachedOxy?.RemoveHazardDrain(extraDrainPerSecond);
        _cachedOxy = null;

        _hazardAudio?.Stop();

        if (_suppressedSceneMusic && MusicManager.Instance != null)
        {
            MusicManager.Instance.ReleaseMusicSuppression();
            _suppressedSceneMusic = false;
        }
    }

    public void ResetZone()
    {
        _inside = false;
        _cachedOxy = null;
        _hazardAudio?.Stop();

        if (_suppressedSceneMusic && MusicManager.Instance != null)
        {
            MusicManager.Instance.ReleaseMusicSuppression();
            _suppressedSceneMusic = false;
        }
    }

    static bool IsPlayer(Collider other, out OxygenSystem oxy)
    {
        oxy = null;

        bool isPlayer = other.CompareTag("Player")
                     || other.GetComponentInParent<PlayerController3P>() != null;

        if (!isPlayer) return false;

        oxy = other.GetComponentInParent<OxygenSystem>();

        if (oxy == null)
            Debug.LogWarning(
                $"[HazardZone] OxygenSystem was not found. Collider: '{other.gameObject.name}'",
                other.gameObject);

        return oxy != null;
    }

    void ApplySettingsVolume()
    {
        if (_hazardAudio == null) return;
        _hazardAudio.volume = hazardMusicVolume * LostEnergy.SettingsManager.GetEffectiveSfxVolume01();
    }
}
