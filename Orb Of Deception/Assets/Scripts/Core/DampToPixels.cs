using UnityEngine;

namespace OrbOfDeception.Core
{
    public class DampToPixels : MonoBehaviour
    {
        void Update()
        {
            transform.localPosition = Vector3.zero;
            var position = transform.position;
            position.x = Mathf.RoundToInt(position.x * 16) / 16.0f;
            position.y = Mathf.RoundToInt(position.y * 16) / 16.0f;
            transform.position = position;
        }
    }
}
