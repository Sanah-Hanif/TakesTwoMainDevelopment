using System.Collections.Generic;
using System.Linq;
using Interactions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interaction.Level_Elements
{
    public class EnvironmentInteraction : InteractionController
    {
        [SerializeField] protected List<LevelInteraction> dependancies = new List<LevelInteraction>();
        
        protected InputActionMap PlayerMovement;
        protected InputActionMap PlayerAbility;

        private void Awake()
        {
            var child = GetComponentInChildren<LevelInteraction>();
            if(child)
                dependancies.Add(child);
        }

        public override void Interact()
        {
            //Debug.Log(dependancies.Count);
            if(dependancies.Count == 0)
                return;
            foreach (var interaction in dependancies.Where(interaction => interaction))
            {
                interaction.Interact();
            }
        }
        
    }
}
