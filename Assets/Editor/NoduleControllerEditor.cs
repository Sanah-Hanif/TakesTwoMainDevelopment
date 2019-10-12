using System;
using Nodule;
using UnityEngine;
using UnityEditor;

namespace Editor
{
    [CustomEditor(typeof(NoduleController))]
    public class NoduleControllerEditor : UnityEditor.Editor
    {
        private NoduleController _controller;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Clear Nodules"))
            {
                _controller.ClearNodules();
            }
        }

        private void Awake()
        {
            _controller = (NoduleController) target;
        }
    }
}
