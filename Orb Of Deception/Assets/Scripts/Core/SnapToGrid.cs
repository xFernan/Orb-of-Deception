using UnityEngine;

namespace OrbOfDeception.Core
{
    public class SnapToGrid : MonoBehaviour
    {
        [SerializeField] private Transform targetTransformPosition;
        private void LateUpdate()
        {
            var position = targetTransformPosition.position;
            position.x = SnapValueToGrid(position.x);
            position.y = SnapValueToGrid(position.y);
            transform.position = position;
        }

        public static float SnapValueToGrid(float value)
        {
            const float ppu = (float)GameManager.Ppu;
            return Mathf.FloorToInt(value * ppu) / ppu;
        }
        public static Vector3 SnapVector3ToGrid(Vector3 value)
        {
            const float ppu = (float)GameManager.Ppu;
            return new Vector3(
                Mathf.FloorToInt(value.x * ppu) / ppu,
                Mathf.FloorToInt(value.y * ppu) / ppu,
                Mathf.FloorToInt(value.z * ppu) / ppu);
        }
    }
}
