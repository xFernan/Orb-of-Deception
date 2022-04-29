using System.Collections;
using DG.Tweening;
using OrbOfDeception.Core;
using OrbOfDeception.Core.Scenes;
using UnityEngine;

namespace OrbOfDeception.Cinematic
{
    public class CinematicController : MonoBehaviour
    {
        [System.Serializable]
        private class TextToShow
        {
            public HideableElementAnimated[] texts;
            public float timeToShow;

            public TextToShow(HideableElementAnimated[] texts, float timeToShow)
            {
                this.texts = texts;
                this.timeToShow = timeToShow;
            }
        }

        [SerializeField] private TextToShow[] texts;
        [SerializeField] private Transform cameraTransform;
        [SerializeField] private CinematicWhiteFlash cinematicWhiteFlash;
        [SerializeField] private Animator cinematicBackgroundLightAnimator;
        
        private static readonly int Appear = Animator.StringToHash("Appear");

        private void Start()
        {
            StartCoroutine(CinematicCoroutine());
        }

        private IEnumerator CinematicCoroutine()
        {
            cameraTransform.DOMoveY(4.08f, 10).SetEase(Ease.OutSine);

            foreach (var textToShow in texts)
            {
                yield return new WaitForSeconds(textToShow.timeToShow);
                foreach (var text in textToShow.texts)
                {
                    text.Show();
                }
            }

            yield return new WaitForSeconds(2);

            foreach (var textToShow in texts)
            {
                foreach (var text in textToShow.texts)
                {
                    text.Hide();
                }
            }

            cinematicBackgroundLightAnimator.SetTrigger(Appear);
            
            yield return new WaitForSeconds(1);
            
            cinematicWhiteFlash.FlashAndMakePlayerVisible();
            
            yield return new WaitForSeconds(2);
            
            LevelChanger.Instance.FadeToScene("ConfigurationScene");
        }
    }
}
