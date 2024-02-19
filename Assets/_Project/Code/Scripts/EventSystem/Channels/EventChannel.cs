using System.Collections.Generic;
using UnityEngine;

public abstract class EventChannel<T> : ScriptableObject
{
  private readonly HashSet<EventListener<T>> observers = new();

  public void Invoke(T value)
  {
    foreach (var observer in observers)
    {
      observer.Raise(value);
    }
  }

  public void RegisterListener(EventListener<T> listener) => observers.Add(listener);

  public void UnregisterListener(EventListener<T> listener) => observers.Remove(listener);
}
public readonly struct Empty { }
[CreateAssetMenu(fileName = "EventChannel", menuName = "Custom/Events/EventChannel/Empty")]
public class EventChannel : EventChannel<Empty> { }