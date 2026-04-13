using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LostEnergy
{
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Settings")]
    public int targetCrystals = 10;

    public int CurrentCrystals { get; private set; }

    public event Action OnCrystalCollected;
    public event Action<int, int> OnCrystalCountChanged;
    public event Action OnTargetReached;
    public event Action<string> OnPlayerDied;

    private bool _targetReached;
    private List<CrystalCollectible> _allCrystals = new List<CrystalCollectible>();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("[GameManager] Sahnede birden fazla GameManager var!", gameObject);
            Destroy(gameObject);
            return;
        }

        if (Instance == null)
            Instance = this;
    }

    public void AddCrystal(int amount)
    {
        if (amount <= 0) return;

        CurrentCrystals += amount;

        OnCrystalCollected?.Invoke();
        OnCrystalCountChanged?.Invoke(CurrentCrystals, targetCrystals);

        if (!_targetReached && CurrentCrystals >= targetCrystals)
        {
            _targetReached = true;
            OnTargetReached?.Invoke();
        }
    }

    public void TryWin()
    {
        if (CurrentCrystals >= targetCrystals && !_targetReached)
        {
            _targetReached = true;
            OnTargetReached?.Invoke();
        }
    }

    public void RegisterCrystal(CrystalCollectible crystal)
    {
        if (!_allCrystals.Contains(crystal))
        {
            _allCrystals.Add(crystal);
        }
    }

    public void PlayerDied(string reason = null)
    {
        OnPlayerDied?.Invoke(reason);
    }

    public void ResetCrystals()
    {
        CurrentCrystals = 0;
        _targetReached = false;
        OnCrystalCountChanged?.Invoke(CurrentCrystals, targetCrystals);

        foreach (var crystal in _allCrystals)
        {
            if (crystal != null)
                crystal.Respawn();
        }
    }

    public static void RestartGame()
    {
        Time.timeScale = 1f;
        if (Instance != null)
        {
            Instance.CurrentCrystals = 0;
            Instance._targetReached = false;
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
} // LostEnergy namespace
