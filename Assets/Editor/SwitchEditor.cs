using System;
using Interaction.Level_Elements;
using Nodule;
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
            using (var check = new EditorGUI.ChangeCheckScope())
            {
                base.OnInspectorGUI();
                if (GUILayout.Button("Toggle"))
                {
                    _interaction.Interact();
                }

                if (!check.changed)
                {
                    return;
                }
                var nodules = FindObjectsOfType<NoduleController>();
                var environments = FindObjectsOfType<EnvironmentInteraction>();

                foreach (var obj in nodules)
                {
                    obj.ClearNodules();
                }

                foreach (var obj in environments)
                {
                    obj.DrawNodules();
                }
            }
        }
    }
}
