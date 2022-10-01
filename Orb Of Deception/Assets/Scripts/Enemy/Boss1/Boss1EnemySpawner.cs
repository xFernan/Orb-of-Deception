using System.Collections;
using UnityEngine;

namespace OrbOfDeception.Enemy.Boss1
{
    public class Boss1EnemySpawner : MonoBehaviour
    {
        [SerializeField] private float timeToSpawnFirstEnemy = 8;
        [SerializeField] private float timeToRespawn = 5;

        private EnemySpawner _enemySpawner;
        
        private Coroutine _spawnCoroutine;

        private void Awake()
        {
            _enemySpawner = GetComponent<EnemySpawner>();
        }

        public void StartSpawning()
        {
            _spawnCoroutine = StartCoroutine(FirstSpawnCoroutine());
        }

        private void Respawn()
        {
            _spawnCoroutine = StartCoroutine(RespawnCoroutine());
        }
        
        public void StopSpawning()
        {
            _enemySpawner.EnemyController?.Kill();
            StopCoroutine(_spawnCoroutine);
        }

        private IEnumerator RespawnCoroutine()
        {
            yield return new WaitForSeconds(timeToRespawn);
            SpawnEnemy();
        }

        private IEnumerator FirstSpawnCoroutine()
        {
            yield return new WaitForSeconds(timeToSpawnFirstEnemy);
            SpawnEnemy();
        }

        private void SpawnEnemy()
        {
            _enemySpawner.Spawn();
            _enemySpawner.EnemyController.onDie += Respawn;
        }
        
    }
}
