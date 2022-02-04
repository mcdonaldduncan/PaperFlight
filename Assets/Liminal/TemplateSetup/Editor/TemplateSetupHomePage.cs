using UnityEditor;
using UnityEngine;

namespace Liminal.Editor.TemplateSetup
{
    public class TemplateSetupHomePage
        : TemplateSetupPage
    {
        public override bool CanProceed => true;
        public override string Name => "Welcome";

        private bool _initialized;
        private Texture _icon;

        public override void DrawPage()
        {
            Init();

            GUILayout.BeginHorizontal();
            GUILayout.Space(16);
            GUILayout.BeginVertical();
            GUILayout.Space(16);
            GUILayout.Label(_icon, new GUIStyle(GUI.skin.label){alignment = TextAnchor.MiddleCenter}, GUILayout.Height(128));
            GUILayout.BeginVertical();
            GUILayout.Label("Welcome to your new Liminal project!", new GUIStyle("AM MixerHeader2"){alignment = TextAnchor.MiddleCenter});
            GUILayout.EndVertical();
            GUILayout.Space(8);
            GUILayout.Label("Congratulations on starting a new Liminal project. Before you can get started a few things must be set up.", new GUIStyle("AM HeaderStyle") { wordWrap = true });
            GUILayout.EndVertical();
            GUILayout.Space(16);
            GUILayout.EndHorizontal();
            GUILayout.Space(16);
        }

        private void Init()
        {
            if (_initialized)
                return;

            var ids = AssetDatabase.FindAssets("LiminalTemplateIcon t:Texture");
            if (ids.Length == 1)
            {
                var readmeObject = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(ids[0]));
                _icon = readmeObject as Texture;
            }

            _initialized = true;
        }
    }
}