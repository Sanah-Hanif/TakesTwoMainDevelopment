using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Interaction.Level_Elements
{
    public abstract class ToggleInteraction : LevelInteraction
    {

        protected int toggled = 0;
        protected  Sequence movement;
        
        [SerializeField] protected float tweenDuration = 1.5f;

        public override void Interact()
        {
            Toggle();
        }

        private void Toggle()
        {
            toggled += 1;
            toggled %= 2;
        }
    }
}