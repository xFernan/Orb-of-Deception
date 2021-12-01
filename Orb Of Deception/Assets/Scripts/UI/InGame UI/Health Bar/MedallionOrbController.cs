using OrbOfDeception.Core;
using OrbOfDeception.Gameplay.Player;
using UnityEngine;
using UnityEngine.UI;

namespace OrbOfDeception.UI.Health_Bar
{
    public class MedallionOrbController : MonoBehaviour
    {
        [SerializeField] private Image blackOrbImage;
        [SerializeField] private float opacityChangeVelocity;

        private float _currentBlackOrbOpacity;

        private void Start()
        {
            SetInitialColor();
        }
        
        private void SetInitialColor()
        {
            _currentBlackOrbOpacity = GameManager.Orb.GetColor() == GameEntity.EntityColor.Black ? 1 : 0;
        }

        private void Update()
        {
            var orbIsBlack = GameManager.Orb.GetColor() == GameEntity.EntityColor.Black;
            
            switch (orbIsBlack)
            {
                case true when _currentBlackOrbOpacity < 1:
                    _currentBlackOrbOpacity += opacityChangeVelocity * Time.deltaTime;
                    break;
                case false when _currentBlackOrbOpacity > 0:
                    _currentBlackOrbOpacity -= opacityChangeVelocity * Time.deltaTime;
                    break;
            }
            
            _currentBlackOrbOpacity = Mathf.Clamp(_currentBlackOrbOpacity, 0, 1);

            var newColor = blackOrbImage.color;
            newColor.a = _currentBlackOrbOpacity;
            blackOrbImage.color = newColor;
        }
    }
}
