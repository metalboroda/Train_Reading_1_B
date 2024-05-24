using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.__Game.Resources.Scripts.Train
{
  public class Answer : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
  {
    [SerializeField] public TextMeshProUGUI _textMesh;

    public string AnswerText { get; private set; }

    private AudioClip _wordAudioCLip;

    private Vector3 _initLocalPosition;
    private Vector3 _offset;
    private bool _placed = false;

    private Camera _mainCamera;
    private AudioSource _audioSource;

    void Awake()
    {
      _mainCamera = Camera.main;
      _audioSource = GetComponent<AudioSource>();

      _initLocalPosition = transform.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.TryGetComponent(out Variant.Variant variant))
      {
        if (variant.ShowSprite == false)
        {
          variant.Place(this);

          _placed = true;
        }
      }
    }

    public void SetSpriteAndImage(string text)
    {
      AnswerText = text;
      _textMesh.text = AnswerText;
    }

    public void SetAudioCLip(AudioClip audioClip)
    {
      _wordAudioCLip = audioClip;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
      _offset = transform.position - _mainCamera.ScreenToWorldPoint(
        new Vector3(eventData.position.x, eventData.position.y, transform.position.z));

      _audioSource.PlayOneShot(_wordAudioCLip);
    }

    public void OnDrag(PointerEventData eventData)
    {
      if (_placed == true) return;

      Vector3 newPosition = new Vector3(eventData.position.x, eventData.position.y, transform.position.z);

      transform.position = _mainCamera.ScreenToWorldPoint(newPosition) + _offset;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
      if (_placed == true) return;

      transform.DOLocalMove(_initLocalPosition, 0.25f);
    }
  }
}