using System;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts.Train
{
  [Serializable]
  public class CartItem
  {
    [field: SerializeField] public string AnswerText { get; private set; }
    [field: Space]
    [field: SerializeField] public AudioClip AnswerAudioClip { get; private set; }
  }
}