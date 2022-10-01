using System.Collections;
using System.Linq;
using DG.Tweening;
using OrbOfDeception.Audio;
using OrbOfDeception.Core;
using OrbOfDeception.Core.Scenes;
using OrbOfDeception.UI;
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
        [SerializeField] private LoopSoundPlayer altarLoopSoundPlayer;

        public SoundsPlayer SoundsPlayer { get; private set; }
        
        private static readonly int Appear = Animator.StringToHash("Appear");

        private void Awake()
        {
            SoundsPlayer = GetComponentInChildren<SoundsPlayer>();
        }

        private void Start()
        {
            StartCoroutine(CinematicCoroutine());
            CursorController.Instance.HideCursor();
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

                SoundsPlayer.Play(textToShow == texts.Last() ? "TextAppearingLast" : "TextAppearing");
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
            SoundsPlayer.Play("LightAppearing");
            
            yield return new WaitForSeconds(1);
            
            cinematicWhiteFlash.FlashAndMakePlayerVisible();
            
            yield return new WaitForSeconds(2);
            
            altarLoopSoundPlayer.Stop();
            LevelChanger.Instance.FadeToScene("ConfigurationScene");
        }
    }
}
