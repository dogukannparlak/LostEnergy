using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace LostEnergy
{
    public class SettingsManager : MonoBehaviour
    {
        public static event Action AudioSettingsChanged;

        [Header("Audio Settings")]
        public AudioMixer audioMixer;

        [Header("UI Elements")]
        public Slider masterSlider;
        public Slider musicSlider;
        public Slider sfxSlider;
        public Toggle muteToggle;

        private const string MASTER_KEY = "MasterVolume";
        private const string MUSIC_KEY = "MusicVolume";
        private const string SFX_KEY = "SFXVolume";
        private const string MUTE_KEY = "IsMuted";

        public static float MasterVolume01 { get; private set; } = 1f;
        public static float MusicVolume01  { get; private set; } = 1f;
        public static float SfxVolume01    { get; private set; } = 1f;
        public static bool  IsMuted        { get; private set; }

        void Start()
        {
            if (masterSlider != null) masterSlider.onValueChanged.AddListener(SetMasterVolume);
            if (musicSlider != null)  musicSlider.onValueChanged.AddListener(SetMusicVolume);
            if (sfxSlider != null)    sfxSlider.onValueChanged.AddListener(SetSFXVolume);
            if (muteToggle != null)   muteToggle.onValueChanged.AddListener(SetMute);

            RegisterSliderDragLogger(masterSlider, "Master volume");
            RegisterSliderDragLogger(musicSlider,  "Music volume");
            RegisterSliderDragLogger(sfxSlider,    "SFX volume");

            LoadSettings();
        }

        private void OnEnable()
        {
            // Keep all open/close cycles in sync across menu and pause panels.
            RefreshFromSaved();
        }

        public void SetMasterVolume(float volume)
        {
            MasterVolume01 = Mathf.Clamp01(volume);

            if (muteToggle != null && muteToggle.isOn) return; // Muted

            float db = volume > 0.001f ? Mathf.Log10(volume) * 20 : -80f;
            if (audioMixer != null)
            {
                bool ok = audioMixer.SetFloat("MasterVol", db);
                if (!ok) Debug.LogWarning("[SettingsManager] 'MasterVol' parameter not found in AudioMixer. Check Exposed Parameters.");
            }
            PlayerPrefs.SetFloat(MASTER_KEY, volume);
            AudioSettingsChanged?.Invoke();
        }

        public void SetMusicVolume(float volume)
        {
            MusicVolume01 = Mathf.Clamp01(volume);

            float db = volume > 0.001f ? Mathf.Log10(volume) * 20 : -80f;
            if (audioMixer != null)
            {
                bool ok = audioMixer.SetFloat("MusicVol", db);
                if (!ok) Debug.LogWarning("[SettingsManager] 'MusicVol' parameter not found in AudioMixer.");
            }
            PlayerPrefs.SetFloat(MUSIC_KEY, volume);
            AudioSettingsChanged?.Invoke();
        }

        public void SetSFXVolume(float volume)
        {
            SfxVolume01 = Mathf.Clamp01(volume);

            float db = volume > 0.001f ? Mathf.Log10(volume) * 20 : -80f;
            if (audioMixer != null)
            {
                bool ok = audioMixer.SetFloat("SFXVol", db);
                if (!ok) Debug.LogWarning("[SettingsManager] 'SFXVol' parameter not found in AudioMixer.");
            }
            PlayerPrefs.SetFloat(SFX_KEY, volume);
            AudioSettingsChanged?.Invoke();
        }

        public void SetMute(bool isMuted)
        {
            IsMuted = isMuted;
            GameLogger.Instance?.LogEvent("MUTE", isMuted ? "Muted" : "Unmuted");
            PlayerPrefs.SetInt(MUTE_KEY, isMuted ? 1 : 0);

            if (isMuted)
            {
                if (audioMixer != null)
                {
                    audioMixer.SetFloat("MasterVol", -80f);
                    audioMixer.SetFloat("MusicVol",  -80f);
                    audioMixer.SetFloat("SFXVol",    -80f);
                }
            }
            else
            {
                if (masterSlider != null) SetMasterVolume(masterSlider.value);
                if (musicSlider  != null) SetMusicVolume(musicSlider.value);
                if (sfxSlider    != null) SetSFXVolume(sfxSlider.value);
            }

            AudioSettingsChanged?.Invoke();
        }

        private void LoadSettings()
        {
            float masterVol = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
            float musicVol = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
            float sfxVol = PlayerPrefs.GetFloat(SFX_KEY, 1f);
            bool isMuted = PlayerPrefs.GetInt(MUTE_KEY, 0) == 1;

            MasterVolume01 = Mathf.Clamp01(masterVol);
            MusicVolume01 = Mathf.Clamp01(musicVol);
            SfxVolume01 = Mathf.Clamp01(sfxVol);
            IsMuted = isMuted;

            if (masterSlider != null) masterSlider.value = masterVol;
            if (musicSlider != null) musicSlider.value = musicVol;
            if (sfxSlider != null) sfxSlider.value = sfxVol;
            if (muteToggle != null) muteToggle.isOn = isMuted;

            SetMasterVolume(masterVol);
            SetMusicVolume(musicVol);
            SetSFXVolume(sfxVol);
            SetMute(isMuted);
        }

        public void RefreshFromSaved()
        {
            LoadSettings();
        }

        public static void RefreshAllFromSaved()
        {
            SettingsManager[] managers = FindObjectsByType<SettingsManager>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            for (int i = 0; i < managers.Length; i++)
            {
                if (managers[i] != null)
                {
                    managers[i].RefreshFromSaved();
                }
            }
        }

        public static float GetEffectiveSfxVolume01()
        {
            return IsMuted ? 0f : Mathf.Clamp01(MasterVolume01 * SfxVolume01);
        }

        private void RegisterSliderDragLogger(Slider slider, string label)
        {
            if (slider == null) return;

            EventTrigger trigger = slider.gameObject.GetComponent<EventTrigger>()
                                ?? slider.gameObject.AddComponent<EventTrigger>();

            float startVal = 0f;

            var begin = new EventTrigger.Entry { eventID = EventTriggerType.BeginDrag };
            begin.callback.AddListener(_ => startVal = slider.value);
            trigger.triggers.Add(begin);

            var end = new EventTrigger.Entry { eventID = EventTriggerType.EndDrag };
            end.callback.AddListener(_ =>
            {
                float endVal = slider.value;
                if (Mathf.Abs(endVal - startVal) < 0.001f) return;
                string dir = endVal > startVal ? "increased" : "decreased";
                GameLogger.Instance?.LogEvent("SETTINGS", $"{label} {dir}: {startVal:F2} → {endVal:F2}");
            });
            trigger.triggers.Add(end);
        }
    }
}
