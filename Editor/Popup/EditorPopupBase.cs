using System;
using UnityEditor;
using UnityEngine;

namespace CodeqoEditor
{
    public abstract class EditorPopupBase<Window, Value> : EditorWindow
        where Window : EditorPopupBase<Window, Value>
    {
        const float WINDOW_HEIGHT = 180f;
        const float WINDOW_WIDTH = 340f;
        
        public string _title;
        public string _description;
        public Value _value;
        public Action<Value> _callback;
        
        public static void ShowWindow(string title, Action<Value> onComplete)
        {
            ShowWindow(title, null, default, onComplete);
        }

        public static void ShowWindow(string title, Value defaultValue, Action<Value> onComplete)
        {            
            ShowWindow(title, null, defaultValue, onComplete);
        }
        public static void ShowWindow(string title, string description, Action<Value> onComplete)
        {
            ShowWindow(title, description, default, onComplete);
        }

        public static void ShowWindow(string title, string description, Value defaultValue, Action<Value> onComplete) 
        {
            if (onComplete == null) throw new ArgumentNullException(nameof(onComplete), "Callback cannot be null");

            try
            {
                var window = GetWindow<Window>(true, title, true);
                window._title = title;
                window._description = description;
                window._callback = onComplete;
                window._value = defaultValue;
                window.minSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
                window.maxSize = new Vector2(WINDOW_WIDTH, WINDOW_HEIGHT);
                window.Show();
            }
            catch
            {
                Debug.LogError("Failed to create window of type " + typeof(Window));
            }              
        }

        void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            // Draw Title and Description
            EditorGUILayout.LabelField(_title, EditorStyles.boldLabel);
            EditorGUILayout.Space();

            if (!string.IsNullOrEmpty(_description))
            {
                EditorGUILayout.LabelField(_description, EditorStyles.wordWrappedLabel);
                EditorGUILayout.Space();
            }

            // Draw Content
            _value = DrawContent(_value);
            EditorGUILayout.Space();

            // Draw Buttons
            DrawButtons();

            EditorGUILayout.EndVertical();
        }

        protected abstract Value DrawContent(Value value);

        void DrawButtons()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("OK", GUILayout.Height(30)))
            {
                _callback?.Invoke(_value);
                Close();
            }
            if (GUILayout.Button("Cancel", GUILayout.Height(30)))
            {
                Close();
            }
            EditorGUILayout.EndHorizontal();
        }

        protected void SetDefaultValue(Value value)
        {
            _value = value;
        }
    }
}