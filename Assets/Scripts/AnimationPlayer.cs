using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

/// <summary>
/// Karaktere ekle, Project penceresinden AnimationClip sürükle-bırak yap.
/// Animator Controller'a GEREK YOK — Animator bileşeni yeterli.
/// Kliplerin Import Settings → Rig → Animation Type = Humanoid olduğundan emin ol.
/// </summary>
[RequireComponent(typeof(Animator))]
public class AnimationPlayer : MonoBehaviour
{
    public const string SLOT_NAME = "AnimationSlot"; // geriye dönük uyumluluk

    [System.Serializable]
    public class AnimationEntry
    {
        public string label = "";
        public AnimationClip clip;
        [Range(0f, 3f)] public float speed = 1f;
        public bool loop = true;
    }

    [Header("Animasyon Listesi")]
    public List<AnimationEntry> animations = new List<AnimationEntry>();

    [Header("Başlangıç")]
    [Tooltip("Oyun başlayınca oynatılacak indeks (-1 = hiçbiri)")]
    public int autoPlayIndex = 0;

    [Header("Geçiş")]
    [Range(0f, 1f)] public float crossFadeDuration = 0.15f;

    // ── Playables ───────────────────────────────────────────────────────────
    private Animator               _animator;
    private PlayableGraph          _graph;
    private AnimationMixerPlayable _mixer;
    private int                    _currentIndex = -1;
    public  int                    CurrentIndex => _currentIndex;

    // Fade state
    private bool  _fading;
    private float _fadeElapsed;

    // ── Lifecycle ────────────────────────────────────────────────────────────
    private void Awake()
    {
        _animator = GetComponent<Animator>();

        // Controller varsa kaldır — Playables ile çakışır
        if (_animator.runtimeAnimatorController != null)
        {
            Debug.Log("[AnimationPlayer] Animator Controller kaldırıldı (Playables kullanılıyor).");
            _animator.runtimeAnimatorController = null;
        }

        _graph = PlayableGraph.Create(name + "_AnimPlayer");
        _graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        _mixer = AnimationMixerPlayable.Create(_graph, 2);

        var output = AnimationPlayableOutput.Create(_graph, "output", _animator);
        output.SetSourcePlayable(_mixer);

        _mixer.SetInputWeight(0, 1f);
        _mixer.SetInputWeight(1, 0f);

        _graph.Play();
    }

    private void Start()
    {
        if (autoPlayIndex >= 0 && autoPlayIndex < animations.Count)
            PlayByIndex(autoPlayIndex);
    }

    private void Update()
    {
        // Loop desteği: klip bittiyse başa sar
        if (!_fading && _currentIndex >= 0 && _currentIndex < animations.Count)
        {
            var entry = animations[_currentIndex];
            if (entry.loop && entry.clip != null && SlotHasClip(0))
            {
                var p = _mixer.GetInput(0);
                if (p.IsValid())
                {
                    double t = p.GetTime();
                    double len = entry.clip.length;
                    if (len > 0.001 && t >= len - 0.02)
                        p.SetTime(0.0);
                }
            }
        }

        if (!_fading) return;

        _fadeElapsed += Time.deltaTime;
        float ft = crossFadeDuration > 0f
            ? Mathf.Clamp01(_fadeElapsed / crossFadeDuration)
            : 1f;

        _mixer.SetInputWeight(0, 1f - ft);
        _mixer.SetInputWeight(1, ft);

        if (ft >= 1f) CompleteFade();
    }

    private void OnDestroy()
    {
        if (_graph.IsValid()) _graph.Destroy();
    }

    // ── Public API ───────────────────────────────────────────────────────────

    public void PlayByIndex(int index)
    {
        if (index < 0 || index >= animations.Count)
        {
            Debug.LogWarning($"[AnimationPlayer] Geçersiz indeks: {index}");
            return;
        }
        TryPlay(index);
    }

    public void PlayByName(string label)
    {
        int idx = animations.FindIndex(a => a.label == label);
        if (idx < 0) { Debug.LogWarning($"[AnimationPlayer] '{label}' bulunamadı."); return; }
        TryPlay(idx);
    }

    public void Pause()  => _graph.Stop();
    public void Resume() => _graph.Play();
    public void Stop()
    {
        _graph.Stop();
        ClearSlot(0);
        ClearSlot(1);
        _fading = false;
        _currentIndex = -1;
    }

    // ── Internals ─────────────────────────────────────────────────────────────

    private void TryPlay(int index)
    {
        var entry = animations[index];

        if (entry.clip == null)
        {
            Debug.LogWarning($"[AnimationPlayer] [{index}] '{entry.label}' — clip atanmamış!");
            return;
        }

        // Aynı klip zaten oynuyor VE henüz bitmemişse geç
        if (index == _currentIndex && !_fading && SlotHasClip(0))
        {
            var p = _mixer.GetInput(0);
            double clipLen = entry.clip != null ? entry.clip.length : 0;
            if (p.IsValid() && (entry.loop || p.GetTime() < clipLen - 0.05))
                return;
        }

        _currentIndex = index;
        _fading       = false;
        _fadeElapsed  = 0f;

        if (crossFadeDuration > 0f && SlotHasClip(0))
        {
            // Slot 1'e yeni klip bağla → fade başlat
            ClearSlot(1);
            ConnectClip(1, entry);
            _mixer.SetInputWeight(0, 1f);
            _mixer.SetInputWeight(1, 0f);
            _fading = true;
        }
        else
        {
            // Direkt geç
            ClearSlot(0);
            ClearSlot(1);
            ConnectClip(0, entry);
            _mixer.SetInputWeight(0, 1f);
            _mixer.SetInputWeight(1, 0f);
        }
    }

    private void CompleteFade()
    {
        _fading = false;

        // Slot 0'daki eski klibi temizle
        ClearSlot(0);

        // Slot 1'deki yeni klibi slot 0'a taşı
        if (SlotHasClip(1))
        {
            var moved = _mixer.GetInput(1);
            _graph.Disconnect(_mixer, 1);
            _graph.Connect(moved, 0, _mixer, 0);
            _mixer.SetInputWeight(0, 1f);
            _mixer.SetInputWeight(1, 0f);
        }
    }

    private void ConnectClip(int slot, AnimationEntry entry)
    {
        var p = AnimationClipPlayable.Create(_graph, entry.clip);
        p.SetSpeed(entry.speed);
        _graph.Connect(p, 0, _mixer, slot);
        _mixer.SetInputWeight(slot, slot == 0 ? 1f : 0f);
    }

    private void ClearSlot(int slot)
    {
        if (!SlotHasClip(slot)) return;
        var p = _mixer.GetInput(slot);
        _graph.Disconnect(_mixer, slot);
        if (p.IsValid()) p.Destroy();
        _mixer.SetInputWeight(slot, 0f);
    }

    private bool SlotHasClip(int slot) => _mixer.GetInput(slot).IsValid();
}
