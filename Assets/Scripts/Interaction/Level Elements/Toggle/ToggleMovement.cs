using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Interaction.Level_Elements.Toggle
{
    public class ToggleMovement : ToggleInteraction
    {
        [SerializeField] private List<Transform> positions = new List<Transform>();
        

        private void Awake()
        {
            movement = DOTween.Sequence();
            if (positions.Count > 0)
                transform.DOMove(positions[0].position, tweenDuration);
        }

        public override void Interact()
        {
            base.Interact();
            if(toggled < positions.Count)
                transform.DOMove(positions[toggled].position, tweenDuration);
        }
    }
}
