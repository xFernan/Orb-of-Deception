using System;
using UnityEngine;

namespace OrbOfDeception.Cinematic
{
    public class CinematicPlayer : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer bodySpriteRenderer;
        [SerializeField] private SpriteRenderer maskSpriteRenderer;
        [SerializeField] private SpriteRenderer shadowSpriteRenderer;
        [SerializeField] private ParticleSystem maskParticles;

        private CinematicController _cinematicController;

        private void Awake()
        {
            _cinematicController = FindObjectOfType<CinematicController>();
        }

        private void Start()
        {
            bodySpriteRenderer.enabled = false;
            maskSpriteRenderer.enabled = false;
            shadowSpriteRenderer.enabled = false;
        }

        public void Appear()
        {
            bodySpriteRenderer.enabled = true;
            maskSpriteRenderer.enabled = true;
            shadowSpriteRenderer.enabled = true;
            maskParticles.Play();
            _cinematicController.SoundsPlayer.Play("PlayerAppearing");
        }
    }
}
