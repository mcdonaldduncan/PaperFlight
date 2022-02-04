using UnityEditor;
using UnityEngine;

namespace Liminal.Editor.TemplateSetup
{
    public class TemplateSetupNamePage
        : TemplateSetupPage
    {
        public TemplateSetupNamePage(string companyName, string appName)
        {
            _companyName = companyName;
            _appName = appName;
        }

        private string _companyName;
        private string _appName;
        public string ApplicationIdentifier => $"com.{_companyName}.{_appName}";
        public override bool CanProceed => !string.IsNullOrWhiteSpace(_companyName) && !string.IsNullOrWhiteSpace(_appName) && _companyName != "TemplateCompany" && _appName != "TemplateLimapp";

        public override string Name => "Project Name";
        public override void DrawPage()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(16);
            GUILayout.BeginVertical();
            GUILayout.Space(16);
            GUILayout.Label("Choose a project name:", "AM MixerHeader2");
            GUILayout.Space(8);
            GUILayout.Label("Please let us know what you will be calling the project (don't worry, you can change this later), as well as the name of your team!.", new GUIStyle("AM HeaderStyle") { wordWrap = true });

            GUILayout.Space(8);
            EditorGUI.BeginChangeCheck();
            _companyName = EditorGUILayout.TextField("Company Name", _companyName);
            _companyName = _companyName.AlphaNumOnly();
            _appName = EditorGUILayout.TextField("Project Name", _appName);
            _appName = _appName.AlphaNumOnly();
            if (EditorGUI.EndChangeCheck())
            {
                PlayerSettings.companyName = _companyName;
                PlayerSettings.productName = _appName;
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, ApplicationIdentifier);
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, ApplicationIdentifier);
            }

            EditorGUILayout.LabelField("Application Identifier", ApplicationIdentifier, (GUIStyle)"MeTimeBlockLeft");

            GUILayout.EndVertical();
            GUILayout.Space(16);
            GUILayout.EndHorizontal();
            GUILayout.Space(16);
        }
    }
}