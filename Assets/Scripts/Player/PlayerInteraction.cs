using System;
using Interaction;
using Interaction.player;
using ScriptableObjects.Interactions;
using ScriptableObjects.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace Player
{
    public class PlayerInteraction : InteractionController
    {
        private void OnDestroy()
        {
            Debug.Log(gameObject);
        }

        [SerializeField] private ScriptableObjects.Interactions.Interaction interaction;
        
        private PlayerSettings settings;
        private PlayerInputSystem input;
        private InputActionMap _ability;
        private InputActionMap _movement;
        private GameObject _createdObject;
        private PlayerCreationInteraction _createdInteraction;

        [SerializeField] private string interact = "Block";

        public bool HasCreation => _createdObject != null;

        [HideInInspector] public bool canUseInteraction = true;

        private void Awake()
        {
            Initialize();
        }

        private void FixedUpdate()
        {
            if(_createdObject)
                Move();
        }

        public void Initialize()
        {
            input = GetComponent<PlayerInputSystem>();
            _ability = input.Ability;
            _movement = input.Player;
            _ability.Disable();
            _movement["Interact"].Disable();
            _movement["Ability"].performed += ctx => Interact();
            _ability["Place"].performed += Place;
            _ability["Cancel"].performed += Cancel;
            settings = input.Settings;

            if (!_createdObject) return;
            Destroy(_createdObject);
            _createdInteraction = null;
            _createdObject = null;
        }
        
        public override void Interact()
        {
            if(!canUseInteraction) return;
            interaction.Interact(transform.position);
            var create = (CreationInteraction)interaction;
            _createdObject = create.createdObject;
            _createdInteraction = _createdObject.GetComponent<PlayerCreationInteraction>();
            InitMovingObject();
        }

        private void InitMovingObject()
        {
            _movement["Ability"].Disable();
            _movement["Move"].Disable();
            _ability["Movement"].Enable();
            //_ability.GetAction("Place").performed += _createdInteraction.OnPlaced;
            _createdInteraction._ability = _ability;
            _createdInteraction.ReCreated();
            _ability.Enable();
        }
        
        private void Cancel(InputAction.CallbackContext ctx)
        {
            Destroy(_createdObject);
            _ability.Disable();
            _movement["Move"].Enable();
            _ability["Movement"].Disable();
            _movement["Ability"].Enable();
            _createdObject = null;
            //interaction.Interact(transform.position);
            var create = (CreationInteraction)interaction;
            create.createdObject = null;
            _movement["Ability"].Enable();
        }

        private void Place(InputAction.CallbackContext ctx)
        {
            _ability.Disable();
            _movement["Move"].Enable();
            _ability["Movement"].Disable();
            _movement["Ability"].Enable();
            if(_createdInteraction == null) return;
            _createdInteraction.Interact();
            
            _createdInteraction.OnPlaced();
            _createdObject = null;
            _createdInteraction = null;
        }

        private void Move()
        {
            var position = transform.position;
            var move = _ability["Movement"].ReadValue<Vector2>() * 0.1f;
            Vector2 oldPos = _createdObject.transform.position;
            oldPos += move;
            var moveAmmount = oldPos - (Vector2) position;
            var clamped = Vector2.ClampMagnitude(moveAmmount, settings.interactionSpawnRadius);
            var newPos = (Vector2) position + clamped;
            _createdObject.transform.position = newPos;
            if (!interact.Equals("Air")) return;
            Vector2 direc = _createdObject.transform.position - transform.position;
            float angle = Vector2.SignedAngle(Vector2.right, direc);
            Vector3 rot = new Vector3(0, 0, angle);
            _createdObject.transform.rotation = Quaternion.Euler(rot);
        }

        private void OnDisable()
        {
            _movement["Ability"].performed -= ctx => Interact();
            _ability["Place"].performed -= Place;
            _ability["Cancel"].performed -= Cancel;
            _ability.Disable();
        }
    }
}

