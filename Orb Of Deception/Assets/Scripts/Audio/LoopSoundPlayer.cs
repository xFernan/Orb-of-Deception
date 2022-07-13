using DG.Tweening;
using OrbOfDeception.Rooms;
using UnityEngine;

namespace OrbOfDeception.Audio
{
    public class LoopSoundPlayer : MonoBehaviour
    {
        [SerializeField] private bool playOnStart;
        public bool stopOnSceneExit = true;
        private AudioSource _audioSource;
        private float _soundVolume = 0;
        
        private const float SoundFadeDuration = 0.75f;
        private Tween _tween;
        
        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            if (playOnStart) Play();
        }

        private void Update()
        {
            _audioSource.volume = _soundVolume;
            _audioSource.pitch = Time.timeScale;
        }

        public void Play()
        {
            _tween.Kill();
            _tween = DOTween.To(() => _soundVolume, x => _soundVolume = x, 1, SoundFadeDuration * (1 - _soundVolume));
        }

        public void Stop()
        {
            _tween.Kill();
            _tween = DOTween.To(() => _soundVolume, x => _soundVolume = x, 0, SoundFadeDuration * _soundVolume);
        }
    }
}
