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

        public PlayerSettings Settings
        {
            get => settings;
            private set => settings = value;
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _input = GetComponent<PlayerInput>();
            Ability = _input.actions.GetActionMap("Ability");
            Player = _input.actions.GetActionMap("Player");

            Player.GetAction("Reload").performed += ctx => SceneLoader.Instance.ReloadScene();
        }
    }
}
