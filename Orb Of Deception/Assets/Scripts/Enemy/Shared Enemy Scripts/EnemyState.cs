using OrbOfDeception.Patterns;

namespace OrbOfDeception.Enemy
{
    public abstract class EnemyState : State
    {
        #region Variables
        protected readonly EnemyController enemy;
        protected string animatorBoolParameterName;
        #endregion
        
        #region Methods

        protected EnemyState(EnemyController enemy)
        {
            this.enemy = enemy;
        }

        public override void Enter()
        {
            base.Enter();

            if (animatorBoolParameterName == default)
            {
                return;
            }
            
            // Al comienzo del estado, se poner a true el booleano que activará la animación correspondiente.
            enemy.Anim.SetBool(animatorBoolParameterName, true);
            
            // Debido a que puede ocurrir que se vuelva a entrar al mismo estado del que se vino, para evitar que la
            // animación no se reinicie adecuadamente (ya que el Animator de Unity detecta que se encuentra en el mismo
            // frame y que nada ha cambiado), forzamos el reinicio nosotros directamente.
            ResetCurrentState();
        }

        public override void Exit()
        {
            base.Exit();
            
            if (animatorBoolParameterName == default)
            {
                return;
            }
            
            // Desactivamos el booleano en cuestión al finalizar el estado.
            enemy.Anim.SetBool(animatorBoolParameterName, false);
        }
        
        private void ResetCurrentState()
        { 
            // Al no especificar el nombre del estado a reproducir, Unity da por hecho que es el actual.
            enemy.Anim.Play(0,0, 0.0f);
        }
        #endregion
    }
}