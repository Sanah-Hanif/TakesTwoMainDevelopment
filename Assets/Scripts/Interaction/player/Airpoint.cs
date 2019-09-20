using System;
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
        
        //[SerializeField] private MeshRenderer animation;

        /*[SerializeField] private float endRadius = 1.5f;
        [SerializeField] private float endAlpha = 0.5f;
        [SerializeField] private float endAnimationAlpha = 0;
        [SerializeField] private float tweenDuration = 1.5f;*/
        
        private PointEffector2D _point;
        //private Sequence _sequence;

        private void Awake()
        {
            _point = GetComponent<PointEffector2D>();
            _point.colliderMask = 0;
            /*_sequence = DOTween.Sequence();
            TweenAnimation();*/
        }

        private void TweenAnimation()
        {
            /*//inserting an animation for the air pulse
            _sequence.Append(animation.transform.DOScale(new Vector3(endRadius, endRadius, 0.01f), tweenDuration)) 
                .Insert(0, animation.sharedMaterial.DOFade(endAnimationAlpha, tweenDuration))
                .Insert(0,GetComponent<MeshRenderer>().sharedMaterial.DOFade(endAlpha, tweenDuration));
            _sequence.Append(animation.transform.DOScale(new Vector3(1, 1, 0.01f), tweenDuration/4));
            _sequence.SetLoops(-1);//makes it loop infinitely*/
        }

        public override void OnPlaced()
        { 
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
            CancelInvoke(nameof(DisablePoint));
            _point.colliderMask = 0;
        }

        private void DisablePoint()
        {
            DestroyImmediate(gameObject);
        }
    }
}
