using Assets.__Game.Resources.Scripts.Balloon;
using Assets.__Game.Scripts.Enums;
using System.Collections.Generic;

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

    #region BalloonSpawner
    public struct BalloonSpawnerEvent : IEvent
    {
      public List<BalloonHandler> CorrectBalloonHandlers;
      public int CorrectBalloonCount;
      public List<BalloonHandler> IncorrectBalloonhHandlers;
      public int IncorrectBalloonCount;
    }
    #endregion

    #region BalloonManager
    public struct BalloonReceiveEvent : IEvent
    {
      public bool CorrectBalloon;
      public string[] CorrectValues;
      public int CorrectBalloonIncrement;
      public int IncorrectBalloonIncrement;
    }
    #endregion

    #region Balloon
    public struct BalloonUiEvent : IEvent
    {
      public int BalloonId;
      public string BalloonValue;
      public bool Correct;
      public bool Tutorial;
    }

    public struct BalloonClickEvent : IEvent
    {
      public string BalloonValue;
      public BalloonHandler BalloonHandler;
    }

    public struct BalloonReMovementEvent : IEvent
    {
      public BalloonController BalloonController;
    }

    public struct BalloonDestroyEvent : IEvent
    {
      public int BalloonId;
      public bool Correct;
    }
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