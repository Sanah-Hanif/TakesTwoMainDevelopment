using UnityEngine.InputSystem;

namespace Interaction.player
{
    public abstract class PlayerCreationInteraction : InteractionController
    {
    
        public InputActionMap _ability { protected get; set; }

        public virtual void OnPlaced(){}
    
        public virtual void ReCreated(){}
    }
}
