using OrbOfDeception.Core;
using OrbOfDeception.Enemy;
using UnityEngine;

namespace OrbOfDeception.Wave_Room
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameEntity.EntityColor maskColor;
        [SerializeField] private bool orientationIsRight = true;

        [HideInInspector] public EnemyController enemyController;

        public void Spawn()
        {
            var enemyObject = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
            enemyController = enemyObject.GetComponent<EnemyController>();
            enemyController.OnSpawn(maskColor, orientationIsRight);
            enemyObject.transform.parent = transform;
        }
    }
}
