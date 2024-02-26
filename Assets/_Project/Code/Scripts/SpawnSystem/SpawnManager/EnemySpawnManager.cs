using UnityEngine;

public class EnemySpawnManager : EntitySpawnManager
{
  [SerializeField] EnemyData[] enemyData;

  [SerializeField] WaveEntitySpawner<EnemyController> spawner;

  private bool canSpawn = false;

  protected override void Awake()
  {
    base.Awake();

    spawner.Setup(new EntityFactory<EnemyController>(enemyData), spawnPointStrategy);
  }

  private void Start() => spawner.Initialise();

  private void Update()
  {
    if (!canSpawn) return;

    spawner.Update(Time.deltaTime);
    Spawn();
  }

  public override void Spawn() => spawner.UpdateWaves();

  private void OnTriggerEnter(Collider c)
  {
    if (c.CompareTag("Player"))
    {
      canSpawn = true;
    }
  }
}