using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Exit
{
    public class ExitSceneLoader : MonoBehaviour
    {
        [SerializeField] private string sceneToLoad;
        
        private List<GameObject> _players = new List<GameObject>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!other.gameObject.tag.Equals("Player")) return;
            _players.Add(other.gameObject);
            if (_players.Count == 2)
                LoadNextScene();
        }

        private void LoadNextScene()
        {
            SceneManager.LoadScene(sceneToLoad);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(!other.gameObject.tag.Equals("Player")) return;
            _players.Remove(other.gameObject);
        }
    }
}
