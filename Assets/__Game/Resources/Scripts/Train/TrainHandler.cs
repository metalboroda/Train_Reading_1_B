using __Game.Resources.Scripts.EventBus;
using Assets.__Game.Resources.Scripts.Variant;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts.Train
{
  public class TrainHandler : MonoBehaviour
  {
    [SerializeField] private Transform _cartJoint;
    [Space]
    [SerializeField] private CartHandler _cartPrefab;
    [Space]
    [SerializeField] private Answer _answerObject;
    [Space]
    [SerializeField] private CartItem[] _answers;
    [Header("Tutorial")]
    [SerializeField] private bool _tutorial;
    [SerializeField] private GameObject _tutorialFinger;

    private readonly List<CartHandler> _spawnedCartHandlers = new List<CartHandler>();
    private GameObject _spawnedTutorialFinger;
    private bool _tutorialCompleted = false;

    private VariantBoard _variantBoard;

    private EventBinding<EventStructs.ComponentEvent<VariantBoard>> _variantBoardComponentEvent;
    private EventBinding<EventStructs.TrainMovementEvent> _trainMovementEvent;
    private EventBinding<EventStructs.WinEvent> _winEvent;

    private void OnEnable()
    {
      _variantBoardComponentEvent = new EventBinding<EventStructs.ComponentEvent<VariantBoard>>(ReceiveVariantBoard);
      _trainMovementEvent = new EventBinding<EventStructs.TrainMovementEvent>(Tutorial);
      _winEvent = new EventBinding<EventStructs.WinEvent>(StopTutorial);
    }

    private void OnDisable()
    {
      _variantBoardComponentEvent.Remove(ReceiveVariantBoard);
      _trainMovementEvent.Remove(Tutorial);
      _winEvent.Remove(StopTutorial);
    }

    private void Start()
    {
      SpawnCarts();
    }

    private void ReceiveVariantBoard(EventStructs.ComponentEvent<VariantBoard> variantBoardComponentEvent)
    {
      _variantBoard = variantBoardComponentEvent.Data;
    }

    private void SpawnCarts()
    {
      Transform lastCartJoint = _cartJoint;
      CartHandler spawnedCart = null;
      Answer spawnedAnswer = null;

      for (int i = 0; i < _answers.Length; i++)
      {
        spawnedCart = Instantiate(_cartPrefab, lastCartJoint.position, Quaternion.Euler(0, 90, 0), transform);
        _spawnedCartHandlers.Add(spawnedCart);

        lastCartJoint = spawnedCart.CartJoint;

        spawnedAnswer =  Instantiate(
          _answerObject, spawnedCart.AnswerPlacePoint.position,
          spawnedCart.AnswerPlacePoint.rotation, spawnedCart.AnswerPlacePoint);

        spawnedAnswer.SetSpriteAndImage(_answers[i].AnswerText);
      }
    }

    private void Tutorial(EventStructs.TrainMovementEvent trainMovementEvent)
    {
      if (_tutorial == false) return;
      if (_tutorialCompleted == true) return;
      if (trainMovementEvent.IsMoving == true) return;

      Vector3 startPosition = new Vector3(
        _spawnedCartHandlers[_spawnedCartHandlers.Count - 1].transform.position.x + 0.75f,
        _spawnedCartHandlers[_spawnedCartHandlers.Count - 1].transform.position.y + 0.75f, 0f);

      _spawnedTutorialFinger = Instantiate(
        _tutorialFinger, startPosition, Quaternion.identity);

      _spawnedTutorialFinger.transform.DOMove(_variantBoard.GetFirstVariantObjectTransform().position, 1.5f)
        .SetLoops(-1)
        .SetEase(Ease.InOutQuad);
    }

    private void StopTutorial(EventStructs.WinEvent winEvent)
    {
      if (_tutorial == false) return;

      DOTween.Kill(_spawnedTutorialFinger.transform);
      Destroy(_spawnedTutorialFinger);

      _tutorialCompleted = true;
    }
  }
}