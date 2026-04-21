using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using LostEnergy;

[RequireComponent(typeof(Button))]
public class GameOverRestartButton : MonoBehaviour
{
    void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        // Restore normal game state before reloading.
        Time.timeScale   = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;

        if (GameManager.Instance != null)
            GameManager.RestartGame();
        else
            SceneManager.LoadScene(1);
    }
}
