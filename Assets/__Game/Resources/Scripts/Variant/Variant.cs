using __Game.Resources.Scripts.EventBus;
using Assets.__Game.Resources.Scripts.Train;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.__Game.Resources.Scripts.Variant
{
  public class Variant : MonoBehaviour
  {
    [SerializeField] private TextMeshProUGUI _variantTextMesh;
    [SerializeField] private GameObject _unknownTextObject;
    [Space]
    [SerializeField] private Color _transparentColor;
    [Header("Effects")]
    [SerializeField] private GameObject _correctParticlesPrefab;
    [SerializeField] private GameObject _incorrectParticlesPrefab;

    public bool ShowSprite { get; private set; }
    public string VariantText { get; private set; }
    public string ReceivedText { get; private set; }

    public void SetSpriteAndImage(string variantText, bool showSprite)
    {
      _variantTextMesh.text = null;
      VariantText = variantText;
      ShowSprite = showSprite;

      if (showSprite == true)
      {
        _variantTextMesh.text = VariantText;
        _unknownTextObject.SetActive(false);
      }
      else
      {
        _variantTextMesh.color = _transparentColor;
        _unknownTextObject.SetActive(true);
      }
    }

    public void Place(Answer answerToPlace)
    {
      answerToPlace.transform.DOMove(transform.position, 0.1f)
        .OnComplete(() =>
        {
          answerToPlace.transform.SetParent(transform);

          CheckForCorrectAnswer();
        });

      ShowSprite = true;
      ReceivedText = answerToPlace.AnswerText;
      _unknownTextObject.SetActive(false);
    }

    private void CheckForCorrectAnswer()
    {
      if (VariantText == ReceivedText)
      {
        EventBus<EventStructs.CorrectAnswerEvent>.Raise(new EventStructs.CorrectAnswerEvent());

        SpawnParticles(_correctParticlesPrefab);
      }
      else
      {
        EventBus<EventStructs.IncorrectCancelEvent>.Raise(new EventStructs.IncorrectCancelEvent());

        SpawnParticles(_incorrectParticlesPrefab);
      }
    }

    private void SpawnParticles(GameObject particlesPrefab)
    {
      Instantiate(particlesPrefab, transform.position, Quaternion.identity);
    }
  }
}