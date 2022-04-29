using UnityEngine;

namespace OrbOfDeception.Player
{
    public class PlayerTriggerDetector
    {
        public void OnEnter(Collider2D colliderHit)
        {
            var objectHit = colliderHit.gameObject;
            var playerHittable = objectHit.GetComponent<IPlayerHittable>();

            playerHittable?.OnPlayerHitEnter();
        }
        
        public void OnExit(Collider2D colliderHit)
        {
            var objectHit = colliderHit.gameObject;
            var playerHittable = objectHit.GetComponent<IPlayerHittable>();

            playerHittable?.OnPlayerHitExit();
        }
    }
}