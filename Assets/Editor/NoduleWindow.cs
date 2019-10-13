using System;
using System.Linq;
using Interaction.Level_Elements;
using Nodule;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class NoduleWindow : EditorWindow
    {
        private void OnGUI()
        {
            if (GUILayout.Button("Create Nodules"))
            {
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

            if (GUILayout.Button("Clear Nodules"))
            {
                var nodules = FindObjectsOfType<NoduleController>();
                foreach (var obj in nodules)
                {
                    obj.ClearNodules();
                }
            }
        }
        
        [MenuItem ("Nodule/Nodule Window")]
        public static void  ShowWindow () {
            EditorWindow.GetWindow(typeof(NoduleWindow));
        }
    }
}
