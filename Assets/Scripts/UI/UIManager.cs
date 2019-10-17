using System;
using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;

        [SerializeField] private PlayerInputSystem[] playerInputSystems = new PlayerInputSystem[2];
        
        private Rigidbody2D[] _playerRB = new Rigidbody2D[2];

        private Vector2 _gravity;

        private bool _paused = false;

        private void Awake()
        {
            pausePanel.SetActive(false);
        }

        private void Start()
        {
            for (int i = 0; i < 2; i++)
            {
                _playerRB[i] = playerInputSystems[i].GetComponent<Rigidbody2D>();
                playerInputSystems[i].UI.FindAction("Menu").performed += ctx => MenuAction();
                playerInputSystems[i].UI.Enable();
            }
        }

        private void MenuAction()
        {
            Debug.Log("Menu");
            if(_paused)
                ResumeGame();
            else
                PauseGame();
        }

        private void PauseGame()
        {
            _gravity = Physics2D.gravity;
            Physics2D.gravity = Vector2.zero;
            _paused = true;
            foreach (var rb in _playerRB)
            {
                rb.gravityScale = 0;
                rb.GetComponent<Movement>().PausePhysics();
            }

            Time.timeScale = 0;
            pausePanel.SetActive(true);
        }

        public void ResumeGame()
        {
            Physics2D.gravity = _gravity;
            _paused = false;
            foreach (var rb in _playerRB)
            {
                rb.gravityScale = 1;
                rb.GetComponent<Movement>().ResumePhysics();
            }
            
            Time.timeScale = 1;
            pausePanel.SetActive(false);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
