using System;

namespace __Game.Resources.Scripts.EventBus
{
  public interface IEventBinding<T> where T : struct, IEvent
  {
    public Action<T> OnEvent { get; set; }
    public Action OnEventArgs { get; set; }
  }

  public class EventBinding<T> : IEventBinding<T> where T : struct, IEvent
  {
    public int InternalIndex { get; set; } = -1;

    public bool Registered => InternalIndex != -1;

    public bool Listen
    {
      get => _listen;
      set => SetListen(value);
    }
    private bool _listen;

    private Action<T> _onEvent;
    private Action _onEventNoArgs;

    Action<T> IEventBinding<T>.OnEvent { get => _onEvent; set => _onEvent = value; }
    Action IEventBinding<T>.OnEventArgs { get => _onEventNoArgs; set => _onEventNoArgs = value; }

    public EventBinding(Action<T> onEvent)
    {
      _onEvent = onEvent;
      Listen = true;
    }

    public EventBinding(Action onEventNoArgs)
    {
      _onEventNoArgs = onEventNoArgs;
      Listen = true;
    }

    public void Add(Action<T> onEvent) => _onEvent += onEvent;
    public void Remove(Action<T> onEvent) => _onEvent -= onEvent;

    public void Add(Action onEvent) => _onEventNoArgs += onEvent;
    public void Remove(Action onEvent) => _onEventNoArgs -= onEvent;

    private void SetListen(bool value)
    {
      if (value == _listen)
        return;

      if (value)
        EventBus<T>.Register(this);
      else
        EventBus<T>.Unregister(this);

      _listen = value;
    }



    public static implicit operator EventBinding<T>(Action onEventNoArgs)
    {
      return new EventBinding<T>(onEventNoArgs);
    }

    public static implicit operator EventBinding<T>(Action<T> onEvent)
    {
      return new EventBinding<T>(onEvent);
    }

    public static implicit operator bool(EventBinding<T> bind)
    {
      return bind != null;
    }
  }
}