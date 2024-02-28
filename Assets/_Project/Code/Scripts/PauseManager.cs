using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
  public static PauseManager instance;

  [SerializeField] string pauseSceneName;

  public bool IsPaused {  get; private set; }

  private void Awake()
  {
    if(instance == null)
    {
      instance = this;
    }
  }

  public void PauseGame()
  {
    Cursor.lockState = CursorLockMode.Confined;
    Cursor.visible = true;
    SceneManager.LoadScene(pauseSceneName, LoadSceneMode.Additive);
    IsPaused = true;
    Time.timeScale = 0f;
  }

  public void UnpauseGame()
  {
    Cursor.lockState = CursorLockMode.Locked;
    Cursor.visible = false;
    SceneManager.UnloadSceneAsync(pauseSceneName);
    IsPaused = false;
    Time.timeScale = 1f;
  }
}
