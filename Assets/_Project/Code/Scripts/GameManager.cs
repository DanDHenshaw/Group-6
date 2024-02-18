using UnityEngine;

public class GameManager : MonoBehaviour
{
  [Header("Player References")]
  [SerializeField] PlayerController playerController;
  [SerializeField] HealthSystem playerHealthSystem;

  private void Awake()
  {
    playerController = FindObjectOfType<PlayerController>();
    playerHealthSystem = playerController.GetComponent<HealthSystem>();

    playerHealthSystem.OnHealthChanged += HealthChanged;
  }

  private void HealthChanged(int newHealth)
  {
    if (newHealth <= 0)
    {
      Debug.Log("Dead");
    }
  }
}
