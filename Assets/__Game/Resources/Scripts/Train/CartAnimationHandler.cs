using __Game.Resources.Scripts.EventBus;
using DG.Tweening;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts.Train
{
  public class CartAnimationHandler : MonoBehaviour
  {
    [Header("Wheels")]
    [SerializeField] private float _wheelsRotationSpeed;
    [SerializeField] private Vector3 _wheelsRotationDirection;
    [Space]
    [SerializeField] private GameObject[] _wheels;

    private EventBinding<EventStructs.TrainMovementEvent> _trainMovementEvent;

    private void OnEnable()
    {
      _trainMovementEvent = new EventBinding<EventStructs.TrainMovementEvent>(RotateWheels);
    }

    private void OnDisable()
    {
      _trainMovementEvent.Remove(RotateWheels);
    }

    private void RotateWheels(EventStructs.TrainMovementEvent trainMovementEvent)
    {
      if (trainMovementEvent.IsMoving == true)
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