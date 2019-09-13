using System.Collections;
using System.Collections.Generic;
using Interactions;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerCreationInteraction : InteractionController
{
    
    public InputActionMap _ability { protected get; set; }

    public virtual void OnPlaced(InputAction.CallbackContext ctx){}
    
    public virtual void ReCreated(){}
}
