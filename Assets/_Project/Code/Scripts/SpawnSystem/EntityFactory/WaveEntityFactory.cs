using UnityEngine;
using Utilities;

[System.Serializable]
public class WaveEntitySpawner<T> where T : Entity
{
  IEntityFactory<T> entityFactory;
  ISpawnPointStrategy spawnPointStrategy;

  #region Wave System

  public enum SpawnState
  {
    SPAWNING,
    WAITING,
    COUNTING,
    COMPLETE
  }

  private SpawnState state = SpawnState.COUNTING;

  [System.Serializable]
  public class Wave
  {
    [Tooltip("Name of the wave")] public string name;

    [Header("Spawning")] [Tooltip("Amount of enemies to spawn in this wave")]
    public float enemiesToSpawn;
    [Tooltip("Rate to spawn the enemies at")]
    public float enemySpawnRate;
  }

  [Tooltip("All enemy waves")] [SerializeField]
  private Wave[] waves;

  private int nextWave = 0;

  [Tooltip("Time to wait between waves")] [SerializeField]
  private float timeBetweenWaves = 5f;

  private CountdownTimer waveCountdown;
  private bool isSpawning = false;

  CountdownTimer waveTimer;

  [SerializeField] private float searchCountdownTime = 1f;
  private CountdownTimer searchCountdown;
  private bool enemyIsAlive = false;

  public bool IsFinished { get; private set; }

  [Header("Event Channels")]
  [SerializeField] private EventChannel waveInitialisedEventChannel;
  [SerializeField] private EventChannel wavesCompleteEventChannel;

  #endregion

  public WaveEntitySpawner(IEntityFactory<T> entityFactory, ISpawnPointStrategy spawnPointStrategy)
  {
    this.entityFactory = entityFactory;
    this.spawnPointStrategy = spawnPointStrategy;
  }

  public void Setup(IEntityFactory<T> entityFactory, ISpawnPointStrategy spawnPointStrategy)
  {
    this.entityFactory = entityFactory;
    this.spawnPointStrategy = spawnPointStrategy;
  }

  public void Initialise()
  {
    waveInitialisedEventChannel?.Invoke(default);

    waveCountdown = new CountdownTimer(timeBetweenWaves);
    waveCountdown.Start();
    waveCountdown.OnTimerStop += () =>
    {
      if (state == SpawnState.SPAWNING && !isSpawning)
      {
        isSpawning = true;
        SpawnWave(waves[nextWave]);
      }

      waveCountdown.Start();
    };

    searchCountdown = new CountdownTimer(searchCountdownTime);
    searchCountdown.Start();
    searchCountdown.OnTimerStop += () =>
    {
      enemyIsAlive = GameObject.FindGameObjectWithTag("Enemy") != null;

      searchCountdown.Start();
    };
  }

  public void UpdateWaves()
  {
    if (state == SpawnState.WAITING)
    {
      // Check if enemies are still alive
      if (!enemyIsAlive)
      {
        // Begin new round
        WaveComplete();
      }
      else
      {
        return;
      }
    }

    if (state == SpawnState.COMPLETE)
    {
      IsFinished = true;
      // Stops wave progressing
      return;
    }

    if (state == SpawnState.COUNTING)
    {
      // Spawn Wave
      state = SpawnState.SPAWNING;
    }
  }

  void WaveComplete()
  {
    if (nextWave + 1 > waves.Length - 1)
    {
      Debug.Log("All Waves Complete!");

      wavesCompleteEventChannel?.Invoke(default);

      state = SpawnState.COMPLETE;
    }
    else
    {
      Debug.Log("Wave Complete!");

      state = SpawnState.COUNTING;

      nextWave++;
    }
  }

  private void SpawnWave(Wave wave)
  {
    Debug.Log("Spawning Wave: " + wave.name);

    int counter = 0;
    waveTimer = new CountdownTimer(wave.enemySpawnRate);
    waveTimer.Start();
    waveTimer.OnTimerStop += () =>
    {
      if (counter >= wave.enemiesToSpawn)
      {
        waveTimer.Stop();

        isSpawning = false;
        state = SpawnState.WAITING;

        return;
      }

      Spawn();
      counter++;
      waveTimer.Start();
    };
  }

  public void Update(float deltaTime)
  {
    waveCountdown.Tick(deltaTime);
    searchCountdown.Tick(deltaTime);
    waveTimer?.Tick(deltaTime);
  }

  public T Spawn()
  {
    return entityFactory.Create(spawnPointStrategy.NextSpawnPoint());
  }
}