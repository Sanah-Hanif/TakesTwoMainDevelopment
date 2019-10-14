using System;
using System.Collections.Generic;
using Interactions;
using UnityEngine;

namespace Interaction.Level_Elements
{
    public class PressurePad : InteractionController
    {
        
        [SerializeField] private List<EnvironmentInteraction> dependancies;
        
        public override void Interact()
        {
            if(dependancies == null)
                dependancies = new List<EnvironmentInteraction>();
            if(dependancies.Count == 0)
                return;
            foreach (var interaction in dependancies)
            {
                interaction.Interact();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Interact();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Interact();
        }
    }
}
