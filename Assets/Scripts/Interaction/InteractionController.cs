using UnityEngine;

namespace Interactions
{
   public abstract class InteractionController : MonoBehaviour
   {
      public virtual void Interact() { }
      
      protected int instanceID => gameObject.GetInstanceID();
   }
}
