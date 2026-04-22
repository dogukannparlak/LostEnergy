using UnityEngine;
using UnityEngine.SceneManagement;

namespace LostEnergy
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("UI Panels")]
        public GameObject mainMenuPanel;
        public GameObject settingsPanel;
        public GameObject controlsPanel;    // Kontrol şeması paneli

        void Start()
        {
            // Menü başlarken zamanın normal aktığından ve imlecin serbest olduğundan emin ol
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            ShowMainMenu();
        }

        public void PlayGame()
        {
            GameLogger.Instance?.LogEvent("SCENE_LOAD", "SampleScene");
            SceneLoader.LoadScene("SampleScene");
        }

        public void ShowSettings()
        {
            GameLogger.Instance?.LogEvent("UI", "Main Menu: Settings opened");
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(true);
            try { SettingsManager.RefreshAllFromSaved(); }
            catch (System.Exception e) { Debug.LogWarning("[MainMenuManager] SettingsManager.RefreshAllFromSaved hata verdi: " + e.Message); }
        }

        public void ShowControls()
        {
            GameLogger.Instance?.LogEvent("UI", "Main Menu: Controls opened");
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (controlsPanel != null) controlsPanel.SetActive(true);
        }

        public void HideControls()
        {
            GameLogger.Instance?.LogEvent("UI", "Main Menu: Controls closed");
            if (controlsPanel != null) controlsPanel.SetActive(false);
            if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        }

        public void ShowMainMenu()
        {
            if (settingsPanel != null) settingsPanel.SetActive(false);
            if (controlsPanel != null) controlsPanel.SetActive(false);
            if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        }

        public void QuitGame()
        {
            GameLogger.Instance?.LogEvent("QUIT", "Application quit");
            Debug.Log("[MainMenuManager] Oyundan Çıkılıyor...");
            Application.Quit();
        }
    }
}
