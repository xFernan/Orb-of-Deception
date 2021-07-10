using System.Collections;
using NaughtyAttributes;
using OrbOfDeception.Gameplay.Player;
using UnityEngine;

namespace OrbOfDeception
{
    public class EssenceOfPunishmentController : MonoBehaviour
    {
        #region Variables

        private enum EoP_State
        {
            Detach,
            FollowPlayer,
            Acquire
        }

        private EoP_State _state;

        [Header("Essence of Punishment Variables")]
        [SerializeField] [MinMaxSlider(0, 25)] private Vector2 initialVelocity;
        [SerializeField] private float decelerationFactor;
        [SerializeField] private float attractionForce;
        [SerializeField] private float followPlayerDelay = 0.7f;
        
        [Header("Particles")]
        [SerializeField] private GameObject onAcquireParticles;
        [SerializeField] private ParticleSystem particlesTrail;
        [SerializeField] private ParticleSystem particlesIdle;

        private Animator _animator;
        private Rigidbody2D _rigidbody;
        
        private static readonly int Acquire = Animator.StringToHash("Acquire");
        #endregion
        
        #region Methods
        #region MonoBehaviour Methods
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void Start()
        {
            _state = EoP_State.Detach;
            OnEnterDetachState();
        }
        
        private void FixedUpdate()
        {
            switch (_state)
            {
                case EoP_State.Detach:
                    _rigidbody.velocity *= decelerationFactor;
                    if (_rigidbody.velocity.magnitude <= 1)
                    {
                        ChangeToFollowPlayerState();
                    }
                    break;
                case EoP_State.FollowPlayer:
                    var playerPosition = PlayerGroupController.Instance.playerController.transform.position;
                    var velocityValue = _rigidbody.velocity.magnitude;
                    var direction = (playerPosition - transform.position).normalized;
                    _rigidbody.velocity = direction * velocityValue;
                    _rigidbody.AddForce(direction * attractionForce);
                    break;
            }
        }

        private void ChangeToFollowPlayerState()
        {
            StartCoroutine(ChangeToFollowPlayerStateCoroutine());
        }

        private IEnumerator ChangeToFollowPlayerStateCoroutine()
        {
            yield return new WaitForSeconds(followPlayerDelay);
            _state = EoP_State.FollowPlayer;
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_state != EoP_State.FollowPlayer || !other.CompareTag("Player" /*Provisional*/)) return;
            
            _state = EoP_State.Acquire;
            OnEnterAcquireState();
        }
        #endregion
        
        #region State Methods
        private void OnEnterDetachState()
        {
            Vector2 detachDirection;
            do
                detachDirection = Random.insideUnitCircle.normalized;
            while (detachDirection == Vector2.zero);
            _rigidbody.velocity = detachDirection * initialVelocity;
        }
        
        private void OnEnterAcquireState()
        {
            _animator.SetTrigger(Acquire);
            _rigidbody.velocity = Vector2.zero; // ¿Hacer que desacelere?
            // Provisional (hacer con PoolObject):
            Instantiate(onAcquireParticles, transform.position, Quaternion.identity);
            particlesIdle.Stop();
            particlesTrail.Stop();
        }
        #endregion
        #endregion
    }
}
