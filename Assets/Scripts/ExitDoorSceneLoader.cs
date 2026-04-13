using UnityEngine;
using UnityEngine.SceneManagement;
using LostEnergy;

/// <summary>
/// ExitDoorController ile aynı kapı/ışık mantığını taşır; fark olarak
/// Win paneli göstermek yerine doğrudan sonraki sahneye geçer.
/// ExitDoor GameObject'inden ExitDoorController'ı kaldırıp bunu ekle.
/// </summary>
public class ExitDoorSceneLoader : MonoBehaviour, IInteractable
{
    [Header("Door")]
    public GameObject doorObject;

    [Header("Indicator Lights")]
    public GameObject lockedLight;
    public GameObject unlockedLight;

    [Header("Scene Transition")]
    [Tooltip("Geçilecek sahnenin adı (Build Settings'teki isimle aynı olmalı).")]
    public string nextSceneName = "SampleScene2";

    public bool IsUnlocked { get; private set; }

    void Start()
    {
        ApplyLockedState();

        if (GameManager.Instance != null)
            GameManager.Instance.OnTargetReached += OnTargetReached;
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnTargetReached -= OnTargetReached;
    }

    // ── IInteractable ─────────────────────────────────────────────────────────

    public string GetPrompt()
    {
        if (IsUnlocked) return "Kapıdan Geç";

        int cur = GameManager.Instance != null ? GameManager.Instance.CurrentCrystals : 0;
        int max = GameManager.Instance != null ? GameManager.Instance.targetCrystals  : 0;
        return $"Önce kristalleri topla! ({cur}/{max})";
    }

    public void Interact(PlayerInteraction interactor)
    {
        if (!IsUnlocked) return;

        LoadNextScene();
    }

    // ── Kilit ─────────────────────────────────────────────────────────────────

    void OnTargetReached() => SetUnlocked();

    public void SetUnlocked()
    {
        if (IsUnlocked) return;
        IsUnlocked = true;

        if (lockedLight   != null) lockedLight.SetActive(false);
        if (unlockedLight != null) unlockedLight.SetActive(true);
    }

    // ── Sahne Geçişi ──────────────────────────────────────────────────────────

    void LoadNextScene()
    {
        Time.timeScale   = 1f;   // Donmuş zamanı sıfırla (güvenlik için)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;

        SceneLoader.LoadScene(nextSceneName);
    }

    void ApplyLockedState()
    {
        if (doorObject    != null) doorObject.SetActive(true);
        if (lockedLight   != null) lockedLight.SetActive(true);
        if (unlockedLight != null) unlockedLight.SetActive(false);
    }
}
