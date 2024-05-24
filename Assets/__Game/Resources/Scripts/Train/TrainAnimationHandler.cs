using __Game.Resources.Scripts.EventBus;
using DG.Tweening;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts.Train
{
  public class TrainAnimationHandler : MonoBehaviour
  {
    [Header("Train")]
    [SerializeField] private float _trainMovementSpeed;
    [SerializeField] private Vector3 _trainFirstPoint;
    [SerializeField] private Vector3 _trainSecondPoint;
    [Header("Wheels")]
    [SerializeField] private float _wheelsRotationSpeed;
    [SerializeField] private Vector3 _wheelsRotationDirection;
    [Space]
    [SerializeField] private GameObject[] _wheels;

    private EventBinding<EventStructs.WinEvent> _winEvent;

    private void OnEnable()
    {
      _winEvent = new EventBinding<EventStructs.WinEvent>(MoveTrainToSecondPoin);
    }

    private void OnDisable()
    {
      _winEvent.Remove(MoveTrainToSecondPoin);
    }

    private void Start()
    {
      MoveTrainToFirstPoint();
      RotateWheels(true);
    }

    private void MoveTrainToFirstPoint()
    {
      EventBus<EventStructs.TrainMovementEvent>.Raise(new EventStructs.TrainMovementEvent { IsMoving = true });

      transform.DOLocalMove(_trainFirstPoint, _trainMovementSpeed)
        .SetSpeedBased(true)
        .OnComplete(() =>
        {
          RotateWheels(false);

          EventBus<EventStructs.TrainMovementEvent>.Raise(new EventStructs.TrainMovementEvent { IsMoving = false });
        });
    }

    private void MoveTrainToSecondPoin(EventStructs.WinEvent winEvent)
    {
      EventBus<EventStructs.TrainMovementEvent>.Raise(new EventStructs.TrainMovementEvent { IsMoving = true });

      transform.DOLocalMove(_trainSecondPoint, _trainMovementSpeed * 1.5f)
        .SetSpeedBased(true)
        .OnComplete(() =>
        {
          RotateWheels(false);

          EventBus<EventStructs.TrainMovementEvent>.Raise(new EventStructs.TrainMovementEvent { IsMoving = false });
        });
    }

    private void RotateWheels(bool rotate)
    {
      if (rotate == true)
      {
        foreach (var wheel in _wheels)
        {
          wheel.transform.DOLocalRotate(_wheelsRotationDirection, _wheelsRotationSpeed, RotateMode.FastBeyond360)
            .SetLoops(-1)
            .SetSpeedBased(true);
        }
      }
      else
      {
        foreach (var wheel in _wheels)
        {
          DOTween.Kill(wheel.transform);
        }
      }
    }
  }
}