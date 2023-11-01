using System;
using UnityEditor;
using UnityEngine;

namespace CodeqoEditor
{
    public class CUITextWindow : EditorWindow
    {
        static string _title;
        static string _text;
        static Action<string> _callback = null;
        static float _height = 400;

        public static void Create(string title, string text, Action<string> onComplete)
        {
            _title = title;
            _text = text;
            _callback = onComplete;
            _height = 400;
            Init(title);
        }
        public static void Create(string title, Action<string> onComplete)
        {
            _title = title;
            _callback = onComplete;
            _height = 20;
            Init(title);
        }

        private static void Init(string title)
        {
            var window = (CUITextWindow)GetWindow(typeof(CUITextWindow), true, title, true);
            window.Show();
        }

        void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField(_title, EditorStyles.boldLabel);
            _text = EditorGUILayout.TextArea(_text, GUILayout.MaxHeight(_height));
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("OK", GUILayout.Height(30)))
            {
                _callback(_text);
                Close();
            }
            if (GUILayout.Button("Cancel", GUILayout.Height(30)))
            {
                Close();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
    }
}