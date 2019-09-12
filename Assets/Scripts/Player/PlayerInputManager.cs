using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerInputManager : MonoBehaviour
    {
        [SerializeField] private PlayerInput input;

        public InputActionMap Ability { get; private set; }
        public InputActionMap Player { get; private set; }

        private void Awake()
        {
            Ability = input.actions.GetActionMap("Ability");
            Player = input.actions.GetActionMap("Player");
        }
    }
}
