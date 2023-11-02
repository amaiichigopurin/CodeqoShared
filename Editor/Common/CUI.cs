using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace CodeqoEditor
{
    public static class CUI
    {
        private static Dictionary<string, GUIStyle> cachedStyles = new Dictionary<string, GUIStyle>();

        private static GUISkin _skin;
        public static GUISkin skin => _skin ?? (_skin = EditorSkin.skin);

        private static GUIStyle _box;
        public static GUIStyle box
        {
            get
            {
                if (_box == null)
                {
                    _box = new GUIStyle(GUI.skin.box)
                    {
                        margin = new RectOffset(2, 2, 2, 2),
                        border = new RectOffset(10, 10, 10, 10),
                        padding = new RectOffset(6, 6, 6, 6)
                    };
                }
                return _box;
            }
        }

        public static string CurrentField
        {
            get => EditorPrefs.GetString("CUI.CurrentField", "");
            set
            {
                EditorPrefs.SetString("CUI.CurrentField", value);
                GUI.SetNextControlName(value);
            }
        }

        public static string GetFocus() => GUI.GetNameOfFocusedControl();

        public static Rect GetHeaderRect(Rect r, float indent = 10, float margin = 6, float width = 22, float height = 22)
            => new Rect(r.x + indent, r.y + margin, width, height);

        public static GUIStyle Background()
        {
            string key = "background";
            if (!cachedStyles.ContainsKey(key))
            {
                cachedStyles.Add(key, new GUIStyle 
                { 
                    normal = { background = EditorTexture.Background } 
                });
            }
            return cachedStyles[key];
        }

        private static GUIStyle BoxInternal(CUIColor color, RectOffset margin)
        {
            string key = $"{color}_{margin.left},{margin.right},{margin.top},{margin.bottom}";
            if (!cachedStyles.ContainsKey(key))
            {
                cachedStyles.Add(key, new GUIStyle
                {
                    border = new RectOffset(10, 10, 10, 10),
                    margin = margin,
                    padding = new RectOffset(6, 6, 6, 6),
                    normal = { background = EditorTexture.Box(color) }
                });
            }
            return cachedStyles[key];
        }
        public static GUIStyle Box(CUIColor color)
            => BoxInternal(color, new RectOffset(0, 0, 0, 0));
        public static GUIStyle Box(int margin = 0, CUIColor color = CUIColor.None)
            => BoxInternal(color, new RectOffset(margin, margin, margin, margin));
        public static GUIStyle Box(int margin)
            => BoxInternal(0, new RectOffset(margin, margin, margin, margin));
        public static GUIStyle Box(int left, int right, int top, int bottom, CUIColor color = CUIColor.None)
            => BoxInternal(color, new RectOffset(left, right, top, bottom));
        

        private static GUIStyle BorderInternal(BorderDirection direction, RectOffset padding)
        {
            string key = $"{direction}_{padding.left},{padding.right},{padding.top},{padding.bottom}";
            if (!cachedStyles.ContainsKey(key))
            {
                Texture2D boxTex = direction == BorderDirection.Top ? EditorTexture.BorderTop : EditorTexture.BorderBottom;
                cachedStyles.Add(key, new GUIStyle
                {
                    border = new RectOffset(10, 10, 10, 10),
                    margin = new RectOffset(0, 0, 0, 0),
                    padding = padding,
                    normal = { background = boxTex }
                });
            }
            return cachedStyles[key];
        }

        public static GUIStyle Border(BorderDirection direction)
            => BorderInternal(direction, new RectOffset(0, 0, 0, 0));
        public static GUIStyle Border(int margin, BorderDirection direction)
            => BorderInternal(direction, new RectOffset(margin, margin, margin, margin));
        public static GUIStyle Border(int left, int right, int top, int bottom, BorderDirection direction)
            => BorderInternal(direction, new RectOffset(left, right, top, bottom));

        public static bool Foldout(Rect position, string label, Action callback)
        {
            // draw lines on top and bottom of the foldout label
            CUIUtility.DrawHorizontalLine(1);
            GUILayout.Space(5);
            bool b = EditorPrefs.GetBool(label, true);
            b = EditorGUI.Foldout(position, b, label);
            EditorPrefs.SetBool(label, b);

            if (b)
            {
                GUILayout.Space(5);
                CUIUtility.DrawHorizontalLine(1);

                GUIStyle style = new GUIStyle();
                style.margin = new RectOffset(0, 0, 10, 10);

                GUILayout.BeginVertical(style);
                callback?.Invoke();
                GUILayout.EndVertical();
            }
            else
            {
                GUILayout.Space(5);
                CUIUtility.DrawHorizontalLine(1);
            }

            return b;
        }

        public static void ColorLabelField(Rect rect, string label, Color color, bool bold = true)
        {
            var style = bold ? new GUIStyle(EditorStyles.boldLabel) : new GUIStyle(GUI.skin.label);
            style.normal.textColor = color;

            EditorGUI.LabelField(rect, label, style);
        }

        public static void DrawTexture(Rect textureRect, Texture2D texture)
        {
            GUI.DrawTexture(textureRect, texture, ScaleMode.ScaleToFit);
        }

        public static void DrawContent(Rect textureRect, GUIContent content)
        {
            GUI.DrawTexture(textureRect, content.image, ScaleMode.ScaleToFit);
        }

        public static string ListDropdownField(Rect rect, string currentValue, IList<string> list, GUIContent label = null)
        {
            return GenericDropdownField(rect, currentValue, list, label);
        }

        public static T GenericDropdownField<T>(Rect rect, T currentValue, IList<T> list, GUIContent label = null)
        {
            if (list == null || list.Count == 0)
            {
                EditorGUI.HelpBox(rect, "No list found.", MessageType.None);
                return default;
            }

            if (label != null)
            {
                rect = EditorGUI.PrefixLabel(rect, label);
            }

            int index = list.IndexOf(currentValue);
            string[] options = Array.ConvertAll(list.ToArray(), item => item.ToString());

            index = EditorGUI.Popup(rect, index, options);
            return list[Mathf.Max(index, 0)];
        }

        public static void DrawCircle(Rect rect, Texture2D tex)
        {
            GUI.DrawTexture(rect, tex, ScaleMode.ScaleToFit);
        }

        public static Texture2D CreateColorTexture(Color color)
        {
            Texture2D texture = new Texture2D(1, 1);
            texture.SetPixel(0, 0, color);
            texture.Apply();
            return texture;
        }
    }
}