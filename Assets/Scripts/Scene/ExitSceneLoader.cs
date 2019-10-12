using System;
using System.Collections.Generic;
using Scene;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Exit
{
    public class ExitSceneLoader : MonoBehaviour
    {
        [SerializeField] protected string sceneToLoad;
        
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
            if (sceneToLoad != null)
            {
                var playermanager = FindObjectOfType<PlayerManager>();
                var baseScene = SceneManager.GetSceneByBuildIndex(0);
                SceneManager.MoveGameObjectToScene(playermanager.Chaos, baseScene);
                SceneManager.MoveGameObjectToScene(playermanager.Harmony, baseScene);
                SceneLoader.Instance.LoadScene(sceneToLoad);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if(!other.gameObject.tag.Equals("Player")) return;
            _players.Remove(other.gameObject);
        }
    }
}
