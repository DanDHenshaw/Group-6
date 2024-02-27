using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
private void Awake()
{
  Cursor.lockState = CursorLockMode.Confined;
  Cursor.visible = true;
}

public void LoadScene(string sceneName)
{
  SceneManager.LoadScene(sceneName);
}

public void ExitGame()
{
  Debug.Log("Game Closed");
  Application.Quit();
}
}