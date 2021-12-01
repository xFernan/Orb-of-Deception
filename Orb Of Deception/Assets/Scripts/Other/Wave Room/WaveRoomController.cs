using System.Collections;
using System.Linq;
using OrbOfDeception.CameraBehaviours;
using OrbOfDeception.Core;
using OrbOfDeception.Gameplay.Player;
using OrbOfDeception.Rooms;
using UnityEngine;

namespace OrbOfDeception.Wave_Room
{
    public class WaveRoomController : MonoBehaviour
    {
        #region Variables
        
        [SerializeField] private ColliderEventTrigger eventTrigger;
        [SerializeField] private IronDoor[] doors;
        [SerializeField] private GameObject[] waves;
        
        [Space]
        
        [SerializeField] private float timeToSpawnEnemies = 2;
        [SerializeField] private float timeBetweenWaves = 1.5f;
        [SerializeField] private float timeUntilFinishingWaveRoom = 1;

        private bool _hasStarted;
        private CameraLimits _waveRoomCameraLimits;
        private int _numWaves;
        private int _currentWave;
        private int _currentWaveEnemiesRemaining;

        #endregion
        
        #region Methods
        
        #region Monobehaviour Methods

        private void Awake()
        {
            _waveRoomCameraLimits = GetComponentInChildren<CameraLimits>();
        }

        private void Start()
        {
            eventTrigger.onTrigger += StartWaveRoom;
            _numWaves = waves.Length;
            _currentWave = 0;
        }
        
        #endregion
        
        #region Wave Room Methods
        
        private void StartWaveRoom()
        {
            if (_hasStarted) return;
            
            _hasStarted = true;
            StartCoroutine(StartWaveRoomCoroutine());
        }

        private IEnumerator StartWaveRoomCoroutine()
        {
            CloseDoors();
            CenterCamera();
            yield return new WaitForSeconds(timeToSpawnEnemies);
            SpawnWave(0);
        }
        
        private void EndWaveRoom()
        {
            StartCoroutine(EndWaveRoomCoroutine());
        }

        private IEnumerator EndWaveRoomCoroutine()
        {
            yield return new WaitForSeconds(timeUntilFinishingWaveRoom);
            OpenDoors();
            DecenterCamera();
        }
        
        #endregion
        
        #region Wave Methods
        
        private void SpawnWave(int waveIndex)
        {
            var waveToSpawn = waves[waveIndex];
            var enemySpawners = waveToSpawn.GetComponentsInChildren<EnemySpawner>().ToList();
            _currentWaveEnemiesRemaining = enemySpawners.Count;

            foreach (var enemySpawner in enemySpawners)
            { 
                enemySpawner.Spawn();
                enemySpawner.enemyController.onDie += OnWaveEnemiesDie;
            }
        }
        
        private void SpawnNextWave()
        {
            _currentWave++;
            if (_currentWave >= _numWaves)
            {
                EndWaveRoom();
            }
            else
            {
                StartCoroutine(SpawnInBetweenWave(_currentWave));
            }
        }

        private IEnumerator SpawnInBetweenWave(int waveIndex)
        {
            yield return new WaitForSeconds(timeBetweenWaves);
            SpawnWave(waveIndex);
        }
        
        private void OnWaveEnemiesDie()
        {
            _currentWaveEnemiesRemaining--;
            if (_currentWaveEnemiesRemaining == 0)
            {
                SpawnNextWave();
            }
        }
        
        #endregion

        #region Camera Methods
        
        private void CenterCamera()
        {
            GameManager.Camera.cameraLimits = _waveRoomCameraLimits;
        }

        private void DecenterCamera()
        {
            GameManager.Camera.cameraLimits = RoomManager.Instance.cameraLimits;
        }
        
        #endregion

        #region Doors Methods
        
        private void CloseDoors()
        {
            foreach (var door in doors)
            {
                door.Close();
            }
        }

        private void OpenDoors()
        {
            foreach (var door in doors)
            {
                door.Open();
            }
        }

        #endregion
        
        #endregion
    }
}