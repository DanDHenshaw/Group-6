using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinScreen : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI timerText;

  private void Start()
  {
    timerText.text = "Congratulations You Won: " + TimeData.instance.timeFormat;
  }
}
