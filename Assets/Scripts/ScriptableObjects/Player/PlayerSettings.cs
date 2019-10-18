using UnityEngine;

namespace ScriptableObjects.Player
{
    [CreateAssetMenu(menuName = "Player Settings", fileName = "New Player Settings")]
    public class PlayerSettings : ScriptableObject
    {
        [Header("Movement Parameters")]
        public float maxSpeed = 2f;

        [Header("Jump Parameters")] 
        public bool useUnityGravity = true;
        public float jumpVelocity = 2f;

        [Header("Falling Parameters")] 
        public float gravity = -20f;
        public float fallDownThreshHold = 0f;
        public float fallSpeedIncrease = 2.5f;
        public float smallJumpIncrease = 2f;

        [Header("Interaction Spawn Radius")] 
        public float interactionSpawnRadius = 3f;
    }
}
