using System.Collections;
using OrbOfDeception.Audio;
using UnityEngine;

namespace OrbOfDeception.Enemy.Boss1
{
    public class ChargeState : EnemyState
    {
        #region Variables
        
        private readonly Boss1Parameters _parameters;
        private readonly Transform _transform;
        private readonly Rigidbody2D _rigidbody;
        private readonly MonoBehaviour _monoBehaviour;
        private readonly ParticleSystem _chargeParticles;
        private readonly SoundsPlayer _soundsPlayer;
        private readonly GameObject _ringParticlesPrefab;

        private Coroutine _chargeCoroutine;
        
        private bool _isCharging;
        private Vector3 _chargeTargetPosition;
        private Vector2 _chargeDirection;
        private int _chargeRingParticlesSpawned;
        private float _chargeInitY;
        
        private bool _isReturning;
        private Vector3 _returningPosition;
        private Vector2 _returningDirection;
        
        #endregion
            
        #region Methods
            
        #endregion
        
        public ChargeState(Boss1Controller enemy) : base(enemy)
        {
            _parameters = enemy.Parameters;
            _transform = enemy.transform;
            _rigidbody = enemy.Rigidbody;
            _monoBehaviour = enemy;
            _chargeParticles = enemy.chargeParticles;
            _soundsPlayer = enemy.soundsPlayer;
            _ringParticlesPrefab = enemy.chargeRingParticles;
        }

        public override void Enter()
        {
            base.Enter();

            _isCharging = false;
            _isReturning = false;

            _chargeRingParticlesSpawned = 0;
            _chargeInitY = _transform.position.y;
            
            _chargeCoroutine = _monoBehaviour.StartCoroutine(ChargeCoroutine());
        }

        public override void Exit()
        {
            base.Exit();
            
            _monoBehaviour.StopCoroutine(_chargeCoroutine);
        }

        public override void FixedUpdate(float deltaTime)
        {
            base.FixedUpdate(deltaTime);

            if (_isCharging)
            {
                if (IsCloseToPosition(_chargeTargetPosition))
                {
                    _rigidbody.velocity = Vector2.zero;
                    
                    _isCharging = false;
                    _isReturning = true;
                    
                    _chargeParticles.Stop();

                    _returningDirection = (_returningPosition - _transform.position).normalized;
                }
                else
                {
                    var force = _chargeDirection * (_parameters.Stats.chargeVelocity);
                    _rigidbody.AddForce(force);

                    if (_chargeRingParticlesSpawned >= 3)
                        return;

                    var nextRingSpawnPosition = Mathf.Lerp(_chargeInitY, _chargeTargetPosition.y,
                        ((float)_chargeRingParticlesSpawned + 1) / 4);
                    
                    if (_transform.position.y <= nextRingSpawnPosition)
                    {
                        SpawnRingParticles();
                        _chargeRingParticlesSpawned++;
                    }
                }
            }
            else if (_isReturning)
            {
                if (IsCloseToPosition(_returningPosition))
                {
                    _rigidbody.velocity = Vector2.zero;
                    enemy.SetState(Boss1Controller.TeleportingState);
                }
                else
                {
                    var force = _returningDirection * (_parameters.Stats.chargeReturnVelocity);
                    _rigidbody.AddForce(force);
                }
            }
        }

        private void SpawnRingParticles()
        {
            var ringParticlesObject = (GameObject) Object.Instantiate(_ringParticlesPrefab, _transform.position, Quaternion.LookRotation(_rigidbody.velocity.normalized)); // Cambiar por Object Pool.
            /*var ringParticlesMain = ringParticlesObject.GetComponent<ParticleSystem>().main;
            ringParticlesMain.startSpeed = speed;
            ringParticlesMain.startColor = _orbController.CurrentParticlesColor;*/
        }
        
        private IEnumerator ChargeCoroutine()
        {
            yield return new WaitForSeconds(_parameters.Stats.chargeDelay);
            
            InitCharge();
        }

        private void InitCharge()
        {
            _returningPosition = _transform.position;
            _chargeTargetPosition = new Vector3(GameManager.Player.transform.position.x, _parameters.Stats.chargeTargetY, 0);
            _chargeDirection = (_chargeTargetPosition - _returningPosition).normalized;
            _chargeParticles.Play();
            _soundsPlayer.Play("Charge");
            
            _isCharging = true;
        }

        private bool IsCloseToPosition(Vector3 position)
        {
            return Vector3.Distance(_transform.position, position) <= 0.5f;
        }
        
    }
}