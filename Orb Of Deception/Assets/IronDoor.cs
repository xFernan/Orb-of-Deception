using OrbOfDeception.Core;
using OrbOfDeception.Gameplay.Player;
using UnityEngine;

namespace OrbOfDeception
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
    }
}
