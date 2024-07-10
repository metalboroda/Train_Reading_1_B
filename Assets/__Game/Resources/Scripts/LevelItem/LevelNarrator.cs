using __Game.Resources.Scripts.EventBus;
using Assets.__Game.Resources.Scripts.Game.States;
using Assets.__Game.Scripts.Tools;
using UnityEngine;

namespace Assets.__Game.Resources.Scripts.LevelItem
{
  public class LevelNarrator : MonoBehaviour
  {
    [Header("Announcer")]
    [SerializeField] private AudioClip _questAudio;
    [SerializeField] private AudioClip _levelWord;
    [SerializeField] private AudioClip[] _winAnnouncerClips;
    [SerializeField] private AudioClip[] _loseAnnouncerClips;
    [SerializeField] private AudioClip[] _stuporAnnouncerClips;

    private AudioSource _audioSource;

    private AudioTool _audioTool;

    private EventBinding<EventStructs.StateChanged> _stateEvent;
    private EventBinding<EventStructs.StuporEvent> _stuporEvent;
    private EventBinding<EventStructs.VariantAudioClickedEvent> _variantAudioClickedEvent;

    private void Awake() {
      _audioSource = GetComponent<AudioSource>();

      _audioTool = new AudioTool(_audioSource);
    }

    private void OnEnable() {
      _stateEvent = new EventBinding<EventStructs.StateChanged>(PlayScreenSound);
      _stuporEvent = new EventBinding<EventStructs.StuporEvent>(PlayStuporSound);
      _variantAudioClickedEvent = new EventBinding<EventStructs.VariantAudioClickedEvent>(VariantAudio);
    }

    private void OnDisable() {
      _stateEvent.Remove(PlayScreenSound);
      _stuporEvent.Remove(PlayStuporSound);
      _variantAudioClickedEvent.Remove(VariantAudio);
    }

    private void Start() {
      _audioSource.PlayOneShot(_questAudio);
    }

    private void PlayScreenSound(EventStructs.StateChanged state) {
      switch (state.State) {
        case GameplayState:
          if (_levelWord != null) {
            _audioSource.Stop();
            _audioSource.PlayOneShot(_levelWord);
          }
          break;
        case GameWinState:
          _audioSource.Stop();
          _audioSource.PlayOneShot(_audioTool.GetRandomCLip(_winAnnouncerClips));
          break;
        case GameLoseState:
          _audioSource.Stop();
          _audioSource.PlayOneShot(_audioTool.GetRandomCLip(_loseAnnouncerClips));
          break;
      }
    }

    private void PlayStuporSound(EventStructs.StuporEvent stuporEvent) {
      _audioSource.Stop();
      _audioSource.PlayOneShot(_audioTool.GetRandomCLip(_stuporAnnouncerClips));
    }

    private void VariantAudio(EventStructs.VariantAudioClickedEvent variantAudioClickedEvent) {
      _audioSource.Stop();
      _audioSource.PlayOneShot(variantAudioClickedEvent.Clip);
    }
  }
}