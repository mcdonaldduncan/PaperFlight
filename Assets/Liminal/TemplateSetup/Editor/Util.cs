using System.Text.RegularExpressions;

namespace Liminal.Editor.TemplateSetup
{
    public static class Util
    {
        public static string AlphaNumOnly(this string str)
        {
            return string.IsNullOrWhiteSpace(str)
                ? string.Empty
                : Regex.Replace(str, @"[^a-zA-Z0-9 ]", "");
        }
    }
}