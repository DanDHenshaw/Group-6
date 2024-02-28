using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUI : MonoBehaviour
{
  [SerializeField] private Sprite[] images;

  [SerializeField] private Image cooldownImage;

  public void UpdateCooldown(float percentage)
  {
    int index = Mathf.RoundToInt((images.Length - 1) * percentage);
    cooldownImage.sprite = images[index];
  }
}
