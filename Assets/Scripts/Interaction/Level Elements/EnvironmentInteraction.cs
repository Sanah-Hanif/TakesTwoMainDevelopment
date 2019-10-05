using System.Collections.Generic;
using System.Linq;
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

        private void Awake()
        {
            target = FindObjectOfType<CinemachineTargetGroup>();
            child = GetComponentInChildren<LevelInteraction>();
        }

        public override void Interact()
        {
            child.Interact();
            if(dependancies.Count == 0)
                return;
            CancelInvoke(nameof(RemoveTarget));
            foreach (var interaction in dependancies.Where(interaction => interaction))
            {
                Debug.Log(target.FindMember(interaction.transform));
                if(target.FindMember(interaction.transform) == -1)
                    target.AddMember(interaction.transform, 5, 5);
                interaction.Interact();
            }
            
            Invoke(nameof(RemoveTarget), 2f);
        }

        void RemoveTarget()
        {
            foreach (var interaction in dependancies.Where(interaction => interaction))
            {
                if(target.FindMember(interaction.transform) != -1)
                    target.RemoveMember(interaction.transform);
            }
            
        }
        
    }
}
