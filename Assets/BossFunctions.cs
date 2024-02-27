using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFunctions : MonoBehaviour
{
  [SerializeField] private EventChannel bossDiedEventChannel;

  private EnemyController enemyController;

  private void Awake()
  {
    enemyController = GetComponentInParent<EnemyController>();
  }

  public void DamagePlayer()
  {
    enemyController.DamagePlayer();
  }

  public void KillBoss()
  {
    bossDiedEventChannel?.Invoke(default);
  }
}
