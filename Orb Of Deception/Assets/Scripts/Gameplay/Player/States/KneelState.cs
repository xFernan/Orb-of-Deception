using OrbOfDeception.Patterns;

namespace OrbOfDeception.Player
{
    public class KneelState : State
    {
        public KneelState()
        {
        }

        public override void Update(float deltaTime)
        {
            base.Update(deltaTime);
            
            GameManager.Player.isControlled = false;
        }
    }
}