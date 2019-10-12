using UnityEngine;

namespace Nodule
{
    public class ChildScaler : MonoBehaviour
    {
        private void OnValidate()
        {
            var parentTransform = transform.parent.localScale;
            var childTransform = transform.GetChild(0).localScale;
        
            var newScale = new Vector3(
                childTransform.x / parentTransform.x, 
                childTransform.y / parentTransform.y, 
                childTransform.z / parentTransform.z);

            transform.GetChild(0).localScale = newScale;
        }
    }
}
