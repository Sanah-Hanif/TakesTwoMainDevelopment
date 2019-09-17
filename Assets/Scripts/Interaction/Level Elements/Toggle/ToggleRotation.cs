using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Interaction.Level_Elements.Toggle
{
    public class ToggleRotation : ToggleInteraction
    {
        [SerializeField] private List<Vector3> rotation = new List<Vector3>();

        private void Awake()
        {
            if (rotation.Count > 0)
                transform.DORotate(rotation[0], tweenDuration);
        }

        public override void Interact()
        {
            base.Interact();
            if(toggled < rotation.Count)
                transform.DORotate(rotation[toggled], tweenDuration);
        }
    }
}
