using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
  public static AudioManager instance;

  [SerializeField] private AudioMixer mixer;

	[Header("Music")]
  [SerializeField] private AudioSource musicSource;
  [SerializeField] private AudioClip menuMusicAudioClip;
  [SerializeField] private AudioClip gameMusicAudioClip;

  [Header("SFX")]
	[SerializeField] private AudioSource sfxSource;
	[SerializeField] List<AudioClip> sfxClips = new List<AudioClip>();

	public const string MASTER_KEY = "masterVolume";
  public const string MUSIC_KEY = "musicVolume";
  public const string SFX_KEY = "sfxVolume";

  private void Awake()
  {
	  if (instance == null)
	  {
		  instance = this;

			DontDestroyOnLoad(gameObject);
	  }
	  else
	  {
			Destroy(gameObject);
	  }

		LoadVolume();
  }

  public bool GameMusicPlaying() => musicSource.clip == gameMusicAudioClip && musicSource.isPlaying;
  public void PlayGameMusic()
  {
    musicSource.clip = gameMusicAudioClip;
    musicSource.Play();
  }

  public bool MenuMusicPlaying() => musicSource.clip == menuMusicAudioClip && musicSource.isPlaying;
  public void PlayMenuMusic()
  {
    musicSource.clip = menuMusicAudioClip;
    musicSource.Play();
  }

  public void SoundEffects()
  {
    sfxSource.clip = sfxClips[Random.Range(0, sfxClips.Count)];
	  sfxSource.Play();
	}

  void LoadVolume() // Volume saved in VolumeSettings.cs
  {
	  float masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
	  float musicVolume = PlayerPrefs.GetFloat(MUSIC_KEY, 1f);
	  float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);

	  mixer.SetFloat(VolumeSettings.MIXER_MASTER, Mathf.Log10(masterVolume) * 20);
	  mixer.SetFloat(VolumeSettings.MIXER_MUSIC, Mathf.Log10(musicVolume) * 20);
	  mixer.SetFloat(VolumeSettings.MIXER_SFX, Mathf.Log10(sfxVolume) * 20);
  }
}
