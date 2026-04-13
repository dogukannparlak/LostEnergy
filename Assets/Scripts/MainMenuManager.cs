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
            SceneLoader.LoadScene("SampleScene");   // LoadingScreen üzerinden geçiş
        }

        public void ShowSettings()
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (settingsPanel != null) settingsPanel.SetActive(true);
            try { SettingsManager.RefreshAllFromSaved(); }
            catch (System.Exception e) { Debug.LogWarning("[MainMenuManager] SettingsManager.RefreshAllFromSaved hata verdi: " + e.Message); }
        }

        public void ShowControls()
        {
            if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
            if (controlsPanel != null) controlsPanel.SetActive(true);
        }

        public void HideControls()
        {
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
            Debug.Log("[MainMenuManager] Oyundan Çıkılıyor...");
            Application.Quit();
        }
    }
}
