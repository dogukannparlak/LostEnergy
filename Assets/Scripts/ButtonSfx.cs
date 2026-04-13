using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Herhangi bir UI butonuna ekle; tıklandığında ses çalar.
/// AudioSource gerektirmez, PlayClipAtPoint ile 2D çalar.
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonSfx : MonoBehaviour
{
    [Tooltip("Butona basılınca çalacak ses.")]
    public AudioClip clickSfx;

    [Range(0f, 1f)]
    public float volume = 1f;

    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(PlaySound);
    }

    void PlaySound()
    {
        if (clickSfx != null)
            AudioSource.PlayClipAtPoint(
                clickSfx,
                Camera.main != null ? Camera.main.transform.position : Vector3.zero,
                volume * LostEnergy.SettingsManager.GetEffectiveSfxVolume01());
    }
}
