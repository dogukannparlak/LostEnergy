using System.Collections;
using UnityEngine;
using LostEnergy;

public class RespawnManager : MonoBehaviour
{
    [Header("Player")]
    public Transform playerRoot;
    public CharacterController characterController;

    [Header("Respawn")]
    public Transform respawnPoint;

    [Header("Systems")]
    public OxygenSystem oxygenSystem;

    [Header("UI")]
    public GameObject losePanel;

    [Min(0f)]
    public float loseDuration = 2f;

    private bool _respawning;

    void Start()
    {
        if (characterController == null && playerRoot != null)
            characterController = playerRoot.GetComponent<CharacterController>();

        if (oxygenSystem == null && playerRoot != null)
            oxygenSystem = playerRoot.GetComponent<OxygenSystem>();

        if (playerRoot       == null) Debug.LogError("[RespawnManager] 'playerRoot' is not assigned!",       this);
        if (respawnPoint     == null) Debug.LogError("[RespawnManager] 'respawnPoint' is not assigned!",     this);
        if (oxygenSystem     == null) Debug.LogError("[RespawnManager] 'oxygenSystem' not found!",    this);
        if (characterController == null) Debug.LogWarning("[RespawnManager] 'characterController' not found.", this);
        if (losePanel        == null) Debug.LogWarning("[RespawnManager] 'losePanel' is not assigned.", this);
        else losePanel.SetActive(false);

        if (GameManager.Instance != null)
            GameManager.Instance.OnPlayerDied += HandlePlayerDied;
    }

    void OnDestroy()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnPlayerDied -= HandlePlayerDied;
    }



    void HandlePlayerDied(string reason)
    {
        if (_respawning) return;
        _respawning = true;

        if (losePanel != null) losePanel.SetActive(true);
    }

    // Called by the Restart button.
    public void TriggerManualRespawn()
    {
        Teleport();
        oxygenSystem?.ResetOxygen();
        GameManager.Instance?.ResetCrystals();

        HazardZone[] hazardZones = FindObjectsByType<HazardZone>(FindObjectsSortMode.None);
        foreach (var hz in hazardZones) hz.ResetZone();

        if (losePanel != null) losePanel.SetActive(false);

        _respawning = false;
        Time.timeScale = 1f; // Resume time after freeze
    }

    void Teleport()
    {
        if (playerRoot == null || respawnPoint == null) return;

        if (characterController != null)
        {
            characterController.enabled = false;
            playerRoot.SetPositionAndRotation(respawnPoint.position, respawnPoint.rotation);
            characterController.enabled = true;
        }
        else
        {
            playerRoot.SetPositionAndRotation(respawnPoint.position, respawnPoint.rotation);
        }
    }
}
