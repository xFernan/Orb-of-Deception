using UnityEngine;

namespace OrbOfDeception.Patterns
{
    public class PoolObjectReturn : MonoBehaviour
    {
        private void OnDisable()
        {
            if (ObjectPool.Instance != null)
            {
                ObjectPool.Instance.ReturnObject(this.gameObject);
            }
        }
    }
}