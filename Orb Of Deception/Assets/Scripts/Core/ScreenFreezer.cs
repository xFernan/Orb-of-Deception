using System.Collections;
using UnityEngine;

namespace OrbOfDeception.Core
{
    public class ScreenFreezer : MonoBehaviour
    {
        
        private static ScreenFreezer _instance;
        public static ScreenFreezer Instance
        {
            get
            {
                if (_instance == null)
                {
                    var newScreenFreezer = Instantiate(new GameObject());
                    _instance = newScreenFreezer.AddComponent<ScreenFreezer>();
                }
                
                return _instance;
            }
        }
        
        private bool _isFrozen = false;

        public void Freeze(float freezeDuration)
        {
            if (_isFrozen)
                return;
            
            StartCoroutine(DoFreeze(freezeDuration));
        }

        private IEnumerator DoFreeze(float freezeDuration)
        {
            _isFrozen = true;
            var originalTimeScale = Time.timeScale;
            Time.timeScale = 0;

            yield return new WaitForSecondsRealtime(freezeDuration);

            Time.timeScale = originalTimeScale;
            _isFrozen = false;
        }
        
    }
}
