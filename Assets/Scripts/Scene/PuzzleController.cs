using Unity.Mathematics;
using UnityEngine;

namespace Scene
{
    public class PuzzleController : MonoBehaviour
    {

        [SerializeField] private Transform harmonyPosition;
        [SerializeField] private Transform chaosPosition;

        [SerializeField] private bool canUseAbility = true;

        public bool CanUseAbilty => canUseAbility;

        void Start()
        {
            var player_manager = FindObjectOfType<PlayerManager>();
            
            //settings harmonies position to the spawn position of the puzzle
            player_manager.Harmony.transform.position = harmonyPosition.position;
            player_manager.Harmony.transform.rotation = quaternion.identity;
            
            //settings chaos position to the spawn position of the puzzle
            player_manager.Chaos.transform.position = chaosPosition.position;
            player_manager.Chaos.transform.rotation = quaternion.identity;
        }
        
    }
}
