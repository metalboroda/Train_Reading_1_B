using System;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts.Variant
{
  [Serializable]
  public class VariantItem
  {
    [field: SerializeField] public string VariantText { get; private set; }
    [field: SerializeField] public bool ShowText = true;
  }
}