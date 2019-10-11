using CameraScripts;
using UnityEditor;
using UnityEngine;


//code snippets taken from https://github.com/SebLague/Procedural-Planets/blob/master/Procedural%20Planet%20E02/Assets/Editor/PlanetEditor.cs credits to Sebastian Lague
namespace Editor
{
    [CustomEditor(typeof(TargetGroupController))]
    public class TargetControllerEditor : UnityEditor.Editor
    {
        private TargetGroupController _controller;
        private UnityEditor.Editor _settings;

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            if (_controller.settings.startWeight > _controller.settings.endWeight)
                _controller.settings.endWeight = _controller.settings.startWeight;
            
            if (_controller.settings.startRadius > _controller.settings.endRadius)
                _controller.settings.endRadius = _controller.settings.startRadius;
            DrawSettingsMenu(_controller.settings, ref _controller.foldout, ref _settings);
        }

        private void DrawSettingsMenu(Object settings, ref bool fold, ref UnityEditor.Editor editor)
        {
            if (settings == null) return;
            fold = EditorGUILayout.InspectorTitlebar(fold, settings);
            if (!fold) return;
            CreateCachedEditor(settings, null, ref editor); 
            editor.OnInspectorGUI();
        }

        private void OnEnable()
        {
            _controller = (TargetGroupController)target;
        }
    }
}
