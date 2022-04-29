using OrbOfDeception.Core;

namespace OrbOfDeception.Orb
{
    public interface IOrbHittable
    {
        public void OnOrbHitEnter(GameEntity.EntityColor damageColor = GameEntity.EntityColor.Other, int damage = 0);
    }
}