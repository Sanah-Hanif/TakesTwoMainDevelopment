using System.Collections;
using System.Collections.Generic;
using Interaction;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerCreationInteraction : InteractionController
{
    
    public InputActionMap _ability { protected get; set; }

    public virtual void OnPlaced(){}
    
    public virtual void ReCreated(){}
}
