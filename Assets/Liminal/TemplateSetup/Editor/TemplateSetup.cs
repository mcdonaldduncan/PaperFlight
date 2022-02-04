using System.IO;
using Newtonsoft.Json;

namespace Liminal.Editor.TemplateSetup
{
    using UnityEngine;
    using UnityEditor;

    [InitializeOnLoad]
    public class TemplateSetup
        : MonoBehaviour
    {
        public static string SetupInfoPath => Application.dataPath + "/Liminal/TemplateSetup/liminal_setup";
        public static bool IsSetup { get; set; }

        static TemplateSetup()
        {
            if (File.Exists(SetupInfoPath))
            {
                IsSetup = true;
                return;
            }

            IsSetup = false;

            EditorApplication.update += Init;
        }

        [MenuItem("Liminal/Reset Setup Key")]
        private static void ResetKey()
        {
            File.Delete(SetupInfoPath);
            IsSetup = false;
        }

        private static void Init()
        {
            EditorApplication.update -= Init;
            TemplateSetupWindow.OpenWindow();
        }
    }
}