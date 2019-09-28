using System;
using System.Collections.Generic;
using System.Linq;
using Interaction.player;
using UnityEngine;

namespace Interaction.Level_Elements
{
    public class PressurePad : EnvironmentInteraction
    {

        private readonly Dictionary<int, GameObject> _objectsOnPad = new Dictionary<int, GameObject>();
        
        private bool _triggered = false;
        
        public override void Interact()
        {
            base.Interact();
        }

        private void OnTriggerEnter2D(Collider2D obj)
        {
            if ((!obj.CompareTag("Block") || !obj.gameObject.layer.Equals(LayerMask.NameToLayer("Block"))) &&
                !obj.gameObject.CompareTag("Player")) return;
            
            AddObjectOnPad(obj.gameObject);
        }

        private void OnTriggerExit2D(Collider2D obj)
        {
            if ((!obj.tag.Equals("Block") || !obj.gameObject.layer.Equals(LayerMask.NameToLayer("Creation"))) &&
                !obj.gameObject.CompareTag("Player")) return;

            RemoveObjectOnPad(obj.gameObject);
        }

        public void RemoveObjectOnPad(GameObject obj)
        {
            var other = _objectsOnPad[obj.GetInstanceID()];
            var block = other.GetComponent<Block>();
            
            if (block != null)
            {
                block.RemovePadOnBlock(this);
            }
            
            _objectsOnPad.Remove(obj.GetInstanceID());
            if(_objectsOnPad.Count != 0 || !_triggered)
                return;
            _triggered = false;
            //Debug.Log("Interacted because of removing object", gameObject);
            Interact();
        }

        public void AddObjectOnPad(GameObject obj)
        {
            if (_objectsOnPad.ContainsKey(obj.GetInstanceID()))
                return;
            _objectsOnPad.Add(obj.GetInstanceID(), obj);
            if (_objectsOnPad.Count != 1 || _triggered) return;
            _triggered = true;
            //Debug.Log("Interacted because of adding object", gameObject);
            Interact();
        }
    }
}
