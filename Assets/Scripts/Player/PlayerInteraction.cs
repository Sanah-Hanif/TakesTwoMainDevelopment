using Interactions;
using ScriptableObjects.Interactions;
using ScriptableObjects.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Player
{
    public class PlayerInteraction : InteractionController
    {

        [SerializeField] private ScriptableObjects.Interactions.Interaction Interaction;
        [SerializeField] private PlayerSettings Settings;
        [SerializeField] private PlayerInputManager input;

        private InputActionMap ability;
        private InputActionMap movement;
        private GameObject CreatedObject;

        private Rigidbody2D _createdRigidbody2D;

        private void Awake()
        {
            Initialize();
        }

        private void FixedUpdate()
        {
            if(CreatedObject)
                Move();
        }

        private void Initialize()
        {
            ability = input.Ability;
            movement = input.Player;
            ability.Disable();
            movement.GetAction("Ability").performed += ctx => Interact();
            ability.GetAction("Rotate").performed += Rotate;
            ability.GetAction("Place").performed += Place;
            ability.GetAction("Cancel").performed += Cancel;
        }
        
        public override void Interact()
        {
            Interaction.Interact(transform.position);
            var create = (CreationInteraction)Interaction;
            CreatedObject = create.createdObject;
            _createdRigidbody2D = CreatedObject.GetComponent<Rigidbody2D>();
            if (_createdRigidbody2D)
            {
                CreatedObject.layer = LayerMask.NameToLayer("CreationNoneCollision");
                _createdRigidbody2D.gravityScale = 0;
            }

            InitMovingObject();
        }

        private void InitMovingObject()
        {
            movement.GetAction("Ability").Disable();
            ability.Enable();
        }
        
        private void Cancel(InputAction.CallbackContext ctx)
        {
            ability.Disable();
            Destroy(CreatedObject);
            CreatedObject = null;
            Interaction.Interact(transform.position);
            var create = (CreationInteraction)Interaction;
            create.createdObject = null;
        }

        private void Place(InputAction.CallbackContext ctx)
        {
            ability.Disable();
            movement.GetAction("Ability").Enable();
            CreatedObject.layer = LayerMask.NameToLayer("Creation");
            if (_createdRigidbody2D)
            {
                _createdRigidbody2D.gravityScale = 1f;
            }

            if(CreatedObject.GetComponent<InteractionController>())
                CreatedObject.GetComponent<InteractionController>().Interact();
            
            CreatedObject = null;
            _createdRigidbody2D = null;
        }

        private void Rotate(InputAction.CallbackContext ctx)
        {
            var value = ctx.ReadValue<float>();
            CreatedObject.transform.Rotate(0,0, 90f * value);
        }

        private void Move()
        {
            var move = ability.GetAction("Movement").ReadValue<Vector2>() * 0.1f;
            Vector2 oldPos = CreatedObject.transform.position;
            oldPos += move;
            var moveAmmount = oldPos - (Vector2) transform.position;
            var clamped = Vector2.ClampMagnitude(moveAmmount, Settings.interactionSpawnRadius);
            var newPos = (Vector2) transform.position + clamped;
            CreatedObject.transform.position = newPos;
        }

        private void OnDisable()
        {
            movement.GetAction("Ability").performed -= ctx => Interact();
            ability.GetAction("Rotate").performed -= Rotate;
            ability.GetAction("Place").performed -= Place;
            ability.GetAction("Cancel").performed -= Cancel;
            ability.Disable();
        }
    }
}

