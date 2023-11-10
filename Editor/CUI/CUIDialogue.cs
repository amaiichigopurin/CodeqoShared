using System;
using UnityEditor;

namespace CodeqoEditor
{
    public class CUIDialogue
    {
        public static bool Warning(string message) => EditorUtility.DisplayDialog("Warning", message, "Yes", "Cancel");
        public static bool Error(string message) => EditorUtility.DisplayDialog("Error", message, "Ok");
        public static bool Error(Exception exception) => EditorUtility.DisplayDialog("Error", exception.Message, "Ok");
    }
}
