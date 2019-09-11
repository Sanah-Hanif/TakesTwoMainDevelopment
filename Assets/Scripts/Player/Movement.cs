using System;
using System.Collections;
using System.Collections.Generic;
using ScriptableObjects.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class Movement : MonoBehaviour
{
    private Gamepad gamepad;
    private bool IsGrounded = true;
    private bool DoubleJumped = false;
    private bool Moved = false;
    private PlayerControls Control;

    [SerializeField] private Rigidbody2D rigidBody;
    [SerializeField] private int playerNumber;
    [SerializeField] private PlayerSettings settings;
    [SerializeField] private PlayerInputManager input;

    private InputActionMap movement;

    private void Awake()
    {
        Initialize();
    }

    private void FixedUpdate()
    {
        MoveLeftStick();
        FallDown();
    }

    private void Initialize()
    {
        movement = input.Player;
        movement.Enable();
        movement.GetAction("Jump").performed += Jump;
        movement.GetAction("JumpHold").performed += JumpHold;
    }

    private void MoveLeftStick()
    {
        var direc =  movement.GetAction("move").ReadValue<Vector2>();
        var velocity = rigidBody.velocity;
        if (direc.magnitude > 0.5f)
        {
            Moved = true;
            velocity.x = settings.maxSpeed * direc.x;
            rigidBody.velocity = velocity;
        }
        else if(Moved)
        {
            Moved = false;
            velocity = rigidBody.velocity;
            velocity.x = 0;
            rigidBody.velocity = velocity;
        }
    }

    private void Jump(InputAction.CallbackContext ctx)
    {
        if (!IsGrounded)
            return;
        IsGrounded = false;
        //Debug.Log("Jump", gameObject);
        var velocity = rigidBody.velocity;
        velocity.y = settings.jumpVelocity;
        rigidBody.velocity = velocity;
    }

    private void JumpHold(InputAction.CallbackContext ctx)
    {
        if (DoubleJumped)
            return;
        DoubleJumped = true;
        //Debug.Log("JumpHold", gameObject);
        var velocity = rigidBody.velocity;
        velocity.y = settings.jumpVelocity * settings.jumpHoldMultiplier;
        rigidBody.velocity = velocity;
    }

    private void FallDown()
    {
        var velocity = rigidBody.velocity;
        if (velocity.y < settings.fallDownThreshHold)
        {
            //Debug.Log("falling down");
            velocity.y -= settings.fallSpeedIncrease;
            rigidBody.velocity = velocity;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer.Equals(LayerMask.NameToLayer("Ground")) ||
            other.gameObject.layer.Equals(LayerMask.NameToLayer("Player")) ||
            (other.gameObject.layer.Equals(LayerMask.NameToLayer("Creation")) && 
             other.gameObject.tag.Equals("Block")))
        {
            DoubleJumped = false;
            IsGrounded = true;
        }
    }

    private void OnDisable()
    {
        movement.GetAction("Jump").performed -= Jump;
        movement.GetAction("JumpHold").performed -= JumpHold;
        movement.Disable();
    }
}