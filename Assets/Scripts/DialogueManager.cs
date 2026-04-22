using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    // Simple global access for dialogue UI.
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text speakerText;
    public TMP_Text lineText;
    public TMP_Text continueHintText;

    [Header("Settings")]
    public float oxygenResumeDelay = 2f;

    [Header("Input")]
    [Tooltip("Seconds the key must be held to advance dialogue.")]
    public float holdDuration = 0.5f;

    [Header("Audio")]
    [Tooltip("Plays when the dialogue starts.")]
    public AudioClip dialogueStartSfx;
    [Tooltip("Plays when advancing to the next line.")]
    public AudioClip dialogueNextSfx;
    [Tooltip("Optional SFX mixer group for dialogue sounds.")]
    public UnityEngine.Audio.AudioMixerGroup sfxMixerGroup;

    private AudioSource _audio;

    private DialogueData _current;
    private int _index;
    private OxygenSystem _oxygenSystem;
    private float _holdTimer = 0f;

    public bool IsActive { get; private set; }

    void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);

        // Reuse or create a 2D audio source for dialogue sounds.
        _audio = GetComponent<AudioSource>();
        if (_audio == null) _audio = gameObject.AddComponent<AudioSource>();
        _audio.spatialBlend = 0f;
        _audio.playOnAwake  = false;
        if (sfxMixerGroup != null) _audio.outputAudioMixerGroup = sfxMixerGroup;
    }

    void Start()
    {
        _oxygenSystem = FindFirstObjectByType<OxygenSystem>();
    }

    public void StartDialogue(DialogueData data)
    {
        if (IsActive) return;
        StopAllCoroutines();
        _current = data;
        _index = 0;
        IsActive = true;
        dialoguePanel.SetActive(true);
        if (_oxygenSystem != null) _oxygenSystem.SetPaused(true);
        if (dialogueStartSfx != null) _audio.PlayOneShot(dialogueStartSfx, LostEnergy.SettingsManager.GetEffectiveSfxVolume01());
        ShowLine();
    }

    void Update()
    {
        if (!IsActive) return;
        if (Keyboard.current == null) return;

        // Tab: skip entire dialogue instantly.
        if (Keyboard.current.tabKey.wasPressedThisFrame)
        {
            SkipDialogue();
            return;
        }

        // Space: hold for holdDuration seconds to advance.
        if (Keyboard.current.spaceKey.isPressed)
        {
            _holdTimer += Time.deltaTime;
            if (_holdTimer >= holdDuration)
            {
                _holdTimer = 0f;
                NextLine();
            }
        }
        else
        {
            _holdTimer = 0f;
        }
    }

    void SkipDialogue()
    {
        _index = _current.lines.Length;
        EndDialogue();
    }

    void ShowLine()
    {
        var line = _current.lines[_index];
        speakerText.text = line.speakerName;
        lineText.text = line.text;
        bool isLast = (_index == _current.lines.Length - 1);
        if (continueHintText != null)
            continueHintText.text = isLast ? "[Space Tut] Kapat  |  [Tab] Atla" : "[Space Tut] ›  |  [Tab] Atla";
    }

    void NextLine()
    {
        _index++;
        if (_index >= _current.lines.Length)
            EndDialogue();
        else
        {
            if (dialogueNextSfx != null) _audio.PlayOneShot(dialogueNextSfx, LostEnergy.SettingsManager.GetEffectiveSfxVolume01());
            ShowLine();
        }
    }

    void EndDialogue()
    {
        IsActive = false;
        dialoguePanel.SetActive(false);

        // Resume oxygen after a short delay.
        StartCoroutine(ResumeOxygenAfterDelay());
    }

    IEnumerator ResumeOxygenAfterDelay()
    {
        yield return new WaitForSeconds(oxygenResumeDelay);
        if (_oxygenSystem != null) _oxygenSystem.SetPaused(false);
    }
}
