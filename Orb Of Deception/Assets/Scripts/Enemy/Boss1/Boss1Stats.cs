using UnityEngine;

namespace OrbOfDeception.Enemy.Boss1
{
    [CreateAssetMenu(fileName = "BossStats", menuName = "BossStats/Boss1")]
    public class Boss1Stats : EnemyStats
    {
        
        [Space]
        
        public float timeBetweenMaskColorChange = 5;
        public float spellStateProbability = 0.5f;
        public float chargeStateProbability = 0.5f;
        
        [Space]
        
        public float timeBetweenTeleports = 0.6f;
        public float timeBetweenGroupOfTeleports = 0.9f;
        public int maxTeleportsPerRound = 3;
        public float secondSetOfTeleportsProbability = 0.5f;

        [Space]
        
        public float castSpellDelay = 0.2f;
        public float exitStateDelay = 1.5f;
        public float charmAmount = 7;

        [Space]
        
        public float chargeDelay = 1;
        public float chargeVelocity = 30;
        public float chargeReturnVelocity = 10;
        public float chargeTargetY = 20.5625f;
    }
}