using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace OrbOfDeception.Audio
{
    public class MusicManager : MonoBehaviour
    {
        private static MusicManager _instance;
        public static MusicManager Instance 
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<MusicManager>();
                    if (_instance == null)
                    {
                        var newMusicManager = Instantiate((GameObject) Resources.Load("MusicManager"));
                        _instance = newMusicManager.GetComponent<MusicManager>();
                    }
                }
                
                return _instance;
            }
        }
        
        [Serializable]
        private class MusicTrack
        {
            public string name;
            public AudioClip clip;
        }

        [SerializeField] private MusicTrack[] musicTracks;

        private AudioSource _audioSource;
        private readonly Dictionary<string, AudioClip> _musicTracksDictionary = new Dictionary<string, AudioClip>();

        private float _musicFadeVolume;
        private float _musicReducedVolume = 1;
        private bool _isBeingPlayed;
        private Tween _fadeTween;
        private Tween _reducedTween;

        private const float TweenDuration = 0.75f;
        private const float ReducedVolumeValue = 0.2f;
        
        private void Awake()
        {
            if (_instance != null)
                Destroy(gameObject);
            
            _audioSource = GetComponent<AudioSource>();
            
            foreach (var musicTrack in musicTracks)
            {
                _musicTracksDictionary.Add(musicTrack.name, musicTrack.clip);
            }
            
            DontDestroyOnLoad(this);
        }
        
        public void PlayMusic(string trackName, bool isFading = true)
        {
            if (!_musicTracksDictionary.ContainsKey(trackName))
            {
                Debug.Log("Music not found.");
                return;
            }
            
            var clip = _musicTracksDictionary[trackName];
            
            if (clip == null)
            {
                Debug.Log("Music not found.");
                return;
            }

            _isBeingPlayed = true;
            _audioSource.Stop();
            _audioSource.clip = clip;
            _audioSource.Play();

            _fadeTween?.Kill();
            if (isFading)
                _fadeTween = DOTween.To(() => _musicFadeVolume, x => _musicFadeVolume = x,
                    1, TweenDuration * (1 - _musicFadeVolume));
            else
                _musicFadeVolume = 1;
            
        }

        public void PlayMusic(string trackName, float delay)
        {
            StartCoroutine(PlayMusicCoroutine(trackName, delay));
        }

        private IEnumerator PlayMusicCoroutine(string trackName, float delay)
        {
            yield return new WaitForSeconds(delay);
            
            PlayMusic(trackName);
        }

        public void StopMusic()
        {
            if (!_isBeingPlayed) return;

            _isBeingPlayed = false;
            
            _fadeTween?.Kill();
            _fadeTween = DOTween.To(() => _musicFadeVolume, x => _musicFadeVolume = x,
                0, TweenDuration * _musicFadeVolume);
        }

        public void StopMusic(float delay)
        {
            StartCoroutine(StopMusicCoroutine(delay));
        }

        private IEnumerator StopMusicCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            
            StopMusic();
        }

        public void ReduceVolume()
        {
            _reducedTween?.Kill();
            _reducedTween = DOTween.To(() => _musicReducedVolume, x => _musicReducedVolume = x,
                ReducedVolumeValue, (TweenDuration * (1 - ReducedVolumeValue)) * _musicReducedVolume).SetUpdate(true);
        }
        
        public void RevertReducedVolume()
        {
            _reducedTween?.Kill();
            _reducedTween = DOTween.To(() => _musicReducedVolume, x => _musicReducedVolume = x,
                1, (TweenDuration * (1 - ReducedVolumeValue)) * (1 - _musicReducedVolume)).SetUpdate(true);
        }
        
        private void Update()
        {
            _audioSource.volume = _musicFadeVolume * _musicReducedVolume;
        }
    }
}
