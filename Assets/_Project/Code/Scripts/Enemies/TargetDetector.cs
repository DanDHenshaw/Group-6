using UnityEngine;
using Utilities;

public class TargetDetector : MonoBehaviour
{
  [Header("Config")]
  [SerializeField] float detectionAngle = 60f; // Cone in front of enemy
  [SerializeField] float detectionRadius = 10f; // Cone distance from enemy
  [SerializeField] float innerDetectionRadius = 5f; // Small circle around enemy
  [SerializeField] float detectionCooldown = 1f; // Time between detection

  [HideInInspector] public float attackRange = 2f;

  public Transform Target { get; private set; }

  CountdownTimer detectionTimer;

  IDetectionStrategy detectionStrategy;

  private void Start()
  {
    detectionTimer = new CountdownTimer(detectionCooldown);
    Target = GameObject.FindGameObjectWithTag("Player").transform;
    detectionStrategy = new ConeDetectionStrategy(detectionAngle, detectionRadius, innerDetectionRadius);
  }

  private void Update() => detectionTimer.Tick(Time.deltaTime);

  public bool CanDetectTarget()
  {
    return detectionTimer.IsRunning || detectionStrategy.Execute(Target, transform, detectionTimer);
  }

  public bool CanAttackTarget()
  {
    var directionToTarget = Target.position - transform.position;
    return directionToTarget.magnitude <= attackRange;
  }

  public bool InAttackTargetRange()
  {
    var directionToTarget = Target.position - transform.position;
    return directionToTarget.magnitude <= attackRange + 0.5f;
  }

  public void SetDetectionStrategy(IDetectionStrategy detectionStrategy) => this.detectionStrategy = detectionStrategy;
}
