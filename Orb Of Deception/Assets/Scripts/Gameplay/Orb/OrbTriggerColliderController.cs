using UnityEngine;

namespace OrbOfDeception.Orb
{
    public class OrbTriggerColliderController : MonoBehaviour
    {
        private OrbController _orbController;

        private void Awake()
        {
            _orbController = GetComponentInParent<OrbController>();
        }

        // Mirar si hay alguna manera de poder dejar esta función en el script principal.
        private void OnTriggerEnter2D(Collider2D other)
        {
            _orbController.OnTriggerObjectInit(other);
        }
    }
}
