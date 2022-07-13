using OrbOfDeception.Audio;
using UnityEngine;

namespace OrbOfDeception.UI.Main_Menu
{
    public class MainMenuTitleSoundPlayer : MonoBehaviour
    {
        [SerializeField] private SoundsPlayer soundsPlayer;

        private void PlaySlide1()
        {
            soundsPlayer.Play("Slide1");
        }
        
        private void PlaySlide2()
        {
            soundsPlayer.Play("Slide2");
        }
    }
}
