using UnityEditor;
using UnityEngine;

namespace CodeqoEditor
{
    public abstract class PaddedEditorWindow : EditorWindow
    {
        const int PADDING_LEFT = 12;
        const int PADDING_TOP = 10;
        const int PADDING_RIGHT = 12;
        const int PADDING_BOTTOM = 10;

        private void OnGUI()
        {
            // Top padding
            GUILayout.Space(PADDING_TOP);

            // Start horizontal group for left padding
            GUILayout.BeginHorizontal();
            GUILayout.Space(PADDING_LEFT);

            GUILayout.BeginVertical();
            
            // Your GUI content goes here
            OnGUIUpdate();

            GUILayout.EndVertical();
            // End horizontal group for right padding
            GUILayout.Space(PADDING_RIGHT);
            GUILayout.EndHorizontal();

            // Bottom padding
            GUILayout.Space(PADDING_BOTTOM);
        }

        protected abstract void OnGUIUpdate();
    }
}
