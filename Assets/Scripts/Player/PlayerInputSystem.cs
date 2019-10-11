using Interaction.player;
using Scene;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerSettings = ScriptableObjects.Player.PlayerSettings;

namespace Player
{
    public class PlayerInputSystem : MonoBehaviour
    {
        [SerializeField] private PlayerSettings settings;
        
        private PlayerInput _input;

        public InputActionMap Ability { get; private set; }
        public InputActionMap Player { get; private set; }

        private Movement _movement;
        private PlayerInteraction _interaction;
        private PlayerAnimationController _animationController;

        public PlayerSettings Settings
        {
            get => settings;
            private set => settings = value;
        }

        private void Awake()
        {
            _movement = GetComponent<Movement>();
            _interaction = GetComponent<PlayerInteraction>();
            _animationController = GetComponent<PlayerAnimationController>();
            _input = GetComponent<PlayerInput>();
            Ability = _input.actions.FindActionMap("Ability");
            Player = _input.actions.FindActionMap("Player");

            OnReloadGame();

            Player["Reload"].performed += ctx => SceneLoader.Instance.ReloadScene();
        }

        public void OnReloadGame()
        {
            Ability.Disable();
            
            _movement.Initialize();
            _interaction.Initialize();
            _animationController.Initialize();
        }
    }
}
