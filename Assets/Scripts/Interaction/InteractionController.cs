using UnityEngine;

namespace Interaction
{
   public abstract class InteractionController : MonoBehaviour
   {

      protected int instanceid = 0;

      public virtual void Interact() { }

      protected int InstanceID
      {
         get
         {
            if (instanceid == 0)
               instanceid = GetInstanceID();
            return instanceid;
         }
      }
   }
}
