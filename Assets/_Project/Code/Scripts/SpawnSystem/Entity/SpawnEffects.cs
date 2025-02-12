﻿using DG.Tweening;
using UnityEngine;

public class SpawnEffects : MonoBehaviour
{
  [SerializeField] GameObject spawnVFX;
  [SerializeField] float animationDuration = 1.0f;

  private void Start()
  {
    transform.localScale = Vector3.zero;
    transform.DOScale(Vector3.one, animationDuration).SetEase(Ease.OutBack);

    if (spawnVFX != null)
    {
      Instantiate(spawnVFX, transform.position, Quaternion.identity);
    }

    GetComponent<AudioSource>().Play();
  }
}
