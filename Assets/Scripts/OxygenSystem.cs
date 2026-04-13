using System;
using UnityEngine;
using LostEnergy;

public class OxygenSystem : MonoBehaviour
{
    [Header("Oxygen Settings")]
    public float maxOxygen = 100f;
    public float startOxygen = 100f;
    public float drainPerSecond = 2f;
    public float crystalBonus = 5f;

    public float CurrentOxygen { get; private set; }

    public event Action<float, float> OnOxygenChanged;
    public event Action OnOxygenDepleted;

    private float     _hazardDrain;
    private bool      _depleted;
    private bool      _paused;
    private UIManager _uiManager;

    void Start()
    {
        CurrentOxygen = Mathf.Clamp(startOxygen, 0f, maxOxygen);
        _uiManager    = FindFirstObjectByType<UIManager>();
        FireOxygenChanged();

        // Start'ta abone olunur: bu noktada tüm Awake'ler bitmiş,
        // GameManager.Instance kesinlikle set edilmiştir.
        if (GameManager.Instance != null)
            GameManager.Instance.OnCrystalCollected += OnCrystalCollected;
        else
            Debug.LogWarning("[OxygenSystem] GameManager.Instance bulunamadı, crystalBonus çalışmayacak!", this);
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnCrystalCollected -= OnCrystalCollected;
    }

    void Update()
    {
        if (_depleted || _paused) return;

        float totalDrain = drainPerSecond + _hazardDrain;
        CurrentOxygen    = Mathf.Clamp(CurrentOxygen - totalDrain * Time.deltaTime, 0f, maxOxygen);

        FireOxygenChanged();

        if (CurrentOxygen <= 0f)
            TriggerDepletion();
    }

    public void SetPaused(bool paused)
    {
        _paused = paused;
    }

    public void AddHazardDrain(float amount)
    {
        if (amount <= 0f) return;
        _hazardDrain += amount;
    }

    public void RemoveHazardDrain(float amount)
    {
        if (amount <= 0f) return;
        _hazardDrain = Mathf.Max(0f, _hazardDrain - amount);
    }

    public void ResetOxygen()
    {
        _depleted     = false;
        _hazardDrain  = 0f;
        CurrentOxygen = Mathf.Clamp(startOxygen, 0f, maxOxygen);
        FireOxygenChanged();
    }

    public void RestoreToPercent(float percent)
    {
        _depleted     = false;
        CurrentOxygen = Mathf.Clamp(maxOxygen * Mathf.Clamp01(percent), 0f, maxOxygen);
        FireOxygenChanged();
    }

    void OnCrystalCollected()
    {
        CurrentOxygen = Mathf.Clamp(CurrentOxygen + crystalBonus, 0f, maxOxygen);
        FireOxygenChanged();
    }

    void TriggerDepletion()
    {
        _depleted = true;
        OnOxygenDepleted?.Invoke();
        GameManager.Instance?.PlayerDied("Oksijen tükendi!");
    }

    void FireOxygenChanged()
    {
        OnOxygenChanged?.Invoke(CurrentOxygen, maxOxygen);
        _uiManager?.SetOxygenNormalized(CurrentOxygen / maxOxygen);
        _uiManager?.SetOxygenValue(CurrentOxygen, maxOxygen);
    }
}
