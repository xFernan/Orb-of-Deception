using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Door
{
    public class IronDoor : Door
    {
        [SerializeField] private MultipleParticlesController closeParticles;

        protected override void OnClosingEnd()
        {
            base.OnClosingEnd();
            
            GameManager.Camera.Shake(0.3f); // Provisional.
            closeParticles.Play();
        }

        protected override void OnOpening()
        {
            base.OnOpening();
            
            soundsPlayer.Play("Opening");
        }

        protected override void OnClosing()
        {
            base.OnClosing();
            
            soundsPlayer.Play("Closing");
        }
    }
}
