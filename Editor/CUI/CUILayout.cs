using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace CodeqoEditor
{
    public partial class CUILayout
    {
        private static Dictionary<string, GUIStyle> cachedStyles = new Dictionary<string, GUIStyle>();
        private static Dictionary<string, GUILayoutOption[]> cachedLayoutOptions = new Dictionary<string, GUILayoutOption[]>();

        public static void InfoVisitButton(string label, string url)
        {
            GUILayout.BeginHorizontal();

            EditorGUILayout.HelpBox(
                "More information about " + label
                , MessageType.Info);

            if (GUILayout.Button("Visit", new GUILayoutOption[] {
                GUILayout.Width(50),
                GUILayout.Height(38),
                }))
            {
                Application.OpenURL(url);
            }

            GUILayout.EndHorizontal();
        }

        public static void Foldout(string label, Action callback)
        {
            string styleKey = "CUI_Foldout";
            if (!cachedStyles.ContainsKey(styleKey))
            {
                GUIStyle style = new GUIStyle(EditorStyles.foldout);
                style.fontStyle = FontStyle.Bold;
                cachedStyles.Add(styleKey, style);
            }

            GUIStyle foldoutStyle = cachedStyles[styleKey];
            int space = 3;

            CUIUtility.DrawHorizontalLine(1);
            GUILayout.Space(space);
            bool b = EditorPrefs.GetBool(label, true);
            b = EditorGUILayout.Foldout(b, label, foldoutStyle);
            EditorPrefs.SetBool(label, b);

            if (b)
            {
                GUILayout.Space(space);
                CUIUtility.DrawHorizontalLine(1);

                GUIStyle style = new GUIStyle();
                style.margin = new RectOffset(0, 0, 10, 10);

                GUILayout.BeginVertical(style);
                callback?.Invoke();
                GUILayout.EndVertical();
            }
            else
            {
                GUILayout.Space(space);
                CUIUtility.DrawHorizontalLine(1);
            }

            GUILayout.Space(-space);
        }

        public static void Title(string label)
        {
            string styleKey = "CUI_Title";
            if (!cachedStyles.ContainsKey(styleKey))
            {
                GUIStyle style = new GUIStyle();
                style.fontSize = 14;
                style.fontStyle = FontStyle.Bold;
                style.margin = new RectOffset(0, 0, 10, 10);
                cachedStyles.Add(styleKey, style);
            }
            EditorGUILayout.Space();
            GUIStyle titleStyle = cachedStyles[styleKey];
            EditorGUILayout.LabelField(label, titleStyle);
            CUIUtility.DrawTitleLine();
        }

        public static void BoxedLayout(string label, Action callback, Texture2D texture = null)
        {
            Rect r = (Rect)EditorGUILayout.BeginVertical(CUI.skin.box);
            if (texture == null) texture = EditorTexture.NoImageHighRes;
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

        #region Toggle
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
        #endregion

        #region ButtonToggle
        public static bool ButtonToggle(GUIContent content, bool value, params GUILayoutOption[] options)
        {
            if (value) GUI.backgroundColor = new Color(0.5f, 0.9f, 0.9f);
            if (GUILayout.Button(content, options))
            {
                value = !value;
            }
            GUI.backgroundColor = Color.white;
            return value;
        }

        public static bool ButtonToggle(string label, bool value, params GUILayoutOption[] options)
            => ButtonToggle(new GUIContent(label), value, options);
        public static bool ButtonToggle(Texture2D tex, bool value, params GUILayoutOption[] options)
            => ButtonToggle(new GUIContent(tex), value, options);
        #endregion

        #region BoxedLabel
        private static GUIStyle GetCachedBoxStyle(TextAnchor alignment, CUIColor color = CUIColor.None)
        {
            string key = $"{alignment}_{color}";
            if (!cachedStyles.TryGetValue(key, out var style))
            {
                style = new GUIStyle
                {
                    border = new RectOffset(5, 5, 5, 5),
                    margin = new RectOffset(0, 0, 0, 0),
                    padding = new RectOffset(6, 6, 2, 2),
                    fontSize = 12,
                    normal = { background = EditorTexture.Box(color) },
                    overflow = new RectOffset(0, 0, 0, 0),
                    alignment = alignment,
                    wordWrap = true
                };
                cachedStyles[key] = style;
            }
            return style;
        }

        public static void BoxedLabel(GUIContent label, TextAnchor alignment, params GUILayoutOption[] options)
            => EditorGUILayout.LabelField(label, GetCachedBoxStyle(alignment), options);
        public static void BoxedLabel(GUIContent label, TextAnchor alignment, CUIColor color, params GUILayoutOption[] options)
            => EditorGUILayout.LabelField(label, GetCachedBoxStyle(alignment, color), options);
        public static void BoxedLabel(string label, TextAnchor alignment, params GUILayoutOption[] options)
           => EditorGUILayout.LabelField(label, GetCachedBoxStyle(alignment), options);
        public static void BoxedLabel(GUIContent label, params GUILayoutOption[] options)
            => EditorGUILayout.LabelField(label, GetCachedBoxStyle(TextAnchor.MiddleCenter), options);
        public static void BoxedLabel(string label, params GUILayoutOption[] options)
            => EditorGUILayout.LabelField(label, GetCachedBoxStyle(TextAnchor.MiddleCenter), options);
        #endregion

        #region MiniButton
        private static GUIStyle GetCachedMiniButtonStyle(TextAnchor alignment)
        {
            string key = $"MiniButton_{alignment}";
            if (!cachedStyles.TryGetValue(key, out var style))
            {
                style = new GUIStyle(GUI.skin.button);
                style.fontSize = 10;
                style.wordWrap = true;
                style.alignment = alignment;
                cachedStyles[key] = style;
            }
            return style;
        }

        private static GUILayoutOption[] GetCachedMiniButtonLayoutOptions()
        {
            string key = "MiniButton";
            if (!cachedLayoutOptions.TryGetValue(key, out var options))
            {
                options = new GUILayoutOption[] {
                    GUILayout.Height(40),
                    GUILayout.ExpandWidth(true),
                    GUILayout.ExpandHeight(false)

                };
                cachedLayoutOptions[key] = options;
            }
            return options;
        }

        public static bool MiniButton(GUIContent label, TextAnchor alignment = TextAnchor.MiddleCenter)
            => GUILayout.Button(label, GetCachedMiniButtonStyle(alignment), GetCachedMiniButtonLayoutOptions());
        public static bool MiniButton(string label, TextAnchor alignment = TextAnchor.MiddleCenter)
            => GUILayout.Button(new GUIContent(label), GetCachedMiniButtonStyle(alignment), GetCachedMiniButtonLayoutOptions());
        public static bool MiniButton(Texture2D tex, TextAnchor alignment = TextAnchor.MiddleCenter)
            => GUILayout.Button(new GUIContent(tex), GetCachedMiniButtonStyle(alignment), GetCachedMiniButtonLayoutOptions());

        #endregion

        #region OtherLabel
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

        public static void ColorLabelField(string label, Color color, bool bold = true)
        {
            var colorStyle = new GUIStyle();
            Color saveColor = colorStyle.normal.textColor;
            if (bold) colorStyle = EditorStyles.boldLabel;
            colorStyle.normal.textColor = color;
            colorStyle.alignment = TextAnchor.MiddleLeft;
            EditorGUILayout.LabelField(label, colorStyle);
            colorStyle.normal.textColor = saveColor;
        }
        #endregion

        #region DescriptionField
        public static string DescriptionField(string text, params GUILayoutOption[] options)
        {
            GUIStyle style = CUI.skin.textArea;
            return EditorGUILayout.TextArea(text, style, options);
        }

        public static string DescriptionField(SerializedProperty property, params GUILayoutOption[] options)
        {
            GUIStyle style = CUI.skin.textArea;
            return EditorGUILayout.TextArea(property.stringValue, style, options);
        }

        public static string DescriptionField(string label, SerializedProperty property, params GUILayoutOption[] options)
        {
            GUIStyle style = CUI.skin.textArea; ;
            EditorGUILayout.PrefixLabel(label);
            return EditorGUILayout.TextArea(property.stringValue, style, options);
        }
        #endregion

        #region Horizontal/VerticalLayout
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

        #region ColorButton        
        public static bool ColorButton(GUIContent content, Color backgroundColor, Color textColor, GUIStyle style = null, params GUILayoutOption[] options)
        {
            GUIStyle buttonStyle = new GUIStyle(style ?? GUI.skin.button);
            buttonStyle.normal.textColor = textColor;
            buttonStyle.normal.background = CUI.CreateColorTexture(backgroundColor);
            buttonStyle.hover.background = CUI.CreateColorTexture(backgroundColor * 1.1f); // Slightly brighter when hovered
            buttonStyle.active.background = CUI.CreateColorTexture(backgroundColor * 0.9f); // Slightly darker when clicked

            return GUILayout.Button(content, buttonStyle, options);
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

        #region ColorSelectToolbar
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
        #endregion

        #region ListDropdownField
        private static T GenericDropdownField<T>(T currentValue, IList<T> list, GUIContent label = null, Rect? position = null, params GUILayoutOption[] options)
        {
            if (list == null || list.Count == 0)
            {
                if (position.HasValue) EditorGUI.HelpBox(position.Value, "No list found.", MessageType.None);
                else EditorGUILayout.HelpBox("No list found.", MessageType.None);

                return default;
            }

            int index = list.IndexOf(currentValue);
            List<string> stringArray = list.Select(enumValue => enumValue.ToString()).ToList();

            if (position.HasValue) index = EditorGUI.Popup(position.Value, index, stringArray.ToArray());
            else index = EditorGUILayout.Popup(label, index, stringArray.ToArray(), options);

            if (index < 0) index = 0;
            return list[index];
        }

        // GUILayoutOption 사용할때
        public static string ListDropdownField(string currentValue, List<string> list, GUIContent label = null, params GUILayoutOption[] options)
            => GenericDropdownField(currentValue, list, label, null, options);

        public static string ListDropdownField(string currentValue, string[] array, GUIContent label = null, params GUILayoutOption[] options)
            => GenericDropdownField(currentValue, array, label, null, options);
        // end

        // Rect 사용할때
        public static string ListDropdownField(string currentValue, List<string> list, GUIContent label = null, Rect? position = null)
            => GenericDropdownField(currentValue, list, label, position);

        public static string ListDropdownField(string currentValue, string[] array, GUIContent label = null, Rect? position = null)
            => GenericDropdownField(currentValue, array, label, position);
        // end

        #endregion

        #region ListToolbarField
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
                bool isActive = GUILayout.Toggle(index == i, list[i], EditorStyles.miniButton, options);
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

        public static bool DrawEnumCheckboxes<T>(IList<T> allValues, IDictionary<T, bool> checkDictionary, int columns = 3, int defaultValue = -1) where T : Enum
        {
            bool updated = false;
            int rowCount = Mathf.CeilToInt((float)allValues.Count / columns);

            for (int row = 0; row < rowCount; row++)
            {
                EditorGUILayout.BeginHorizontal();

                for (int col = 0; col < columns; col++)
                {
                    int index = row * columns + col;

                    if (index < allValues.Count)
                    {
                        T value = allValues[index];

                        bool isOn = checkDictionary[value];
                        bool isDefault = defaultValue != -1 && Convert.ToInt32(value) == defaultValue;
                        string label = isDefault ? value.ToString() + " (Default)" : value.ToString();

                        bool toggle = EditorGUILayout.ToggleLeft(label, isOn, GUILayout.Width(EditorGUIUtility.currentViewWidth / columns - 10));

                        if (defaultValue != -1)
                        {
                            int enumIndex = Convert.ToInt32(value);
                            if (enumIndex == defaultValue && !isOn) toggle = true;
                        }

                        if (toggle != isOn)
                        {
                            checkDictionary[value] = toggle;
                            updated = true;
                        }
                    }
                }

                EditorGUILayout.EndHorizontal();
            }

            return updated;
        }

        public static bool EnumPropertyFieldWithExclusion<T>(SerializedProperty enumProperty, IList<T> toExclude, GUIContent label = null, params GUILayoutOption[] options)
                where T : Enum
        {
            if (enumProperty == null)
            {
                throw new ArgumentNullException(nameof(enumProperty));
            }

            if (enumProperty.propertyType != SerializedPropertyType.Enum)
            {
                throw new ArgumentException($"Property {enumProperty.name} is not an enum.");
            }

            if (toExclude == null)
            {
                throw new ArgumentNullException(nameof(toExclude));
            }

            T[] allValues = (T[])Enum.GetValues(typeof(T));
            List<GUIContent> displayedOptions = new List<GUIContent>();
            List<int> optionValues = new List<int>();

            T enumValue = (T)Enum.ToObject(typeof(T), enumProperty.intValue);

            for (int i = 0; i < allValues.Length; i++)
            {
                if (!toExclude.Contains(allValues[i]) || allValues[i].Equals(enumValue))
                {
                    displayedOptions.Add(new GUIContent(allValues[i].ToString()));
                    optionValues.Add(Convert.ToInt32(allValues[i]));
                }
            }

            int currentIndex = Array.IndexOf(allValues, enumValue);
            int newIndex = EditorGUILayout.IntPopup(label, currentIndex, displayedOptions.ToArray(), optionValues.ToArray(), options);

            if (newIndex != currentIndex)
            {
                enumProperty.intValue = newIndex;
                return true;
            }

            return false;
        }

        public static bool EnumPopupWithExclusion<T>(T currentEnum, IList<T> toExclude, out T selectedEnum, GUIContent label = null, params GUILayoutOption[] options) where T : Enum
        {
            if (toExclude == null)
            {
                throw new ArgumentNullException(nameof(toExclude));
            }

            T[] allValues = (T[])Enum.GetValues(typeof(T));
            List<GUIContent> displayedOptions = new List<GUIContent>();
            List<T> optionValues = new List<T>();

            for (int i = 0; i < allValues.Length; i++)
            {
                if (!toExclude.Contains(allValues[i]) || EqualityComparer<T>.Default.Equals(allValues[i], currentEnum))
                {
                    displayedOptions.Add(new GUIContent(allValues[i].ToString()));
                    optionValues.Add(allValues[i]);
                }
            }

            int currentIndex = optionValues.IndexOf(currentEnum);
            int newIndex = EditorGUILayout.Popup(label ?? GUIContent.none, currentIndex, displayedOptions.ToArray(), options);

            selectedEnum = optionValues[newIndex];

            return newIndex != currentIndex;
        }


        public static string TextArea(string text, Object assetToSave, params GUILayoutOption[] options)
           => TextArea(text, assetToSave, null, options);

        public static string TextArea(string text, Object assetToSave, GUIStyle style, params GUILayoutOption[] options)
        {
            if (assetToSave == null) throw new ArgumentNullException(nameof(assetToSave));
            if (style == null) style = EditorStyles.textArea;

            CUI.CurrentField = assetToSave.GetHashCode().ToString();
            string newText = EditorGUILayout.TextArea(text, style, options);

            if (CUI.CurrentField != CUI.CurrentField && newText != text)
            {
                Undo.RecordObject(assetToSave, $"Update {assetToSave.name}");
                EditorUtility.SetDirty(assetToSave);
            }

            return newText;
        }
    }
}

