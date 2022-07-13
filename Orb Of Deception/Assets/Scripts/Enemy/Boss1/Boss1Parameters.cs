using UnityEngine;

namespace OrbOfDeception.Enemy.Boss1
{
    public class Boss1Parameters : EnemyParameters
    {
        public new Boss1Stats Stats => stats as Boss1Stats;
    }
}