using System;
using DG.Tweening;
//using DG.Tweening;
using Interaction.Level_Elements;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interaction.player
{
    public class Airpoint : PlayerCreationInteraction
    {

        [SerializeField] private LayerMask mask;
        [SerializeField] private CircleCollider2D _collider2D;
        [SerializeField] private float duration = 2f;
        
        [SerializeField] private SpriteRenderer animation;
        [SerializeField] private SpriteRenderer objectSprite;

        [SerializeField] private float endRadius = 1.5f;
        [SerializeField] private float endAlpha = 0.5f;
        [SerializeField] private float endAnimationAlpha = 0;
        [SerializeField] private float tweenDuration = 1.5f;
        
        private PointEffector2D _point;
        private Sequence _sequence;

        private void Awake()
        {
            _point = GetComponent<PointEffector2D>();
            _point.colliderMask = 0;
            _sequence = DOTween.Sequence();
            TweenAnimation();
        }

        private void TweenAnimation()
        {
            animation.gameObject.SetActive(true);
            //inserting an animation for the air pulse
            _sequence.Append(animation.transform.DOScale(Vector2.one * endRadius, tweenDuration)) 
                .Insert(0, animation.DOFade(endAnimationAlpha, tweenDuration))
                .Insert(0,objectSprite.DOFade(endAlpha, tweenDuration))
                .Insert(tweenDuration, animation.transform.DOScale(Vector2.one, tweenDuration/4))
                .Insert(tweenDuration, objectSprite.DOFade(1f, tweenDuration/4));
            _sequence.Append(animation.transform.DOScale(Vector2.one, tweenDuration/4));
            _sequence.SetLoops(-1);//makes it loop infinitely
        }

        public override void OnPlaced()
        {
            _sequence.SetLoops(1);
            _sequence.Kill(true);
            objectSprite.DOFade(1, 1f);
            animation.gameObject.SetActive(false);
            Invoke(nameof(DisablePoint), duration);
            _point.colliderMask = mask;
            foreach (var col in Physics2D.OverlapCircleAll(transform.position, _collider2D.radius + 1))
            {
                var _switch = col.GetComponent<Switch>();
                if(_switch)
                    _switch.Interact();
            }
        }

        public override void ReCreated()
        {
            TweenAnimation();
            CancelInvoke(nameof(DisablePoint));
            _point.colliderMask = 0;
        }

        private void DisablePoint()
        {
            DestroyImmediate(gameObject);
        }
    }
}
