using UnityEngine;

public class HealthSystem : MonoBehaviour
{
  [Header("Config")]
  [SerializeField] int maxHealth = 100;
  [SerializeField] private FloatEventChannel healthChannel;

  public bool IsDead => Health <= 0;

  public int Health { get; private set; }

  private void Awake()
  {
    Health = maxHealth;
  }

  private void Start()
  {
    PublishHealthPercentage();
  }

  public void TakeDamage(int damage)
  {
    Health = Mathf.Max(0, Health - damage);
    PublishHealthPercentage();
  }

  public void Heal(int amount)
  {
    Health = Mathf.Min(maxHealth, Health + amount);
    PublishHealthPercentage();
  }

  public void SetHealth(int health)
  {
    Health = Mathf.Clamp(health, 0, maxHealth);
    PublishHealthPercentage();
  }

  private void PublishHealthPercentage()
  {
    healthChannel?.Invoke(Health / (float)maxHealth);
  }
}
