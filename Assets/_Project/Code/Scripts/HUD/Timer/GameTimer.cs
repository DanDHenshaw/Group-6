using System;
using UnityEngine;
using Utilities;
using TMPro;

public class GameTimer : MonoBehaviour
{
  private TextMeshProUGUI timerText;
  public StopwatchTimer timer;

  private void Awake()
  {
    timer = new StopwatchTimer();
    timer.Start();
    timerText = GetComponentInChildren<TextMeshProUGUI>();
  }

  void Update()
  {
    timer.Tick(Time.deltaTime);
    var ts = TimeSpan.FromSeconds(timer.GetTime());
    timerText.text = ts.ToString("mm\\:ss\\:ff");
  }
}