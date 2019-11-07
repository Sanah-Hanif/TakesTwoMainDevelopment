using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UI;
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

        private const string MenuScene = "MainMenuAdditive";

        private string _currentScene;
        private string _nextScene;

        private static SceneLoader _sceneLoader;

        private IEnumerator _loadRoutine;

        public static SceneLoader Instance => _sceneLoader;

        private PlayerManager _playerManager;
        [SerializeField] private TransitionManager transition;

        private void Awake()
        {
            //singleton stuff
            if (_sceneLoader == null)
                _sceneLoader = this;
            else if (_sceneLoader != this)
                Destroy(gameObject);
            DontDestroyOnLoad(gameObject);

            //if the editor is open, use the active scene, if not, load the first scene we need
            if (!Application.isEditor)
            {
                firstSceneToLoad = MenuScene;
                _loadRoutine = LoadFirstScene();
                StartCoroutine(_loadRoutine);
            }
            else if (loadSceneOnStart)
            {
                _loadRoutine = LoadFirstScene();
                StartCoroutine(_loadRoutine);
            }
            else
                _currentScene = SceneManager.GetActiveScene().name;
        }

        private void Start()
        {
            transition = FindObjectOfType<TransitionManager>();
            _playerManager = FindObjectOfType<PlayerManager>();
        }

        private IEnumerator LoadFirstScene()
        {
            yield return SceneManager.LoadSceneAsync(firstSceneToLoad, LoadSceneMode.Additive);
            var scene = SceneManager.GetSceneByName(firstSceneToLoad);
            _currentScene = firstSceneToLoad;
            SceneManager.SetActiveScene(scene);
            FindObjectOfType<PuzzleController>().Reload();
            yield return StartCoroutine(transition.FadeStart());
        }

        public void LoadScene(string newScene)
        {
            _loadRoutine = LoadNewScene(newScene);
            StartCoroutine(_loadRoutine);
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

            yield return StartCoroutine(transition.FadeIn());
            
            FindObjectOfType<PuzzleController>().OnSceneReload();
            yield return SceneManager.UnloadSceneAsync(_currentScene);

            yield return SceneManager.LoadSceneAsync(newScene, LoadSceneMode.Additive);

            _currentScene = newScene;
            var scene = SceneManager.GetSceneByName(_currentScene);
            SceneManager.SetActiveScene(scene);
            
            //reset player stuff
            onSceneReload?.Invoke();
            FindObjectOfType<PuzzleController>().Reload();
            
            yield return StartCoroutine(transition.FadeOut());
        }

        public void ReloadScene()
        {
            _loadRoutine = RestartScene();
            StartCoroutine(_loadRoutine);
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
