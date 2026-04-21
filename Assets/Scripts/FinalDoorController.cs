using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalDoorController : MonoBehaviour, IInteractable
{
    public enum DoorMode { Loop, Win }

    [Header("Door Mode")]
    [Tooltip("Loop returns to Scene 1. Win shows the congratulations panel.")]
    public DoorMode doorMode = DoorMode.Loop;

    [Header("Door")]
    public GameObject doorObject;

    [Header("Congratulations Panel (Win mode only)")]
    [Tooltip("Assign the congratulations UI panel for the Win door.")]
    public GameObject winPanel;

    void Start()
    {
        // Doors are always open in this scene.
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

    // Hook this to the Return to Main Menu button.
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
}
