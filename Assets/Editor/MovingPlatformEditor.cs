using System;
using Interaction.Level_Elements;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(MovingPlatform))]
    public class MovingPlatformEditor : UnityEditor.Editor
    {

        private MovingPlatform _platform;

        private void Awake()
        {
            _platform = (MovingPlatform) target;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Next Position"))
            {
                _platform.NextPosition();
            }

            if (GUILayout.Button("Reset Position"))
            {
                _platform.ResetPosition();
            }

            if (GUILayout.Button("Set New Position"))
            {
                _platform.SetPosition();
            }
        }
    }
}

