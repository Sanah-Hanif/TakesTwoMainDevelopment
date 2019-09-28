using System;
using System.Collections.Generic;
using System.Linq;
using Interaction.Level_Elements;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interaction.player
{
    public class Block : PlayerCreationInteraction
    {
        private readonly Dictionary<int, PressurePad> _pads = new Dictionary<int, PressurePad>();

        private Collider2D _collider2D;
        private Rigidbody2D _rb;
        private Bounds _bounds;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<Collider2D>();
            _bounds = _collider2D.bounds;
            ReCreated();
        }

        public override void ReCreated()
        {
            gameObject.layer = LayerMask.NameToLayer("CreationNoneCollision");
            _rb.gravityScale = 0;
            _rb.constraints = RigidbodyConstraints2D.FreezeAll;
            ClearPads();
        }

        public override void OnPlaced()
        {
            _rb.gravityScale = 1;
            _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            _rb.velocity = Vector2.zero;
            gameObject.layer = LayerMask.NameToLayer("Creation");
            var cols = Physics2D.OverlapBoxAll(transform.position, _bounds.size, 0);
        
            foreach (var col in cols.Where(col => col.GetComponent<PressurePad>()))
            {
                AddPad(col.GetComponent<PressurePad>());
            }

            foreach (var col in cols.Where(col => col.GetComponent<Airblast>()))
            {
                col.GetComponent<Airblast>().AddObject(_rb);
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
