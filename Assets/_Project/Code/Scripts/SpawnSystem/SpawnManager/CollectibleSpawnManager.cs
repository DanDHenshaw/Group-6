using UnityEngine;
using Utilities;

public class CollectibleSpawnManager : EntitySpawnManager
{
  [SerializeField] CollectibleData[] collectibleData;
  [SerializeField] float spawnInterval = 1.0f;

  EntitySpawner<Collectible> spawner;

  CountdownTimer spawnTimer;
  int counter;

  protected override void Awake()
  {
    base.Awake();

    spawner = new EntitySpawner<Collectible>(new EntityFactory<Collectible>(collectibleData), spawnPointStrategy);

    spawnTimer = new CountdownTimer(spawnInterval);
    spawnTimer.OnTimerStop += () =>
    {
      if(counter >= collectibleData.Length)
      {
        spawnTimer.Stop();
        return;
      }
      Spawn();
      counter++;
      spawnTimer.Start();
    };
  }

  private void Start() => spawnTimer.Start();
  private void Update() => spawnTimer.Tick(Time.deltaTime);

  public override void Spawn() => spawner.Spawn();
}
