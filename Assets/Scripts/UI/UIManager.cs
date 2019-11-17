using System;
using Player;
using Scene;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject controlsPanel;

        [SerializeField] private Button back;
        [SerializeField] private Button resume;

        [SerializeField] private EventSystem events;

        [SerializeField] private PlayerInputSystem[] playerInputSystems = new PlayerInputSystem[2];
        
        private Rigidbody2D[] _playerRB = new Rigidbody2D[2];

        private Vector2 _gravity;

        private bool _paused = false;

        private void Awake()
        {
            pausePanel.SetActive(false);
            controlsPanel.SetActive(false);
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
            if(_paused)
                ResumeGame();
            else
                PauseGame();
        }

        public void LoadMainMenu()
        {
            SceneLoader.Instance.LoadScene("MainMenuAdditive");
            ResumeGame();
        }

        public void PauseGame()
        {
            events.SetSelectedGameObject(resume.gameObject);
            resume.Select();
            resume.OnSelect(null);
            _gravity = Physics2D.gravity;
            Physics2D.gravity = Vector2.zero;
            _paused = true;
            foreach (var rb in _playerRB)
            {
                rb.gravityScale = 0;
                rb.GetComponent<Movement>().PausePhysics();
            }
            
            foreach (var input in playerInputSystems)
            {
                input.Player.Disable();
                input.Ability.Disable();
            }

            Time.timeScale = 1;
            pausePanel.SetActive(true);
            controlsPanel.SetActive(false);
        }

        public void PauseMenu()
        {
            events.SetSelectedGameObject(resume.gameObject);
            resume.Select();
            resume.OnSelect(null);
            pausePanel.SetActive(true);
            controlsPanel.SetActive(false);
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

            foreach (var input in playerInputSystems)
            {
                input.Player.Enable();
                input.Ability.Enable();
            }
            
            Time.timeScale = 1;
            pausePanel.SetActive(false);
            controlsPanel.SetActive(false);
        }

        public void ControlsMenu()
        {
            events.SetSelectedGameObject(back.gameObject);
            back.Select();
            back.OnSelect(null);
            pausePanel.SetActive(false);
            controlsPanel.SetActive(true);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
