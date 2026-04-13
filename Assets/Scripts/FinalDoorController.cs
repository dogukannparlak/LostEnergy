using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDoorController : MonoBehaviour, IInteractable
{
    public enum DoorMode { Loop, Win }

    [Header("Kapı Modu")]
    [Tooltip("Loop = Döngü (Scene 1'e döner) | Win = Çıkış (Tebrikler ekranı)")]
    public DoorMode doorMode = DoorMode.Loop;

    [Header("Door")]
    public GameObject doorObject;

    [Header("Tebrikler Paneli (sadece Win modu)")]
    [Tooltip("Sahnede hazırladığın Tebrikler UI paneli. Sadece Win kapısı için doldur.")]
    public GameObject winPanel;

    void Start()
    {
        // Kapılar bu sahnede her zaman açık (kristal yok)
    }

    public string GetPrompt()
    {
        return doorMode == DoorMode.Loop ? "Döngüye Gir" : "Oyundan Çık";
    }

    public void Interact(PlayerInteraction interactor)
    {
        if (doorMode == DoorMode.Loop)
            LoadLoop();
        else
            ShowWin();
    }

    void LoadLoop()
    {
        Time.timeScale   = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
        SceneManager.LoadScene(2);
    }

    void ShowWin()
    {
        if (winPanel != null) winPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;
        Time.timeScale   = 0f;
    }

    // UI butonundan çağrılır: "Ana Menüye Dön" butonu OnClick'ine bağla
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
