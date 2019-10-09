﻿using System.Collections.Generic;
using Player;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene
{
    public class PuzzleController : MonoBehaviour
    {

        [SerializeField] private Transform harmonyPosition;
        [SerializeField] private Transform chaosPosition;

        [SerializeField] private bool canUseAbility = true;

        private List<GameObject> _objectsInScene = new List<GameObject>();

        public bool CanUseAbilty => canUseAbility;

        void Start()
        {
            var player_manager = FindObjectOfType<PlayerManager>();

            //settings harmonies position to the spawn position of the puzzle
            var harmonyrb = player_manager.Harmony.GetComponent<Rigidbody2D>();
            harmonyrb.position = harmonyPosition.position;
            player_manager.Harmony.transform.rotation = quaternion.identity;
            player_manager.Harmony.GetComponent<PlayerInteraction>().CanUseInteraction = canUseAbility;
            //player_manager.Harmony.SetActive(true);

            //settings chaos position to the spawn position of the puzzle
            var chaosrb = player_manager.Chaos.GetComponent<Rigidbody2D>();
            chaosrb.position = chaosPosition.position;
            player_manager.Chaos.transform.rotation = quaternion.identity;
            player_manager.Chaos.GetComponent<PlayerInteraction>().CanUseInteraction = canUseAbility;
            //player_manager.Chaos.SetActive(true);

            var rootObjects = new List<GameObject>();
            var scene = SceneManager.GetActiveScene();
            scene.GetRootGameObjects(rootObjects);

            // iterate root objects and do something
            for (int i = 0; i < rootObjects.Count; ++i)
            {
                _objectsInScene.Add(rootObjects[i]);
            }
            
            SceneLoader.Instance.onSceneReload.AddListener(OnSceneReload);
        }

        private void OnSceneReload()
        {
            var active = SceneManager.GetActiveScene();
            foreach (var obj in _objectsInScene)
            {
                SceneManager.MoveGameObjectToScene(obj, active);
            }
            SceneLoader.Instance.onSceneReload.RemoveListener(OnSceneReload);
        }
    }
}
