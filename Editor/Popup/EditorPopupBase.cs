using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CodeqoEditor
{
    public abstract class EditorPopupBase<Window, Value> : EditorWindow
        where Window : EditorPopupBase<Window, Value>
    {
        const float WINDOW_HEIGHT = 180f;
        const float WINDOW_WIDTH = 340f;

        //public string _title;
        public string _description;
        public Value _value;
        public List<Value> _valueList;
        public Action<Value> _callback;

        public static void ShowWindow(string title, Action<Value> onComplete, IEnumerable<Value> valueList = null)
        {
            ShowWindow((title, null), default, onComplete, valueList);
        }

        public static void ShowWindow(string title, Value defaultValue, Action<Value> onComplete, IEnumerable<Value> valueList = null)
        {
            ShowWindow((title, null), defaultValue, onComplete, valueList);
        }

        public static void ShowWindow((string, string) titleAndDescription, Action<Value> onComplete, IEnumerable<Value> valueList = null)
        {
            ShowWindow(titleAndDescription, default, onComplete, valueList);
        }

        public static void ShowWindow((string, string) titleAndDescription, Value defaultValue, Action<Value> onComplete, IEnumerable<Value> valueList = null)
        {
            if (onComplete == null) throw new ArgumentNullException(nameof(onComplete), "Callback cannot be null");

            try
            {
                var window = GetWindow<Window>(true, titleAndDescription.Item1, true);
                //window._title = titleAndDescription.Item1;
                window._description = titleAndDescription.Item2;
                window._callback = onComplete;
                window._value = defaultValue;
                window._valueList = valueList != null ? new List<Value>(valueList) : null;
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
            //EditorGUILayout.LabelField(_title, EditorStyles.boldLabel);
            //EditorGUILayout.Space();

            // Draw Description
            DrawDescription();
            EditorGUILayout.Space();

            // Draw Content
            _value = DrawContent(_value);
            GUILayout.FlexibleSpace();

            // Draw Buttons
            DrawButtons();

            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawDescription()
        {
            if (!string.IsNullOrEmpty(_description))
            {
                EditorGUILayout.LabelField(_description, EditorStyles.wordWrappedLabel);
            }
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