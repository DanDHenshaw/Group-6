using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  public static GameManager instance;

  private int spawnManagers = 0;
  private int spawnManagersComplete = 0;

  private bool isBossSpawned = false;

  private void Awake()
  {
    if (instance == null)
    {
      instance = this;
    }
  }

  private void Update()
  {
    CheckIfEmemiesDefeat();
  }

  public void IncrementSpawnManager() => spawnManagers++;
  public void IncrementSpawnManagerComplete() => spawnManagersComplete++;

  private void CheckIfEmemiesDefeat()
  {
    if (!isBossSpawned) return;
    if (spawnManagers == 0) return;

    if(spawnManagersComplete >= spawnManagers)
    {
      SpawnBoss();
    }
  }

  private void SpawnBoss()
  {
    print("Boss Spawned");
  }
}
