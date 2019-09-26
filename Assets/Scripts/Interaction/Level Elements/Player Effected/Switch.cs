using System;
using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interaction.Level_Elements
{
    public class Switch : EnvironmentInteraction
    {

        public override void Interact()
        {
            base.Interact();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            var hasCreation = other.GetComponent<PlayerInteraction>().HasCreation;
            var playerInputManager = other.GetComponent<PlayerInputSystem>();
            PlayerMovement = playerInputManager.Player;
            PlayerAbility = playerInputManager.Ability;

            if (!hasCreation)
                AssignCallBacks();
            else
                PlayerAbility.GetAction("Place").performed += PlacedObject;
        }

        private void AssignCallBacks()
        {
            PlayerMovement.GetAction("Ability").Disable();
            PlayerMovement.GetAction("Interact").performed += Performed;
            PlayerMovement.GetAction("Interact").Enable();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            
            if(!other.GetComponent<PlayerInteraction>().HasCreation)
                PlayerMovement.GetAction("Ability").Enable();
            
            PlayerMovement.GetAction("Interact").performed -= Performed;
            PlayerAbility.GetAction("Place").performed -= PlacedObject;
            PlayerMovement.GetAction("Interact").Disable();
            PlayerMovement = null;
            PlayerAbility = null;
        }

        private void Performed(InputAction.CallbackContext ctx)
        {
            Interact();
        }

        private void PlacedObject(InputAction.CallbackContext ctx)
        {
            PlayerAbility.GetAction("Place").performed -= PlacedObject;
            AssignCallBacks();
        }
    }
}
