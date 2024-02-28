using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  [SerializeField] private EventChannel unpauseEventChannel;

  private void Awake()
  {
    Cursor.lockState = CursorLockMode.Confined;
    Cursor.visible = true;
  }

  public void LoadScene(string sceneName)
  {
    Time.timeScale = 1.0f;
    SceneManager.LoadScene(sceneName);
  }

  public void ExitGame()
  {
    Debug.Log("Game Closed");
    Application.Quit();
  }

  public void UnpauseGame()
  {
    unpauseEventChannel?.Invoke(default);
  }
}