using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPlayer : MonoBehaviour
{
  [Header("Player Config")]
  [SerializeField] private AudioClip jumpClip;
  [SerializeField] private AudioClip grappleClip;

  [Header("Enemy Config")]
  [SerializeField] private AudioClip attackClip;
  [SerializeField] private AudioClip[] enemyChatter;

  [Header("Config")]
  [SerializeField] private AudioClip damageClip;
  [SerializeField] private AudioClip deathClip;

  private AudioSource audioSource;

  private void Awake()
  {
    audioSource = GetComponent<AudioSource>();
  }

  public void PlayJump()
  {
    if (jumpClip == null) return;

    audioSource.clip = jumpClip;
    audioSource.Play();
  }

  public void PlayGrapple()
  {
    if (grappleClip == null) return;

    audioSource.clip = grappleClip;
    audioSource.Play();
  }

  public void PlayDamage()
  {
    if (damageClip == null) return;

    audioSource.clip = damageClip;
    audioSource.Play();
  }

  public void PlayDeath()
  {
    if (deathClip == null) return;

    audioSource.clip = deathClip;
    audioSource.Play();
  }

  public void PlayAttack()
  {
    if (attackClip == null) return;

    audioSource.clip = attackClip;
    audioSource.Play();
  }

  public void PlayChatter()
  {
    if (enemyChatter.Length == 0) return;

    audioSource.clip = enemyChatter[Random.Range(0, enemyChatter.Length - 1)];
    audioSource.Play();
  }
}
