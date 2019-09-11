using System;
using System.Collections.Generic;
using System.Linq;
using Interactions;
using UnityEngine;

namespace Interaction.Level_Elements
{
    public class PressurePad : InteractionController
    {
        
        [SerializeField] private List<EnvironmentInteraction> dependancies = new List<EnvironmentInteraction>();

        private bool triggered = false;
        
        public override void Interact()
        {
            if(dependancies.Count == 0)
                return;
            foreach (var interaction in dependancies.Where(interaction => interaction))
            {
                interaction.Interact();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if ((!other.tag.Equals("Block") || !other.gameObject.layer.Equals(LayerMask.NameToLayer("Creation"))) &&
                !other.gameObject.layer.Equals(LayerMask.NameToLayer("Player"))) return;
            triggered = true;
            Interact();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!triggered) return;
            triggered = false;
            Interact();
        }
    }
}
