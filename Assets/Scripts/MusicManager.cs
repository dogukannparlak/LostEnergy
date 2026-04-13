using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

/// <summary>
/// Sahneler arası kalıcı arka plan müziği yöneticisi.
/// Hierarchy'de boş bir objeye ekle, DontDestroyOnLoad ile hayatta kalır.
/// Her sahneye farklı müzik atayabilirsin; boş bırakılırsa mevcut müzik devam eder.
/// </summary>
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Varsayılan Müzik")]
    [Tooltip("Hiçbir sahne müziği atanmamışsa çalar.")]
    public AudioClip defaultMusic;

    [Header("Sahne Bazlı Müzikler")]
    public SceneMusicEntry[] sceneMusics;

    [Header("Ayarlar")]
    [Range(0f, 1f)] public float volume = 0.5f;

    [Tooltip("AudioMixer'daki Music grubunu buraya sürükle.")]
    public AudioMixerGroup musicMixerGroup;

    private AudioSource _source;
    private AudioClip _sceneClip;
    private AudioClip _zoneOverrideClip;
    private int _suppressionCount;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _source              = gameObject.AddComponent<AudioSource>();
        _source.loop         = true;
        _source.playOnAwake  = false;
        _source.spatialBlend = 0f; // 2D
        _source.volume       = volume;
        if (musicMixerGroup != null) _source.outputAudioMixerGroup = musicMixerGroup;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        PlayForScene(SceneManager.GetActiveScene().name);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayForScene(scene.name);
    }

    void PlayForScene(string sceneName)
    {
        _sceneClip = ResolveSceneClip(sceneName);
        _zoneOverrideClip = null;
        ApplyCurrentClip();
    }

    public void SetVolume(float v)
    {
        volume = Mathf.Clamp01(v);
        _source.volume = volume;
    }

    public void SetZoneOverride(AudioClip clip)
    {
        _zoneOverrideClip = clip;
        ApplyCurrentClip();
    }

    public void ClearZoneOverride(AudioClip clip = null)
    {
        if (clip != null && _zoneOverrideClip != clip) return;

        _zoneOverrideClip = null;
        ApplyCurrentClip();
    }

    public void SuppressMusic()
    {
        _suppressionCount++;
        ApplyCurrentClip();
    }

    public void ReleaseMusicSuppression()
    {
        if (_suppressionCount <= 0) return;

        _suppressionCount--;
        ApplyCurrentClip();
    }

    AudioClip ResolveSceneClip(string sceneName)
    {
        AudioClip clip = defaultMusic;

        foreach (var entry in sceneMusics)
        {
            if (entry.sceneName == sceneName && entry.music != null)
            {
                clip = entry.music;
                break;
            }
        }

        return clip;
    }

    void ApplyCurrentClip()
    {
        if (_suppressionCount > 0)
        {
            _source.Stop();
            return;
        }

        AudioClip clip = _zoneOverrideClip != null ? _zoneOverrideClip : _sceneClip;

        if (clip == null)
        {
            _source.Stop();
            _source.clip = null;
            return;
        }

        if (_source.clip == clip && _source.isPlaying)
        {
            _source.volume = volume;
            return;
        }

        _source.clip = clip;
        _source.volume = volume;
        _source.Play();
    }
}

[System.Serializable]
public class SceneMusicEntry
{
    [Tooltip("Build Settings'teki sahne adıyla aynı olmalı.")]
    public string    sceneName;
    public AudioClip music;
}
