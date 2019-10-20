using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Scene
{
    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] protected bool loadSceneOnStart = true;
        [SerializeField] protected string firstSceneToLoad;

        public UnityEvent onSceneReload;

        private string _currentScene;
        private string _nextScene;

        private static SceneLoader _sceneLoader;

        private IEnumerator loadRoutine;

        public static SceneLoader Instance => _sceneLoader;

        private PlayerManager _playerManager;

        private void Awake()
        {
            //singleton stuff
            if (_sceneLoader == null)
                _sceneLoader = this;
            else if (_sceneLoader != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

            //if the editor is open, use the active scene, if not, load the first scene we need
            if (!Application.isEditor || loadSceneOnStart)
            {
                loadRoutine = LoadFirstScene();
                StartCoroutine(loadRoutine);
            }
            else
                _currentScene = SceneManager.GetActiveScene().name;
        }

        private void Start()
        {
            _playerManager = FindObjectOfType<PlayerManager>();
        }

        private IEnumerator LoadFirstScene()
        {
            yield return SceneManager.LoadSceneAsync(firstSceneToLoad, LoadSceneMode.Additive);
            var scene = SceneManager.GetSceneByName(firstSceneToLoad);
            _currentScene = firstSceneToLoad;
            SceneManager.SetActiveScene(scene);
        }

        public void LoadScene(string newScene)
        {
            loadRoutine = LoadNewScene(newScene);
            StartCoroutine(loadRoutine);
        }

        private IEnumerator LoadNewScene(string newScene)
        {
            
            /*
             * TODO: add scene loading logic
             * TODO: ensure the same scene doesnt load twice
             * TODO: unload previous scene
             * TODO: load new scene
             * TODO: fades/cuts between puzzles
             */
            
            if(newScene.Equals("Exit"))
                Application.Quit();

            if (_currentScene.Equals(newScene)) yield break;
            yield return SceneManager.UnloadSceneAsync(_currentScene);

            yield return SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);

            _currentScene = newScene;
            var scene = SceneManager.GetSceneByName(_currentScene);
            SceneManager.SetActiveScene(scene);
            
            //reset player stuff
            onSceneReload?.Invoke();
            FindObjectOfType<PuzzleController>().Reload();
        }

        public void ReloadScene()
        {
            loadRoutine = RestartScene();
            StartCoroutine(loadRoutine);
        }

        private IEnumerator RestartScene()
        {
            Debug.Log(_currentScene);
            yield return SceneManager.UnloadSceneAsync(_currentScene);

            yield return SceneManager.LoadSceneAsync(_currentScene, LoadSceneMode.Additive);
            var scene = SceneManager.GetSceneByName(_currentScene);
            SceneManager.SetActiveScene(scene);
            
            //reset player stuff
            onSceneReload?.Invoke();
        }
    }
}
