using Player;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(PlayerInputSystem))]
    public class PlayerInputEditor : UnityEditor.Editor
    {
        private PlayerInputSystem _system;
        private UnityEditor.Editor _settings;
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            DrawSettingsMenu(_system._settings, ref _system.foldout, ref _settings);
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
            _system = (PlayerInputSystem)target;
        }
    }
}
