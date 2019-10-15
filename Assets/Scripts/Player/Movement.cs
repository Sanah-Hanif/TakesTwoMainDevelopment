using System;
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
        private bool _moved;
        private bool _isJumping;
        public bool CanMove { get; set; }
        
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

        private void FixedUpdate()
        {
            //Debug.Log(rigidBody.velocity);
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
            if(gameObject.layer.Equals(LayerMask.NameToLayer("Harmony")))
                slideOffOf &= ~LayerMask.NameToLayer("HarmonyGateway");
            else
                slideOffOf &= ~LayerMask.NameToLayer("ChaosGateway");
            settings = input.Settings;
        }

        private void MoveLeftStick()
        {
            var direc =  movement["move"].ReadValue<Vector2>();
            var velocity = rigidBody.velocity;
            if(!CheckIfCanMove(direc)) return;
            
            /*if(Math.Abs(velocity.x) < 0.1f)
                CheckIfCanMove(direc);*/
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

        private bool CheckIfCanMove(Vector2 direction)
        {
            if (direction.magnitude < 0.5) return true;
            var direc =  direction.x / Mathf.Abs(direction.x);
            var position = direc == 1 ? SideTransform.position : SideTransformLeft.position;
            //Debug.Log(position);
            LayerMask mask = LayerMask.GetMask("Ground");
            var obj = Physics2D.OverlapBox(position,
                       new Vector2(boxCheckRadius, _collider.bounds.size.y - groundCheckRadius),
                       0,
                       mask);
            //Debug.Log(obj == null);
            return obj == null;
        }

        private void FallDown()
        {
            var velocity = rigidBody.velocity;
            if (!(velocity.y < settings.fallDownThreshHold)) return;
            //Debug.Log("falling down");
            velocity += Physics2D.gravity * (settings.fallSpeedIncrease - 1) * Vector2.up * Time.fixedDeltaTime;
            rigidBody.velocity = velocity;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if(!other.enabled) return;
            var dot = Vector2.Dot(other.GetContact(0).normal, Vector2.up);
            Debug.Log(dot);
            if (!_isGrounded && !other.gameObject.layer.Equals(LayerMask.NameToLayer("MovingPlatform")))
            {
                if(other.enabled)
                    _isJumping = false;
                CanMove = !(dot < 0.7);
            }

            var objectByFeet = Physics2D.OverlapBox(SideTransform.position,
                new Vector2(_collider.bounds.size.x, boxCheckRadius),
                canJumpOff);
            if (!CanMove)
            {
                CanMove = objectByFeet != null || rigidBody.velocity.y > 0;
            }
            if (other.gameObject.tag.Equals("Block"))
            {
                _isJumping = false;
                _isGrounded = true;
                OnLand?.Invoke(gameObject);
            }

            if (objectByFeet != null)
            {
                Debug.Log("Things at feet");
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
            Gizmos.DrawWireCube(feetTransform.position, new Vector2(_collider.bounds.size.x, boxCheckRadius));
        }
    }
}