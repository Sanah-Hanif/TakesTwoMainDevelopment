using System;
using System.Collections.Generic;
using System.Linq;
using CameraScripts;
using Cinemachine;
using Nodule;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interaction.Level_Elements
{
    public class EnvironmentInteraction : InteractionController
    {
        [SerializeField] protected Color rayColour = Color.white;
        [SerializeField] protected Color noduleColour = Color.white;
        [SerializeField] protected GameObject nodulePrefab;
        [SerializeField] protected SpriteRenderer aura;
        [SerializeField] protected List<LevelInteraction> dependancies = new List<LevelInteraction>();

        private LevelInteraction _child;
        
        protected InputActionMap PlayerMovement;
        protected InputActionMap PlayerAbility;

        private CinemachineTargetGroup _target;
        private TargetGroupController _targetGroupController;

        private void Awake()
        {
            aura.color = noduleColour;
            _target = FindObjectOfType<CinemachineTargetGroup>();
            _child = GetComponentInChildren<LevelInteraction>();
            _targetGroupController = _target.GetComponent<TargetGroupController>();
        }

        private void OnValidate()
        {
            aura.color = noduleColour;
            _child = GetComponentInChildren<LevelInteraction>();
            //target = FindObjectOfType<CinemachineTargetGroup>();
            //_targetGroupController = target.GetComponent<TargetGroupController>();
        }

        public void DrawNodules()
        {
            foreach (var obj in dependancies)
            {
                GameObject newNodule = Instantiate(nodulePrefab, obj.transform.position + Vector3.left, quaternion.identity);
                newNodule.GetComponent<SpriteRenderer>().color = noduleColour;
                obj.GetComponentInParent<NoduleController>().AddNodule(this, newNodule);
            }
        }

        public override void Interact()
        {
            _child.Interact();
            if(dependancies.Count == 0)
                return;
            foreach (var interaction in dependancies.Where(interaction => interaction))
            {
                interaction.Interact();
                if(_targetGroupController != null && _target != null && _target.FindMember(interaction.transform) == -1)
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
