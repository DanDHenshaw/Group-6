using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeath : MonoBehaviour
{
  [SerializeField] private EventChannel bossDiedEventChannel;

  public void KillBoss()
  {
    bossDiedEventChannel?.Invoke(default);
  }
}
