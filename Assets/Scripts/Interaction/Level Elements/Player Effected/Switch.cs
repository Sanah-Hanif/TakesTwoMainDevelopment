using System;
using System.Collections.Generic;
using System.Linq;
using Player;
using Scene;
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
                PlayerAbility["Place"].performed += PlacedObject;
        }

        private void AssignCallBacks()
        {
            PlayerMovement["Ability"].Disable();
            PlayerMovement["Interact"].performed += Performed;
            PlayerMovement["Interact"].Enable();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.gameObject.CompareTag("Player")) return;
            
            if(!other.GetComponent<PlayerInteraction>().HasCreation)
                PlayerMovement["Ability"].Enable();
            
            PlayerMovement["Interact"].performed -= Performed;
            PlayerAbility["Place"].performed -= PlacedObject;
            PlayerMovement["Interact"].Disable();
            PlayerMovement = null;
            PlayerAbility = null;
        }

        private void Performed(InputAction.CallbackContext ctx)
        {
            Interact();
        }

        private void PlacedObject(InputAction.CallbackContext ctx)
        {
            PlayerAbility["Place"].performed -= PlacedObject;
            AssignCallBacks();
        }

        private void OnDisable()
        {
            var manager = FindObjectOfType<PlayerManager>();
            if(manager == null) return;
            var chaosInput = manager.Chaos.GetComponent<PlayerInputSystem>();
            var harmonyInput = manager.Chaos.GetComponent<PlayerInputSystem>();
            
            chaosInput.Ability["Place"].performed -= PlacedObject;
            harmonyInput.Ability["Place"].performed -= PlacedObject;
            
            //PlayerAbility.GetAction("Place").performed -= PlacedObject;
            
            chaosInput.Player["Interact"].performed -= PlacedObject;
            chaosInput.Player["Interact"].Disable();

            harmonyInput.Player["Interact"].performed -= PlacedObject;
            harmonyInput.Player["Interact"].Disable();
            
            
            //PlayerMovement.GetAction("Interact").performed -= Performed;
            //PlayerMovement.GetAction("Interact").Disable();
        }
    }
}
