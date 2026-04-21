using UnityEngine;
using UnityEngine.InputSystem;
using LostEnergy;

public class ExitDoorController : MonoBehaviour, IInteractable
{
    [Header("Door")]
    public GameObject doorObject;

    [Header("Indicator Lights")]
    public GameObject lockedLight;
    public GameObject unlockedLight;

    [Header("Win")]
    public GameObject        winPanel;

    public bool IsUnlocked { get; private set; }

    private bool _winShown;

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

    void Update()
    {
        if (_winShown && Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            GameManager.RestartGame();
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

        ShowWin();
    }

    // Unlock state
    void OnTargetReached() => SetUnlocked();

    public void SetUnlocked()
    {
        if (IsUnlocked) return;
        IsUnlocked = true;

        if (doorObject    != null) doorObject.SetActive(false);
        if (lockedLight   != null) lockedLight.SetActive(false);
        if (unlockedLight != null) unlockedLight.SetActive(true);
    }

    void ShowWin()
    {
        if (_winShown) return;
        _winShown = true;

        if (winPanel != null) winPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;
        Time.timeScale   = 0f;
    }

    void ApplyLockedState()
    {
        if (doorObject    != null) doorObject.SetActive(true);
        if (lockedLight   != null) lockedLight.SetActive(true);
        if (unlockedLight != null) unlockedLight.SetActive(false);
    }
}
