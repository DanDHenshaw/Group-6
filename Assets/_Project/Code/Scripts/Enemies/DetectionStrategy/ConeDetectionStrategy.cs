using UnityEngine;
using Utilities;

public class ConeDetectionStrategy : IDetectionStrategy
{
  private readonly float detectionAngle;
  private readonly float detectionRadius;
  private readonly float innerDetectionRadius;

  public ConeDetectionStrategy(float detectionAngle, float detectionRadius, float innerDetectionRadius)
  {
    this.detectionAngle = detectionAngle;
    this.detectionRadius = detectionRadius;
    this.innerDetectionRadius = innerDetectionRadius;
  }

  public bool Execute(Transform target, Transform detector, CountdownTimer timer)
  {
    if (timer.IsRunning) return false;

    Vector3 directionToTarget = target.position - detector.position;
    float angleToTarget = Vector3.Angle(directionToTarget, detector.forward);

    // If the player is not within the detection angle + outer radus (cone in front of enemy),
    // or is within the inner radius, return false
    if((!(angleToTarget < detectionAngle / 2f) || !(directionToTarget.magnitude < detectionRadius))
      && !(directionToTarget.magnitude < innerDetectionRadius))
      return false;

    timer.Start();
    return true;
  }
}