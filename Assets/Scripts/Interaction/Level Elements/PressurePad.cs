using System;
using System.Collections.Generic;
using System.Linq;
using Interactions;
using UnityEngine;

namespace Interaction.Level_Elements
{
    public class PressurePad : EnvironmentInteraction
    {

        private readonly Dictionary<int, GameObject> _objectsOnPad = new Dictionary<int, GameObject>();
        
        private bool _triggered = false;
        
        public override void Interact()
        {
            //if(_objectsOnPad.Count > 0)
            //    return;
            base.Interact();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            CheckForDisabled();
            if(_triggered)
                return;
            if ((!other.tag.Equals("Block") || !other.gameObject.layer.Equals(LayerMask.NameToLayer("Creation"))) &&
                !other.gameObject.layer.Equals(LayerMask.NameToLayer("Player"))) return;
            _triggered = true;
            Interact();
            if(!_objectsOnPad.ContainsKey(other.GetInstanceID()))
                _objectsOnPad.Add(other.GetInstanceID(),other.gameObject);
            
        }

        private void CheckForDisabled()
        {
            foreach (var obj in _objectsOnPad.Where(obj => obj.Value == null))
            {
                _objectsOnPad.Remove(obj.Key);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            CheckForDisabled();
            if (!_triggered) return;
            _triggered = false;
            
            _objectsOnPad.Remove(other.GetInstanceID());
            var block = other.GetComponent<Block>();
            if (block != null)
            {
                block.RemovePad(this);
            }
            Interact();
            
        }

        public void RemoveObjectOnPad(int key)
        {
            _objectsOnPad.Remove(key);
            Interact();
        }

        public void AddObjectOnPad(GameObject obj)
        {
            _objectsOnPad.Add(obj.GetInstanceID(), gameObject);
            Interact();
        }
    }
}
