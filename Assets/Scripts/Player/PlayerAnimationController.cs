using System;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interaction.player
{
    public class PlayerAnimationController : MonoBehaviour
    {

        [SerializeField] private Animator animator;
        [SerializeField] private GameObject spriteParent;
        [SerializeField] private Rigidbody2D rb;
    
        private PlayerInputSystem _system;
        
        //animator cached hash values
        private static readonly int HorizontalVelocity = Animator.StringToHash("HorizontalVelocity");
        private static readonly int VerticalVelocity = Animator.StringToHash("VerticalVelocity");
        private static readonly int Jump = Animator.StringToHash("Jump");
        private static readonly int Interact = Animator.StringToHash("Interact");
        private static readonly int Ability = Animator.StringToHash("Ability");
        
        private Vector2 _prevDirec = Vector2.zero;
        private Vector2 _direc = Vector2.one;

        private int _rotation;

        private void Awake()
        {
            if (!rb)
                rb = GetComponent<Rigidbody2D>();
            _system = GetComponent<PlayerInputSystem>();
            _system.Player.TryGetAction("Interact").performed += ctx => OnInteract();
            _system.Player.TryGetAction("Ability").performed += ctx => OnAbility();
            _system.Player.TryGetAction("Jump").started += ctx => OnJump();
            GetComponent<Movement>().OnLand += OnLand;
        }

        private void FixedUpdate()
        {
            OnMovement();
        }

        private void OnMovement()
        {
            _prevDirec = _direc;
            _direc = _system.Player.GetAction("move").ReadValue<Vector2>();
            if(_prevDirec != _direc && _direc != Vector2.zero)
                _rotation = _direc.x / Mathf.Abs(_direc.x) == 1? 0 : 1;
            animator.SetFloat(HorizontalVelocity, Mathf.Abs(rb.velocity.x));
            animator.SetFloat(VerticalVelocity, rb.velocity.y);
            spriteParent.transform.rotation = Quaternion.Euler(0, 180 * _rotation, 0);
        }

        private void OnJump()
        {
            animator.SetBool(Jump, true);
        }

        private void OnInteract()
        {
            animator.SetTrigger(Interact);
        }

        private void OnAbility()
        {
            animator.SetTrigger(Ability);
        }

        private void OnLand(GameObject obj)
        {
            animator.SetBool(Jump, false);
            animator.SetFloat(VerticalVelocity, 0);
        }

        private void OnDisable()
        {
            //_system.Player.GetAction("Jump").started -= OnJump;
            GetComponent<Movement>().OnLand -= OnLand;
        }
    }
}
