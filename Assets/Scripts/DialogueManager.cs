using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text speakerText;
    public TMP_Text lineText;
    public TMP_Text continueHintText;

    [Header("Settings")]
    public float oxygenResumeDelay = 2f;

    [Header("Ses")]
    [Tooltip("Diyalog başladığında çalar.")]
    public AudioClip dialogueStartSfx;
    [Tooltip("Her satır geçişinde çalar.")]
    public AudioClip dialogueNextSfx;
    [Tooltip("AudioMixer'daki SFX grubunu buraya sürükle.")]
    public UnityEngine.Audio.AudioMixerGroup sfxMixerGroup;

    private AudioSource _audio;

    private DialogueData _current;
    private int _index;
    private OxygenSystem _oxygenSystem;

    public bool IsActive { get; private set; }

    void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
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
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
            NextLine();
    }

    void ShowLine()
    {
        var line = _current.lines[_index];
        speakerText.text = line.speakerName;
        lineText.text = line.text;
        bool isLast = (_index == _current.lines.Length - 1);
        if (continueHintText != null)
            continueHintText.text = isLast ? "[E] X " : "[E] ›";
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
        StartCoroutine(ResumeOxygenAfterDelay());
    }

    IEnumerator ResumeOxygenAfterDelay()
    {
        yield return new WaitForSeconds(oxygenResumeDelay);
        if (_oxygenSystem != null) _oxygenSystem.SetPaused(false);
    }
}
