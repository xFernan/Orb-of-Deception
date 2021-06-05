namespace OrbOfDeception.Core
{
    public interface IOrbHittable
    {
        public void Hit(GameEntity.EntityColor damageColor = GameEntity.EntityColor.Other, int damage = 0);
    }
}