using UnityEngine;
using UnityEngine.Events;

public abstract class EventListener<T> : MonoBehaviour
{
  [SerializeField] private EventChannel<T> eventChannel;
  [SerializeField] private UnityEvent<T> unityEvent;

  protected void Awake()
  {
    eventChannel.RegisterListener(this);
  }

  protected void OnDestroy()
  {
    eventChannel.UnregisterListener(this);
  }

  public void Raise(T value)
  {
    unityEvent?.Invoke(value);
  } 
}
public class EventListener : EventListener<Empty> { }