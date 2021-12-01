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
        [SerializeField] private int value;
        [SerializeField] [MinMaxSlider(0, 25)] private Vector2 initialVelocity;
        [SerializeField] private float decelerationFactor;
        [SerializeField] private float attractionForce;
        [SerializeField] private float followPlayerDelay = 0.7f;
        [SerializeField] private float maxVelocity = 20;
        
        [Header("Particles")]
        [SerializeField] private ParticleSystem particlesTrail;
        [SerializeField] private ParticleSystem particlesIdle;
        [SerializeField] private TrailRenderer trail;

        private Animator _animator;
        private Rigidbody2D _rigidbody;
        private Collider2D _collider;

        public int Value => value;

        private const float BlendDirectionValue = 0.3f;
        
        private static readonly int Acquire = Animator.StringToHash("Acquire");
        #endregion
        
        #region Methods
        
        #region MonoBehaviour Methods
        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
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
                    var playerPosition = GameManager.Player.transform.position;
                    var velocityValue = _rigidbody.velocity.magnitude;
                    
                    var targetDirection = (playerPosition - transform.position).normalized;
                    var currentDirection = _rigidbody.velocity.normalized;
                    var blendedDirection = Vector2.Lerp(currentDirection, targetDirection, BlendDirectionValue);
                    
                    _rigidbody.velocity = blendedDirection * velocityValue;
                    _rigidbody.AddForce(targetDirection * attractionForce);
                    
                    var velocityMagnitude = _rigidbody.velocity.magnitude;
                    if (velocityMagnitude > maxVelocity)
                    {
                        _rigidbody.velocity *= (maxVelocity / velocityMagnitude);
                    }
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
            if (_state != EoP_State.FollowPlayer || other.GetComponent<PlayerAreaDamage>() == null) return;
            
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
            _rigidbody.velocity = Vector2.zero;
            particlesIdle.Stop();
            particlesTrail.Stop();
            trail.widthCurve = AnimationCurve.Constant(0, 1, 0);
            _collider.enabled = false;
            //PlayerGroupController.Instance.playerController.SpriteAnimator.PlayOnAcquireEssenceOfPunishmentAnimation();
            // PROVISIONAL (convertir en lo anterior):
            var playerSpriteAnimator = GameManager.Player.transform.
                GetComponentInChildren<PlayerMaterialController>().transform.GetComponent<Animator>();
            playerSpriteAnimator.SetTrigger("AcquireEoP");

            GameManager.Player.EssenceOfPunishmentCounter.AcquireEssences(value);
        }
        #endregion
        
        #endregion
    }
}
