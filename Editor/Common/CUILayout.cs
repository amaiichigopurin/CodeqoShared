using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeqoEditor
{
    public partial class CUILayout
    {
        public static void InfoVisitButton(string label, string url)
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.HelpBox(
                "More information about " + label
                , MessageType.None);

            if (GUILayout.Button("Visit", GUILayout.Width(50)))
            {
                Application.OpenURL(url);
            }

            GUILayout.EndHorizontal();
        }

        public static void Foldout(string label, Action callback)
        {
            float stroke = 1.2f;
            float padding = 3f; // space between the strokes and the content
            
            CUIUtility.DrawHorizontalLine(stroke);
            GUILayout.Space(padding);
            bool b = EditorPrefs.GetBool(label, true);
            b = EditorGUILayout.Foldout(b, label);
            EditorPrefs.SetBool(label, b);

            if (b)
            {
                GUILayout.Space(padding);
                CUIUtility.DrawHorizontalLine(stroke);

                GUIStyle contentStyle = new GUIStyle();
                contentStyle.margin = new RectOffset(0, 10, 5, 20);

                GUILayout.BeginVertical(contentStyle);
                callback?.Invoke();
                GUILayout.EndVertical();
            }
            else
            {
                GUILayout.Space(padding);
                CUIUtility.DrawHorizontalLine(stroke);
            }

            GUILayout.Space(-padding);
        }

        public static void BoxedLayout(string label, Action callback, Texture2D texture = null)
        {
            Rect r = (Rect)EditorGUILayout.BeginVertical(CUI.skin.box);

            if (texture == null) texture = EditorTexture.Config;

            /* Header */
            GUI.DrawTexture(new Rect(r.position.x + 10, r.position.y + 5, 24, 24), texture);
            GUI.skin.label.fontSize = 14;
            GUI.Label(CUI.GetHeaderRect(r, indent: 40, width: r.width), label.ToUpper());
            GUI.skin.label.fontSize = 12;
            GUILayout.Space(30);
            callback?.Invoke();
            EditorGUILayout.EndVertical();
        }

        public static bool BoolPropertyField(SerializedProperty p)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(p, GUIContent.none, GUILayout.Width(10));
            GUILayout.Label(new GUIContent(p.displayName), GUILayout.MinWidth(10));
            GUILayout.EndHorizontal();
            return p.boolValue;
        }

        public static bool BoolPropertyField(string label, SerializedProperty p)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(p, GUIContent.none, GUILayout.Width(10));
            GUILayout.Label(new GUIContent(label), GUILayout.MinWidth(10));
            GUILayout.EndHorizontal();
            return p.boolValue;
        }

        public static bool Toggle(string label, bool value)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.Toggle(value, GUILayout.Width(10));
            GUILayout.Label(new GUIContent(label), GUILayout.MinWidth(10));
            GUILayout.EndHorizontal();
            return value;
        }

        public static int Toggle(string label, int value)
        {
            GUILayout.BeginHorizontal();
            value = EditorGUILayout.Toggle(value == 1, GUILayout.Width(10)) ? 1 : 0;
            GUILayout.Label(new GUIContent(label), GUILayout.MinWidth(10));
            GUILayout.EndHorizontal();
            return value;
        }

        public static bool ButtonToggle(GUIContent content, bool value, Action<bool> onToggle = null, params GUILayoutOption[] options)
        {
            if (value) GUI.backgroundColor = new Color(0.5f, 0.9f, 0.9f);
            if (GUILayout.Button(content, options))
            {
                value = !value;
                onToggle?.Invoke(value);
            }
            GUI.backgroundColor = Color.white;
            return value;
        }
        
        public static bool ButtonToggle(string label, bool value, Action<bool> onToggle = null, params GUILayoutOption[] options)
            => ButtonToggle(new GUIContent(label), value, onToggle, options);
        
        public static bool ButtonToggle(Texture2D tex, bool value, Action<bool> onToggle = null, params GUILayoutOption[] options)
            => ButtonToggle(new GUIContent(tex), value, onToggle, options);

        public static void SpriteField(SerializedProperty p, int size, int topMargin)
        {
            GUILayout.BeginVertical();
            GUILayout.Space(topMargin);
            p.objectReferenceValue = EditorGUILayout.ObjectField(p.objectReferenceValue, typeof(Sprite), false, new GUILayoutOption[] {
                    GUILayout.Width(size),
                    GUILayout.Height(size)
                }); ;
            GUILayout.EndVertical();
        }
        
        static GUIStyle BoxedLabel(TextAnchor alignment, CUIColor color = CUIColor.None)
        {
            GUIStyle box = new GUIStyle();
            //box.border = new RectOffset(10, 10, 10, 10);
            box.border = new RectOffset(5, 5, 5, 5);
            box.margin = new RectOffset(0, 0, 0, 0);
            box.padding = new RectOffset(6, 6, 2, 2);
            box.fontSize = 12;
            box.normal.background = EditorTexture.Box(color);
            box.overflow = new RectOffset(0, 0, 0, 0);
            box.alignment = alignment;
            //line break true
            box.wordWrap = true;
            return box;
        }

        public static void BoxedLabel(GUIContent label, TextAnchor alignment, params GUILayoutOption[] options)
        {
            EditorGUILayout.LabelField(label, BoxedLabel(alignment), options);
        }

        public static void BoxedLabel(GUIContent label, TextAnchor alignment, CUIColor color, params GUILayoutOption[] options)
        {
            EditorGUILayout.LabelField(label, BoxedLabel(alignment, color), options);
        }

        public static void BoxedLabel(string label, TextAnchor alignment, params GUILayoutOption[] options)
        {
            EditorGUILayout.LabelField(label, BoxedLabel(alignment), options);
        }

        public static void BoxedLabel(GUIContent label, params GUILayoutOption[] options)
        {
            EditorGUILayout.LabelField(label, BoxedLabel(TextAnchor.MiddleCenter), options);
        }

        public static void BoxedLabel(string label, params GUILayoutOption[] options)
        {
            EditorGUILayout.LabelField(label, BoxedLabel(TextAnchor.MiddleCenter), options);
        }

        public static void CenteredLabel(GUIContent label, int fontSize = 10)
        {
            var centeredStyle = new GUIStyle();
            centeredStyle.alignment = TextAnchor.UpperCenter;
            centeredStyle.normal.textColor = Color.black;
            centeredStyle.fontSize = fontSize;
            Rect r = EditorGUILayout.GetControlRect();
            r.x = EditorGUIUtility.currentViewWidth / 2 - r.width / 2;
            GUI.Label(r, label, centeredStyle);
            centeredStyle.alignment = TextAnchor.UpperLeft;
        }

        public static void ColorLabelField(string label, Color color, GUIStyle style, params GUILayoutOption[] options)
        {
            Color saveColor = style.normal.textColor;
            style.normal.textColor = color;
            style.alignment = TextAnchor.MiddleLeft;
            EditorGUILayout.LabelField(label, style, options);
            style.normal.textColor = saveColor;
        }

        public static void ColorLabelField(string label, Color color, params GUILayoutOption[] options)
            => ColorLabelField(label, color, EditorStyles.label, options);

        #region Horizontal/VerticalLayout
        public static void HorizontalLayout(GUIContent label, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(options);
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            action();
            GUILayout.EndHorizontal();
        }

        public static void VerticalLayout(GUIContent label, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(options);
            EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
            action();
            GUILayout.EndVertical();
        }

        public static void HorizontalLayout(Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(options);
            action();
            GUILayout.EndHorizontal();
        }

        public static void VerticalLayout(Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(options);
            action();
            GUILayout.EndVertical();
        }

        public static void HorizontalLayout(GUIStyle style, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(style, options);
            action();
            GUILayout.EndHorizontal();
        }

        public static void VerticalLayout(GUIStyle style, Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(style, options);
            action();
            GUILayout.EndVertical();
        }
        #endregion

        #region ImageField / ImageButton

        public static void ImageField(Vector2 size, Object asset, bool background = true)
        {
            if (background) GUILayout.BeginVertical(CUI.box, GUILayout.MaxWidth(size.x), GUILayout.MaxHeight(size.y));
            Texture2D texture = asset == null ? null : AssetPreview.GetAssetPreview(asset);
            Rect rect = GUILayoutUtility.GetRect(size.x, size.y);
            GUI.DrawTexture(rect, texture != null ? texture : EditorTexture.NoImageHighRes, ScaleMode.ScaleToFit);
            if (background) GUILayout.EndVertical();
        }

        public static void ImageField(Texture2D texture, bool background = true)
        {
            Vector2 size = new Vector2(texture.width, texture.height);
            ImageField(size, texture, background);
        }

        public static void ImageButton(Vector2 size, Object asset, Action buttonAction)
        {
            Texture2D texture = asset == null ? null : AssetPreview.GetAssetPreview(asset);
            GUIStyle customButtonStyle = new GUIStyle(GUIStyle.none);

            if (texture != null)
            {
                customButtonStyle.normal.background = texture;
                customButtonStyle.border = new RectOffset(0, 0, 0, 0);
            }
            else
            {
                customButtonStyle.normal.background = EditorTexture.NoImageHighRes;
            }

            Rect rect = GUILayoutUtility.GetRect(size.x, size.y, GUILayout.Width(size.x), GUILayout.Height(size.y));

            if (GUI.Button(rect, GUIContent.none, customButtonStyle))
            {
                buttonAction();
            }
        }
        #endregion

        #region DateTimeField
        public static DateTime DateTimeField(DateTime dateTime, bool year, bool month, bool day, bool hour = false, bool minute = false, bool second = false, params GUILayoutOption[] options)
        {
            return DateTimeField(null, dateTime, year, month, day, hour, minute, second, options);
        }

        public static DateTime DateTimeField(string label, DateTime dateTime, bool year, bool month, bool day, bool hour = false, bool minute = false, bool second = false, params GUILayoutOption[] options)
        {
            EditorGUILayout.BeginHorizontal(options);

            if (label != null)
            {
                float labelWidth = EditorGUIUtility.labelWidth;
                EditorGUILayout.LabelField(label, GUILayout.Width(labelWidth));
            }

            int YY = 2000;
            int MM = 1;
            int DD = 1;
            int hh = 0;
            int mm = 0;
            int ss = 0;

            if (year)
            {
                YY = EditorGUILayout.IntField(dateTime.Year, GUILayout.Width(50));
                EditorGUILayout.LabelField("년", GUILayout.Width(20));
            }

            if (month)
            {
                MM = EditorGUILayout.IntField(dateTime.Month, GUILayout.Width(30));
                EditorGUILayout.LabelField("월", GUILayout.Width(20));
            }

            if (day)
            {
                DD = EditorGUILayout.IntField(dateTime.Day, GUILayout.Width(30));
                EditorGUILayout.LabelField("일", GUILayout.Width(20));
            }

            if (hour)
            {
                hh = EditorGUILayout.IntField(dateTime.Hour, GUILayout.Width(30));
                EditorGUILayout.LabelField("시", GUILayout.Width(20));
            }

            if (minute)
            {
                mm = EditorGUILayout.IntField(dateTime.Minute, GUILayout.Width(30));
                EditorGUILayout.LabelField("분", GUILayout.Width(20));
            }

            if (second)
            {
                ss = EditorGUILayout.IntField(dateTime.Second, GUILayout.Width(30));
                EditorGUILayout.LabelField("초", GUILayout.Width(20));
            }

            EditorGUILayout.EndHorizontal();
            return new DateTime(YY, MM, DD, hh, mm, ss);
        }
        #endregion



        #region ColoredButton

        public static Color ColorSelectToolbar(GUIContent content, List<Color> colorList, Color selectedColor)
        {
            Color defaultColor = GUI.backgroundColor;

            GUIStyle style = new GUIStyle();
            style.padding = new RectOffset(0, 0, 0, 0);
            style.margin = new RectOffset(0, 0, 0, 0);
            style.border = new RectOffset(0, 0, 0, 0);
            style.fixedWidth = 20;
            style.fixedHeight = 20;

            GUILayout.BeginHorizontal();

            //label
            EditorGUILayout.LabelField(content, GUILayout.Width(EditorGUIUtility.labelWidth));

            foreach (var color in colorList)
            {
                // Change the background texture for the selected color entry
                style.normal.background = color == selectedColor ? EditorTexture.ToolBarButtonOn : EditorTexture.ToolBarButtonOff;
                GUI.backgroundColor = color;

                if (GUILayout.Button("", style))
                {
                    selectedColor = color;
                }
            }
            GUILayout.EndHorizontal();

            GUI.backgroundColor = defaultColor;
            return selectedColor;
        }

        public static Color ColorSelectToolbar(string label, List<Color> colorList, Color selectedColor) 
            => ColorSelectToolbar(new GUIContent(label), colorList, selectedColor);


        public static bool ColorButton(GUIContent content, Color color, Color textColor, GUIStyle style, params GUILayoutOption[] options)
        {
            Color defaultColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
            Color defaultTextColor = GUI.contentColor;
            GUI.contentColor = textColor;

            if (style == null)
            {
                style = GUI.skin.button;
            }

            bool result = GUILayout.Button(content, style, options);
            GUI.backgroundColor = defaultColor;
            GUI.contentColor = defaultTextColor;
            return result;
        }

        public static bool ColorButton(GUIContent content, Color color, GUIStyle style, params GUILayoutOption[] options)        
            => ColorButton(content, color, Color.black, style, options);
        public static bool ColorButton(string label, Color color, GUIStyle style, params GUILayoutOption[] options)        
            => ColorButton(new GUIContent(label), color, style, options);
        public static bool ColorButton(string label, Color color, Color textColor, GUIStyle style, params GUILayoutOption[] options)        
            => ColorButton(new GUIContent(label), color, textColor, style, options);
        public static bool ColorButton(GUIContent content, Color color, params GUILayoutOption[] options)        
            => ColorButton(content, color, Color.black, null, options);
        public static bool ColorButton(string label, Color color, params GUILayoutOption[] options)        
            => ColorButton(new GUIContent(label), color, options);
        public static bool ColorButton(string label, Color color, Color textColor, params GUILayoutOption[] options)        
            => ColorButton(new GUIContent(label), color, textColor, null, options);
        public static bool ColorButton(GUIContent content, Color color, Color textColor, params GUILayoutOption[] options)        
            => ColorButton(content, color, textColor, null, options);


        #endregion

        #region ListDropdownField

        public static string ListDropdownField(string currentValue, List<string> list, GUIContent label = null, params GUILayoutOption[] options)        
            => GenericDropdownField(currentValue, list, label, options);   
        public static string ListDropdownField(string currentValue, string[] array, GUIContent label = null, params GUILayoutOption[] options)        
            => GenericDropdownField(currentValue, array, label, options);
        private static T GenericDropdownField<T>(T currentValue, IList<T> list, GUIContent label = null, params GUILayoutOption[] options)
        {
            if (list == null || list.Count == 0)
            {
                EditorGUILayout.HelpBox("No list found.", MessageType.None);
                return default;
            }

            int index = list.IndexOf(currentValue);
            index = EditorGUILayout.Popup(label, index, ToStringArray(list), options);
            if (index < 0) index = 0;
            return list[index];
        }

        private static string[] ToStringArray<T>(IList<T> list)
        {
            string[] result = new string[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                result[i] = list[i]?.ToString() ?? "null";
            }
            return result;
        }

        public static int ListToolbarField(int currentId, List<string> list, GUIContent label = null, params GUILayoutOption[] options)
        {
            if (list == null || list.Count == 0)
            {
                EditorGUILayout.HelpBox("No list found.", MessageType.Warning);
                return -1;
            }

            int index = currentId;

            float totalWidth = 0;
            GUILayout.BeginHorizontal();


            if (label != null)
            {
                GUILayout.Label(label, GUILayout.Width(EditorGUIUtility.labelWidth));
            }

            for (int i = 0; i < list.Count; i++)
            {
                float buttonWidth = EditorStyles.toolbarButton.CalcSize(new GUIContent(list[i])).x;
                if (totalWidth + buttonWidth > EditorGUIUtility.currentViewWidth - EditorGUIUtility.labelWidth)
                {
                    // If the button will exceed the width of the inspector, wrap to the next line.
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();

                    if (label != null)
                    {
                        //empty space
                        GUILayout.Space(EditorGUIUtility.labelWidth);
                    }

                    totalWidth = 0;
                }

                // Use toggle style buttons for the toolbar buttons
                bool isActive = GUILayout.Toggle(index == i, list[i], EditorStyles.toolbarButton, options);
                if (isActive) index = i;

                totalWidth += buttonWidth;
            }
            GUILayout.EndHorizontal();

            return index;
        }
        

 



        #endregion
        public static T EnumFlagButtons<T>(T enumValue, int startIndex = 1, params GUILayoutOption[] options) where T : Enum
        {
            // Get all values and names of the Enum
            var values = Enum.GetValues(typeof(T));
            var names = Enum.GetNames(typeof(T));

            // Convert the current selected value to int
            int currentIntValue = Convert.ToInt32(enumValue);
            int newIntValue = currentIntValue;

            EditorGUILayout.BeginHorizontal();

            if (values.Length > startIndex)
            {
                for (int i = startIndex; i < values.Length; i++)
                {
                    // Determine if the current flag is set
                    bool flagSet = (currentIntValue & (int)values.GetValue(i)) != 0;

                    // Create a toggle button
                    flagSet = ButtonToggle(names[i], flagSet, options: options);

                    // Update the int value based on toggle button state
                    if (flagSet)
                    {
                        newIntValue |= (int)values.GetValue(i);
                    }
                    else
                    {
                        newIntValue &= ~(int)values.GetValue(i);
                    }
                }
            }

            EditorGUILayout.EndHorizontal();

            // Convert the int value back to the enum type and return
            return (T)Enum.ToObject(typeof(T), newIntValue);
        }

        /// <summary>
        /// Enum변수가 변경되면 true를 리턴한다.
        /// </summary>
        public static bool EnumToolBar<T>(ref T enumValue, bool toolBarStyle = false, params GUILayoutOption[] options) where T : Enum
        {
            var enumNames = Enum.GetNames(typeof(T));
            int currentIndex = Convert.ToInt32(enumValue);
            int newIndex;

            if (toolBarStyle)
            {
                GUIStyle toolbarStyle = new GUIStyle(EditorStyles.toolbarButton);
                newIndex = GUILayout.Toolbar(currentIndex, enumNames, toolbarStyle, options);
            }
            else
            {
                newIndex = GUILayout.Toolbar(currentIndex, enumNames, options);
            }

            if (newIndex != currentIndex)
            {
                enumValue = (T)Enum.ToObject(typeof(T), newIndex);
                return true;
            }

            return false;
        }

        public static HashSet<TEnum> DrawEnumCheckboxes<TEnum>(HashSet<TEnum> selectedValues, int columns = 3, float width = -1f) where TEnum : Enum
        {
            if (width < 0)
            {
                width = EditorGUIUtility.currentViewWidth / columns - 10;
            }

            bool updated = false;
            TEnum[] enumValues = (TEnum[])Enum.GetValues(typeof(TEnum));
            int rowCount = Mathf.CeilToInt((float)enumValues.Length / columns);

            for (int row = 0; row < rowCount; row++)
            {
                EditorGUILayout.BeginHorizontal();

                for (int col = 0; col < columns; col++)
                {
                    int index = row * columns + col;

                    if (index < enumValues.Length)
                    {
                        TEnum enumValue = enumValues[index];

                        bool isCurrentlySelected = selectedValues.Contains(enumValue);
                        bool toggle = EditorGUILayout.ToggleLeft(enumValue.ToString(), isCurrentlySelected, GUILayout.Width(width));

                        if (toggle != isCurrentlySelected)
                        {
                            if (toggle)
                            {
                                selectedValues.Add(enumValue);
                            }
                            else
                            {
                                selectedValues.Remove(enumValue);
                            }

                            updated = true;
                        }
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            if (updated)
            {
                // 이 부분에 필요한 경우 업데이트 로직을 추가할 수 있습니다.
                Debug.Log("Selection Updated");
            }

            return selectedValues;
        }
    }
}

