using System;
using ScriptableObjects.Player;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Player
{
    public class Movement : MonoBehaviour
    {
        private Gamepad _gamepad;
        private bool _isGrounded = true;
        private bool _moved;
        private bool _isJumping;
        public bool CanMove { get; set; }
        
        private Vector2 _prevVel = Vector2.zero;
        
        [SerializeField] protected Rigidbody2D rigidBody;
        [SerializeField] protected Transform feetTransform;
        [SerializeField] protected Transform SideTransform;
        [SerializeField] protected Transform SideTransformLeft;
        [SerializeField] protected LayerMask canJumpOff;
        [SerializeField] protected LayerMask slideOffOf;
        [SerializeField] protected float groundCheckRadius = 0.05f;
        [SerializeField] protected float boxCheckRadius = 0.01f;
        [SerializeField] protected CapsuleCollider2D _collider;
        
        private PlayerSettings settings;
        
        private PlayerInputSystem input;

        private InputActionMap movement;

        public UnityAction<GameObject> OnJump;
        public UnityAction<GameObject> OnMove;
        public UnityAction<GameObject> OnStop;
        public UnityAction<GameObject> OnLand;

        public bool IsMoving => _moved;

        private void OnValidate()
        {
            settings = GetComponent<PlayerInputSystem>()._settings;
            if (!settings.useUnityGravity)
            {
                rigidBody.gravityScale = 0;
            }
            else
            {
                rigidBody.gravityScale = 1;
            }
        }

        private void FixedUpdate()
        {
            //Debug.Log(rigidBody.velocity);
            MoveLeftStick();
            FallDown();
            if(_isJumping)
                JumpUp();
        }

        public void PausePhysics()
        {
            rigidBody.gravityScale = 0;
            _prevVel = rigidBody.velocity;
            rigidBody.velocity = Vector2.zero;
        }

        public void ResumePhysics()
        {
            rigidBody.velocity = _prevVel;
            _prevVel = Vector2.zero;
            rigidBody.gravityScale = 1;
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

        private void CancelEarly(InputAction.CallbackContext ctx)
        {
            _isJumping = false;
        }
        
        public void Initialize()
        {
            //reset action events
            OnJump = null;
            OnLand = null;
            OnMove = null;
            OnStop = null;
            
            //resed boolean values
            _isGrounded = true;
            _moved = false;
            _isJumping = false;

            //reset player input key bindings
            input = GetComponent<PlayerInputSystem>();
            CanMove = true;
            movement = input.Player;
            movement.Enable();
            movement["Jump"].started += StartJump;
            movement["Jump"].performed += CancelJump;
            movement["Jump"].canceled += CancelEarly;
            if (gameObject.layer.Equals(LayerMask.NameToLayer("Harmony")))
            {
                slideOffOf &= ~LayerMask.NameToLayer("HarmonyGateway");
                canJumpOff |= 1 << LayerMask.NameToLayer("Chaos");
            }
            else
            {
                slideOffOf &= ~LayerMask.NameToLayer("ChaosGateway");
                canJumpOff |= 1 << LayerMask.NameToLayer("Harmony");
            }

            settings = input.Settings;
        }

        private void MoveLeftStick()
        {
            var direc =  movement["move"].ReadValue<Vector2>();
            var velocity = rigidBody.velocity;
            CheckJump();
            if(!CheckIfCanMove(direc)) return;

            if (direc.magnitude > 0.5f)
            {
                if (_moved == false)
                {
                    OnMove?.Invoke(gameObject);
                    _moved = true;
                }
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

        private void CheckJump()
        {
            if (rigidBody.velocity.y < 1f) 
            {
                var objectByFeet = Physics2D.OverlapCircle(feetTransform.position, boxCheckRadius*2, canJumpOff);
                if (objectByFeet != null)
                {
                    _isJumping = false;
                    _isGrounded = true;
                    OnLand?.Invoke(gameObject);
                }
            }
        }

        private bool CheckIfCanMove(Vector2 direction)
        {
            if (direction.magnitude < 0.5) return true;
            var direc =  direction.x / Mathf.Abs(direction.x);
            var position = direc == 1 ? SideTransform.position : SideTransformLeft.position;

            LayerMask mask = LayerMask.GetMask("Ground");
            var obj = Physics2D.OverlapBox(position,
                       new Vector2(boxCheckRadius, _collider.bounds.size.y - groundCheckRadius),
                       0,
                       mask);

            return obj == null;
        }

        private void FallDown()
        {
            if (settings.useUnityGravity)
            {
                var velocity = rigidBody.velocity;
                if (!(velocity.y < settings.fallDownThreshHold)) return;

                velocity += Physics2D.gravity * (settings.fallSpeedIncrease - 1) * Vector2.up * Time.fixedDeltaTime;
                rigidBody.velocity = velocity;
            }
            else
            {
                /*var objectByFeet = Physics2D.OverlapBox(SideTransform.position,
                    new Vector2(_collider.bounds.size.x, boxCheckRadius),
                    0,
                    canJumpOff);
                if (objectByFeet == null)
                {
                    
                }*/
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(!other.enabled) return;
            var dot = Vector2.Dot(other.GetContact(0).normal, Vector2.up);
            //Debug.Log(dot);

            if (dot < -0.9f)
                _isJumping = false;
            var objectByFeet = Physics2D.OverlapCircle(feetTransform.position, boxCheckRadius*2, canJumpOff);
            if (other.gameObject.tag.Equals("Block"))
            {
                _isJumping = false;
                _isGrounded = true;
                OnLand?.Invoke(gameObject);
            }

            if (objectByFeet != null)
            {
                //Debug.Log("Things at feet");
                Debug.Log(objectByFeet.name);
                _isJumping = false;
                _isGrounded = true;
                OnLand?.Invoke(gameObject);
            }


            if (!other.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")) &&
                !other.gameObject.CompareTag("Player") &&
                !other.gameObject.layer.Equals(LayerMask.NameToLayer("MovingPlatform"))) 
                return;

            if (!(dot > 0.7f)) return;
            _isGrounded = true;
            _isJumping = false;
            OnLand?.Invoke(gameObject);
        }

        private void OnDisable()
        {
            //movement.TryGetAction("Jump").performed -= Jump;
            //movement.TryGetAction("JumpHold").performed -= JumpHold;
            movement["Jump"].started -= StartJump;
            movement["Jump"].performed -= CancelJump;
            movement["Jump"].canceled -= CancelEarly;
            movement.Disable();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawCube(SideTransform.position, new Vector2(boxCheckRadius, _collider.bounds.size.y - groundCheckRadius));
            Gizmos.DrawCube(SideTransformLeft.position, new Vector2(boxCheckRadius, _collider.bounds.size.y - groundCheckRadius));
            
            Gizmos.color = Color.blue;
            //Gizmos.DrawWireCube(feetTransform.position, new Vector2(_collider.bounds.size.x, boxCheckRadius));
            Gizmos.DrawWireSphere(feetTransform.position, boxCheckRadius*2);
        }
    }
}