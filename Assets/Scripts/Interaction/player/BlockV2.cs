using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Interaction.Level_Elements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Interaction.player
{
    public class BlockV2 : PlayerCreationInteraction
    {
        private readonly Dictionary<int, PressurePad> _pads = new Dictionary<int, PressurePad>();

        [SerializeField] protected LayerMask whatCanLandOnBlock;

        private Collider2D _collider2D;
        private Rigidbody2D _rb;
        private Bounds _bounds;
        private PlatformEffector2D _platform;
        private Sequence _sequence;
        private SpriteRenderer[] _spriteRenderer = new SpriteRenderer[2];

        public UnityAction<GameObject> OnRecreated;

        private void Awake()
        {
            _sequence = DOTween.Sequence();
            _spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
            CreateSequence();
            _sequence = DOTween.Sequence();
            _platform = GetComponent<PlatformEffector2D>();
            _rb = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            _bounds = _collider2D.bounds;
            ReCreated();
        }

        private void CreateSequence()
        {
            _sequence.Append(_spriteRenderer[0].DOFade(0, 0.5f));
            _sequence.Append(_spriteRenderer[0].DOFade(0.7f, 0.5f));
            _sequence.SetLoops(-1);
        }

        public override void ReCreated()
        {
            _spriteRenderer[0].gameObject.SetActive(true);
            _spriteRenderer[1].gameObject.SetActive(false);
            _sequence.Play();
            transform.rotation = Quaternion.identity;
            _platform.colliderMask = 0;
            gameObject.layer = LayerMask.NameToLayer("CreationNoneCollision");
            _rb.gravityScale = 0;
            _rb.velocity = Vector2.zero;
            OnRecreated?.Invoke(gameObject);
            ClearPads();
        }

        public override void OnPlaced()
        {
            //_sequence.SetLoops(1);
            _sequence.Kill(true);
            _spriteRenderer[0].DOFade(1f,0.2f);
            DOTween.Kill(_spriteRenderer);
            _spriteRenderer[0].gameObject.SetActive(false);
            _spriteRenderer[1].gameObject.SetActive(true);
            _platform.colliderMask = whatCanLandOnBlock;
            gameObject.layer = LayerMask.NameToLayer("Block");
            _rb.gravityScale = 1;
            _rb.velocity = Vector2.zero;
            var cols = Physics2D.OverlapBoxAll(transform.position, _bounds.size, 0);
        
            foreach (var col in cols.Where(col => col.GetComponent<PressurePad>()))
            {
                AddPad(col.GetComponent<PressurePad>());
            }
        }

        private void AddPad(PressurePad pad)
        {
            if (_pads.ContainsKey(pad.GetInstanceID())) return;
            _pads.Add(pad.GetInstanceID(), pad);
            pad.AddObjectOnPad(gameObject);
        }

        public void RemovePadOnBlock(PressurePad pad)
        {
            _pads.Remove(pad.GetInstanceID());
        }

        public void RemovePad(PressurePad pad)
        {
            _pads.Remove(pad.GetInstanceID());
            pad.RemoveObjectOnPad(gameObject);
        }

        private void ClearPads()
        {
            var itemsToRemove = _pads.ToArray();
            foreach (var item in itemsToRemove)
                RemovePad(item.Value);
        }

        private void OnDestroy()
        {
            ClearPads();
        }
    }
}
