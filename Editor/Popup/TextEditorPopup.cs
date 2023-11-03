using UnityEditor;

namespace CodeqoEditor
{
    public class TextEditorPopup : EditorPopupBase<TextEditorPopup, string>
    {
        protected override string DrawContent(string value)
        {
            return EditorGUILayout.TextArea(value);
        }
    }
}