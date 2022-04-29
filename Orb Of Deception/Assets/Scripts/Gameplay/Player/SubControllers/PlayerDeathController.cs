using System.Collections;
using OrbOfDeception.Core.Scenes;
using OrbOfDeception.Rooms;
using UnityEngine;

namespace OrbOfDeception.Player
{
    public class PlayerDeathController
    {
        public bool isDying = false;
        private static readonly int DieTrigger = Animator.StringToHash("Die");
        private static readonly int Normal = Animator.StringToHash("Normal");

        public void Die()
        {
            isDying = true;
            
            SaveSystem.AddDeathToCounter();
            SaveSystem.ResetEnemies();
            SaveSystem.ResetDecorations();

            var player = GameManager.Player;
            player.isControlled = false;
            
            player.maskParticles.Stop();
            
            player.SpriteAnimator.SetTrigger(DieTrigger);
            player.AnimationController.SetAnimatorUpdateToUnscaledTime();

            player.deathParticlesController.Play();
            
            player.Rigidbody.velocity = Vector2.zero;
            player.Rigidbody.isKinematic = true;
            
            GameManager.Orb.Hide();

            GameManager.Player.StartCoroutine(RespawnCoroutine());

            RoomManager.targetRoomChangerID = -1;

            //PauseController.Instance.Pause();
        }

        public void ConfigRespawn()
        {
            isDying = false;

            var player = GameManager.Player;
            
            player.maskParticles.Play();
            
            player.SpriteAnimator.SetTrigger(Normal);
            player.AnimationController.SetAnimatorUpdateToNormal();
            
            player.Rigidbody.isKinematic = false;
            
            player.SpawnStand();
            
            player.HealthController.RecoverAll();
        }
        
        private IEnumerator RespawnCoroutine()
        {
            yield return new WaitForSeconds(1);
            LevelChanger.Instance.FadeToScene(SaveSystem.GetSpawnSceneName());
        }
    }
}