using UnityEngine;
using UnityEngine.Events;

public class HealthSystem : MonoBehaviour
{
  [Header("Config")]
  [SerializeField] int maxHealth = 100;

  public event UnityAction<int> OnHealthChanged = delegate { };

  public int Health { get; private set; }

  private void Awake()
  {
    Health = maxHealth;
  }

  public void TakeDamage(int damage)
  {
    Health = Mathf.Max(0, Health - damage);
    OnHealthChanged?.Invoke(Health);
  }

  public void Heal(int amount)
  {
    Health = Mathf.Min(maxHealth, Health + amount);
    OnHealthChanged?.Invoke(Health);
  }

  public void SetHealth(int health)
  {
    Health = Mathf.Clamp(health, 0, maxHealth);
    OnHealthChanged?.Invoke(Health);
  }
}
