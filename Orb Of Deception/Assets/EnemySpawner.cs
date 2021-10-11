using OrbOfDeception.Core;
using OrbOfDeception.Enemy;
using UnityEngine;

namespace OrbOfDeception
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
            enemyController.SetSpawnConfig(maskColor);
            enemyController.PlayAppearAnimation(); // Provisional.
            enemyObject.transform.parent = transform;
        }
    }
}
