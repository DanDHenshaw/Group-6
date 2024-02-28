using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeData : MonoBehaviour
{
  public static TimeData instance;

  public float time = 0f;
  public string timeFormat;

  private void Awake()
  {
    if(instance == null)
    {
      instance = this;

      transform.parent = null;
      DontDestroyOnLoad(this);
    }
  }
}
