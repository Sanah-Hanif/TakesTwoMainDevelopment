using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] protected string firstSceneToLoad;

        private string _currentScene;
        private string _nextScene;

        private static SceneLoader _sceneLoader;

        private IEnumerator loadRoutine;

        public static SceneLoader Instance => _sceneLoader;

        private void Awake()
        {
            //singleton stuff
            if(_sceneLoader == null)
                _sceneLoader = this;
            else if(_sceneLoader != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            
            //if the editor is open, use the active scene, if not, load the first scene we need
            if(!Application.isEditor)
                LoadScene(firstSceneToLoad);
            else
                _currentScene = SceneManager.GetActiveScene().name;
        }

        public void LoadScene(string newScene)
        {
            loadRoutine = LoadNewScene(newScene);
            StartCoroutine(loadRoutine);
        }

        private IEnumerator LoadNewScene(string newScene)
        {
            
            /* TODO: add scene loading logic
             * TODO: ensure the same scene doesnt load twice
             * TODO: unload previous scene
             * TODO: load new scene
             * TODO: fades/cuts between puzzles
             */

            if (_currentScene.Equals(newScene)) yield break;
            yield return SceneManager.UnloadSceneAsync(_currentScene);

            yield return SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);

            _currentScene = newScene;
            var scene = SceneManager.GetSceneByName(_currentScene);
            SceneManager.SetActiveScene(scene);
        }

        public void ReloadScene()
        {
            loadRoutine = RestartScene();
            StartCoroutine(loadRoutine);
        }

        private IEnumerator RestartScene()
        {
            
            yield return SceneManager.UnloadSceneAsync(_currentScene);

            yield return SceneManager.LoadSceneAsync(_currentScene, LoadSceneMode.Additive);
            var scene = SceneManager.GetSceneByName(_currentScene);
            SceneManager.SetActiveScene(scene);
        }
    }
}
