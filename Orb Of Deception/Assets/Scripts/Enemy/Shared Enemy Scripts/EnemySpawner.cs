using System;
using OrbOfDeception.Audio;
using OrbOfDeception.Core;
using UnityEngine;

namespace OrbOfDeception.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameEntity.EntityColor maskColor;
        [SerializeField] private bool orientationIsRight = true;
        [SerializeField] private bool doesEnemyDropEssences = true;

        public EnemyController EnemyController { get; private set; }
        private SoundsPlayer _soundsPlayer;

        private void Awake()
        {
            _soundsPlayer = GetComponentInChildren<SoundsPlayer>();
        }

        public void Spawn()
        {
            var enemyObject = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            EnemyController = enemyObject.GetComponent<EnemyController>();
            EnemyController.OnSpawn(maskColor, orientationIsRight);
            EnemyController.BaseParameters.doesDropEssences = doesEnemyDropEssences;
            enemyObject.transform.parent = transform;
            _soundsPlayer.Play("Spawn");
        }
    }
}
