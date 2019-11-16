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

        [SerializeField] protected CircleCollider2D _collider2D;
        [SerializeField] protected float duration = 2f;
        [SerializeField] protected SpriteRenderer animation;
        [SerializeField] protected float endRadius = 1.5f;
        [SerializeField] protected float endAlpha = 0.5f;
        [SerializeField] protected float endAnimationAlpha = 0;
        [SerializeField] protected float tweenDuration = 1.5f;
        
        private Sequence _sequence;

        private void Awake()
        {
            _sequence = DOTween.Sequence();
            _collider2D.radius = endRadius;
            TweenAnimation();
        }

        private void TweenAnimation()
        {
            animation.gameObject.SetActive(true);
            animation.DOFade(endAlpha, tweenDuration);
        }

        public override void OnPlaced()
        {
            animation.DOFade(1, tweenDuration);
            Invoke(nameof(DisablePoint), duration);
            foreach (var col in Physics2D.OverlapCircleAll(transform.position, _collider2D.radius))
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
        }

        private void DisablePoint()
        {
            DestroyImmediate(gameObject);
        }
    }
}
