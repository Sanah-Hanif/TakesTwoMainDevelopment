using System;
using Player;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private GameObject pausePanel;

        [SerializeField] private PlayerInputSystem[] playerInputSystems = new PlayerInputSystem[2];
        
        private Rigidbody2D[] _playerRB = new Rigidbody2D[2];

        private void Awake()
        {
            pausePanel.SetActive(false);
        }

        private void Start()
        {
            for (int i = 0; i < 2; i++)
            {
                _playerRB[i] = playerInputSystems[i].GetComponent<Rigidbody2D>();
            }
        }

        public void PauseGame()
        {
            foreach (var rb in _playerRB)
            {
                rb.gravityScale = 0;
            }
            pausePanel.SetActive(true);
            
        }

        public void ResumeGame()
        {
            foreach (var rb in _playerRB)
            {
                rb.gravityScale = 1;
            }
            pausePanel.SetActive(false);
        }
    }
}
