using System;
using System.Collections.Generic;
using System.Linq;
using Interactions;
using UnityEngine;

namespace Interaction.Level_Elements
{
    public class PressurePad : EnvironmentInteraction
    {

        private bool _triggered = false;
        
        public override void Interact()
        {
            base.Interact();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(_triggered)
                return;
            if ((!other.tag.Equals("Block") || !other.gameObject.layer.Equals(LayerMask.NameToLayer("Creation"))) &&
                !other.gameObject.layer.Equals(LayerMask.NameToLayer("Player"))) return;
            _triggered = true;
            Interact();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!_triggered) return;
            _triggered = false;
            Interact();
        }
    }
}
