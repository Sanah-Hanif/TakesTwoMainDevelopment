using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace ScriptableObjects.Camera
{
    [CreateAssetMenu(menuName = "TargetGroup", fileName = "New Target Group Controller Settings")]
    public class TargetSettings : ScriptableObject
    {
        [Range(0,4f)] public float startWeight = 2.5f;
        [Range(0,4f)] public float startRadius = 2.5f;
        [Range(2.5f,10f)] public float endWeight = 5f;
        [Range(2.5f,10f)] public float endRadius = 5f;
        [Range(0,10f)] public float lerpDuration = 2f;

        private void OnValidate()
        {
            if (startWeight > endWeight)
                endWeight = startWeight;
            if (startRadius > endRadius)
                endRadius = startRadius;
        }
    }
}
