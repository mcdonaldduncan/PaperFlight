using UnityEditor;
using UnityEngine;

namespace Liminal.Editor.TemplateSetup
{
    public class TemplateSetupInfoPage
        : TemplateSetupPage
    {
        public override bool CanProceed => true;
        public override string Name => "Info";

        public override void DrawPage()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(16);
            GUILayout.BeginVertical();
            GUILayout.Space(16);
            GUILayout.Label("That's it!", "AM MixerHeader2");
            GUILayout.Space(8);
            GUILayout.Label("You are ready to start creating your experience for the Liminal Platform. This template has already setup your scene and VRAvatar, just start creating using the <color=#00CAFF>[ExperienceApp]</color> GameObject as your new scene root.", new GUIStyle("AM HeaderStyle") { wordWrap = true, richText = true});
            GUILayout.Label("You can find more information on the Liminal SDK Wiki, or join the Liminal VR Partner's Slack:", new GUIStyle("AM HeaderStyle") { wordWrap = true });
            GUILayout.Space(8);
            if (LinkLabel(new GUIContent("Liminal Wiki")))
            {
                Application.OpenURL(@"https://www.notion.so/Liminal-Partners-Wiki-a1c244df9e564b48bd0fbcb6f9becbfc");
            }
            if (LinkLabel(new GUIContent("Liminal Partner's Slack")))
            {
                Application.OpenURL(@"https://liminalvrpartners.slack.com");
            }
            GUILayout.Space(8);
            GUILayout.EndVertical();
            GUILayout.Space(16);
            GUILayout.EndHorizontal();
            GUILayout.Space(16);
        }

        bool LinkLabel(GUIContent label)
        {
            var linkStyle = new GUIStyle("AM HeaderStyle") {wordWrap = true};
            linkStyle.wordWrap = false;
            linkStyle.normal.textColor = new Color(0x00 / 255f, 0x78 / 255f, 0xDA / 255f, 1f);
            linkStyle.stretchWidth = false;

            var button = GUILayout.Button(label, linkStyle);
            var position = GUILayoutUtility.GetLastRect();

            Handles.BeginGUI();
            Handles.color = linkStyle.normal.textColor;
            Handles.DrawLine(new Vector3(position.xMin, position.yMax), new Vector3(position.xMax, position.yMax));
            Handles.color = Color.white;
            Handles.EndGUI();

            EditorGUIUtility.AddCursorRect(position, MouseCursor.Link);

            return button;
        }
    }
}