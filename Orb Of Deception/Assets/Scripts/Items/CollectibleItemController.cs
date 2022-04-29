using System.Collections;
using OrbOfDeception.Rooms;
using OrbOfDeception.UI;
using UnityEngine;

namespace OrbOfDeception.Items
{
    public abstract class CollectibleItemController : CollectibleController
    {
        [SerializeField] protected Item item;

        protected override void OnCollect()
        {
            StartCoroutine(OnCollectCoroutine());
        }

        private IEnumerator OnCollectCoroutine()
        {
            ItemObtainedMenu.Instance.ShowItem(item);

            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            foreach (var particle in onGetParticles.GetParticles())
            {
                var main = particle.main;
                main.useUnscaledTime = true;
            }
            var idleMain = idleParticles.main;
            idleMain.useUnscaledTime = true;
            
            yield return new WaitForSeconds(0.05f);
            
            AfterExitingItemObtainedMenu();
        }

        protected virtual void AfterExitingItemObtainedMenu()
        {
            
        }
    }
}