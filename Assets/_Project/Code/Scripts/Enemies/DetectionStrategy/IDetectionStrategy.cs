using UnityEngine;
using Utilities;

public interface IDetectionStrategy
{
  bool Execute(Transform target, Transform detector, CountdownTimer timer);
}
