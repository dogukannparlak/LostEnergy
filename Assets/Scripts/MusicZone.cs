using UnityEngine;

[RequireComponent(typeof(Collider))]
public class MusicZone : MonoBehaviour
{
    [Header("Music")]
    [Tooltip("Oyuncu bu alana girince çalacak müzik.")]
    public AudioClip zoneMusic;

    private bool _playerInside;

    void Reset()
    {
        EnsureTriggerCollider();
    }

    void OnValidate()
    {
        EnsureTriggerCollider();
    }

    void OnTriggerEnter(Collider other)
    {
        if (_playerInside || zoneMusic == null) return;
        if (!IsPlayer(other)) return;

        _playerInside = true;
        MusicManager.Instance?.SetZoneOverride(zoneMusic);
    }

    void OnTriggerExit(Collider other)
    {
        if (!_playerInside) return;
        if (!IsPlayer(other)) return;

        _playerInside = false;
        MusicManager.Instance?.ClearZoneOverride(zoneMusic);
    }

    void OnDisable()
    {
        if (!_playerInside) return;

        _playerInside = false;
        MusicManager.Instance?.ClearZoneOverride(zoneMusic);
    }

    void EnsureTriggerCollider()
    {
        var triggerCollider = GetComponent<Collider>();
        if (triggerCollider == null) triggerCollider = gameObject.AddComponent<BoxCollider>();
        triggerCollider.isTrigger = true;
    }

    static bool IsPlayer(Collider other)
    {
        return other.CompareTag("Player")
            || other.GetComponentInParent<PlayerController3P>() != null;
    }
}
