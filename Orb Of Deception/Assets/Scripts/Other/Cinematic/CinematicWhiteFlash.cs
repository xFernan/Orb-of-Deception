using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace OrbOfDeception.Cinematic
{
    public class CinematicWhiteFlash : MonoBehaviour
    {
        [SerializeField] private float flashDuration = 0.35f;
        [SerializeField] private CinematicPlayer cinematicPlayer;
        
        private Image _image;

        private void Awake()
        {
            _image = GetComponent<Image>();
        }

        public void FlashAndMakePlayerVisible()
        {
            var colorTransparent = _image.color;
            var colorOpaque = _image.color;
            colorOpaque.a = 1;

            DOTween.Sequence()
                .Append(_image.DOColor(colorOpaque, flashDuration))
                .Append(_image.DOColor(colorTransparent, flashDuration));
            
            Invoke(nameof(MakePlayerVisible), flashDuration);
        }

        private void MakePlayerVisible()
        {
            cinematicPlayer.Appear();
        }
    }
}
