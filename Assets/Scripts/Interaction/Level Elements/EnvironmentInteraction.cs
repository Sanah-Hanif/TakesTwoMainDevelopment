using System;
using System.Collections.Generic;
using System.Linq;
using CameraScripts;
using Cinemachine;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interaction.Level_Elements
{
    public class EnvironmentInteraction : InteractionController
    {
        [SerializeField] Color rayColour = Color.white;
        [SerializeField] private Color _noduleColour = Color.white;
        [SerializeField] private Sprite _noduleSprite;
        [SerializeField] private GameObject _nodulePrefab;
        [SerializeField] protected List<LevelInteraction> dependancies = new List<LevelInteraction>();
        
        private Dictionary<LevelInteraction, GameObject> _nodules = new Dictionary<LevelInteraction, GameObject>();

        private LevelInteraction child;
        
        protected InputActionMap PlayerMovement;
        protected InputActionMap PlayerAbility;

        private CinemachineTargetGroup target;
        private TargetGroupController _targetGroupController;

        private void Awake()
        {
            target = FindObjectOfType<CinemachineTargetGroup>();
            child = GetComponentInChildren<LevelInteraction>();
            _targetGroupController = target.GetComponent<TargetGroupController>();
        }

        private void OnValidate()
        {
            child = GetComponentInChildren<LevelInteraction>();
            //target = FindObjectOfType<CinemachineTargetGroup>();
            //_targetGroupController = target.GetComponent<TargetGroupController>();
        }

        public void DrawNodules()
        {
            var oldNodules = _nodules.Keys.ToArray();
            var oldNoduleValues = _nodules.Values.ToArray();
            
            if (_nodules.Count > 0)
            {
                for (int i = 0; i < _nodules.Count; i++)
                {
                    var check = _nodules.TryGetValue(oldNodules[i], out var checkObj);
                    if (check && checkObj == null)
                    {
                        DestroyImmediate(_nodules[oldNodules[i]].gameObject);
                        _nodules.Remove(oldNodules[i]);
                    }

                    if (check) continue;
                    DestroyImmediate(oldNodules[i].gameObject);
                    _nodules.Remove(oldNodules[i]);
                }
            }

            foreach (var obj in dependancies)
            {
                if(_nodules.ContainsKey(obj)) continue;
                Debug.Log(gameObject.name);
                GameObject newNodule = Instantiate(_nodulePrefab, obj.transform.position + Vector3.left, quaternion.identity,
                    obj.transform.parent);
                newNodule.GetComponent<SpriteRenderer>().sprite = _noduleSprite;
                newNodule.GetComponent<SpriteRenderer>().color = _noduleColour;
                _nodules.Add(obj, newNodule);
            }
        }

        public override void Interact()
        {
            child.Interact();
            if(dependancies.Count == 0)
                return;
            foreach (var interaction in dependancies.Where(interaction => interaction))
            {
                interaction.Interact();
                if(_targetGroupController != null && target != null && target.FindMember(interaction.transform) == -1)
                    _targetGroupController.AddObjectToTargetGroup(interaction.gameObject);
            }
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var obj in dependancies)
            {
                Gizmos.color = rayColour;
                Gizmos.DrawLine(transform.position, obj.transform.position);
            }
        }
    }
}
