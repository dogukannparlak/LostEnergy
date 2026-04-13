using UnityEngine;
using UnityEngine.UI;
using TMPro;
using LostEnergy;

public class UIManager : MonoBehaviour
{
    [Header("HUD — Kristal")]
    public TMP_Text crystalText;

    [Header("HUD — Oksijen")]
    public Slider oxygenSlider;
    public TMP_Text oxygenValueText;

    [Header("HUD — Interact Prompt")]
    public TMP_Text interactText;

    [Header("Panels")]
    public GameObject gameOverPanel;
    public GameObject winPanel;

    private GameManager _boundGameManager;

    void Start()
    {
        gameOverPanel?.SetActive(false);
        winPanel?.SetActive(false);

        SetInteractPrompt(string.Empty);

        if (oxygenSlider != null)
        {
            oxygenSlider.minValue = 0f;
            oxygenSlider.maxValue = 1f;
            oxygenSlider.value    = 1f;
        }

        SetOxygenValue(1f, 1f);

        TryBindGameManager();
    }

    void Update()
    {
        if (_boundGameManager == null)
            TryBindGameManager();
    }

    void OnDestroy()
    {
        if (_boundGameManager == null) return;

        _boundGameManager.OnCrystalCountChanged -= UpdateCrystalText;
        _boundGameManager.OnPlayerDied          -= ShowGameOverPanel;
        _boundGameManager = null;
    }

    void TryBindGameManager()
    {
        if (_boundGameManager != null) return;
        if (GameManager.Instance == null) return;

        _boundGameManager = GameManager.Instance;
        _boundGameManager.OnCrystalCountChanged += UpdateCrystalText;
        _boundGameManager.OnPlayerDied          += ShowGameOverPanel;

        UpdateCrystalText(_boundGameManager.CurrentCrystals,
                          _boundGameManager.targetCrystals);
    }

    void LateUpdate()
    {
        // If any modal end-state panel is open, keep cursor usable even if another script relocks it.
        bool gameOverOpen = gameOverPanel != null && gameOverPanel.activeInHierarchy;
        bool winOpen = winPanel != null && winPanel.activeInHierarchy;
        if (!gameOverOpen && !winOpen) return;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void UpdateCrystalText(int current, int target)
    {
        if (crystalText == null) return;
        crystalText.text = $"Kristal: {current}/{target}";
    }

    public void SetOxygenNormalized(float v)
    {
        if (oxygenSlider == null) return;
        oxygenSlider.value = Mathf.Clamp01(v);
    }

    public void SetOxygenValue(float current, float max)
    {
        if (oxygenValueText == null) return;

        int curInt = Mathf.CeilToInt(Mathf.Max(0f, current));
        int maxInt = Mathf.Max(1, Mathf.CeilToInt(max));
        oxygenValueText.text = $"O2: {curInt}/{maxInt}";
    }


    public void SetInteractPrompt(string prompt)
    {
        if (interactText == null) return;

        bool hasPrompt        = !string.IsNullOrEmpty(prompt);
        interactText.text     = hasPrompt ? prompt : string.Empty;
        interactText.enabled  = hasPrompt;
    }

    void ShowWinPanel()
    {
        if (winPanel != null) winPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;
        Time.timeScale   = 0f;
    }

    void ShowGameOverPanel(string reason)
    {
        if (gameOverPanel == null) return;

        if (!string.IsNullOrEmpty(reason))
        {
            TMP_Text msgText = gameOverPanel.GetComponentInChildren<TMP_Text>();
            if (msgText != null) msgText.text = reason;
        }
        else
        {
            TMP_Text msgText = gameOverPanel.GetComponentInChildren<TMP_Text>();
            if (msgText != null) msgText.text = "Oksijen Tükendi!";
        }

        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void HideGameOverPanel()
    {
        gameOverPanel?.SetActive(false);
    }

    public void OnRestartButton()
    {
        RespawnManager rm = FindFirstObjectByType<RespawnManager>();
        if (rm != null)
        {
            rm.TriggerManualRespawn();
        }
        else
        {
            Time.timeScale = 1f;
            GameManager.RestartGame();
        }
    }
}
