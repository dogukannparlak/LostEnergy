using UnityEngine;
using LostEnergy;

public class CrystalCollectible : MonoBehaviour, IInteractable
{
    [Header("Collection")]
    public int amount = 1;
    public string promptText = "Topla";

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
            Debug.LogWarning($"[CrystalCollectible] '{gameObject.name}' üzerinde Collider yok!", this);

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

        // VFX/SFX önce ateşle (transform hâlâ geçerli)
        if (pickupVfxPrefab != null)
        {
            ParticleSystem vfx = Instantiate(pickupVfxPrefab, transform.position, transform.rotation);
            Destroy(vfx.gameObject, vfxLifetime);
        }

        if (pickupSfx != null)
            AudioSource.PlayClipAtPoint(pickupSfx, transform.position, LostEnergy.SettingsManager.GetEffectiveSfxVolume01());

        // Nesneyi gizle (destroy yerine) - GameManager'dan tekrar aktif edilebilir
        gameObject.SetActive(false);

        GameManager gm = GameManager.Instance != null
            ? GameManager.Instance
            : FindFirstObjectByType<GameManager>();

        if (gm != null)
            gm.AddCrystal(amount);
        else
            Debug.LogWarning("[CrystalCollectible] GameManager.Instance bulunamadı!", this);
    }
}
