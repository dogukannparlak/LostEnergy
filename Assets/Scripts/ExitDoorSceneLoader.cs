using UnityEngine;
using UnityEngine.SceneManagement;
using LostEnergy;

/// <summary>
/// Uses the same door and light flow as ExitDoorController.
/// Loads the next scene instead of showing a win panel.
/// </summary>
public class ExitDoorSceneLoader : MonoBehaviour, IInteractable
{
    [Header("Door")]
    public GameObject doorObject;

    [Header("Indicator Lights")]
    public GameObject lockedLight;
    public GameObject unlockedLight;

    [Header("Scene Transition")]
    [Tooltip("Name of the scene to load. Must match Build Settings.")]
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

    // IInteractable

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

    // Unlock state

    void OnTargetReached() => SetUnlocked();

    public void SetUnlocked()
    {
        if (IsUnlocked) return;
        IsUnlocked = true;

        if (lockedLight   != null) lockedLight.SetActive(false);
        if (unlockedLight != null) unlockedLight.SetActive(true);
    }

    // Scene transition

    void LoadNextScene()
    {
        Time.timeScale   = 1f;   // Reset time scale before loading.
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
