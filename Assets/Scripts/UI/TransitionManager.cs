using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TransitionManager : MonoBehaviour
    {
        [SerializeField] private Image transitionImage;
        [SerializeField, Range(0.5f,2f)] private float transitionDuration = 1f;
        [SerializeField, Range(0.5f,1f)] private float transitionEndAlpha = 0.5f;

        private RectTransform _transitionTransform;

        private Sequence _transition;

        private void Start()
        {
            _transition = DOTween.Sequence();
            _transitionTransform = transitionImage.GetComponent<RectTransform>();
        }

        public IEnumerator FadeStart()
        {
            transitionImage.color = new Color(0,0,0,1);
            Tween t = transitionImage.DOFade(0, transitionDuration);
            yield return t.WaitForCompletion();
            _transitionTransform.localPosition = new Vector3(-1921,0,0);
        }

        public IEnumerator FadeIn()
        {
            _transitionTransform.localPosition = new Vector3(-1921,0,0);
            transitionImage.color = new Color(0,0,0,transitionEndAlpha);
            Tween t = _transitionTransform.DOLocalMoveX(0, transitionDuration);
            transitionImage.DOFade(1f, transitionDuration);
            yield return t.WaitForCompletion();
        }

        public IEnumerator FadeOut()
        {
            Tween t = _transitionTransform.DOLocalMoveX(1921, transitionDuration);
            transitionImage.DOFade(transitionEndAlpha, transitionDuration);
            yield return t.WaitForCompletion();
            _transitionTransform.localPosition = new Vector3(-1921,0,0);
            transitionImage.color = new Color(0,0,0,transitionEndAlpha);
        }
    }
}
