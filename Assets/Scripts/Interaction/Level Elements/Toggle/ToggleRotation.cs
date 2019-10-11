using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor;
using UnityEngine;

namespace Interaction.Level_Elements.Toggle
{
    public class ToggleRotation : ToggleInteraction
    {
        [SerializeField] private List<Vector3> rotation = new List<Vector3>();

        private void Awake()
        {
            if (rotation.Count > 0)
                transform.rotation = Quaternion.Euler(rotation[toggled]);
        }

        private void OnValidate()
        {
            if (rotation.Count > 0)
                transform.rotation = Quaternion.Euler(rotation[toggled]);
        }

        public override void Interact()
        {
            base.Interact();
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying)
            {
                if (toggled < rotation.Count)
                    transform.rotation = Quaternion.Euler(rotation[toggled]);
            }
            else if(toggled < rotation.Count)
                transform.DORotate(rotation[toggled], tweenDuration);
#else
            if(toggled < rotation.Count)
                transform.DORotate(rotation[toggled], tweenDuration);
#endif
        }
    }
}
