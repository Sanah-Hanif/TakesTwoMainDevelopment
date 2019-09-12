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

        [SerializeField] private ScriptableObjects.Interactions.Interaction interaction;
        [SerializeField] private PlayerSettings settings;
        [SerializeField] private PlayerInputManager input;

        private InputActionMap _ability;
        private InputActionMap _movement;
        private GameObject _createdObject;

        public bool HasCreation => _createdObject != null;

        private Rigidbody2D _createdRigidbody2D;

        private void Awake()
        {
            Initialize();
        }

        private void FixedUpdate()
        {
            if(_createdObject)
                Move();
        }

        private void Initialize()
        {
            _ability = input.Ability;
            _movement = input.Player;
            _ability.Disable();
            _movement.GetAction("Ability").performed += ctx => Interact();
            _ability.GetAction("Rotate").performed += Rotate;
            _ability.GetAction("Place").performed += Place;
            _ability.GetAction("Cancel").performed += Cancel;
        }
        
        public override void Interact()
        {
            interaction.Interact(transform.position);
            var create = (CreationInteraction)interaction;
            _createdObject = create.createdObject;
            _createdRigidbody2D = _createdObject.GetComponent<Rigidbody2D>();
            if (_createdRigidbody2D)
            {
                _createdObject.layer = LayerMask.NameToLayer("CreationNoneCollision");
                _createdRigidbody2D.gravityScale = 0;
            }

            InitMovingObject();
        }

        private void InitMovingObject()
        {
            _movement.GetAction("Ability").Disable();
            _ability.Enable();
        }
        
        private void Cancel(InputAction.CallbackContext ctx)
        {
            _ability.Disable();
            Destroy(_createdObject);
            _createdObject = null;
            interaction.Interact(transform.position);
            var create = (CreationInteraction)interaction;
            create.createdObject = null;
        }

        private void Place(InputAction.CallbackContext ctx)
        {
            _ability.Disable();
            _movement.GetAction("Ability").Enable();
            _createdObject.layer = LayerMask.NameToLayer("Creation");
            if (_createdRigidbody2D)
            {
                _createdRigidbody2D.gravityScale = 1f;
            }

            if(_createdObject.GetComponent<InteractionController>())
                _createdObject.GetComponent<InteractionController>().Interact();
            
            _createdObject = null;
            _createdRigidbody2D = null;
        }

        private void Rotate(InputAction.CallbackContext ctx)
        {
            var value = ctx.ReadValue<float>();
            _createdObject.transform.Rotate(0,0, 90f * value);
        }

        private void Move()
        {
            var position = transform.position;
            var move = _ability.GetAction("Movement").ReadValue<Vector2>() * 0.1f;
            Vector2 oldPos = _createdObject.transform.position;
            oldPos += move;
            var moveAmmount = oldPos - (Vector2) position;
            var clamped = Vector2.ClampMagnitude(moveAmmount, settings.interactionSpawnRadius);
            var newPos = (Vector2) position + clamped;
            _createdObject.transform.position = newPos;
        }

        private void OnDisable()
        {
            _movement.GetAction("Ability").performed -= ctx => Interact();
            _ability.GetAction("Rotate").performed -= Rotate;
            _ability.GetAction("Place").performed -= Place;
            _ability.GetAction("Cancel").performed -= Cancel;
            _ability.Disable();
        }
    }
}

