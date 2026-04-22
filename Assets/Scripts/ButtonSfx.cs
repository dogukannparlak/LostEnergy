using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Plays a click sound when this UI button is pressed.
/// Uses PlayClipAtPoint, so no dedicated AudioSource is required.
/// </summary>
[RequireComponent(typeof(Button))]
public class ButtonSfx : MonoBehaviour
{
    [Tooltip("Sound played when the button is clicked.")]
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

        LostEnergy.GameLogger.Instance?.LogEvent("BUTTON", gameObject.name);
    }
}
