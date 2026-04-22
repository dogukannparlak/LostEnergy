using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace LostEnergy
{
    public class PauseManager : MonoBehaviour
    {
        [Header("Panels")]
        public GameObject pausePanel;
        public GameObject settingsPanel;
        public GameObject confirmQuitPanel;
        public GameObject controlsPanel;    // Kontrol şeması paneli

        private bool _isPaused = false;

        void Start()
        {
            if (pausePanel != null) pausePanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(false);
            if (confirmQuitPanel != null) confirmQuitPanel.SetActive(false);
            if (controlsPanel != null) controlsPanel.SetActive(false);
        }

        void Update()
        {
            // Eğer karakter ölüyse (Game Over), menüyü açmayı engellemek iyi olabilir,
            // ama şuan basit tutuyoruz.
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                // Ayarlar veya Çıkış onayı açıksa onu kapat (Geri git)
                if (settingsPanel != null && settingsPanel.activeSelf)
                {
                    settingsPanel.SetActive(false);
                    pausePanel.SetActive(true);
                    return;
                }

                if (confirmQuitPanel != null && confirmQuitPanel.activeSelf)
                {
                    confirmQuitPanel.SetActive(false);
                    pausePanel.SetActive(true);
                    return;
                }

                if (controlsPanel != null && controlsPanel.activeSelf)
                {
                    controlsPanel.SetActive(false);
                    pausePanel.SetActive(true);
                    return;
                }

                TogglePause();
            }
        }

        public void TogglePause()
        {
            _isPaused = !_isPaused;

            if (_isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        public void ResumeGame()
        {
            _isPaused = false;
            Time.timeScale = 1f;

            if (pausePanel != null) pausePanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(false);
            if (confirmQuitPanel != null) confirmQuitPanel.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            GameLogger.Instance?.LogEvent("RESUME", "Game resumed");
        }

        private void PauseGame()
        {
            Time.timeScale = 0f;

            if (pausePanel != null) pausePanel.SetActive(true);

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            GameLogger.Instance?.LogEvent("PAUSE", "Game paused");
        }

        public void ShowSettings()
        {
            SettingsManager.RefreshAllFromSaved();
            if (pausePanel != null) pausePanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(true);

            GameLogger.Instance?.LogEvent("UI", "Settings opened");
        }

        public void HideSettings()
        {
            if (settingsPanel != null) settingsPanel.SetActive(false);
            if (pausePanel != null) pausePanel.SetActive(true);

            GameLogger.Instance?.LogEvent("UI", "Settings closed");
        }

        public void ShowControls()
        {
            if (pausePanel != null) pausePanel.SetActive(false);
            if (controlsPanel != null) controlsPanel.SetActive(true);

            GameLogger.Instance?.LogEvent("UI", "Controls opened");
        }

        public void HideControls()
        {
            if (controlsPanel != null) controlsPanel.SetActive(false);
            if (pausePanel != null) pausePanel.SetActive(true);

            GameLogger.Instance?.LogEvent("UI", "Controls closed");
        }

        public void RequestMainMenu()
        {
            if (pausePanel != null) pausePanel.SetActive(false);
            if (confirmQuitPanel != null) confirmQuitPanel.SetActive(true);
        }

        public void ConfirmMainMenu()
        {
            GameLogger.Instance?.LogEvent("SCENE_LOAD", "Main Menu");
            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }

        public void ShowMainMenu()
        {
            ConfirmMainMenu();
        }

        public void CancelMainMenu()
        {
            if (confirmQuitPanel != null) confirmQuitPanel.SetActive(false);
            if (pausePanel != null) pausePanel.SetActive(true);
        }
    }
}
