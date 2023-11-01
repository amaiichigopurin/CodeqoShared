using UnityEditor;
using UnityEngine;

namespace CodeqoEditor
{
    [CustomPropertyDrawer(typeof(EditorTexture2D))]
    public class EditorTexture2DDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var lightTex = property.FindPropertyRelative("light");
            var darkTex = property.FindPropertyRelative("dark");

            position.height = EditorGUIUtility.singleLineHeight;

            // Indent label
            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
            Rect lightTexRect = new Rect(labelRect.xMax, position.y, (position.width - labelRect.width) * 0.5f, position.height);
            Rect darkTexRect = new Rect(lightTexRect.xMax, position.y, (position.width - labelRect.width) * 0.5f, position.height);


            // Draw fields
            EditorGUI.LabelField(labelRect, label);
            EditorGUI.PropertyField(lightTexRect, lightTex, GUIContent.none);
            EditorGUI.PropertyField(darkTexRect, darkTex, GUIContent.none);
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }

}