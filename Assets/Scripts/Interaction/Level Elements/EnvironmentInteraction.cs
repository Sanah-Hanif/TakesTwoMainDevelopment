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
        [SerializeField] Color rayColour = Color.white;
        [SerializeField] protected Color _noduleColour = Color.white;
        [SerializeField] protected GameObject _nodulePrefab;
        [SerializeField] protected List<LevelInteraction> dependancies = new List<LevelInteraction>();
        
        private SpriteRenderer Aura;

        private LevelInteraction child;
        
        protected InputActionMap PlayerMovement;
        protected InputActionMap PlayerAbility;

        private CinemachineTargetGroup target;
        private TargetGroupController _targetGroupController;

        private void Awake()
        {
            Aura = GetComponent<SpriteRenderer>();
            Aura.color = _noduleColour;
            target = FindObjectOfType<CinemachineTargetGroup>();
            child = GetComponentInChildren<LevelInteraction>();
            _targetGroupController = target.GetComponent<TargetGroupController>();
        }

        private void OnValidate()
        {
            Aura = GetComponent<SpriteRenderer>();
            Aura.color = _noduleColour;
            child = GetComponentInChildren<LevelInteraction>();
            //target = FindObjectOfType<CinemachineTargetGroup>();
            //_targetGroupController = target.GetComponent<TargetGroupController>();
        }

        public void DrawNodules()
        {
            foreach (var obj in dependancies)
            {
                GameObject newNodule = Instantiate(_nodulePrefab, obj.transform.position + Vector3.left, quaternion.identity);
                newNodule.GetComponent<SpriteRenderer>().color = _noduleColour;
                obj.GetComponentInParent<NoduleController>().AddNodule(this, newNodule);
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
