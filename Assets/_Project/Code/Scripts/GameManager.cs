using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;

  [Header("Sub Scenes")]
  [SerializeField] string healthBarSceneName;
  [SerializeField] string timerSceneName;
  [SerializeField] string cooldownSceneName;

  [Header("Boss Config")]
  [SerializeField] private TextMeshProUGUI wavesDefeated;
  [SerializeField] private Transform bossSpawn;
  [SerializeField] private GameObject bossObject;

  private int spawnManagers = 0;
  private int spawnManagersComplete = 0;

  private bool isBossSpawned = false;

  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }

    if (!string.IsNullOrEmpty(cooldownSceneName))
      SceneManager.LoadScene(cooldownSceneName, LoadSceneMode.Additive);

    if (!string.IsNullOrEmpty(healthBarSceneName))
      SceneManager.LoadScene(healthBarSceneName, LoadSceneMode.Additive);

    if(!string.IsNullOrEmpty(timerSceneName))
      SceneManager.LoadScene(timerSceneName, LoadSceneMode.Additive);
  }

  private void Update()
  {
    CheckIfEmemiesDefeat();
  }

  public void IncrementSpawnManager()
  {
    spawnManagers++;

    if(wavesDefeated != null)
      wavesDefeated.text = FormatDefeatedText();
  }

  public void IncrementSpawnManagerComplete()
  {
    spawnManagersComplete++;

    if (wavesDefeated != null)
      wavesDefeated.text = FormatDefeatedText();
  }

  private void CheckIfEmemiesDefeat()
  {
    if (isBossSpawned) return;
    if (spawnManagers == 0) return;

    if(spawnManagersComplete >= spawnManagers)
    {
      SpawnBoss();

      if (wavesDefeated != null)
        wavesDefeated.text = "Find/Defeat Mr Toaster";
    }
  }

  private string FormatDefeatedText()
  {
    return "Gangs to Defeat : " + (spawnManagers - spawnManagersComplete);
  }

  private void SpawnBoss()
  {
    Debug.Log("Boss Spawned!");
    Instantiate(bossObject, bossSpawn);
    isBossSpawned = true;
  }

  public void WinGame()
  {
    SceneManager.LoadScene("WinScreen");
  }

  public void LoseGame()
  {
    SceneManager.LoadScene("GameOver");
  }

  public void PauseGame()
  {
    if(!PauseManager.instance.IsPaused)
    {
      PauseManager.instance.PauseGame();
    } 
    else
    {
      PauseManager.instance.UnpauseGame();
    }
  }
}
