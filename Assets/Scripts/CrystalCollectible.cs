using UnityEngine;
using LostEnergy;

public class CrystalCollectible : MonoBehaviour, IInteractable
{
    [Header("Collection")]
    public int amount = 1;
    public string promptText = "Collect";

    [Header("Effects")]
    public AudioClip pickupSfx;
    public ParticleSystem pickupVfxPrefab;

    [Header("VFX Lifetime")]
    [Min(0.1f)]
    public float vfxLifetime = 3f;

    private bool _collected;

    void Start()
    {
        if (GetComponent<Collider>() == null)
            Debug.LogWarning($"[CrystalCollectible] '{gameObject.name}' has no Collider!", this);

        GameManager gm = GameManager.Instance != null
            ? GameManager.Instance
            : FindFirstObjectByType<GameManager>();

        if (gm != null)
            gm.RegisterCrystal(this);
    }

    public void Respawn()
    {
        _collected = false;
        gameObject.SetActive(true);
    }

    public string GetPrompt() => promptText;

    public void Interact(PlayerInteraction interactor)
    {
        if (_collected) return;
        _collected = true;

        // Play effects before hiding the object.
        if (pickupVfxPrefab != null)
        {
            ParticleSystem vfx = Instantiate(pickupVfxPrefab, transform.position, transform.rotation);
            Destroy(vfx.gameObject, vfxLifetime);
        }

        if (pickupSfx != null)
            AudioSource.PlayClipAtPoint(pickupSfx, transform.position, LostEnergy.SettingsManager.GetEffectiveSfxVolume01());

        // Hide instead of destroying so it can be respawned later.
        gameObject.SetActive(false);

        GameManager gm = GameManager.Instance != null
            ? GameManager.Instance
            : FindFirstObjectByType<GameManager>();

        if (gm != null)
            gm.AddCrystal(amount);
        else
            Debug.LogWarning("[CrystalCollectible] GameManager.Instance was not found!", this);
    }
}
