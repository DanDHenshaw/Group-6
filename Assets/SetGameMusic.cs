using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameMusic : MonoBehaviour
{
  [SerializeField] bool isGameMusic;

  private void Start()
  {
    if (isGameMusic)
    {
      if (AudioManager.instance.GameMusicPlaying()) return;

      AudioManager.instance.PlayGameMusic();
    }
    else
    {
      if (AudioManager.instance.MenuMusicPlaying()) return;

      AudioManager.instance.PlayMenuMusic();
    }
  }
}
