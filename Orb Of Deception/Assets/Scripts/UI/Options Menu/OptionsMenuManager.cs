using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace OrbOfDeception.UI.Options_Menu
{
    public class OptionsMenuManager : MonoBehaviour
    {
        [SerializeField] private AudioMixer mixer;
        [SerializeField] private Slider masterSlider;
        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider soundsSlider;

        private const string MIXER_MASTER = "MasterVolume";
        private const string MIXER_MUSIC = "MusicVolume";
        private const string MIXER_SOUNDS = "SoundsVolume";
        
        private void Awake()
        {
            masterSlider.onValueChanged.AddListener(SetMasterVolume);
            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            soundsSlider.onValueChanged.AddListener(SetSoundsVolume);
        }

        public void Start()
        {
            mixer.GetFloat(MIXER_MASTER, out var masterVolumeDefault);
            mixer.GetFloat(MIXER_MUSIC, out var musicVolumeDefault);
            mixer.GetFloat(MIXER_SOUNDS, out var soundsVolumeDefault);
            
            masterSlider.value = Mathf.Pow(10, masterVolumeDefault / 20);
            musicSlider.value = Mathf.Pow(10, musicVolumeDefault / 20);
            soundsSlider.value = Mathf.Pow(10, soundsVolumeDefault / 20);
        }

        private void SetMasterVolume(float value)
        {
            mixer.SetFloat(MIXER_MASTER, Mathf.Log10(value) * 20);
        }
        
        private void SetMusicVolume(float value)
        {
            mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
        }
        
        private void SetSoundsVolume(float value)
        {
            mixer.SetFloat(MIXER_SOUNDS, Mathf.Log10(value) * 20);
        }
    
    }
}
