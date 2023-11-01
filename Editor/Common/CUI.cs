using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


namespace CodeqoEditor
{
    public static class CUI
    {
        public static GUISkin skin => EditorSkin.skin;
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
        {
            return new Rect(r.position.x + indent, r.position.y + margin, width, height);
        }

        public static GUIStyle BG()
        {
            GUIStyle box = new GUIStyle();
            box.normal.background = EditorTexture.Background;
            return box;
        }

        public static GUIStyle box
        {
            get
            {
                GUIStyle box = GUI.skin.box;
                box.margin = new RectOffset(2, 2, 2, 2);
                box.border = new RectOffset(10, 10, 10, 10);
                box.padding = new RectOffset(6, 6, 6, 6);
                return box;
            }
        }

        public static GUIStyle Box(CUIColor color = CUIColor.None)
            => Box(0, 0, 0, 0, color);
        public static GUIStyle Box(int margins, CUIColor color = CUIColor.None)
            => Box(margins, margins, margins, margins, color);
        public static GUIStyle Box(int left, int right, int top, int bottom, CUIColor color = CUIColor.None)
        {
            GUIStyle box = new GUIStyle();
            Texture2D boxTex = EditorTexture.Box(color);
            box.border = new RectOffset(10, 10, 10, 10);
            box.margin = new RectOffset(left, right, top, bottom);
            box.padding = new RectOffset(6, 6, 6, 6);
            box.normal.background = boxTex;
            return box;
        }

        public static GUIStyle Border(BorderDirection direction)
        {
            GUIStyle box = new GUIStyle();
            Texture2D boxTex = direction == BorderDirection.Top ? EditorTexture.BorderTop : EditorTexture.BorderBottom;
            box.border = new RectOffset(10, 10, 10, 10);
            box.margin = new RectOffset(0, 0, 0, 0);
            box.padding = new RectOffset(6, 6, 6, 6);
            box.normal.background = boxTex;
            return box;
        }

        public static GUIStyle BoxWithMargins(int margin, CUIColor boxColor = CUIColor.None)
        {
            GUIStyle box = new GUIStyle();
            Texture2D boxTex = EditorTexture.Box(boxColor);
            box.border = new RectOffset(10, 10, 10, 10);
            box.margin = new RectOffset(margin, margin, margin, margin);
            box.padding = new RectOffset(6, 6, 6, 6);
            box.normal.background = boxTex;
            return box;
        }

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
            var colorStyle = new GUIStyle();
            Color saveColor = colorStyle.normal.textColor;
            if (bold) colorStyle = EditorStyles.boldLabel;
            colorStyle.normal.textColor = color;
            EditorGUI.LabelField(rect, label, colorStyle);
            colorStyle.normal.textColor = saveColor;
        }


        public static void DrawTexture(Rect textureRect, Texture2D texture)
        {
            GUI.DrawTexture(textureRect, texture, ScaleMode.ScaleToFit, true, 0, Color.white, 0, 0);
        }
        public static void DrawContent(Rect textureRect, GUIContent content)
        {
            GUI.DrawTexture(textureRect, content.image, ScaleMode.ScaleToFit, true, 0, Color.white, 0, 0);
        }



        public static string ListDropdownField(Rect rect, string currentValue, List<string> list, GUIContent label = null)
             => GenericDropdownField(rect, currentValue, list, label);
        public static string ListDropdownField(Rect rect, string currentValue, string[] array, GUIContent label = null)
            => GenericDropdownField(rect, currentValue, array, label);
        private static T GenericDropdownField<T>(Rect rect, T currentValue, IList<T> list, GUIContent label = null)
        {
            if (list == null || list.Count == 0)
            {
                EditorGUILayout.HelpBox("No list found.", MessageType.None);
                return default;
            }

            if (label != null)
            {
                rect = EditorGUI.PrefixLabel(rect, label);
            }

            int index = list.IndexOf(currentValue);

            List<string> stringArray = new List<string>();
            foreach (var enumValue in list)
            {
                stringArray.Add(enumValue.ToString());
            }

            index = EditorGUI.Popup(rect, index, stringArray.ToArray());
            if (index < 0) index = 0;
            return list[index];
        }


        public static void DrawCircle(Rect rect, Texture2D tex)
        {
            // draw a circle with a texture inside
            GUI.BeginGroup(rect);
            GUI.DrawTexture(new Rect(0, 0, rect.width, rect.height), tex, ScaleMode.ScaleToFit, true, 0, Color.white, 0, 0);
            GUI.EndGroup();
        }

    }
}