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

        private Bounds _bounds;

        private void Awake()
        {
            _collider2D = GetComponent<Collider2D>();
            _bounds = _collider2D.bounds;
        }

        public override void ReCreated()
        {
            ClearPads();
        }

        public override void OnPlaced()
        {
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
