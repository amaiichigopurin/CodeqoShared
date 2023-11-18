using UnityEditor;

namespace CodeqoEditor
{
    public class EditorInputFieldPopup : EditorPopupBase<EditorInputFieldPopup, string>
    {
        protected override string DrawContent(string value)
        {
            return EditorGUILayout.TextArea(value);
        }
    }
}