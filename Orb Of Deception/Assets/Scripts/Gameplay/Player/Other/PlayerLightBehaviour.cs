using UnityEngine;


namespace OrbOfDeception.Player
{
    public class PlayerLightBehaviour : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float lightIntensity = 5;
        [SerializeField] private float lightIntensityVariation = 1;
        [SerializeField] private float lightIntensityVariationVelocity = 1;

        private UnityEngine.Rendering.Universal.Light2D _playerPointLight;
        #endregion
        
        #region Methods
        private void Awake()
        {
            _playerPointLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
        }

        private void Update()
        {
            _playerPointLight.intensity = lightIntensity +
                                         Mathf.Sin(Time.time * lightIntensityVariationVelocity) *
                                         lightIntensityVariation;
        }
        #endregion
        
    }
}
