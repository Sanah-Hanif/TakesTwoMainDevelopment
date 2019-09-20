﻿using ScriptableObjects.Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        private Gamepad _gamepad;
        private bool _isGrounded = true;
        private bool _doubleJumped = false;
        private bool _moved = false;
        public bool CanMove { get; set; }
        private PlayerControls _control;

        [SerializeField] private Rigidbody2D rigidBody;
        [SerializeField] private int playerNumber;
        
        private PlayerSettings settings;
        
        private PlayerInputSystem input;

        private InputActionMap movement;

        private void Awake()
        {
            Initialize();
        }

        private void FixedUpdate()
        {
            //Debug.Log(rigidBody.velocity);d
            MoveLeftStick();
            FallDown();
        }

        private void Initialize()
        {
            input = GetComponent<PlayerInputSystem>();
            CanMove = true;
            movement = input.Player;
            movement.Enable();
            movement.TryGetAction("Jump").performed += Jump;
            movement.TryGetAction("JumpHold").performed += JumpHold;
            settings = input.Settings;
        }

        private void MoveLeftStick()
        {
            if(!CanMove) return;
            var direc =  movement.GetAction("move").ReadValue<Vector2>();
            var velocity = rigidBody.velocity;
            if (direc.magnitude > 0.5f)
            {
                _moved = true;
                //velocity.x = Mathf.Clamp(velocity.x + Time.fixedDeltaTime * settings.acceleration * settings.maxSpeed * direc.x, -settings.maxSpeed, settings.maxSpeed);
                velocity.x = settings.maxSpeed * direc.x;
                rigidBody.velocity = velocity;
            }
            else if(_moved)
            {
                _moved = false;
                velocity = rigidBody.velocity;
                velocity.x = 0;
                rigidBody.velocity = velocity;
            }
        }

        private void Jump(InputAction.CallbackContext ctx)
        {
            if (!_isGrounded)
                return;
            _isGrounded = false;
            //Debug.Log("Jump", gameObject);
            var velocity = rigidBody.velocity;
            velocity.y = settings.jumpVelocity;
            rigidBody.velocity = velocity;
        }

        private void JumpHold(InputAction.CallbackContext ctx)
        {
            if (_doubleJumped)
                return;
            _doubleJumped = true;
            //Debug.Log("JumpHold", gameObject);
            var velocity = rigidBody.velocity;
            velocity.y = settings.jumpVelocity * settings.jumpHoldMultiplier;
            rigidBody.velocity = velocity;
        }

        private void FallDown()
        {
            var velocity = rigidBody.velocity;
            if (!(velocity.y < settings.fallDownThreshHold)) return;
            //Debug.Log("falling down");
            velocity.y -= settings.fallSpeedIncrease;
            rigidBody.velocity = velocity;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag.Equals("Block"))
            {
                _doubleJumped = false;
                _isGrounded = true;
            }

            if (!other.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")) &&
                !other.gameObject.layer.Equals(LayerMask.NameToLayer("Player"))) 
                return;
            _doubleJumped = false;
            _isGrounded = true;
        }

        private void OnDisable()
        {
            movement.TryGetAction("Jump").performed -= Jump;
            movement.TryGetAction("JumpHold").performed -= JumpHold;
            movement.Disable();
        }
    }
}