using System.Linq;
using System.Collections;
using OrbOfDeception.Audio;
using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy.Boss1
{
    public class TeleportingState : EnemyState
    {
        #region Variables

        private readonly Boss1Parameters _parameters;
        private readonly Transform _transform;
        private readonly Animator _animator;
        private readonly MultipleParticlesController _teleportToParticles;
        private readonly ParticleSystem _teleportFromParticles;
        private readonly LightController _lightController;
        private readonly Transform[] _teleportPoints;
        private readonly SoundsPlayer _soundsPlayer;
        private readonly MonoBehaviour _monoBehaviour;

        private Coroutine _teleportCoroutine;
        private Coroutine _teleportCoroutineAux;
        private int _currentTeleportIndex = 0;
        private int _timesTeleported;
        private bool _secondSetOfTeleportsDone;
        
        private static readonly int TeleportTrigger = Animator.StringToHash("Teleport");

        #endregion
            
        #region Methods
        
        public TeleportingState(Boss1Controller enemy) : base(enemy)
        {
            _parameters = enemy.Parameters;
            _transform = enemy.transform;
            _animator = enemy.Animator;
            _teleportToParticles = enemy.teleportToParticles;
            _teleportFromParticles = enemy.teleportFromParticles;
            _lightController = enemy.lightController;
            _teleportPoints = enemy.teleportPoints;
            _soundsPlayer = enemy.soundsPlayer;
            _monoBehaviour = enemy;

            enemy.onTeleport += Teleport;
        }

        public override void Enter()
        {
            base.Enter();

            _timesTeleported = 0;
            _secondSetOfTeleportsDone = false;
            _teleportCoroutine = _monoBehaviour.StartCoroutine(TeleportCoroutine());
        }

        public override void Exit()
        {
            base.Exit();
            
            _monoBehaviour.StopCoroutine(_teleportCoroutine);
            _monoBehaviour.StopCoroutine(_teleportCoroutineAux);
        }

        private IEnumerator TeleportCoroutine()
        {
            if (_timesTeleported >= _parameters.Stats.maxTeleportsPerRound)
            {
                if (!_secondSetOfTeleportsDone && Random.Range(0.0f, 1.0f) <= _parameters.Stats.secondSetOfTeleportsProbability)
                {
                    _timesTeleported = 0;
                    _secondSetOfTeleportsDone = true;
                    yield return new WaitForSeconds(_parameters.Stats.timeBetweenGroupOfTeleports);
                }
                else
                {
                    var randomValue = Random.Range(0.0f, 1.0f);
                    if (randomValue <= _parameters.Stats.spellStateProbability)
                        enemy.SetState(Boss1Controller.SpellState);
                    else
                        enemy.SetState(Boss1Controller.ChargeState);
                    yield break;
                }
            }
            else
                yield return new WaitForSeconds(_parameters.Stats.timeBetweenTeleports);

            _animator.SetTrigger(TeleportTrigger);
            _lightController.Hide(0.1f);
        }

        private void Teleport()
        {
            _teleportCoroutineAux = _monoBehaviour.StartCoroutine(TeleportCoroutineAux());
        }

        private IEnumerator TeleportCoroutineAux()
        {
            _timesTeleported++;
            _teleportFromParticles.Play();
            _lightController.Appear(0.1f);
            yield return 0;
            _transform.position = SelectRandomTeleportPosition();
            _teleportToParticles.Play();
            _soundsPlayer.Play("Teleport");
            
            _teleportCoroutine = _monoBehaviour.StartCoroutine(TeleportCoroutine());
        }
        
        private Vector3 SelectRandomTeleportPosition()
        {
            var teleportsList = _teleportPoints.ToList();
            teleportsList.RemoveAt(_currentTeleportIndex);
            var randomTeleportIndex = Random.Range(0, teleportsList.Count);
            
            if (randomTeleportIndex >= _currentTeleportIndex)
                _currentTeleportIndex = randomTeleportIndex + 1;
            else
                _currentTeleportIndex = randomTeleportIndex;
            
            return teleportsList[randomTeleportIndex].position;
        }
        
        #endregion
    }
}