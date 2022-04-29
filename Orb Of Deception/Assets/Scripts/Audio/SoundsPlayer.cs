using System.Collections.Generic;
using UnityEngine;

namespace OrbOfDeception.Audio
{
    public class SoundsPlayer : MonoBehaviour
    {
        #region Variables

        [System.Serializable]
        private class AudioTrack
        {
            public string name;
            public AudioClip[] clips;
        }

        [SerializeField] private AudioTrack[] audioTracks;

        private AudioSource _audioSource;
        private readonly Dictionary<string, AudioClip[]> _audioTracksDictionary = new Dictionary<string, AudioClip[]>();
        
        #endregion
        
        #region Methods

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            foreach (var audioTrack in audioTracks)
            {
                _audioTracksDictionary.Add(audioTrack.name, audioTrack.clips);
            }
        }

        public void Play(string trackName)
        {
            if (!_audioTracksDictionary.ContainsKey(trackName))
            {
                Debug.Log("Sound not found.");
                return;
            }
            
            var clips = _audioTracksDictionary[trackName];
            
            if (clips == null || clips.Length == 0)
            {
                Debug.Log("Sound not found.");
                return;
            }
            
            var clipToPlay = clips[Random.Range(0, clips.Length - 1)];
            
            _audioSource.PlayOneShot(clipToPlay);
        }
        
        #endregion
    }
}
