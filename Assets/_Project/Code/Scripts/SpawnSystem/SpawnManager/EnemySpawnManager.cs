using UnityEngine;
using Utilities;

public class EnemySpawnManager : EntitySpawnManager
{
  [SerializeField] EnemyData[] enemyData;

  [SerializeField] WaveEntitySpawner<EnemyController> spawner;

  protected override void Awake()
  {
    base.Awake();

    spawner.Setup(new EntityFactory<EnemyController>(enemyData), spawnPointStrategy);
  }

  private void Start() => spawner.Initialise();

  private void Update()
  {
    spawner.Update(Time.deltaTime);
    Spawn();
  }

  public override void Spawn() => spawner.UpdateWaves();
}