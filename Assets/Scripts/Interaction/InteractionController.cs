using UnityEngine;

namespace Interaction
{
   public abstract class InteractionController : MonoBehaviour
   {
      public virtual void Interact() { }
      
      protected int InstanceID => gameObject.GetInstanceID();
   }
}
