using System;
using Interaction.Level_Elements;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(EnvironmentInteraction), true)]
    public class SwitchEditor : UnityEditor.Editor
    {
        private EnvironmentInteraction _interaction;

        private void Awake()
        {
            _interaction = (EnvironmentInteraction)target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if (GUILayout.Button("Toggle"))
            {
                _interaction.Interact();
            }
        }
    }
}
