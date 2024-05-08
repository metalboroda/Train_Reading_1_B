using __Game.Resources.Scripts.EventBus;
using Assets.__Game.Resources.Scripts.Game.States;
using Assets.__Game.Scripts.Infrastructure;
using System.Collections;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts.Variant
{
  [ExecuteAlways]
  public class VariantBoard : MonoBehaviour
  {
    [SerializeField] private bool _allowEdit = true;
    [SerializeField] private float _spacing = 1f;
    [SerializeField] private float _fixedChildWidth = 1f;
    [Space]
    [SerializeField] private VariantItem[] _variantItems;
    [Space]
    [SerializeField] private Variant[] _variantObjects;

    private int _emptyVariantsCounter;
    private int _overallAnswersCounter;
    private int _correctAnswersCounter;
    private int _incorrectAnswerCounter;

    private GameBootstrapper _gameBootstrapper;

    private EventBinding<EventStructs.CorrectAnswerEvent> _correctAnswerEvent;
    private EventBinding<EventStructs.IncorrectCancelEvent> _incorrectAnswerEvent;

    private void Awake()
    {
      _gameBootstrapper = GameBootstrapper.Instance;
    }

    private void OnEnable()
    {
      _correctAnswerEvent = new EventBinding<EventStructs.CorrectAnswerEvent>(ReceiveCorrectAnswer);
      _incorrectAnswerEvent = new EventBinding<EventStructs.IncorrectCancelEvent>(ReceiveIncorrectAnswer);
    }

    private void OnDisable()
    {
      _correctAnswerEvent.Remove(ReceiveCorrectAnswer);
      _incorrectAnswerEvent.Remove(ReceiveIncorrectAnswer);
    }

    private void Start()
    {
      _emptyVariantsCounter = CountVariantsWithSpriteHidden();

      InitVariants();

      EventBus<EventStructs.ComponentEvent<VariantBoard>>.Raise(new EventStructs.ComponentEvent<VariantBoard> { Data = this });
    }

    private void Update()
    {
      if (Application.isPlaying == false)
      {
        RearrangeChildren();
      }
    }

    private void OnValidate()
    {
      if (Application.isPlaying == false && _allowEdit)
      {
        RearrangeChildren();
      }
    }

    private void RearrangeChildren()
    {
      float currentXPosition = 0f;

      foreach (Transform child in transform)
      {
        child.localPosition = new Vector3(currentXPosition, 0f, 0f);
        currentXPosition += _fixedChildWidth + _spacing;
      }
    }

    private void InitVariants()
    {
      string currentText = null;

      for (int i = 0; i < _variantItems.Length; i++)
      {
        currentText = _variantItems[i].VariantText;

        _variantObjects[i].SetSpriteAndImage(currentText, _variantItems[i].ShowText);
      }

      EventBus<EventStructs.VariantsAssignedEvent>.Raise(new EventStructs.VariantsAssignedEvent());
    }

    public int CountVariantsWithSpriteHidden()
    {
      int count = 0;

      foreach (VariantItem item in _variantItems)
      {
        if (item.ShowText == false)
        {
          count++;
        }
      }
      return count;
    }

    private void ReceiveCorrectAnswer(EventStructs.CorrectAnswerEvent correctAnswerEvent)
    {
      _overallAnswersCounter++;
      _correctAnswersCounter++;

      if (_correctAnswersCounter == _emptyVariantsCounter)
      {
        EventBus<EventStructs.WinEvent>.Raise(new EventStructs.WinEvent());
        EventBus<EventStructs.LevelPointEvent>.Raise(new EventStructs.LevelPointEvent());

        StartCoroutine(DoChangeWinLoseGameStateWithDelay(true, 1.5f));

        return;
      }

      CheckOverallAnswer();
    }

    private void ReceiveIncorrectAnswer(EventStructs.IncorrectCancelEvent incorrectCancelEvent)
    {
      _overallAnswersCounter++;
      _incorrectAnswerCounter++;

      if (_incorrectAnswerCounter == _emptyVariantsCounter)
      {
        EventBus<EventStructs.LoseEvent>.Raise(new EventStructs.LoseEvent());

        StartCoroutine(DoChangeWinLoseGameStateWithDelay(false, 0));

        return;
      }
    }

    private void CheckOverallAnswer()
    {
      if (_overallAnswersCounter == _emptyVariantsCounter)
      {
        if (_correctAnswersCounter < _emptyVariantsCounter)
          EventBus<EventStructs.LoseEvent>.Raise(new EventStructs.LoseEvent());

        else if (_incorrectAnswerCounter < _emptyVariantsCounter) { }
      }
    }

    public Transform GetFirstVariantObjectTransform()
    {
      if (_variantObjects != null && _variantObjects.Length > 0)
        return _variantObjects[0].transform;

      return null;
    }

    public Transform GetLastVariantObjectTransform()
    {
      if (_variantObjects != null && _variantObjects.Length > 0)
        return _variantObjects[_variantObjects.Length - 1].transform;

      return null;
    }

    private IEnumerator DoChangeWinLoseGameStateWithDelay(bool win, float delay)
    {
      yield return new WaitForSeconds(delay);

      if (win== true)
        _gameBootstrapper.StateMachine.ChangeState(new GameWinState(_gameBootstrapper));
      else
        _gameBootstrapper.StateMachine.ChangeState(new GameLoseState(_gameBootstrapper));
    }
  }
}