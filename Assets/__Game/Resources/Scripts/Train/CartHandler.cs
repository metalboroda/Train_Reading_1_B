using UnityEngine;

namespace Assets.__Game.Resources.Scripts.Train
{
  public class CartHandler : MonoBehaviour
  {
    [field: SerializeField] public Transform CartJoint { get; private set; }
    [field: Space]
    [field: SerializeField] public Transform AnswerPlacePoint { get; private set; }
  }
}