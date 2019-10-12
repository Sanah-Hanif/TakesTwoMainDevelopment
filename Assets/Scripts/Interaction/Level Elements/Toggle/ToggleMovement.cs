using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEditor;

namespace Interaction.Level_Elements.Toggle
{
    public class ToggleMovement : ToggleInteraction
    {
        [SerializeField] private List<Transform> positions = new List<Transform>();

        private void Awake()
        {
            movement = DOTween.Sequence();
            if (positions.Count > 0)
                transform.position = positions[0].position;
        }

        private void OnValidate()
        {
            if (positions.Count > 0)
                transform.position = positions[0].position;
        }

        public override void Interact()
        {
            base.Interact();
            #if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                if (toggled < positions.Count)
                    transform.position = positions[toggled].position;
            }
            else if (toggled < positions.Count)
            {
                transform.DOMove(positions[toggled].position, tweenDuration);
            }
            #else
            if (toggled < positions.Count)
                transform.DOMove(positions[toggled].position, tweenDuration);
            #endif
        }
        
        
        #if UNITY_EDITOR
        private void OnWillRenderObject()
        {
            transform.position = positions[toggled].position;
        }
        #endif
    }
}
