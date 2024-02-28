using System;
using UnityEngine;
using Utilities;
using TMPro;

public class GameTimer : MonoBehaviour
{
  private TextMeshProUGUI timerText;
  private StopwatchTimer timer;

  private void Awake()
  {
    timer = new StopwatchTimer();
    timer.Start();
    timerText = GetComponentInChildren<TextMeshProUGUI>();
  }

  void Update()
  {
    timer.Tick(Time.deltaTime);
    timerText.text = FormatTime(timer.GetTime());
  }

  private string FormatTime(float time)
  {
    var ts = TimeSpan.FromSeconds(time);
    return ts.ToString("mm\\:ss\\:ff");
  }

  public void WinGame()
  {
    timer.Stop();
    TimeData.instance.time = timer.GetTime();
    TimeData.instance.timeFormat = FormatTime(timer.GetTime());
    GameManager.instance.WinGame();
  }
}