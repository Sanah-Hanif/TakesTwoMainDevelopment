﻿using System;
using ScriptableObjects.Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        private Gamepad _gamepad;
        private bool _isGrounded = true;
        private bool _doubleJumped = false;
        private bool _moved = false;
        private bool _isJumping = false;
        public bool CanMove { get; set; }
        private PlayerControls _control;

        [SerializeField] private Rigidbody2D rigidBody;
        [SerializeField] private int playerNumber;
        [SerializeField] private Transform feetTransform;
        [SerializeField] private LayerMask canJumpOff;
        [SerializeField] private float groundCheckRadius = 0.05f;
        
        private PlayerSettings settings;
        
        private PlayerInputSystem input;

        private InputActionMap movement;

        public UnityAction<GameObject> OnJump;
        public UnityAction<GameObject> OnMove;
        public UnityAction<GameObject> OnStop;

        private BoxCollider2D _collider;

        private void Awake()
        {
            Initialize();
            _collider = GetComponent<BoxCollider2D>();
        }

        private void FixedUpdate()
        {
            //Debug.Log(rigidBody.velocity);d
            MoveLeftStick();
            FallDown();
            if(_isJumping)
                JumpUp();
        }

        private void StartJump(InputAction.CallbackContext ctx)
        {
            if (!_isGrounded)
                return;
            _isGrounded = false;
            _isJumping = true;
            JumpUp();
            OnJump?.Invoke(gameObject);
        }

        private void JumpUp()
        {
            var velocity = rigidBody.velocity;
            velocity.y = settings.jumpVelocity;
            rigidBody.velocity = velocity;
        }

        private void CancelJump(InputAction.CallbackContext ctx)
        {
            _isJumping = false;
        }

        private void Initialize()
        {
            input = GetComponent<PlayerInputSystem>();
            CanMove = true;
            movement = input.Player;
            movement.Enable();
            //movement.TryGetAction("Jump").performed += Jump;
            movement.TryGetAction("Jump").started += StartJump;
            movement.TryGetAction("Jump").performed += CancelJump;
            movement.TryGetAction("Jump").canceled += CancelJump;
            //movement.TryGetAction("JumpHold").performed += JumpHold;
            settings = input.Settings;
        }

        private void MoveLeftStick()
        {
            if(!CanMove) return;
            var direc =  movement.GetAction("move").ReadValue<Vector2>();
            var velocity = rigidBody.velocity;
            if (direc.magnitude > 0.5f)
            {
                if (_moved == false)
                {
                    OnMove?.Invoke(gameObject);
                    _moved = true;
                }

                //velocity.x = Mathf.Clamp(velocity.x + Time.fixedDeltaTime * settings.acceleration * settings.maxSpeed * direc.x, -settings.maxSpeed, settings.maxSpeed);
                velocity.x = settings.maxSpeed * direc.x;
                rigidBody.velocity = velocity;
            }
            else if(_moved)
            {
                _moved = false;
                OnStop?.Invoke(gameObject);
                velocity = rigidBody.velocity;
                velocity.x = 0;
                rigidBody.velocity = velocity;
            }
        }

        private void Jump(InputAction.CallbackContext ctx)
        {
            Debug.Log("Jumped");
            if (!_isGrounded)
                return;
            _isGrounded = false;
            //Debug.Log("Jump", gameObject);
            var velocity = rigidBody.velocity;
            velocity.y = settings.jumpVelocity;
            rigidBody.velocity = velocity;
            OnJump?.Invoke(gameObject);
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
            if(!_isGrounded && !other.gameObject.layer.Equals(LayerMask.NameToLayer("MovingPlatform")))
                CanMove = !(Mathf.Abs(Vector2.Dot(other.GetContact(0).normal, Vector2.up)) < 0.7);
            if (!CanMove)
            {
                CanMove = Physics2D.OverlapBox(feetTransform.position, 
                              new Vector2(_collider.bounds.size.x - groundCheckRadius, groundCheckRadius), 
                              canJumpOff) != null || rigidBody.velocity.y > 0;
            }
            if (other.gameObject.tag.Equals("Block"))
            {
                _doubleJumped = false;
                _isGrounded = true;
            }

            if (!other.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")) &&
                !other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")) &&
                !other.gameObject.layer.Equals(LayerMask.NameToLayer("MovingPlatform"))) 
                return;
            _doubleJumped = false;
            _isGrounded = true;
            
        }

        private void OnDisable()
        {
            //movement.TryGetAction("Jump").performed -= Jump;
            //movement.TryGetAction("JumpHold").performed -= JumpHold;
            movement.TryGetAction("Jump").started -= StartJump;
            movement.TryGetAction("Jump").performed -= CancelJump;
            movement.TryGetAction("Jump").canceled -= CancelJump;
            movement.Disable();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(feetTransform.position, new Vector2(_collider.bounds.size.x - groundCheckRadius, groundCheckRadius));
        }
    }
}