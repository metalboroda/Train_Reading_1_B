using Assets.__Game.Scripts.Enums;

namespace __Game.Resources.Scripts.EventBus
{
  public class EventStructs
  {
    #region FiniteStateMachine
    public struct StateChanged : IEvent
    {
      public State State;
    }
    #endregion

    #region Game
    public struct StuporEvent : IEvent { }
    #endregion

    #region Train
    public struct TrainMovementEvent : IEvent
    {
      public bool IsMoving;
    }
    #endregion

    #region Variants&Answers
    public struct VariantsAssignedEvent : IEvent { }
    public struct CorrectAnswerEvent : IEvent { }
    public struct IncorrectCancelEvent : IEvent { }
    #endregion

    #region Game
    public struct WinEvent : IEvent { }
    public struct LoseEvent : IEvent { }
    #endregion

    #region ScoreManager
    public struct LevelPointEvent : IEvent
    {
      public int LevelPoint;
    }
    #endregion

    #region Ui
    public struct LevelCounterEvent : IEvent
    {
      public int OverallLevelIndex;
    }
    public struct UiButtonEvent : IEvent
    {
      public UiEnums UiEnums;
    }
    #endregion

    #region Audio
    public struct AudioSwitchedEvent : IEvent { }
    #endregion

    #region Components
    public struct ComponentEvent<T> : IEvent
    {
      public T Data { get; set; }
    }
    #endregion

    #region Timer
    public struct TimerEvent : IEvent
    {
      public int Time;
    }
    #endregion
  }
}