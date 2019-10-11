using System;
using System.Collections.Generic;
using System.Linq;
using CameraScripts;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interaction.Level_Elements
{
    public class EnvironmentInteraction : InteractionController
    {
        [SerializeField] protected List<LevelInteraction> dependancies = new List<LevelInteraction>();

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
            target = FindObjectOfType<CinemachineTargetGroup>();
            _targetGroupController = target.GetComponent<TargetGroupController>();
        }

        public override void Interact()
        {
            child.Interact();
            if(dependancies.Count == 0)
                return;
            foreach (var interaction in dependancies.Where(interaction => interaction))
            {
                if(target.FindMember(interaction.transform) == -1)
                    _targetGroupController.AddObjectToTargetGroup(interaction.gameObject);
                interaction.Interact();
            }
        }

        private void OnDrawGizmosSelected()
        {
            foreach (var obj in dependancies)
            {
                Gizmos.DrawLine(transform.position, obj.transform.position);
            }
        }
    }
}
